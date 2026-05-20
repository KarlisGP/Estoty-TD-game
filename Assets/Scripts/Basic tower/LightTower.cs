using UnityEngine;

public class LightTower : MonoBehaviour
{
    [Header("Settings")]
    public float lightRadius = 5f;
    public SpriteRenderer radiusVisual; // A faded circle sprite
    public GameObject lightMask;       // The Sprite Mask child

    void Start()
    {
        // Set the size of the trigger and visuals based on the radius
        GetComponent<CircleCollider2D>().radius = lightRadius;
        
        if (radiusVisual != null)
            radiusVisual.transform.localScale = new Vector3(lightRadius * 2, lightRadius * 2, 1);
            
        if (lightMask != null)
            lightMask.transform.localScale = new Vector3(lightRadius * 2, lightRadius * 2, 1);
    }
}