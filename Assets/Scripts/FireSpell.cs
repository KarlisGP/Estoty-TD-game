using UnityEngine;

public class FireRain : MonoBehaviour
{
    public float damagePerTick = 1f;
    public float tickInterval = 0.5f;
    public float radius = 4f;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ApplyDamage();
            timer = tickInterval;
        }
    }

    void ApplyDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<EnemyBase>().TakeDamage(damagePerTick);
            }
        }
    }
}