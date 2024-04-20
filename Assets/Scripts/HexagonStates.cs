using UnityEngine;

[System.Serializable]
public class HexagonStates
{
    // Fields

    [SerializeField]
    private Color _fillColor;

    [SerializeField]
    private string _stateName;

    // Properties
    
    public Color FillColor 
    {
        get => _fillColor;
        set => _fillColor = value;
    }

    public string StateName
    {
        get => _stateName;
        set => _stateName = value;

    } 
}

