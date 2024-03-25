using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
}
