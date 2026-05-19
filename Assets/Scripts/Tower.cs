using UnityEngine;

public class BasicTower : MonoBehaviour
{
    [Header("Stats")]
    public float range = 4f;
    public float fireRate = 1f;
    public int damage = 1;
    
    [Header("Setup")]
    public Transform firePoint;
    public SpriteRenderer towerVisual;
    
    private float fireCountdown = 0f;
    private bool isInLight = false;

    void Update()
    {
        // Only function if in light
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
        // Find nearest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortestDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < shortestDist && dist <= range)
            {
                shortestDist = dist;
                nearest = e;
            }
        }

        if (nearest != null)
        {
            GameObject projGO = ProjectilePooler.Instance.GetProjectile();
            if (projGO != null)
            {
                projGO.transform.position = firePoint.position;
                projGO.GetComponent<Projectile>().Launch(nearest.transform, damage);
            }
        }
    }

    // --- Light Detection Logic ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) isInLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) isInLight = false;
    }
}