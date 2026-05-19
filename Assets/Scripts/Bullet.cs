using UnityEngine;

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private int damage;

    public void Launch(Transform _target, int _damage)
    {
        target = _target;
        damage = _damage;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        if (enemy != null) enemy.health -= damage;
        gameObject.SetActive(false); // Return to pool
    }
}
