using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SettingsBoard : MonoBehaviour
{
    // Serialized Fields

    [SerializeField]
    private AudioSource applySettingsNoise;
    [SerializeField]
    private AudioSource chooseSettingsNoise;
    [SerializeField]
    private AudioSource mainMenuNoise;
    [SerializeField]
    private Image colorResultTeam1;
    [SerializeField]
    private Image colorResultTeam2;
    [SerializeField]
    private GameObject unsavedPopup;
    [SerializeField]
    private GameObject team1IconShowcase;
    [SerializeField]
    private GameObject team2IconShowcase;

    // Fields

    public bool unsavedChanges = false;
    private string currentTeam1Icon;
    private string currentTeam2Icon;

    // Properties 

    public Image ColorResultTeam1
    {
        get => colorResultTeam1;
        set => colorResultTeam1 = value;
    }
    public Image ColorResultTeam2
    {
        get => colorResultTeam2;
        set => colorResultTeam2 = value;
    }
    public string SelectedBoardSize { get; private set; }

    // Class Methods

    void Start() {
        InitializeColors();
    }

    public void ApplySettings() {
        SaveColour();
        SaveBoardSize();
        SaveIcon();
        PlayerPrefs.Save();
        unsavedChanges = false;
        applySettingsNoise.Play();
    }

    public void AttemptToCloseSettings() {
        if (unsavedChanges == false){
            LoadMenuScene();
        } else{
            unsavedPopup.SetActive(true);
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
        chooseSettingsNoise.Play();
        unsavedChanges = true;
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => mainMenuNoise.isPlaying);
        yield return new WaitWhile(() => applySettingsNoise.isPlaying);

        SceneManager.LoadScene(scene);
    }

    public void LoadMenuScene() {
        mainMenuNoise.Play();
        StartCoroutine(LoadSceneCoroutine("MainMenuScene"));
    } 

    public void MakeTeam1Burger() {
        team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("burger");
        currentTeam1Icon = "burger";
        unsavedChanges = true;
    }

    public void MakeTeam2Burger() {
        team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("burger");
        currentTeam2Icon = "burger";
        unsavedChanges = true;
    }

    public void MakeTeam1Coffee() {
        team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("coffee");
        currentTeam1Icon = "coffee";
        unsavedChanges = true;
    }

    public void MakeTeam2Coffee() {
        team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("coffee");
        currentTeam2Icon = "coffee";
        unsavedChanges = true;
    }
    
    public void MakeTeam1Meat() {
        team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("meat");
        currentTeam1Icon = "meat";
        unsavedChanges = true;
    }

    public void MakeTeam2Meat() {
        team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("meat");
        currentTeam2Icon = "meat";
        unsavedChanges = true;
    }

    public void MakeTeam1Apple() {
        team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        currentTeam1Icon = "apple";
        unsavedChanges = true;
    }

    public void MakeTeam2Apple() {
        team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("apple");
        currentTeam2Icon = "apple";
        unsavedChanges = true;
    }

    public void MakeTeam1Bee() {
        team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        currentTeam1Icon = "bee";
        unsavedChanges = true;
    }

    public void MakeTeam2Bee() {
        team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("bee");
        currentTeam2Icon = "bee";
        unsavedChanges = true;
    }

    public void MakeTeam1Cocktail() {
        team1IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("cocktail");
        currentTeam1Icon = "cocktail";
        unsavedChanges = true;
    }

    public void MakeTeam2Cocktail() {
        team2IconShowcase.GetComponent<Image>().sprite = Resources.Load<Sprite>("cocktail");
        currentTeam2Icon = "cocktail";
        unsavedChanges = true;
    }

    public void MediumBoard() {
        SelectedBoardSize = "medium";
        chooseSettingsNoise.Play();
        unsavedChanges = true;
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
        PlayerPrefs.SetString("team1Icon", currentTeam1Icon);
        PlayerPrefs.SetString("team2Icon", currentTeam2Icon);
    }

    public void SmallBoard() {
        SelectedBoardSize = "small";
        chooseSettingsNoise.Play();
        unsavedChanges = true;
    }
}
