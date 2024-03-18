using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Board : MonoBehaviour
{
    // Hexagon States   
    [SerializeField]
    private HexagonStates homeTeam1;
    [SerializeField]
    private HexagonStates homeTeam2;
    [SerializeField]
    private HexagonStates invisible;
    [SerializeField]
    private HexagonStates neutral;
    [SerializeField]
    private HexagonStates pressedTeam1;
    [SerializeField]
    private HexagonStates pressedTeam2;
    [SerializeField]
    private HexagonStates territoryTeam1;
    [SerializeField]
    private HexagonStates territoryTeam2;

    // Serialized Fields
    [SerializeField]
    private AudioSource audioPressed;
    [SerializeField]
    private AudioSource audioUnPressed;
    [SerializeField]
    private AudioSource audioWordSubmit;
    [SerializeField]
    private AudioSource audioWordFailed;
    [SerializeField]
    private AudioSource audioVictory;
    [SerializeField]
    private AudioSource chooseSettingsNoise;
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private GameObject winnerBlock;
    [SerializeField]
    private GameObject hexagonPrefab;
    [SerializeField]
    private Transform boardTransform;
    [SerializeField]
    
    
    //private CurrentWord currentWordObjectOnScreen; 


    // Fields
    private TextMeshProUGUI winnerBlockText;
    private List<Hexagon> allHexagons;
    private bool bonusTurnActive;    
    private List<string> listOfLettersPressed = new();


    //private SpellCheck spellCheck = new();


    private bool team1Turn = true;
    
    // Properties
    public List<Hexagon> AllHexagons
    {
        get => allHexagons;
        set => allHexagons = value;
    }
    public bool BonusTurnActive
    {
        get => bonusTurnActive;
        set => bonusTurnActive = value;
    }

    /*
    public CurrentWord CurrentWordObjectOnScreen
    {
        get => currentWordObjectOnScreen;
        set => currentWordObjectOnScreen = value;
    }
    */

    public List<string> ListOfLettersPressed 
    {
        get => listOfLettersPressed;
        set => listOfLettersPressed = value;
    }

    /*
    public SpellCheck SpellCheck => spellCheck; 
    */
    public bool Team1Turn
    {
        get => team1Turn;
        set => team1Turn = value;
    }        

    // Hexagon State Properties
    public HexagonStates HomeTeam1 => homeTeam1;
    public HexagonStates HomeTeam2 => homeTeam2;
    public HexagonStates Invisible => invisible;
    public HexagonStates Neutral => neutral;
    public HexagonStates PressedTeam1 => pressedTeam1;
    public HexagonStates PressedTeam2 => pressedTeam2;
    public HexagonStates TerritoryTeam1 => territoryTeam1;
    public HexagonStates TerritoryTeam2 => territoryTeam2;

}
