using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SettingsBoard : MonoBehaviour
{
    // Serialized Fields

    [SerializeField]
    private AudioSource _applySettingsNoise;
    [SerializeField]
    private AudioSource _chooseSettingsNoise;
    [SerializeField]
    private AudioSource _mainMenuNoise;
    [SerializeField]
    private Image _colorResultTeam1;
    [SerializeField]
    private Image _colorResultTeam2;
    [SerializeField]
    private GameObject _unsavedPopup;
    [SerializeField]
    private GameObject _team1IconShowcase;
    [SerializeField]
    private GameObject _team2IconShowcase;
    [SerializeField]
    private GameObject _pointer;
    [SerializeField]
    private GameObject _smallBoardButton;
    [SerializeField]
    private GameObject _mediumBoardButton;
    [SerializeField]
    private GameObject _largeBoardButton;

    // Fields

    private bool unsavedChanges = false;
    private string _currentTeam1Icon;
    private string _currentTeam2Icon;

    // Properties 

    public Image ColorResultTeam1 { get => ColorResultTeam11; set => ColorResultTeam11 = value; }
    public Image ColorResultTeam2 { get => ColorResultTeam21; set => ColorResultTeam21 = value; }
    public bool UnsavedChanges { get => unsavedChanges; set => unsavedChanges = value; }
    public string SelectedBoardSize { get; private set; }
    public AudioSource ApplySettingsNoise { get => _applySettingsNoise; set => _applySettingsNoise = value; }
    public AudioSource ChooseSettingsNoise { get => _chooseSettingsNoise; set => _chooseSettingsNoise = value; }
    public AudioSource MainMenuNoise { get => _mainMenuNoise; set => _mainMenuNoise = value; }
    public Image ColorResultTeam11 { get => _colorResultTeam1; set => _colorResultTeam1 = value; }
    public Image ColorResultTeam21 { get => _colorResultTeam2; set => _colorResultTeam2 = value; }
    public GameObject UnsavedPopup { get => _unsavedPopup; set => _unsavedPopup = value; }
    public GameObject Team1IconShowcase { get => _team1IconShowcase; set => _team1IconShowcase = value; }
    public GameObject Team2IconShowcase { get => _team2IconShowcase; set => _team2IconShowcase = value; }
    public GameObject Pointer { get => _pointer; set => _pointer = value; }
    public GameObject SmallBoardButton { get => _smallBoardButton; set => _smallBoardButton = value; }
    public GameObject MediumBoardButton { get => _mediumBoardButton; set => _mediumBoardButton = value; }
    public GameObject LargeBoardButton { get => _largeBoardButton; set => _largeBoardButton = value; }
    public string CurrentTeam2Icon { get => _currentTeam2Icon; set => _currentTeam2Icon = value; }
    public string CurrentTeam1Icon { get => _currentTeam1Icon; set => _currentTeam1Icon = value; }

    // Class Methods

    void Start() {
        InitializeColors();
        SetInitialPointerLocation();
    }

    public void ApplySettings() {
        SaveColour();
        SaveBoardSize();
        SaveIcon();
        PlayerPrefs.Save();
        UnsavedChanges = false;
        ApplySettingsNoise.Play();
    }

    public void AttemptToCloseSettings() {
        if (UnsavedChanges == false){
            LoadMenuScene();
        } else{
            UnsavedPopup.SetActive(true);
        }
    }

    private Color CreateNewColor(Color colorResultTeam1, float saturationDelta, float lightnessDelta) {
        Color.RGBToHSV(colorResultTeam1, out float h, out float s, out float l);
        s = Mathf.Clamp01(s + saturationDelta);
        l = Mathf.Clamp01(l + lightnessDelta);
        Color modifiedColor = Color.HSVToRGB(h, s, l);
        return modifiedColor;
    }

    private void InitializeColors() {
        string[] colorKeys = {
            "ColorResultTeam1", "ColorResultTeam2"
        };

        foreach (string key in colorKeys)
        {
            string colorString = "#" + PlayerPrefs.GetString(key);
            if (UnityEngine.ColorUtility.TryParseHtmlString(colorString, out Color parsedColor)) {
                switch (key)
                {
                    case "ColorResultTeam1":
                        ColorResultTeam1.color = parsedColor;
                        break;
                    case "ColorResultTeam2":
                        ColorResultTeam2.color = parsedColor;
                        break;
                    default:
                        Debug.LogWarning($"Color key {key} not recognized.");
                        break;
                }
            }
        }
    }

    public void LargeBoard() {
        SelectedBoardSize = "large";
        SetNewPointerLocation("large");
        ChooseSettingsNoise.Play();
        UnsavedChanges = true;
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => MainMenuNoise.isPlaying);
        yield return new WaitWhile(() => ApplySettingsNoise.isPlaying);

        SceneManager.LoadScene(scene);
    }

    public void LoadMenuScene() {
        MainMenuNoise.Play();
        StartCoroutine(LoadSceneCoroutine("MainMenuScene"));
    } 

    public void MakeTeam1Burger() {
        Team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("burger");
        CurrentTeam1Icon = "burger";
        UnsavedChanges = true;
    }

    public void MakeTeam2Burger() {
        Team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("burger");
        CurrentTeam2Icon = "burger";
        UnsavedChanges = true;
    }

    public void MakeTeam1Coffee() {
        Team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("coffee");
        CurrentTeam1Icon = "coffee";
        UnsavedChanges = true;
    }

    public void MakeTeam2Coffee() {
        Team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("coffee");
        CurrentTeam2Icon = "coffee";
        UnsavedChanges = true;
    }
    
    public void MakeTeam1Meat() {
        Team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("meat");
        CurrentTeam1Icon = "meat";
        UnsavedChanges = true;
    }

    public void MakeTeam2Meat() {
        Team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("meat");
        CurrentTeam2Icon = "meat";
        UnsavedChanges = true;
    }

    public void MakeTeam1Apple() {
        Team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        CurrentTeam1Icon = "apple";
        UnsavedChanges = true;
    }

    public void MakeTeam2Apple() {
        Team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        CurrentTeam2Icon = "apple";
        UnsavedChanges = true;
    }

    public void MakeTeam1Bee() {
        Team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        CurrentTeam1Icon = "bee";
        UnsavedChanges = true;
    }

    public void MakeTeam2Bee() {
        Team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        CurrentTeam2Icon = "bee";
        UnsavedChanges = true;
    }

    public void MakeTeam1Cocktail() {
        Team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("cocktail");
        CurrentTeam1Icon = "cocktail";
        UnsavedChanges = true;
    }

    public void MakeTeam2Cocktail() {
        Team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("cocktail");
        CurrentTeam2Icon = "cocktail";
        UnsavedChanges = true;
    }

    public void MediumBoard() {
        SelectedBoardSize = "medium";
        SetNewPointerLocation("medium");
        ChooseSettingsNoise.Play();
        UnsavedChanges = true;
    }

    private void SaveBoardSize() {
        switch (SelectedBoardSize)
        {   
            case "small":
                PlayerPrefs.SetInt("BoardCols", 7);
                PlayerPrefs.SetInt("BoardRows", 9);
                break;
            case "medium":
                PlayerPrefs.SetInt("BoardCols", 9);
                PlayerPrefs.SetInt("BoardRows", 11);
                break;
            case "large":
                PlayerPrefs.SetInt("BoardCols", 11);
                PlayerPrefs.SetInt("BoardRows", 13);
                break;
            default:
                break;
        }
    }

    public void SaveColour() {
        Color newTeam1Pressed = CreateNewColor(ColorResultTeam1.color, -0.4f, 1.2f);
        Color newTeam2Pressed = CreateNewColor(ColorResultTeam2.color, -0.4f, 1.2f);

        PlayerPrefs.SetString("PressedTeam1Color", ColorUtility.ToHtmlStringRGB(newTeam1Pressed));
        PlayerPrefs.SetString("PressedTeam2Color", ColorUtility.ToHtmlStringRGB(newTeam2Pressed));

        PlayerPrefs.SetString("HomeTeam1Color", ColorUtility.ToHtmlStringRGB(ColorResultTeam1.color));
        PlayerPrefs.SetString("HomeTeam2Color", ColorUtility.ToHtmlStringRGB(ColorResultTeam2.color));

        PlayerPrefs.SetString("TerritoryTeam1Color", ColorUtility.ToHtmlStringRGB(ColorResultTeam1.color));
        PlayerPrefs.SetString("TerritoryTeam2Color", ColorUtility.ToHtmlStringRGB(ColorResultTeam2.color));

        PlayerPrefs.SetString("ColorResultTeam1", ColorUtility.ToHtmlStringRGB(ColorResultTeam1.color));
        PlayerPrefs.SetString("ColorResultTeam2", ColorUtility.ToHtmlStringRGB(ColorResultTeam2.color));
    }

    private void SaveIcon() {
        PlayerPrefs.SetString("team1Icon", CurrentTeam1Icon);
        PlayerPrefs.SetString("team2Icon", CurrentTeam2Icon);
    }

    public void SmallBoard() {
        SelectedBoardSize = "small";
        SetNewPointerLocation("small");
        ChooseSettingsNoise.Play();
        UnsavedChanges = true;
    }

    private void SetInitialPointerLocation() {
        switch (PlayerPrefs.GetInt("BoardCols", 9)) 
        {
            case 7:
                Pointer.transform.position = SmallBoardButton.transform.position;
                break;
            case 9:
                Pointer.transform.position = MediumBoardButton.transform.position;
                break;
            case 11:
                Pointer.transform.position = LargeBoardButton.transform.position;
                break;
        }
        Pointer.transform.Translate(new Vector3(0, 80, 0));
    }
    private void SetNewPointerLocation(string boardSize) {
        switch (boardSize) 
        {
            case "small":
                Pointer.transform.position = SmallBoardButton.transform.position;
                break;
            case "medium":
                Pointer.transform.position = MediumBoardButton.transform.position;
                break;
            case "large":
                Pointer.transform.position = LargeBoardButton.transform.position;
                break;
        }
        Pointer.transform.Translate(new Vector3(0, 80, 0));
    }
}
