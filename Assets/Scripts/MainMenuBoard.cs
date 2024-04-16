using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuBoard : MonoBehaviour {

    // Serialized Fields

    [SerializeField]
    private AudioSource buttonSound;

    // Class Methods

    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void LoadGameScene() {
        buttonSound.Play();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => buttonSound.isPlaying);
        SceneManager.LoadScene(scene);
    }

    public void LoadSettingsScene() {
        buttonSound.Play();
        StartCoroutine(LoadSceneCoroutine("SettingsScene"));
    }
}

