using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Hexagon : MonoBehaviour, IPunObservable
{

    // Serialized Fields

    [SerializeField]
    private string _hexagonCurrentState;
    [SerializeField]
    private TextMeshProUGUI _hexagonScoreText;
    [SerializeField]
    private int _hexagonScore = 0;

    // Fields

    private Board _boardObject;
    private Button _hexagonButton;
    private TextMeshProUGUI _hexagonText;
    private Image _hexagonImage;
    private float _hexagonX;
    private float _hexagonY;
    private float _hexagonZ;
    public const int HORIZONTALOFFSET = 70; 
    public const int VERTICALOFFSET = 80; 
    public const int VERTICALDIAGONALOFFSET = 40; 

    // Properties

    public TextMeshProUGUI HexagonText => _hexagonText;
    public string HexagonCurrentState 
    {
        get => _hexagonCurrentState;
        set => _hexagonCurrentState = value;
    }
    public Image HexagonImage => _hexagonImage;

    public float HexagonX
    {
        get => _hexagonX;
        set => _hexagonX = value;
    }
    public float HexagonY
    {
        get => _hexagonY;
        set => _hexagonY = value;
    }
    public float HexagonZ
    {
        get => _hexagonZ;
        set => _hexagonZ = value;
    }
    public int HexagonScore 
    { 
        get => _hexagonScore; 
        set 
        { 
            _hexagonScore = value;
            HexagonScoreText.text = value.ToString();
        }
    }

    public TextMeshProUGUI HexagonScoreText 
    { 
        get => _hexagonScoreText; 
        set => _hexagonScoreText = value; 
    }
    public Board BoardObject { get => _boardObject; set => _boardObject = value; }


    // Class Methods

    void Awake() {
        InitilizeComponents();
        _hexagonButton.onClick.AddListener(() => BoardObject.HexagonPressed(this));
    }

    public void DeleteLetter() {
        HexagonText.text = "";
    }

    public List<Hexagon> FindTouchingHexagons() {
        List<Hexagon> touchingHexagonsArray = new();

        int[] horizontalOffsets = { 0, HORIZONTALOFFSET, HORIZONTALOFFSET, 0, -HORIZONTALOFFSET, -HORIZONTALOFFSET};
        int[] verticalOffsets = { VERTICALOFFSET, VERTICALDIAGONALOFFSET, -VERTICALDIAGONALOFFSET, -VERTICALOFFSET, -VERTICALDIAGONALOFFSET, VERTICALDIAGONALOFFSET  };

        for (int i = 0; i < 6; i++) { 
        
            float targetX = this.HexagonX + horizontalOffsets[i];
            float targetY = this.HexagonY + verticalOffsets[i];

            Hexagon touchingHexagon = BoardObject.AllHexagons.FirstOrDefault(h => h.HexagonX == targetX && h.HexagonY == targetY);

            if (touchingHexagon != null) {
                touchingHexagonsArray.Add(touchingHexagon);
            }
        }
        return touchingHexagonsArray;
    }

    public bool FindIfThereIsATouchingHexagonOfType(string targetStateType) {
        int[] horizontalOffsets = { 0, HORIZONTALOFFSET, HORIZONTALOFFSET, 0, -HORIZONTALOFFSET, -HORIZONTALOFFSET};
        int[] verticalOffsets = { VERTICALOFFSET, VERTICALDIAGONALOFFSET, -VERTICALDIAGONALOFFSET, -VERTICALOFFSET, -VERTICALDIAGONALOFFSET, VERTICALDIAGONALOFFSET  };

        for (int i = 0; i < 6; i++) { 
        
            float targetX = this.HexagonX + horizontalOffsets[i];
            float targetY = this.HexagonY + verticalOffsets[i];

            Hexagon touchingHexagon = BoardObject.AllHexagons.FirstOrDefault(h => h.HexagonX == targetX && h.HexagonY == targetY);

            if (touchingHexagon != null && touchingHexagon.HexagonCurrentState == targetStateType) {
                return true;
            }
        }
        return false;
    } 

    void InitilizeComponents() {
        BoardObject = FindObjectOfType<Board>();
        _hexagonText = GetComponentInChildren<TextMeshProUGUI>();
        HexagonText.outlineWidth = 0.15F;
        HexagonText.outlineColor = Color.black;
        _hexagonImage = GetComponent<Image>();
        _hexagonButton = GetComponent<Button>();
    }

    public void MakeTouchingHexagonsNeutralAroundHome() {
        List<Hexagon> touchingHexagons = FindTouchingHexagons();
        foreach (Hexagon touchingHexagon in touchingHexagons) {
            string hexState = touchingHexagon.HexagonCurrentState;

            if (hexState == "invisible") {
                touchingHexagon.SetHexagonState(BoardObject.Neutral);
            }
        }
    }

    public void SetHexagonState(HexagonStates state) {
        switch (state.StateName)
        {
            case "homeTeam1":
            case "homeTeam2":
                HexagonText.text = "";
                MakeTouchingHexagonsNeutralAroundHome();
                break;

            case "neutral":
                if (string.IsNullOrWhiteSpace(HexagonText.text)) { 
                    SetLetter();
                }
                break;

            default:
                Console.WriteLine("State not accepted");
                break;
        }
        HexagonImage.color = state.FillColor;
        HexagonCurrentState = state.StateName;
    }

    public void SetLetter(){
        HexagonText.text = Letter.GenerateLetter();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Writing data: send the data of hexagon to the stream
            stream.SendNext(HexagonCurrentState);

            float[] colorVector = new float[] {HexagonImage.color.r, HexagonImage.color.g, HexagonImage.color.b, HexagonImage.color.a};
            stream.SendNext(colorVector);

            string hexagonString = HexagonText.text;
            stream.SendNext(hexagonString);

            stream.SendNext(HexagonX);

            stream.SendNext(HexagonY);
        }
        else
        {
            // Reading data: receive the data of hexagon from the stream
            HexagonCurrentState = (string)stream.ReceiveNext();

            float[] colorVector = (float[])stream.ReceiveNext();
            HexagonImage.color = new Color(colorVector[0], colorVector[1], colorVector[2], colorVector[3]);

            HexagonText.text = (string)stream.ReceiveNext();

            HexagonX = (float)stream.ReceiveNext();

            HexagonY = (float)stream.ReceiveNext();
        }
    }

}

