using UnityEngine;

public class BombTower : MonoBehaviour
{
    [Header("Stats")]
    public float range = 7f;
    public float fireRate = 0.5f; 
    public int damage = 3;
    
    [Header("Setup")]
    public Transform firePoint;
    public SpriteRenderer towerVisual;
    public GameObject bombPrefab; 

    private float fireCountdown = 0f;
    public bool isInLight = false; // Public so you can see it in Inspector

    void Update()
    {
        // 1. Visual feedback for light
        if (!isInLight)
        {
            towerVisual.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Darkened
            return;
        }

        towerVisual.color = Color.white;
        
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
        
        if (enemies.Length == 0) return;

        Transform target = null;
        float shortestDist = range;

        foreach (GameObject e in enemies)
        {
            EnemyBase enemyScript = e.GetComponent<EnemyBase>();
            
            // Check if enemy is in range AND visible in light
            if (enemyScript != null && enemyScript.isVisible)
            {
                float dist = Vector2.Distance(transform.position, e.transform.position);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    target = e.transform;
                }
            }
        }

        if (target != null)
        {
            if (bombPrefab == null) {
                Debug.LogError("BOMB PREFAB MISSING on " + gameObject.name);
                return;
            }

            GameObject bombGO = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
            bombGO.GetComponent<BombProjectile>().Launch(target, damage, firePoint.position);
            Debug.Log("Bomb Fired!");
        }
    }

    // --- Physics Detection ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) {
            isInLight = true;
            Debug.Log("Bomb Tower ENTERED light.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) {
            isInLight = false;
            Debug.Log("Bomb Tower LEFT light.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}