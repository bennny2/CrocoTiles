using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Trie node
class TrieNode
{
    public TrieNode[] Children = new TrieNode[26];
    public bool IsEndOfWord;
}

class Trie
{
    private readonly TrieNode root;

    public Trie()
    {
        root = new TrieNode();
        foreach (string word in File.ReadAllLines("C:/Users/benhu/Documents/GitHub/TileWar/TileWarNew/Assets/Dictionary/collins.dic"))
            Insert(word.ToLower());
    }

    public void Insert(string word)
    {
        TrieNode node = root;
        foreach (char c in word)
        {
            int index = c - 'a';
            if (node.Children[index] == null)
                node.Children[index] = new TrieNode();
            node = node.Children[index];
        }
        node.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        TrieNode node = SearchPrefix(word);
        return node != null && node.IsEndOfWord;
    }

    private TrieNode SearchPrefix(string word)
    {
        TrieNode node = root;
        foreach (char c in word)
        {
            int index = c - 'a';
            if (node.Children[index] == null)
                return null;
            node = node.Children[index];
        }
        return node;
    }
    private bool CanTheseLettersMakeAWord(List<string> letters) {
        IEnumerable<string> letterEnumerable = letters;

        for (int length = 3; length <= 5; length++) {
            List<string> permutations = GetPermutationsWithDuplicates(letterEnumerable, length).Select(perm => string.Join("", perm)).ToList();

            foreach (string potentialWord in permutations) {
                if (Search(potentialWord)) {
                    Debug.Log(potentialWord);
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanFormValidWord(List<Hexagon> AllHexagons) {
        List<string> letters = new();
        foreach (Hexagon hex in AllHexagons) {
            if (!string.IsNullOrWhiteSpace(hex.HexagonText.text) && hex.HexagonText.text != "*" ) {
                letters.Add(hex.HexagonText.text);
            }
        }
        return CanTheseLettersMakeAWord(letters);
    }
    
    private IEnumerable<IEnumerable<T>> GetPermutationsWithDuplicates<T>(IEnumerable<T> list, int length) {

        Dictionary<T, int> elementCounts = list.GroupBy(e => e).ToDictionary(g => g.Key, g => g.Count());

        IEnumerable<IEnumerable<T>> GetPermutationsInternal(int remainingLength) {
            if (remainingLength == 1) {
                return list.Select(e => new T[] { e });
            }

            return GetPermutationsInternal(remainingLength - 1)
                .SelectMany(partialPermutation =>
                    elementCounts
                        .Where(kv => kv.Value > partialPermutation.Count(e => EqualityComparer<T>.Default.Equals(kv.Key, e)))
                        .Select(kv => partialPermutation.Concat(new T[] { kv.Key })));
        }

        return GetPermutationsInternal(length);
    }
}
