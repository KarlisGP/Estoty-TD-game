using UnityEngine;

public class SpellObject : MonoBehaviour
{
    public float duration = 5f;

    void Start()
    {
        // When the flare spawns, find everything in its radius and tell them they are in light
        float radius = GetComponent<CircleCollider2D>().radius;
        Collider2D[] objectsInLight = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D col in objectsInLight)
        {
            // This forces the "OnTriggerEnter" logic to run manually
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<EnemyBase>().isVisible = true;
            }
        }
    
        Destroy(gameObject, duration);
    }
    
}