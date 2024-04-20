using UnityEngine;
using TMPro;

public class CurrentWord : MonoBehaviour
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
}