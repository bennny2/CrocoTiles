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
    private AudioSource audioClearWord;
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
    private CurrentWord currentWordObjectOnScreen;
    [SerializeField]
    private TextMeshProUGUI winnerBlockText;
    [SerializeField]
    private GameObject team1Icon;
    [SerializeField]
    private GameObject team2Icon;
    [SerializeField]
    private GameObject bonusReminder;
    [SerializeField]
    private GameObject autoShufflePopup;

    // Fields

    private List<Hexagon> allHexagons;
    private bool bonusTurnActive;    
    private List<string> listOfLettersPressed = new();
    private Trie trie = new();
    private bool team1Turn = true;
    private int shuffleCounter = 0;
    
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

    
    public CurrentWord CurrentWordObjectOnScreen
    {
        get => currentWordObjectOnScreen;
        set => currentWordObjectOnScreen = value;
    }
    

    public List<string> ListOfLettersPressed 
    {
        get => listOfLettersPressed;
        set => listOfLettersPressed = value;
    }
    
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

    // Class Methods

    void Awake() {
        InitializeHexagonsOnBoard(); 
        InitilizeComponents();
    }

    void Start() {
        InitializeColors();
        Letter.InitializeLetters();
        MakeAllHexagonsInvisible();
        SetHomeBases();
        SetCameraZoomAndPosition();
        LoadDictionary();
        ProcessGlowingHexagons();
        Loadicons();
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            ClearWord();
        }
    }

    private void ChangeTurn() { 
        if (Team1Turn){
            Team1Turn = false;
        } else{
            Team1Turn = true;
        }
    }

    private void CheckIfAllLettersOnBoardShouldBeShuffled() {
        if (shuffleCounter < 10) {
            shuffleCounter += 1;
        } else {
            StartCoroutine(ShufflePopup2Seconds());
            shuffleCounter = 0;
        }
    }

    private void CheckBoardIsPlayable() {
        if (!trie.CanFormValidWord(AllHexagons)) {
            StartCoroutine(ShufflePopup2Seconds());
        }
    }

    private void CheckBonusTurn() {
        if (BonusTurnActive == true) {
            ChangeTurn();
            BonusTurnActive = false;

        }
    }

    private void CheckHomesAreSet() {
        bool noHomeTeam1 = true;
        bool noHomeTeam2 = true;
        foreach (Hexagon hex in AllHexagons) {
            if (hex.HexagonCurrentState == "homeTeam1") {
                noHomeTeam1 = false;
            }
            if (hex.HexagonCurrentState == "homeTeam2") {
                noHomeTeam2 = false;
            }
        }
        if (noHomeTeam1 == true) {
            SetNewHome("homeTeam1");
        }
        if (noHomeTeam2 == true) {
            SetNewHome("homeTeam2");
        }
    }

    private void ClearHexagon(Hexagon hex) {
        hex.DeleteLetter();
        hex.SetHexagonState(Neutral);
    }

    private void ClearInvalidHexagon(Hexagon hex) {
        hex.SetHexagonState(Neutral);
    }

    private void ClearLetters() {
        foreach (Hexagon hex in AllHexagons) {
            hex.DeleteLetter();
        }
    }

    public void ClearPressedHexagonsInvalidWord() {
        foreach (Hexagon hex in allHexagons)
        {
            if (IsHexagonPressed(hex))
            {
                ClearInvalidHexagon(hex);
            }
        }
    }

    private void ClearPressedHexagonsValidWord() {
        foreach (Hexagon hex in allHexagons)
        {
            if (IsHexagonPressed(hex))
            {
                ClearHexagon(hex);
            }
        }
    }

    public void ClearWord() {
        if (!string.IsNullOrWhiteSpace(CurrentWordObjectOnScreen.CurrentWordText.text)) {
            ResetWordState();
            audioClearWord.Play();
            ClearPressedHexagonsInvalidWord();
            ProcessGlowingHexagons();
        }
    }

    public void CreateHexagon(Vector3 position) {
        GameObject newHexagonObject = Instantiate(hexagonPrefab, boardTransform);
        newHexagonObject.transform.SetLocalPositionAndRotation(position, Quaternion.identity);
        Hexagon newHexagon = newHexagonObject.GetComponent<Hexagon>();
        newHexagon.HexagonX = position.x;
        newHexagon.HexagonY = position.y;
    }

    private void CurrentWordRemoveMostRecentLetter(string letter){
        ListOfLettersPressed.Reverse();
        ListOfLettersPressed.Remove(letter);
        ListOfLettersPressed.Reverse();
        string word = string.Join("", ListOfLettersPressed);
        CurrentWordObjectOnScreen.UpdateCurrentWord(word);
    }

    private void CurrentWordAddNewLetter(string letter) {
        ListOfLettersPressed.Add(letter);
        string word = string.Join("", ListOfLettersPressed);
        CurrentWordObjectOnScreen.UpdateCurrentWord(word);
    }

    private HexagonStates GetCurrentTeam() {
        return Team1Turn ? PressedTeam1 : PressedTeam2;
    }

    private string GetOpponentHomeState(string hexState) {
        return team1Turn ? "homeTeam2" : "homeTeam1";
    }

    private void HasWon() {
        bonusReminder.SetActive(false);
        if (!team1Turn) {
            winnerBlockText.text = "Team 1 has Won!";
        } else {
            winnerBlockText.text = "Team 2 has Won!";
        }
        audioVictory.Play();
        winnerBlock.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        ChangeTurn();
    }

    public void HexagonPressed(Hexagon hex) {
        string hexState = hex.HexagonCurrentState;
        if (hexState == "neutral") {
            audioPressed.Play();
            CurrentWordAddNewLetter(hex.HexagonText.text);
            hex.SetHexagonState(GetCurrentTeam());

        } else if (hexState == "pressedTeam1" || hexState == "pressedTeam2") {
            audioUnPressed.Play();
            CurrentWordRemoveMostRecentLetter(hex.HexagonText.text);
            hex.SetHexagonState(Neutral);

        } else {
            //if state is home/territory/invisible then do nothing
        }
        ProcessGlowingHexagons();
    }

    private bool IsHexagonPressed(Hexagon hex) {
        return hex.HexagonCurrentState == "pressedTeam1" || hex.HexagonCurrentState == "pressedTeam2";
    }

    private bool IsHomeState(string hexState) {
        return hexState == "homeTeam1" || hexState == "homeTeam2";
    }

    private void InitializeColors() {
        string[] colorKeys = {
            "PressedTeam1Color", "PressedTeam2Color",
            "HomeTeam1Color", "HomeTeam2Color",
            "TerritoryTeam1Color", "TerritoryTeam2Color"
        };

        foreach (string key in colorKeys)
        {
            string colorString = "#" + PlayerPrefs.GetString(key);

            if (UnityEngine.ColorUtility.TryParseHtmlString(colorString, out Color parsedColor))
            {
                switch (key)
                {
                    case "PressedTeam1Color":
                        PressedTeam1.FillColor = parsedColor;
                        break;
                    case "PressedTeam2Color":
                        PressedTeam2.FillColor = parsedColor;
                        break;
                    case "HomeTeam1Color":
                        HomeTeam1.FillColor = parsedColor;
                        break;
                    case "HomeTeam2Color":
                        HomeTeam2.FillColor = parsedColor;
                        break;
                    case "TerritoryTeam1Color":
                        TerritoryTeam1.FillColor = parsedColor;
                        break;
                    case "TerritoryTeam2Color":
                        TerritoryTeam2.FillColor = parsedColor;
                        break;
                    default:
                        Debug.LogWarning($"Color key {key} not recognized.");
                        break;
                }
            }
        }
    }

    private void InitializeHexagonsOnBoard() {
        float boardCols = PlayerPrefs.GetInt("BoardCols", 9);
        float boardRows = PlayerPrefs.GetInt("BoardRows", 11);

        float x = 0;
        float y = 0;

        for (int i = 0; i < boardRows; i++) {
            if (i % 2 == 0) {
                //create short row
                for (int k = 1; k <= (int)Math.Floor((double)boardCols / 2); k++) {
                    x = k * Hexagon.HORIZONTALOFFSET * 2;
                    y = i * Hexagon.VERTICALOFFSET / 2;
                    Vector3 position = new(x - 350, y - 200);
                    CreateHexagon(position);
                }
            } else {
                //create long row
                for (int k = 1; k <= (int)Math.Ceiling((double)boardCols / 2); k++) {
                    x = k * Hexagon.HORIZONTALOFFSET * 2 - Hexagon.HORIZONTALOFFSET;
                    y = i * (Hexagon.VERTICALOFFSET / 2);
                    Vector3 position = new(x - 350, y - 200);
                    CreateHexagon(position);
                }
            }
        }    
    }

    private void InitilizeComponents() {
        AllHexagons = GetComponentsInChildren<Hexagon>().ToList();
        InitializeWinnerBlock();
    }

    private void InitializeWinnerBlock() {
        RectTransform winnerBlockRect = winnerBlock.GetComponent<RectTransform>();
        winnerBlockRect.SetAsLastSibling();
    }

    private void LoadDictionary() {
        TextAsset[] dictionaries = Resources.LoadAll<TextAsset>("collins");
        foreach (TextAsset dictionary in dictionaries)
        {
            string[] words = dictionary.text.Split('\n');
            foreach (string word in words)
            {
                trie.Insert(word.ToLower().Trim());
            }
        }
    }

    private void Loadicons(){
        team1Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("team1Icon"));
        team2Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("team2Icon"));

        if (team1Icon.GetComponent<Image>().sprite == null){
            team1Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        }
        if (team2Icon.GetComponent<Image>().sprite == null){
            team2Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        }
    }

    public void LoadMainMenu() {
        chooseSettingsNoise.Play();
        StartCoroutine(LoadSceneCoroutine("MainMenuScene"));
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => chooseSettingsNoise.isPlaying);
        SceneManager.LoadScene(scene);
    }

    private void MakeAllHexagonsInvisible(){
        foreach (Hexagon hex in AllHexagons){ 
            hex.SetHexagonState(Invisible);
        }
    }

    public void MakePressedHexagonsTerritory(Hexagon hex) {
        string hexState = hex.HexagonCurrentState;

        if (hexState == "territoryTeam1" || hexState == "homeTeam1") {
            ProcessHexagonTerritory(hex, "Team1");
        }
        else if (hexState == "territoryTeam2" || hexState == "homeTeam2") {
            ProcessHexagonTerritory(hex, "Team2");
        }
    }

    public void MakeTouchingHexagonsNeutral(List<Hexagon> touchingHexagonsArray) {
        foreach (Hexagon touchingHexagon in touchingHexagonsArray)
        {
            string hexState = touchingHexagon.HexagonCurrentState;

            if ((team1Turn && ShouldMakeNeutralForTeam1(hexState)) || (!team1Turn && ShouldMakeNeutralForTeam2(hexState)))
            {
                touchingHexagon.SetHexagonState(Neutral);

                if (IsHomeState(hexState))
                {
                    //disable icon for the opposite team
                    if (team1Turn) {
                        team2Icon.SetActive(false);
                    } else {
                        team1Icon.SetActive(false);
                    }

                    bonusReminder.SetActive(true);
                    BonusTurnActive = true;
                    touchingHexagon.SetLetter();
                    SetNewHome(GetOpponentHomeState(hexState));
                }
            }
        }
    }

    private void PlayAgain() {
        winnerBlock.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        Letter.InitializeLetters();
        ClearLetters();
        MakeAllHexagonsInvisible();
        SetHomeBases();
        ProcessGlowingHexagons();
        SetCameraZoomAndPosition();
        Team1Turn = true;
    }

    private void ProcessGlowingHexagons() {
        foreach (Hexagon hex in AllHexagons)
        {
            bool isTeam1Hex = hex.HexagonCurrentState == "territoryTeam1" || hex.HexagonCurrentState == "homeTeam1";
            bool isTeam2Hex = hex.HexagonCurrentState == "territoryTeam2" || hex.HexagonCurrentState == "homeTeam2";
            bool shouldGlow = (Team1Turn && isTeam1Hex) || (!Team1Turn && isTeam2Hex);
            
            if (hex.HexagonImage.material == null || hex.HexagonImage.material.shader.name != "Custom/GlowPulseShader")
            {
                hex.HexagonImage.material = new Material(Shader.Find("Custom/GlowPulseShader"));
            }

            hex.HexagonImage.material.color = hex.HexagonImage.color;
            hex.HexagonImage.material.SetFloat("_GlowStrength", shouldGlow ? 1.0f : 0.0f); 
            hex.HexagonImage.material.SetFloat("_PulseSpeed", shouldGlow ? 3.0f : 0.0f); 
        }
    }

    private void ProcessHexagonTerritory(Hexagon hex, string team) {
        List<Hexagon> touchingHexes = hex.FindTouchingHexagons();
        foreach (Hexagon touchingHex in touchingHexes)
        {
            string touchingHexState = touchingHex.HexagonCurrentState;
            if (touchingHexState == $"pressed{team}") {
                ProcessTouchingHex(touchingHex, team);
            }
        }
    }

    private void ProcessTouchingHex(Hexagon touchingHex, string team) {
        touchingHex.DeleteLetter();
        if (team == "Team1") {
            touchingHex.SetHexagonState(territoryTeam1);
        } else {
            touchingHex.SetHexagonState(territoryTeam2);
        }
        
        List<Hexagon> touchingHexagonsArray = touchingHex.FindTouchingHexagons();
        MakeTouchingHexagonsNeutral(touchingHexagonsArray);

        MakePressedHexagonsTerritory(touchingHex);
    }

    private void ProcessValidWord() {
        foreach (Hexagon hex in AllHexagons)
        {
            MakePressedHexagonsTerritory(hex);
        }
        Letter.AddLetterToAvailableLettersPool(CurrentWordObjectOnScreen.CurrentWordText.text);
        ClearPressedHexagonsValidWord();
        ChangeTurn();
        ResetWordState();
        CheckBoardIsPlayable();
        CheckHomesAreSet();
        CheckBonusTurn();
        audioWordSubmit.Play();
        ProcessGlowingHexagons();
        CheckIfAllLettersOnBoardShouldBeShuffled();
    }

    private void ProcessInvalidWord() {
        ClearPressedHexagonsInvalidWord();
        ResetWordState();
        audioWordFailed.Play();
    }

    public void ResetWordState() {
        CurrentWordObjectOnScreen.UpdateCurrentWord("");
        ListOfLettersPressed.Clear();
    }

    private void SetCameraZoomAndPosition() {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        float boardCols = PlayerPrefs.GetInt("BoardCols", 9);
        float camSize = 2.9f;
        Vector3 camMove = new(0f, 0f);

        switch (boardCols) {
            case 7:
                camSize = 2.6f;
                camMove = new(-0.65f, -0.35f);
                break;
            case 9:
                camSize = 2.9f;
                camMove = new(0f, 0f);
                break;
            case 11:
                camSize = 3.2f;
                camMove = new(0.65f, 0.35f);
                break;
        }

        Camera.main.orthographicSize = camSize;
        Camera.main.transform.Translate(camMove);
    }

    private void SetHomeBases() {

        //the math only works if there are two more rows than columns in the board eg. 7 cols and 9 rows
        int boardCols = PlayerPrefs.GetInt("BoardCols", 9);
        int base1, base2;

        int halfBoardColsRoundedUp = (int)Math.Ceiling((double)boardCols / 2);
        int halfBoardColsRoundedDown = (int)Math.Floor((double)boardCols / 2);
        int totalHexes = halfBoardColsRoundedUp * halfBoardColsRoundedUp * 2;

        base1 = boardCols + halfBoardColsRoundedDown;
        base2 = totalHexes - base1;

        AllHexagons[base1 - 1].SetHexagonState(HomeTeam1);
        AllHexagons[base2 - 1].SetHexagonState(HomeTeam2);

        team1Icon.SetActive(true);
        team2Icon.SetActive(true);

        team1Icon.transform.position = new Vector2(AllHexagons[base1 - 1].HexagonX / 108f, AllHexagons[base1 - 1].HexagonY / 108f);
        team2Icon.transform.position = new Vector2(AllHexagons[base2 - 1].HexagonX / 108f, AllHexagons[base2 - 1].HexagonY / 108f);
    }

    private void SetHexagonStateForTeam(Hexagon hex, string team) {
        HexagonStates homeTeam = team == "homeTeam2" ? HomeTeam2 : HomeTeam1;
        hex.SetHexagonState(homeTeam);
    }

    private void SetNewHome(string team) {
        Hexagon hex = SelectRandomHexagonOfType($"territory{team[4..]}");
        if (hex != null && !BonusTurnActive) {
            ChangeTurn(); // Change turn before setting new home
            SetHexagonStateForTeam(hex, team);
            UpdateTeamIconPosition(hex, team);
            ChangeTurn(); // Change turn after setting new home
        }
    }

    private Hexagon SelectRandomHexagonOfType(string state){
        List<Hexagon> targetHexes = AllHexagons.Where(hex => hex.HexagonCurrentState == state).ToList();
        if (targetHexes.Count == 0) {
            HasWon();
            return null;
        } else {
            return targetHexes[UnityEngine.Random.Range(0, targetHexes.Count)];
        }
    }

    IEnumerator ShufflePopup2Seconds() {
        autoShufflePopup.SetActive(true);
        yield return new WaitForSeconds(2);
        ShuffleLetters();
        autoShufflePopup.SetActive(false);
    }
    private bool ShouldMakeNeutralForTeam1(string hexState) {
        return hexState != "homeTeam1" && hexState != "territoryTeam1" && hexState != "pressedTeam1";
    }

    private bool ShouldMakeNeutralForTeam2(string hexState) {
        return hexState != "homeTeam2" && hexState != "territoryTeam2" && hexState != "pressedTeam2";
    }

    private void ShuffleLetters() {
        foreach (Hexagon hex in AllHexagons) {
            if (hex.HexagonCurrentState == "neutral") {
                hex.SetLetter();
            }
        }
        Debug.Log("SHUFFLING");
        CheckBoardIsPlayable();
    }

    public void SubmitButtonPressed() {

        if (trie.Search(CurrentWordObjectOnScreen.CurrentWordText.text.ToLower()))
        {
            ProcessValidWord();
        }
        else
        {
            ProcessInvalidWord();
        }
    }

    private void UpdateTeamIconPosition(Hexagon hex, string team) {
        GameObject teamIcon = team == "homeTeam2" ? team2Icon : team1Icon;
        teamIcon.SetActive(true);
        teamIcon.transform.position = new Vector2(hex.HexagonX / 108f, hex.HexagonY / 108f);
    }
}