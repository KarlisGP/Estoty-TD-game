using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    private Transform target;
    private int damage;
    private bool isFlying = false;

    // We added _startPos to ensure it never spawns at 0,0,0
    public void Launch(Transform _target, int _damage, Vector3 _startPos)
    {
        target = _target;
        damage = _damage;

        // Move to the tower immediately
        transform.position = new Vector3(_startPos.x, _startPos.y, 0);

        isFlying = true;
        gameObject.SetActive(true);

        // Force the sprite to be visible
        GetComponent<SpriteRenderer>().enabled = true;
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
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * dist, Space.World);
    }

    void HitTarget()
    {
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // This triggers the HP loss and Death
        }

        gameObject.SetActive(false); // Return to pool
    }
}