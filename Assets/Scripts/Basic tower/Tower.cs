using UnityEngine;

public class BasicTower : MonoBehaviour
{
    [Header("Stats")]
    public float range = 5f;
    public float fireRate = 1.5f;
    public int damage = 1;
    
    [Header("Setup")]
    public Transform firePoint;
    public SpriteRenderer towerVisual;
    
    private float fireCountdown = 0f;
    private int lightsNearby = 0; 

    // Property to check if any light source is touching us
    private bool isInLight => lightsNearby > 0;

    void Update()
    {
        // 1. Handle Visuals and State
        if (!isInLight)
        {
            towerVisual.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Dimmed
            return;
        }

        towerVisual.color = Color.white; // Brightened

        // 2. Handle Shooting
        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float shortestDistance = range;

        foreach (GameObject enemy in enemies)
        {
            EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
            
            // CHECK: Is enemy visible in light?
            if (enemyScript != null && enemyScript.isVisible)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
        }

        if (nearestEnemy != null)
        {
            GameObject projGO = ProjectilePooler.Instance.GetProjectile();
            if (projGO != null)
            {
                Projectile p = projGO.GetComponent<Projectile>();
                if (p != null)
                {
                    p.Launch(nearestEnemy, damage, firePoint.position);
                }
            }
        }
    }

    // --- Light Counter Logic ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight"))
        {
            lightsNearby++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight"))
        {
            lightsNearby--;
            if (lightsNearby < 0) lightsNearby = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}