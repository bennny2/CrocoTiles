using UnityEngine;
using TMPro;
using Photon.Pun;

public class CurrentWord : MonoBehaviour, IPunObservable
{
    // Serialized Fields

    [SerializeField]
    private TextMeshProUGUI _currentWordText;

    // Properties

    public TextMeshProUGUI CurrentWordText => _currentWordText;

    // Class Methods

    private void Awake() {
        InitializeComponents();
    }

    private void InitializeComponents() {
        _currentWordText = GetComponentInChildren<TextMeshProUGUI>();
        if (_currentWordText == null) {
            Debug.LogError("TextMeshProUGUI component not found in children.", this);
        }
    }

    public void UpdateCurrentWord(string word) {
        if (CurrentWordText != null) {
            CurrentWordText.text = word;
        }
        else {
            Debug.LogError("TextMeshProUGUI component not initialized.", this);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Writing data: send the data of hexagon to the stream
            string currentWordString = CurrentWordText.text;
            stream.SendNext(currentWordString);
        }
        else
        {
            // Reading data: receive the data of hexagon from the stream
            CurrentWordText.text = (string)stream.ReceiveNext();
        }
    }
}