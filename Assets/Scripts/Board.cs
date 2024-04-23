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
    private HexagonStates _homeTeam1;
    [SerializeField]
    private HexagonStates _homeTeam2;
    [SerializeField]
    private HexagonStates _invisible;
    [SerializeField]
    private HexagonStates _neutral;
    [SerializeField]
    private HexagonStates _pressedTeam1;
    [SerializeField]
    private HexagonStates _pressedTeam2;
    [SerializeField]
    private HexagonStates _territoryTeam1;
    [SerializeField]
    private HexagonStates _territoryTeam2;

    // Serialized Fields

    [SerializeField]
    private AudioSource _audioClearWord;
    [SerializeField]
    private AudioSource _audioPressed;
    [SerializeField]
    private AudioSource _audioUnPressed;
    [SerializeField]
    private AudioSource _audioWordSubmit;
    [SerializeField]
    private AudioSource _audioWordFailed;
    [SerializeField]
    private AudioSource _audioVictory;
    [SerializeField]
    private AudioSource _chooseSettingsNoise;
    [SerializeField]
    private Button _playAgainButton;
    [SerializeField]
    private Button _quitButton;
    [SerializeField]
    private GameObject _winnerBlock;
    [SerializeField]
    private GameObject _hexagonPrefab;
    [SerializeField]
    private Transform _boardTransform;
    [SerializeField]
    private CurrentWord _currentWordObjectOnScreen;
    [SerializeField]
    private TextMeshProUGUI _winnerBlockText;
    [SerializeField]
    private GameObject _team1Icon;
    [SerializeField]
    private GameObject _team2Icon;
    [SerializeField]
    private GameObject _bonusReminder;
    [SerializeField]
    private GameObject _autoShufflePopup;
    [SerializeField]
    private GameObject _boardInteractionBlocker;

    // Fields

    private List<Hexagon> _allHexagons;
    private bool _bonusTurnActive;    
    private List<string> _listOfLettersPressed = new();
    private Trie _trie = new();
    private bool _team1Turn = true;
    private int _shuffleCounter = 0;

    // Hexagon State Properties

    public HexagonStates HomeTeam1 => _homeTeam1;
    public HexagonStates HomeTeam2 => _homeTeam2;
    public HexagonStates Invisible => _invisible;
    public HexagonStates Neutral => _neutral;
    public HexagonStates PressedTeam1 => _pressedTeam1;
    public HexagonStates PressedTeam2 => _pressedTeam2;
    public HexagonStates TerritoryTeam1 => _territoryTeam1;
    public HexagonStates TerritoryTeam2 => _territoryTeam2;
    
    // Properties

    public List<Hexagon> AllHexagons { get => _allHexagons; set => _allHexagons = value; }
    public bool BonusTurnActive { get => _bonusTurnActive; set => _bonusTurnActive = value; }
    public CurrentWord CurrentWordObjectOnScreen { get => CurrentWordObjectOnScreen1; set => CurrentWordObjectOnScreen1 = value; }
    public List<string> ListOfLettersPressed  { get => _listOfLettersPressed; set => _listOfLettersPressed = value; }
    public bool Team1Turn { get => _team1Turn; set => _team1Turn = value; }        
    public int ShuffleCounter { get => _shuffleCounter; set => _shuffleCounter = value; }
    internal Trie Trie { get => _trie; set => _trie = value; }
    public AudioSource AudioClearWord { get => _audioClearWord; set => _audioClearWord = value; }
    public AudioSource AudioPressed { get => _audioPressed; set => _audioPressed = value; }
    public AudioSource AudioUnPressed { get => _audioUnPressed; set => _audioUnPressed = value; }
    public AudioSource AudioWordSubmit { get => _audioWordSubmit; set => _audioWordSubmit = value; }
    public AudioSource AudioWordFailed { get => _audioWordFailed; set => _audioWordFailed = value; }
    public AudioSource AudioVictory { get => _audioVictory; set => _audioVictory = value; }
    public AudioSource ChooseSettingsNoise { get => _chooseSettingsNoise; set => _chooseSettingsNoise = value; }
    public Button PlayAgainButton { get => _playAgainButton; set => _playAgainButton = value; }
    public Button QuitButton { get => _quitButton; set => _quitButton = value; }
    public GameObject WinnerBlock { get => _winnerBlock; set => _winnerBlock = value; }
    public GameObject HexagonPrefab { get => _hexagonPrefab; set => _hexagonPrefab = value; }
    public Transform BoardTransform { get => _boardTransform; set => _boardTransform = value; }
    public CurrentWord CurrentWordObjectOnScreen1 { get => _currentWordObjectOnScreen; set => _currentWordObjectOnScreen = value; }
    public TextMeshProUGUI WinnerBlockText { get => _winnerBlockText; set => _winnerBlockText = value; }
    public GameObject Team1Icon { get => _team1Icon; set => _team1Icon = value; }
    public GameObject Team2Icon { get => _team2Icon; set => _team2Icon = value; }
    public GameObject BonusReminder { get => _bonusReminder; set => _bonusReminder = value; }
    public GameObject AutoShufflePopup { get => _autoShufflePopup; set => _autoShufflePopup = value; }
    public GameObject BoardInteractionBlocker { get => _boardInteractionBlocker; set => _boardInteractionBlocker = value; }

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

    private void ActivateCPUsTurn() {
        BoardInteractionBlocker.SetActive(true);
        PlayCPUsWordOnBoard(GenerateCPUsWord());
    }

    private void ChangeTurn() { 
        if (Team1Turn){
            Team1Turn = false;
        } else{
            Team1Turn = true;
        }
    }

    private void CheckIfAllLettersOnBoardShouldBeShuffled() {
        if (ShuffleCounter < 10) {
            ShuffleCounter += 1;
        } else {
            StartCoroutine(ShufflePopup2Seconds());
            ShuffleCounter = 0;
        }
    }

    private void CheckBoardIsPlayable() {
        if (!Trie.CanFormValidWord(AllHexagons)) {
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

    private void CheckIfIsCPUTurn() {
        switch (PlayerPrefs.GetString("GameType", "Local")) {   
            case "Local":
                return;
            case "CPU":
                if (!Team1Turn) {
                    ActivateCPUsTurn();
                } else {
                    DeactivateCPUsTurn();
                }
                break;
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
        foreach (Hexagon hex in AllHexagons)
        {
            if (IsHexagonPressed(hex))
            {
                ClearInvalidHexagon(hex);
            }
        }
    }

    private void ClearPressedHexagonsValidWord() {
        foreach (Hexagon hex in AllHexagons)
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
            AudioClearWord.Play();
            ClearPressedHexagonsInvalidWord();
            ProcessGlowingHexagons();
        }
    }

    public void CreateHexagon(Vector3 position) {
        GameObject newHexagonObject = Instantiate(HexagonPrefab, BoardTransform);
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

    private void DeactivateCPUsTurn() {
        BoardInteractionBlocker.SetActive(false);
    }

    private List<Hexagon> GenerateCPUsWord() {
        List<Hexagon> result = new();
        List<Hexagon> neutralHexes = new(AllHexagons.Where(hex => hex.HexagonCurrentState == "neutral"));

        for (int length = 3; length <= 5; length++) {
            IEnumerable<IEnumerable<Hexagon>> hexPermutationsEnumerable = Trie.GetPermutationsWithDuplicates(neutralHexes, length);
            List<List<Hexagon>> hexPermutationsList = hexPermutationsEnumerable
                .Select(innerSequence => innerSequence.ToList()) // Convert each inner sequence to a list
                .ToList(); // Convert the outer sequence to a list

            foreach (List<Hexagon> permutation in hexPermutationsList) {
                string word = "";
                foreach (Hexagon hex in permutation) {
                    word += hex.HexagonText.text;
                }
                if (Trie.Search(word)) {
                    return permutation;
                } 
            }
            Debug.Log("CPU could not find a word to play");
        }

        List<Hexagon> ListOfHexesToPlay = new();
        return ListOfHexesToPlay;
    }

    private HexagonStates GetCurrentTeam() {
        return Team1Turn ? PressedTeam1 : PressedTeam2;
    }

    private string GetOpponentHomeState(string hexState) {
        return Team1Turn ? "homeTeam2" : "homeTeam1";
    }

    private void HasWon() {
        BonusReminder.SetActive(false);
        if (!Team1Turn) {
            WinnerBlockText.text = "Team 1 has Won!";
        } else {
            WinnerBlockText.text = "Team 2 has Won!";
        }
        AudioVictory.Play();
        WinnerBlock.SetActive(true);
        PlayAgainButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
        ChangeTurn();
    }

    public void HexagonPressed(Hexagon hex) {
        string hexState = hex.HexagonCurrentState;
        if (hexState == "neutral") {
            AudioPressed.Play();
            CurrentWordAddNewLetter(hex.HexagonText.text);
            hex.SetHexagonState(GetCurrentTeam());

        } else if (hexState == "pressedTeam1" || hexState == "pressedTeam2") {
            AudioUnPressed.Play();
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
        RectTransform winnerBlockRect = WinnerBlock.GetComponent<RectTransform>();
        winnerBlockRect.SetAsLastSibling();
    }

    private void LoadDictionary() {
        TextAsset[] dictionaries = Resources.LoadAll<TextAsset>("collins");
        foreach (TextAsset dictionary in dictionaries)
        {
            string[] words = dictionary.text.Split('\n');
            foreach (string word in words)
            {
                Trie.Insert(word.ToLower().Trim());
            }
        }
    }

    private void Loadicons(){
        Team1Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("team1Icon"));
        Team2Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("team2Icon"));

        if (Team1Icon.GetComponent<Image>().sprite == null){
            Team1Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        }
        if (Team2Icon.GetComponent<Image>().sprite == null){
            Team2Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        }
    }

    public void LoadMainMenu() {
        ChooseSettingsNoise.Play();
        StartCoroutine(LoadSceneCoroutine("MainMenuScene"));
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => ChooseSettingsNoise.isPlaying);
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

            if ((Team1Turn && ShouldMakeNeutralForTeam1(hexState)) || (!Team1Turn && ShouldMakeNeutralForTeam2(hexState)))
            {
                touchingHexagon.SetHexagonState(Neutral);

                if (IsHomeState(hexState))
                {
                    //disable icon for the opposite team
                    if (Team1Turn) {
                        Team2Icon.SetActive(false);
                    } else {
                        Team1Icon.SetActive(false);
                    }

                    BonusReminder.SetActive(true);
                    BonusTurnActive = true;
                    touchingHexagon.SetLetter();
                    SetNewHome(GetOpponentHomeState(hexState));
                }
            }
        }
    }

    private void PlayAgain() { //referenced by button in game
        WinnerBlock.SetActive(false);
        PlayAgainButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        ShuffleCounter = 0;
        Letter.InitializeLetters();
        ClearLetters();
        MakeAllHexagonsInvisible();
        SetHomeBases();
        ProcessGlowingHexagons();
        SetCameraZoomAndPosition();
        Team1Turn = true;
    }

    private void PlayCPUsWordOnBoard(List<Hexagon> hexes) {
        StartCoroutine(PressHexagonsForCPU(hexes));
    }

    IEnumerator PressHexagonsForCPU(List<Hexagon> hexes) {
        foreach (Hexagon hex in hexes) {
            yield return new WaitForSeconds(1);
            HexagonPressed(hex);
        }
        yield return new WaitForSeconds(1);
        SubmitButtonPressed();
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
            touchingHex.SetHexagonState(TerritoryTeam1);
        } else {
            touchingHex.SetHexagonState(TerritoryTeam2);
        }
        
        List<Hexagon> touchingHexagonsArray = touchingHex.FindTouchingHexagons();
        MakeTouchingHexagonsNeutral(touchingHexagonsArray);

        MakePressedHexagonsTerritory(touchingHex);
    }

    private void ProcessValidWord() {
        Letter.DeleteLettersFromInPlayLettersPool(CurrentWordObjectOnScreen.CurrentWordText.text);
        foreach (Hexagon hex in AllHexagons)
        {
            MakePressedHexagonsTerritory(hex);
        }
        Letter.AddLettersToAvailableLettersPool(CurrentWordObjectOnScreen.CurrentWordText.text);
        ClearPressedHexagonsValidWord();
        ChangeTurn();
        ResetWordState();
        CheckBoardIsPlayable();
        CheckHomesAreSet();
        CheckBonusTurn();
        AudioWordSubmit.Play();
        ProcessGlowingHexagons();
        CheckIfAllLettersOnBoardShouldBeShuffled();
        CheckIfIsCPUTurn();
    }

    private void ProcessInvalidWord() {
        ClearPressedHexagonsInvalidWord();
        ResetWordState();
        AudioWordFailed.Play();
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

        Team1Icon.SetActive(true);
        Team2Icon.SetActive(true);

        Team1Icon.transform.position = new Vector2(AllHexagons[base1 - 1].HexagonX / 108f, AllHexagons[base1 - 1].HexagonY / 108f);
        Team2Icon.transform.position = new Vector2(AllHexagons[base2 - 1].HexagonX / 108f, AllHexagons[base2 - 1].HexagonY / 108f);
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
        AutoShufflePopup.SetActive(true);
        yield return new WaitForSeconds(2);
        ShuffleLetters();
        AutoShufflePopup.SetActive(false);
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
        FindAllLettersOnBoardToUpdateInPlayLettersPool();
    }

    private void FindAllLettersOnBoardToUpdateInPlayLettersPool() {
        string newLettersAfterShuffle = "";
        foreach (Hexagon hex in AllHexagons) {
            if (!string.IsNullOrWhiteSpace(hex.HexagonText.text)) {
                newLettersAfterShuffle += hex.HexagonText.text;
            }
        }
        Letter.UpdateInPlayLettersPoolAfterShuffle(newLettersAfterShuffle);
    }

    public void SubmitButtonPressed() {

        if (Trie.Search(CurrentWordObjectOnScreen.CurrentWordText.text.ToLower()))
        {
            ProcessValidWord();
        }
        else
        {
            ProcessInvalidWord();
        }
    }

    private void UpdateTeamIconPosition(Hexagon hex, string team) {
        GameObject teamIcon = team == "homeTeam2" ? Team2Icon : Team1Icon;
        teamIcon.SetActive(true);
        teamIcon.transform.position = new Vector2(hex.HexagonX / 108f, hex.HexagonY / 108f);
    }
}