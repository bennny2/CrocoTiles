using System;
using System.Collections.Generic;
using System.Linq;

public static class Letter
{

    // Fields

    private static readonly Random _random = new();
    private static List<char> _inPlayLettersPool = new();
    private static List<char> _availableLettersPool = new();
    private static readonly List<char> _availableLettersPoolTemplate = new()
    {
        'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
        'b', 'b', 'b', 'b',
        'c', 'c', 'c',
        'd', 'd', 'd', 'd', 'd',
        'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e',
        'f', 'f', 'f',
        'g', 'g', 'g', 'g','g',
        'h', 'h', 'h', 'h',
        'i', 'i', 'i', 'i', 'i', 'i', 'i', 'i',
        'j', 'j',
        'k', 'k',
        'l', 'l', 'l', 'l', 'l',
        'm', 'm', 'm',
        'n', 'n', 'n', 'n', 'n', 'n', 'n',
        'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o',
        'p', 'p', 'p', 'p', 'p',
        'q',
        'r', 'r', 'r', 'r', 'r', 'r', 'r',
        's', 's', 's', 's', 's', 's', 's',
        't', 't', 't', 't', 't', 't', 't', 't',
        'u', 'u', 'u', 'u',
        'v',
        'w', 'w', 'w',
        'x',
        'y', 'y', 'y',
        'z', 'z'
    };

    // Properties

    public static Random Random => _random;
    public static List<char> InPlayLettersPool { get => _inPlayLettersPool; set => _inPlayLettersPool = value; }
    public static List<char> AvailableLettersPool { get => _availableLettersPool; set => _availableLettersPool = value; }
    public static List<char> AvailableLettersPoolTemplate => _availableLettersPoolTemplate;

    // Class Methods

    public static void AddLettersToAvailableLettersPool(string word) {
        List<char> charList = word.ToCharArray().ToList();
        foreach (char letter in charList) {
            AvailableLettersPool.Add(letter);
        }
    }

    public static void AddLetterToInPlayLettersPool(char letter) {
        InPlayLettersPool.Add(letter);
    }

    private static bool CheckForLetterRestrictions(char letterCandidate)
    {
        int totalLetters = InPlayLettersPool.Count;
        bool letterCandidateIsVowel = IsVowel(letterCandidate);

        if (totalLetters < 3) {
            return true;
        }
            
        int amountOfThatLetterInPlay = CountOccurrences(InPlayLettersPool, letterCandidate);
        float totalVowels = CountVowels(InPlayLettersPool);
        float totalConsonants = totalLetters - totalVowels;

        if (ExceedsLetterLimit(totalLetters, amountOfThatLetterInPlay)) {
            return false;
        }
            
        if (ExceedsVowelThreshold(totalLetters, totalVowels) && letterCandidateIsVowel) {
            return false;
        }
            
        if (ExceedsConsonantThreshold(totalLetters, totalConsonants) && !letterCandidateIsVowel) {
            return false;
        }
            
        return true;
    }

    private static int CountOccurrences(IEnumerable<char> letters, char target)
    {
        return letters.Count(letter => letter == target);
    }

    private static float CountVowels(IEnumerable<char> letters)
    {
        return letters.Count(IsVowel);
    }

    public static void DeleteLetterFromAvailableLettersPool(char letter) {
        for (int i = 0; i < AvailableLettersPool.Count; i++) {
            if (AvailableLettersPool[i] == letter) {
                AvailableLettersPool.RemoveAt(i);
                return;
            }
        }
    }

    public static void DeleteLettersFromInPlayLettersPool(string word) {
        List<char> charList = word.ToCharArray().ToList();
        foreach (char letter in charList) {
            for (int i = 0; i < InPlayLettersPool.Count; i++) {
            if (InPlayLettersPool[i] == letter) {
                InPlayLettersPool.RemoveAt(i);
                break;
            }
        }
        }
    }

    private static bool ExceedsConsonantThreshold(int totalLetters, float totalConsonants)
    {
        return totalConsonants / totalLetters > 0.85f;
    }

    private static bool ExceedsLetterLimit(int totalLetters, int amountOfThatLetterInPlay)
    {
        if (totalLetters < 10 && amountOfThatLetterInPlay > 1)
            return true;
        if (totalLetters < 15 && amountOfThatLetterInPlay > 2)
            return true;
        if (totalLetters < 20 && amountOfThatLetterInPlay > 3)
            return true;
        if (totalLetters < 25 && amountOfThatLetterInPlay > 4)
            return true;
        return false;
    }

    private static bool ExceedsVowelThreshold(int totalLetters, float totalVowels)
    {
        return totalVowels / totalLetters > 0.6f;
    }

    public static string GenerateLetter() {
    List<char> validLetters = AvailableLettersPool.Where(CheckForLetterRestrictions).ToList();
    char newLetter = '5';
    if (validLetters.Count == 0) {
        newLetter = AvailableLettersPool[Random.Next(0, AvailableLettersPool.Count-1)];
    } else {
        newLetter = validLetters[Random.Next(0, validLetters.Count)];
    }
    DeleteLetterFromAvailableLettersPool(newLetter);
    AddLetterToInPlayLettersPool(newLetter);
    return newLetter.ToString();
}


    public static void InitializeLetters() {
        AvailableLettersPool = new List<char>(AvailableLettersPoolTemplate);
    }

    private static bool IsVowel(char letter)
    {
        return "aeiou".Contains(letter);
    }

    public static void UpdateInPlayLettersPoolAfterShuffle(string newLetters) {
        InPlayLettersPool =  new List<char>(newLetters.ToCharArray());
    }
}
