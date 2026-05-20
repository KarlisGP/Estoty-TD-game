using UnityEngine;

public class Goal : MonoBehaviour {
    [SerializeField] private int damageToPlayer = 1;

    private void OnTriggerEnter2D(Collider2D collision) {
        // Check if the object entering the goal is an enemy
        if (collision.CompareTag("Enemy")) {
            // Tell the Game Manager to reduce health
            GameManager.instance.TakeDamage(damageToPlayer);
            
            // Destroy the enemy reached the goal
            Destroy(collision.gameObject);
        }
    }
}