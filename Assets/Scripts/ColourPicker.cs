using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour, IPointerClickHandler
{

    // Serialize Fields

    [SerializeField]
    private string _team;

    // Fields

    public Color _output;

    // Properties

    public string Team { get => _team; set => _team = value; }

    // Class Methods

    private Color Pick(Vector2 screenPoint, Image imageToPick) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageToPick.rectTransform, screenPoint, Camera.main, out Vector2 localPoint);
        localPoint += imageToPick.rectTransform.sizeDelta / 2;
        Texture2D texture = imageToPick.sprite.texture;
        Vector2Int texturePoint = new Vector2Int(
            Mathf.Clamp((int)(texture.width * (localPoint.x / imageToPick.rectTransform.sizeDelta.x)), 0, texture.width - 1),
            Mathf.Clamp((int)(texture.height * (localPoint.y / imageToPick.rectTransform.sizeDelta.y)), 0, texture.height - 1)
        );
        return texture.GetPixel(texturePoint.x, texturePoint.y);
    }

    public void OnPointerClick(PointerEventData eventData) {
        FindObjectOfType<SettingsBoard>().ChooseSettingsNoise.Play();
        _output = Pick(Camera.main.WorldToScreenPoint(eventData.position), GetComponent<Image>());
        _output.a = 1.0f;
        if (Team == "team1") {
            FindObjectOfType<SettingsBoard>().ColorResultTeam1.color = _output;
            FindObjectOfType<SettingsBoard>().UnsavedChanges = true;
        } else {
            FindObjectOfType<SettingsBoard>().ColorResultTeam2.color = _output;
            FindObjectOfType<SettingsBoard>().UnsavedChanges = true;
        }
    }
}

