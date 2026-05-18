using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour {
    public float speed = 2f;
    private Transform target; 
    private Pathfinding pathfinding;
    private List<PathNode> path;
    private int targetIndex;

    void Start() {
        pathfinding = FindObjectOfType<Pathfinding>();
        Goal goalScript = FindObjectOfType<Goal>();

        if (goalScript != null) {
            target = goalScript.transform; 
            RecalculatePath();
        }
    }

    public void RecalculatePath() {
        if (target != null && pathfinding != null) {
            path = pathfinding.FindPath(transform.position, target.position);
            targetIndex = 0;
            
            if (path == null || path.Count == 0) {
                Debug.LogError("Enemy path not found! Check if goal/enemy are outside the grid.");
            }
        }
    }

    void Update() {
        if (path != null && targetIndex < path.Count) {
            // Target the center of the next node
            Vector3 targetPos = path[targetIndex].worldPosition;
        
            // Ensure Z stays the same so the enemy doesn't disappear
            targetPos.z = transform.position.z;

            // Move the enemy
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // Check if we are "close enough" to the center of the node
            if (Vector2.Distance(transform.position, targetPos) < 0.1f) {
                targetIndex++;
            }
        }
    }

    // THIS DRAWS THE PATH IN THE SCENE VIEW
    void OnDrawGizmos() {
        if (path != null) {
            Gizmos.color = Color.cyan;
            for (int i = targetIndex; i < path.Count; i++) {
                Gizmos.DrawCube(path[i].worldPosition, Vector3.one * 0.3f);
                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i].worldPosition);
                } else {
                    Gizmos.DrawLine(path[i - 1].worldPosition, path[i].worldPosition);
                }
            }
        }
    }
}