using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuBoard : MonoBehaviour {

    // Serialized Fields

    [SerializeField]
    private AudioSource _buttonSound;

    // Properties

    public AudioSource ButtonSound { get => _buttonSound; set => _buttonSound = value; }

    // Class Methods

    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void LoadGameScene() {
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
}

