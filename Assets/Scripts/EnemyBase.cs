using UnityEngine;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Attributes")]
    public float health = 1f;
    public float speed = 2f;
    private float currentSpeed;

    [Header("A* Pathfinding")]
    private List<PathNode> path;
    private int targetNodeIndex;
    private Pathfinding pathfinder;
    private Transform goal;

    protected virtual void Start()
    {
        currentSpeed = speed;
        
        // 1. Find the Pathfinding script in the scene
        pathfinder = Object.FindAnyObjectByType<Pathfinding>();
        
        // 2. Find the Goal (Ensure your target object is named "The goal" or has a Tag)
        GameObject goalObj = GameObject.Find("The goal");
        if (goalObj != null) goal = goalObj.transform;

        // 3. Calculate the path immediately
        RecalculatePath();
    }

    public void RecalculatePath()
    {
        if (pathfinder != null && goal != null)
        {
            path = pathfinder.FindPath(transform.position, goal.position);
            targetNodeIndex = 0;
        }
    }

    protected virtual void Update()
    {
        FollowAStarPath();
    }

    private void FollowAStarPath()
    {
        if (path == null || targetNodeIndex >= path.Count) return;

        // Move towards the current node's world position
        Vector3 targetPos = path[targetNodeIndex].worldPosition;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);

        // If reached the node, move to the next one
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            targetNodeIndex++;
        }

        // If we reached the final node (the goal)
        if (targetNodeIndex >= path.Count)
        {
            ReachGoal();
        }
    }

    protected virtual void ReachGoal()
    {
        GameManager.instance.TakeDamage(1);
        Destroy(gameObject);
    }

    // --- Vision Gimmick Integration ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) currentSpeed = speed * 0.5f; 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MouseLight")) currentSpeed = speed;
    }
}