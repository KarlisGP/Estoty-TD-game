using UnityEngine;

public class IcePulseEffect : MonoBehaviour
{
    private float growSpeed = 10f;
    private float maxScale = 5f;
    private float fadeSpeed = 1.5f;
    
    private SpriteRenderer sr;
    private Color color;

    // This is called by the Tower right after spawning
    public void Setup(float radius)
    {
        maxScale = radius * 2f; // Diameter
        growSpeed = radius * 4f; // Speed relative to size
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        // 1. Grow the circle until it hits the max scale
        if (transform.localScale.x < maxScale)
        {
            transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
        }

        // 2. Fade the color out
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        // 3. Delete the object when it's invisible
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}