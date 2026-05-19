using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject towerPrefab; // Drag your tower prefab here

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos.z = 0;

        // Instantiate the tower
        Instantiate(towerPrefab, spawnPos, Quaternion.identity);

        // Tell all existing enemies to find a new path around the tower
        EnemyBase[] allEnemies = Object.FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in allEnemies)
        {
            enemy.RecalculatePath();
        }
    }
}