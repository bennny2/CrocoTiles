using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuBoard : MonoBehaviour {

    // Serialized Fields

    [SerializeField]
    private AudioSource _buttonSound;

    // Class Methods

    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void LoadGameScene() {
        _buttonSound.Play();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => _buttonSound.isPlaying);
        SceneManager.LoadScene(scene);
    }

    public void LoadSettingsScene() {
        _buttonSound.Play();
        StartCoroutine(LoadSceneCoroutine("SettingsScene"));
    }
}

