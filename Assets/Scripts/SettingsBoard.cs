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

    public bool _unsavedChanges = false;
    private string _currentTeam1Icon;
    private string _currentTeam2Icon;

    // Properties 

    public Image ColorResultTeam1
    {
        get => _colorResultTeam1;
        set => _colorResultTeam1 = value;
    }
    public Image ColorResultTeam2
    {
        get => _colorResultTeam2;
        set => _colorResultTeam2 = value;
    }
    public string SelectedBoardSize { get; private set; }

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
        _unsavedChanges = false;
        _applySettingsNoise.Play();
    }

    public void AttemptToCloseSettings() {
        if (_unsavedChanges == false){
            LoadMenuScene();
        } else{
            _unsavedPopup.SetActive(true);
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
        _chooseSettingsNoise.Play();
        _unsavedChanges = true;
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => _mainMenuNoise.isPlaying);
        yield return new WaitWhile(() => _applySettingsNoise.isPlaying);

        SceneManager.LoadScene(scene);
    }

    public void LoadMenuScene() {
        _mainMenuNoise.Play();
        StartCoroutine(LoadSceneCoroutine("MainMenuScene"));
    } 

    public void MakeTeam1Burger() {
        _team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("burger");
        _currentTeam1Icon = "burger";
        _unsavedChanges = true;
    }

    public void MakeTeam2Burger() {
        _team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("burger");
        _currentTeam2Icon = "burger";
        _unsavedChanges = true;
    }

    public void MakeTeam1Coffee() {
        _team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("coffee");
        _currentTeam1Icon = "coffee";
        _unsavedChanges = true;
    }

    public void MakeTeam2Coffee() {
        _team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("coffee");
        _currentTeam2Icon = "coffee";
        _unsavedChanges = true;
    }
    
    public void MakeTeam1Meat() {
        _team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("meat");
        _currentTeam1Icon = "meat";
        _unsavedChanges = true;
    }

    public void MakeTeam2Meat() {
        _team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("meat");
        _currentTeam2Icon = "meat";
        _unsavedChanges = true;
    }

    public void MakeTeam1Apple() {
        _team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        _currentTeam1Icon = "apple";
        _unsavedChanges = true;
    }

    public void MakeTeam2Apple() {
        _team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        _currentTeam2Icon = "apple";
        _unsavedChanges = true;
    }

    public void MakeTeam1Bee() {
        _team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        _currentTeam1Icon = "bee";
        _unsavedChanges = true;
    }

    public void MakeTeam2Bee() {
        _team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        _currentTeam2Icon = "bee";
        _unsavedChanges = true;
    }

    public void MakeTeam1Cocktail() {
        _team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("cocktail");
        _currentTeam1Icon = "cocktail";
        _unsavedChanges = true;
    }

    public void MakeTeam2Cocktail() {
        _team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("cocktail");
        _currentTeam2Icon = "cocktail";
        _unsavedChanges = true;
    }

    public void MediumBoard() {
        SelectedBoardSize = "medium";
        SetNewPointerLocation("medium");
        _chooseSettingsNoise.Play();
        _unsavedChanges = true;
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
        PlayerPrefs.SetString("team1Icon", _currentTeam1Icon);
        PlayerPrefs.SetString("team2Icon", _currentTeam2Icon);
    }

    public void SmallBoard() {
        SelectedBoardSize = "small";
        SetNewPointerLocation("small");
        _chooseSettingsNoise.Play();
        _unsavedChanges = true;
    }

    private void SetInitialPointerLocation() {
        switch (PlayerPrefs.GetInt("BoardCols", 9)) 
        {
            case 7:
                _pointer.transform.position = _smallBoardButton.transform.position;
                break;
            case 9:
                _pointer.transform.position = _mediumBoardButton.transform.position;
                break;
            case 11:
                _pointer.transform.position = _largeBoardButton.transform.position;
                break;
        }
        _pointer.transform.Translate(new Vector3(0, 80, 0));
    }
    private void SetNewPointerLocation(string boardSize) {
        switch (boardSize) 
        {
            case "small":
                _pointer.transform.position = _smallBoardButton.transform.position;
                break;
            case "medium":
                _pointer.transform.position = _mediumBoardButton.transform.position;
                break;
            case "large":
                _pointer.transform.position = _largeBoardButton.transform.position;
                break;
        }
        _pointer.transform.Translate(new Vector3(0, 80, 0));
    }
}
