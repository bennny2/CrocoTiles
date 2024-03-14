using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{   
    //Properties
    public Button HexagonButton {get => GetComponent<Button>(); set => HexagonButton = value;}
    public Image HexagonImage {get => GetComponent<Image>(); set => HexagonImage = value;}

    void Awake()
    {   
        HexagonButton.onClick.AddListener(() => ChangeColour());
    }

    void ChangeColour(){
        HexagonImage.color = Color.red;
    }
}
