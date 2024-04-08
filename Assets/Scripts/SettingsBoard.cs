using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SettingsBoard : MonoBehaviour
{
    //Serialized Fields
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

    //Properties 
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

    //Methods

    void Start() {
        InitializeColors();
    }

    private void InitializeColors() {
        string[] colorKeys = {
            "ColorResultTeam1", "ColorResultTeam2"
        };

        foreach (string key in colorKeys)
        {
            string colorString = "#" + PlayerPrefs.GetString(key);

            if (UnityEngine.ColorUtility.TryParseHtmlString(colorString, out Color parsedColor))
            {
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
    private Color CreateNewColor(Color colorResultTeam1, float saturationDelta, float lightnessDelta) {
        // Convert RGB to HSL
        Color.RGBToHSV(colorResultTeam1, out float h, out float s, out float l);
        
        // Modify saturation and lightness
        s = Mathf.Clamp01(s + saturationDelta);
        l = Mathf.Clamp01(l + lightnessDelta);

        // Convert HSL back to RGB
        Color modifiedColor = Color.HSVToRGB(h, s, l);
        return modifiedColor;
    
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

    }

    void SaveBoardSize() {
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
    
    public void LoadMenuScene() {
        mainMenuNoise.Play();
        StartCoroutine(LoadSceneCoroutine("MainMenuScene"));
    } 

    public void ApplySettings() {
        SaveColour();
        SaveBoardSize();
        PlayerPrefs.Save();
        applySettingsNoise.Play();
    }

    public void SmallBoard() {
        SelectedBoardSize = "small";
        chooseSettingsNoise.Play();
    }

    public void MediumBoard() {
        SelectedBoardSize = "medium";
        chooseSettingsNoise.Play();
    }

    public void LargeBoard() {
        SelectedBoardSize = "large";
        chooseSettingsNoise.Play();
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => mainMenuNoise.isPlaying);
        SceneManager.LoadScene(scene);
    }
}
