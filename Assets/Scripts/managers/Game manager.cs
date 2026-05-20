using UnityEngine;
using UnityEngine.UI; // Optional: for UI updates

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int playerLives = 20;

    void Awake() {
        // Basic Singleton setup
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void TakeDamage(int amount) {
        playerLives -= amount;
        Debug.Log("Player Lives: " + playerLives);

        if (playerLives <= 0) {
            GameOver();
        }
    }

    void GameOver() {
        Debug.Log("Game Over!");
        // Add logic here to restart the level or show a UI screen
    }
}