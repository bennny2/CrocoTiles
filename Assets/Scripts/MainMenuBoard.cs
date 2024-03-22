using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuBoard : MonoBehaviour {
    //Serialized Fields
    [SerializeField]
    private AudioSource buttonSound;

    // Class Methods
    public void LoadGameScene() {
        buttonSound.Play();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
        
    }

    public void LoadSettingsScene() {
        buttonSound.Play();
        StartCoroutine(LoadSceneCoroutine("SettingsScene"));
        
    }

    private IEnumerator LoadSceneCoroutine(string scene) {
        yield return new WaitWhile(() => buttonSound.isPlaying);
        SceneManager.LoadScene(scene);
    }
}

