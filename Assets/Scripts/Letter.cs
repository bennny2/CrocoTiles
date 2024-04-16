using System;
using System.Collections.Generic;
using System.Linq;

public static class Letter
{

    // Fields

    private static readonly Random random = new();
    private static readonly List<char> inPlayLettersPool = new();
    private static List<char> availableLettersPool = new();
    private static readonly List<char> availableLettersPoolTemplate = new()
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

    public static void AddLetterToAvailableLettersPool(string word) {
        DeleteLetterFromInPlayLettersPool(word);
        List<char> charList = word.ToCharArray().ToList();
        foreach (char letter in charList) {
            availableLettersPool.Add(letter);
        }
    }

    public static void AddLetterToInPlayLettersPool(char letter) {
        inPlayLettersPool.Add(letter);
    }

    private static bool CheckForLetterRestrictions(char letterCandidate) {

        
        int totalLetters = inPlayLettersPool.Count;

        if (totalLetters < 4) {
            return true;
        }

        float totalVowels = 0;
        int amountOfThatLetterInPlay = 0;

        foreach (char letter in inPlayLettersPool) {
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
        if (totalVowels / totalLetters > 0.6f) {
            return false;
        }      
        if (totalConsonants / totalLetters > 0.75f) {
            return false;
        }
        
        return true;
    }

    public static void DeleteLetterFromAvailableLettersPool(char letter) {
        for (int i = 0; i < availableLettersPool.Count; i++) {
            if (availableLettersPool[i] == letter) {
                availableLettersPool.RemoveAt(i);
                return;
            }
        }
    }

    public static void DeleteLetterFromInPlayLettersPool(string word) {
        char letter = StringToLetter(word);
        for (int i = 0; i < inPlayLettersPool.Count; i++) {
            if (inPlayLettersPool[i] == letter) {
                inPlayLettersPool.RemoveAt(i);
                return;
            }
        }
    }

    public static string GenerateLetter() {
        bool validLetter = false; 
        char newLetter = '5';

        while (!validLetter) {
            newLetter = availableLettersPool[random.Next(0, availableLettersPool.Count-1)];
            validLetter = CheckForLetterRestrictions(newLetter);
        }
        DeleteLetterFromAvailableLettersPool(newLetter);
        AddLetterToInPlayLettersPool(newLetter);
        return newLetter.ToString();
    }

    public static void InitializeLetters() {
        availableLettersPool = new List<char>(availableLettersPoolTemplate);
    }

    public static char StringToLetter(string word) {
        List<char> charList = word.ToCharArray().ToList();
        foreach (char letter in charList) {
            return letter;
        }
        return '5'; //ignore, removes error, will never reach this code
    }
}
