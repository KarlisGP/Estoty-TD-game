using UnityEngine;

public class BuildTile : MonoBehaviour
{
    public Color hoverColor = Color.green;
    private Color startColor;
    private SpriteRenderer rend;

    public GameObject towerOnTile; 
    public bool isOccupied => towerOnTile != null;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;
    }

    public void Highlight(bool canBuild)
    {
        rend.color = canBuild ? hoverColor : Color.red;
    }

    public void ResetColor()
    {
        if(rend != null) rend.color = startColor;
    }
}