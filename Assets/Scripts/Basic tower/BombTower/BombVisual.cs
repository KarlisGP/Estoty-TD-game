using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float growSpeed = 20f;
    public float fadeSpeed = 4f;
    private SpriteRenderer sr;
    private Color color;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
        transform.localScale = Vector3.zero;
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;
    }
}