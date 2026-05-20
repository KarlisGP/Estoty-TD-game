using UnityEngine;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Health Settings")]
    public float health = 3f;
    public int goldReward = 10;

    [Header("Movement Settings")]
    public float baseSpeed = 2f; 
    protected float currentSpeed;
    
    [Range(0f, 1f)] public float lightSpeedMultiplier = 0.5f; 
    private float frostSpeedMultiplier = 1.0f; 
    private float frostTimer = 0f; // How much longer the slow lasts

    [Header("Visibility")]
    public bool isVisible = false; 
    private int lightsTouchingMe = 0;

    [Header("A* Pathfinding")]
    private List<PathNode> path;
    private int targetNodeIndex;
    private Pathfinding pathfinder;
    private Transform goal;

    protected virtual void Start()
    {
        pathfinder = Object.FindAnyObjectByType<Pathfinding>();
        GameObject goalObj = GameObject.Find("The goal");
        if (goalObj != null) goal = goalObj.transform;

        CheckForInitialLight();
        Invoke("RecalculatePath", 0.05f);
    }

    protected virtual void Update()
    {
        HandleSlowTimers();
        CalculateCurrentSpeed();
        FollowAStarPath();
    }

    private void HandleSlowTimers()
    {
        if (frostTimer > 0)
        {
            frostTimer -= Time.deltaTime;
            if (frostTimer <= 0)
            {
                frostSpeedMultiplier = 1.0f; // Thaw out
            }
        }
    }

    // Called by the Ice Tower Pulse
    public void ApplyPulseFrost(float multiplier, float duration)
    {
        frostSpeedMultiplier = multiplier;
        frostTimer = duration; // Reset the thaw timer
    }

    private void CalculateCurrentSpeed()
    {
        float visibilityFactor = isVisible ? lightSpeedMultiplier : 1f;
        currentSpeed = baseSpeed * visibilityFactor * frostSpeedMultiplier;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        
        Destroy(gameObject);
    }

    public void RecalculatePath()
    {
        if (pathfinder != null && goal != null)
        {
            path = pathfinder.FindPath(transform.position, goal.position);
            targetNodeIndex = 0;
        }
    }

    private void FollowAStarPath()
    {
        if (path == null || targetNodeIndex >= path.Count) return;
        Vector3 targetPos = path[targetNodeIndex].worldPosition;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) < 0.1f) targetNodeIndex++;
        if (targetNodeIndex >= path.Count) ReachGoal();
    }

    protected virtual void ReachGoal()
    {
        if (GameManager.instance != null) GameManager.instance.TakeDamage(1);
        Destroy(gameObject);
    }

    private void CheckForInitialLight()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null && hit.CompareTag("MouseLight"))
        {
            lightsTouchingMe = 1;
            isVisible = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) { lightsTouchingMe++; isVisible = true; }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight"))
        {
            lightsTouchingMe--;
            if (lightsTouchingMe <= 0) { lightsTouchingMe = 0; isVisible = false; }
        }
    }
}