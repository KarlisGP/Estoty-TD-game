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
    private int lightsNearby = 0;

    private bool isInLight => lightsNearby > 0;

    void Update()
    {
        if (!isInLight)
        {
            towerVisual.color = new Color(0.2f, 0.2f, 0.2f, 1f);
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
        Transform target = null;
        float shortestDist = range;

        foreach (GameObject e in enemies)
        {
            EnemyBase enemyScript = e.GetComponent<EnemyBase>();
            
            // CHECK: Is enemy visible in light?
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
            if (bombPrefab != null)
            {
                GameObject bombGO = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
                bombGO.GetComponent<BombProjectile>().Launch(target, damage, firePoint.position);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}