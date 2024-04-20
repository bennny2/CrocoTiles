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

    // Class Methods

    public static void AddLettersToAvailableLettersPool(string word) {
        List<char> charList = word.ToCharArray().ToList();
        foreach (char letter in charList) {
            _availableLettersPool.Add(letter);
        }
    }

    public static void AddLetterToInPlayLettersPool(char letter) {
        _inPlayLettersPool.Add(letter);
    }

    private static bool CheckForLetterRestrictions(char letterCandidate) {
        int totalLetters = _inPlayLettersPool.Count;

        if (totalLetters < 3) {
            return true;
        }

        float totalVowels = 0;
        int amountOfThatLetterInPlay = 0;

        foreach (char letter in _inPlayLettersPool) {
            if (letter == letterCandidate) {
                amountOfThatLetterInPlay += 1;
            }
            if (letter == 'a' || letter == 'e' || letter == 'i' || letter == 'o' || letter == 'u') {
                totalVowels += 1;
            }
        }

        float totalConsonants = totalLetters - totalVowels;

        if (letterCandidate == 'a' || letterCandidate == 'e' || letterCandidate == 'i' || letterCandidate == 'o' || letterCandidate == 'u') {
            totalVowels += 1;
        } else {
            totalConsonants += 1;
        }

        if (totalLetters < 10 && amountOfThatLetterInPlay > 1) {
            return false;
        }
        if (totalLetters < 15 && amountOfThatLetterInPlay > 2) {
            return false;
        }
        if (totalLetters < 20 && amountOfThatLetterInPlay > 3) {
            return false;
        } 
        if (totalLetters < 25 && amountOfThatLetterInPlay > 4) {
            return false;
        }
        
        bool tooManyVowels = totalVowels / totalLetters > 0.6f;
        bool tooManyConsonants = totalConsonants / totalLetters > 0.8f;

        if (tooManyVowels) {
            return false;
        } //else if (tooManyConsonants) {
            //return false;
        //}
        return true;
    }

    public static void DeleteLetterFromAvailableLettersPool(char letter) {
        for (int i = 0; i < _availableLettersPool.Count; i++) {
            if (_availableLettersPool[i] == letter) {
                _availableLettersPool.RemoveAt(i);
                return;
            }
        }
    }

    public static void DeleteLettersFromInPlayLettersPool(string word) {
        List<char> charList = word.ToCharArray().ToList();
        foreach (char letter in charList) {
            for (int i = 0; i < _inPlayLettersPool.Count; i++) {
            if (_inPlayLettersPool[i] == letter) {
                _inPlayLettersPool.RemoveAt(i);
                break;
            }
        }
        }
    }

    public static string GenerateLetter() {
        bool validLetter = false; 
        char newLetter = '5';

        while (!validLetter) {
            newLetter = _availableLettersPool[_random.Next(0, _availableLettersPool.Count-1)];
            validLetter = CheckForLetterRestrictions(newLetter);
        }
        DeleteLetterFromAvailableLettersPool(newLetter);
        AddLetterToInPlayLettersPool(newLetter);
        return newLetter.ToString();
    }

    public static void InitializeLetters() {
        _availableLettersPool = new List<char>(_availableLettersPoolTemplate);
    }

    public static void UpdateInPlayLettersPoolAfterShuffle(string newLetters) {
        _inPlayLettersPool =  new List<char>(newLetters.ToCharArray());
    }
}
