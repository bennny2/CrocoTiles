using System.Collections.Generic;
using System.Linq;

class TrieNode
{

    // Properties
    
    public TrieNode[] Children { get; } = new TrieNode[26]; 
    public bool IsEndOfWord { get; set; }

    // Constructor

    public TrieNode()
    {
        for (int i = 0; i < Children.Length; i++) {
            Children[i] = null;
        }
    }
}


class Trie
{

    // Fields

    private readonly TrieNode _root;

    // Properties

    internal TrieNode Root => _root;

    // Constructor

    public Trie()
    {
        _root = new TrieNode();
    }

    // Class Methods

    public bool CanFormValidWord(List<Hexagon> AllHexagons) {
        List<string> letters = new();
        foreach (Hexagon hex in AllHexagons) {
            if (!string.IsNullOrWhiteSpace(hex.HexagonText.text) && hex.HexagonText.text != "*" ) {
                letters.Add(hex.HexagonText.text);
            }
        }
        return CanTheseLettersMakeAWord(letters);
    }

    private bool CanTheseLettersMakeAWord(List<string> letters) {
        IEnumerable<string> letterEnumerable = letters;

        for (int length = 3; length <= 5; length++) {
            List<string> permutations = GetPermutationsWithDuplicates(letterEnumerable, length).Select(perm => string.Join("", perm)).ToList();

            foreach (string potentialWord in permutations) {
                if (Search(potentialWord)) {
                    //Debug.Log(potentialWord);
                    return true;
                }
            }
        }
        return false;
    }

    public void Insert(string word) {
        TrieNode node = Root;
        foreach (char c in word)
        {
            int index = c - 'a';
            if (node.Children[index] == null)
                node.Children[index] = new TrieNode();
            node = node.Children[index];
        }
        node.IsEndOfWord = true;
    }

    public bool Search(string word) {
        TrieNode node = SearchPrefix(word);
        return node != null && node.IsEndOfWord;
    }

    private TrieNode SearchPrefix(string word) {
        TrieNode node = Root;
        foreach (char c in word)
        {
            int index = c - 'a';
            if (node.Children[index] == null)
                return null;
            node = node.Children[index];
        }
        return node;
    }
    public List<string> GenerateWordsFromGameObjects(List<Hexagon> scoredHexes, int wordLength)
    {
        List<string> letters = new List<string>();
        foreach (Hexagon hex in scoredHexes)
        {
            if (!string.IsNullOrWhiteSpace(hex.HexagonText.text))
            {
                letters.Add(hex.HexagonText.text); 
            }
        }
        return GenerateWordsFromLetters(letters, wordLength);
    }

    private List<string> GenerateWordsFromLetters(List<string> letters, int wordLength)
    {
        List<string> validWords = new List<string>();
        List<char> word = new List<char>();
        bool[] used = new bool[letters.Count];

        GenerateWordsHelper(letters, used, _root, word, validWords, wordLength);

        return validWords;
    }

    private void GenerateWordsHelper(List<string> letters, bool[] used, TrieNode node, List<char> word, List<string> validWords, int wordLength)
    {
        if (validWords.Count >= 15)
            return; 

        if (word.Count == wordLength)
        {
            if (node.IsEndOfWord)
            {
                validWords.Add(new string(word.ToArray())); 
            }
            return;
        }

        for (int i = 0; i < letters.Count; i++)
        {
            if (!used[i])
            {
                char letter = letters[i][0];
                int index = letter - 'a';

                if (node.Children[index] != null)
                {
                    used[i] = true;
                    word.Add(letter);
                    GenerateWordsHelper(letters, used, node.Children[index], word, validWords, wordLength);
                    word.RemoveAt(word.Count - 1);
                    used[i] = false;

                    if (validWords.Count >= 10)
                        return; 
                }
            }
        }
    }
    
    public IEnumerable<IEnumerable<T>> GetPermutationsWithDuplicates<T>(IEnumerable<T> list, int length) {

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
