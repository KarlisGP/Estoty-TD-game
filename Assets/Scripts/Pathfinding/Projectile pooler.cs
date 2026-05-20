using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooler : MonoBehaviour
{
    public static ProjectilePooler Instance;
    public GameObject projectilePrefab;
    public int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    void Awake() { Instance = this; }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetProjectile()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null; // All bullets in use
    }
}