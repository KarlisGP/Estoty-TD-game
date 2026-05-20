using UnityEngine;

public class BasicTower : MonoBehaviour
{
    public float range = 5f;
    public float fireRate = 1f;
    public int damage = 1;
    public Transform firePoint;
    public SpriteRenderer towerVisual;

    private float fireCountdown = 0f;
    private bool isInLight = false;

    void Update()
    {
        if (!isInLight) return;

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
            // 1. Get the EnemyBase script from the enemy
            EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();

            // 2. ONLY proceed if the enemy exists AND is currently in the light
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

        // 3. Only fire if we found an enemy that was BOTH in range and in light
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) isInLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) isInLight = false;
    }
}