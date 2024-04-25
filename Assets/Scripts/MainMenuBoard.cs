using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuBoard : MonoBehaviour {

    // Serialized Fields

    [SerializeField]
    private AudioSource _buttonSound;
    [SerializeField]
    private GameObject _pointer;
    [SerializeField]
    private GameObject _cPUEasyButton;
    [SerializeField]
    private GameObject _cPUHardButton;
    [SerializeField]
    private GameObject _cPUMediumButton;
    
    // Properties

    public AudioSource ButtonSound { get => _buttonSound; set => _buttonSound = value; }
    public GameObject Pointer { get => _pointer; set => _pointer = value; }
    public GameObject CPUEasyButton { get => _cPUEasyButton; set => _cPUEasyButton = value; }
    public GameObject CPUHardButton { get => _cPUHardButton; set => _cPUHardButton = value; }
    public GameObject CPUMediumButton { get => _cPUMediumButton; set => _cPUMediumButton = value; }

    // Class Methods

    void Start() {
        SetInitialPointerLocation();
    }

    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void LoadGameSceneLocalGame() {
        PlayerPrefs.SetString("GameType", "Local");
        ButtonSound.Play();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
    }

    public void LoadGameSceneVsCPU() {
        PlayerPrefs.SetString("GameType", "CPU");
        ButtonSound.Play();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => ButtonSound.isPlaying);
        SceneManager.LoadScene(scene);
    }

    public void LoadSettingsScene() {
        ButtonSound.Play();
        StartCoroutine(LoadSceneCoroutine("SettingsScene"));
    }

    public void SetCPUToEasy() {
        PlayerPrefs.SetString("Difficulty", "Easy");
        SetNewPointerLocation("easy");
        ButtonSound.Play();
    }

    public void SetCPUToHard() {
        PlayerPrefs.SetString("Difficulty", "Hard");
        SetNewPointerLocation("hard");
        ButtonSound.Play();
    }

    public void SetCPUToMedium() {
        PlayerPrefs.SetString("Difficulty", "Medium");
        SetNewPointerLocation("medium");
        ButtonSound.Play();
    }

    private void SetInitialPointerLocation() {
        switch (PlayerPrefs.GetString("Difficulty", "Medium")) 
        {
            case "Easy":
                Pointer.transform.position = CPUEasyButton.transform.position;
                break;
            case "Medium":
                Pointer.transform.position = CPUMediumButton.transform.position;
                break;
            case "Hard":
                Pointer.transform.position = CPUHardButton.transform.position;
                break;
        }
        Pointer.transform.Translate(new Vector3(0, 80, 0));
    }

    private void SetNewPointerLocation(string difficulty) {
        switch (difficulty) 
        {
            case "easy":
                Pointer.transform.position = CPUEasyButton.transform.position;
                break;
            case "medium":
                Pointer.transform.position = CPUMediumButton.transform.position;
                break;
            case "hard":
                Pointer.transform.position = CPUHardButton.transform.position;
                break;
        }
        Pointer.transform.Translate(new Vector3(0, 80, 0));
    }
}

