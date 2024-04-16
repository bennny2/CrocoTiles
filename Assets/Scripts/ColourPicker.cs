using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour, IPointerClickHandler
{

    // Serialize Fields

    [SerializeField]
    private string team;

    // Fields

    public Color output;

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
        //FindObjectOfType<SettingsBoard>().ChooseSettingsNoise.Play();
        output = Pick(Camera.main.WorldToScreenPoint(eventData.position), GetComponent<Image>());
        output.a = 1.0f;
        if (team == "team1") {
            FindObjectOfType<SettingsBoard>().ColorResultTeam1.color = output;
            FindObjectOfType<SettingsBoard>().unsavedChanges = true;
        } else {
            FindObjectOfType<SettingsBoard>().ColorResultTeam2.color = output;
            FindObjectOfType<SettingsBoard>().unsavedChanges = true;
        }
    }
}

