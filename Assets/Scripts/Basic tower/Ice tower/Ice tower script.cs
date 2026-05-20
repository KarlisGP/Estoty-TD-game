using UnityEngine;

public class IceTower : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseRadius = 5f;
    public float pulseInterval = 2f; // How often it pulses
    
    [Header("Slow Settings")]
    [Range(0.1f, 1f)] public float slowMultiplier = 0.6f; // 40% slow
    public float slowDuration = 1.5f; // How long the slow lasts after the pulse

    [Header("Visuals")]
    public GameObject pulseEffectPrefab; // Optional: A circular "wave" effect

    private float pulseCountdown = 0f;

    void Update()
    {
        pulseCountdown -= Time.deltaTime;
        if (pulseCountdown <= 0f)
        {
            Pulse();
            pulseCountdown = pulseInterval;
        }
    }

    void Pulse()
    {
        // Create the visual wave
        if (pulseEffectPrefab != null)
        {
            GameObject effect = Instantiate(pulseEffectPrefab, transform.position, Quaternion.identity);
            IcePulseEffect script = effect.GetComponent<IcePulseEffect>();
            if (script != null)
            {
                script.Setup(pulseRadius); // Tell it how big to get
            }
        }

        // Apply the slow to real enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, pulseRadius);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemy = hit.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.ApplyPulseFrost(slowMultiplier, slowDuration);
                }
            }
        }
    }
    // Visualize the pulse range in Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pulseRadius);
    }
}