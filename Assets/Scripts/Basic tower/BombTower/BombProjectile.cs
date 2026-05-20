using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float explosionRadius = 3f;
    public int damage = 2;
    public GameObject explosionEffectPrefab;

    private Transform target;
    private bool isFlying = false;

    public void Launch(Transform _target, int _damage, Vector3 _startPos)
    {
        target = _target;
        damage = _damage;
        transform.position = _startPos;
        isFlying = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isFlying || target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float dist = speed * Time.deltaTime;

        if (dir.magnitude <= dist)
        {
            Explode();
            return;
        }

        transform.Translate(dir.normalized * dist, Space.World);
    }

    void Explode()
    {
        // 1. Visuals
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. Logic: Find all enemies in blast radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyBase enemy = hit.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // 3. Clean up
        isFlying = false;
        gameObject.SetActive(false);
    }
}