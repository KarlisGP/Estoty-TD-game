using UnityEngine;
using System.Collections.Generic;

public class PathfindingGrid : MonoBehaviour {
    public LayerMask obstacleLayer;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    PathNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public void CreateGrid() {
        grid = new PathNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
            
                // FIX: Use the obstacleLayer mask here. 
                // This tells Unity: "Only return a hit if it's on the Obstacle Layer"
                Collider2D hit = Physics2D.OverlapCircle(worldPoint, nodeRadius * 0.8f, obstacleLayer);
            
                // If hit is not null, it MUST be an obstacle because of the mask
                bool walkable = (hit == null);

                grid[x, y] = new PathNode(walkable, worldPoint, x, y);
            }
        }
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPosition) {
        // 1. Calculate position relative to the Grid's center
        float percentX = (worldPosition.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y - transform.position.y + gridWorldSize.y / 2) / gridWorldSize.y;

        // 2. Clamp values so they don't go outside the array
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
    
        return grid[x, y];
    }
    public List<PathNode> GetNeighbors(PathNode node) {
        List<PathNode> neighbors = new List<PathNode>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    // Visualizes the grid in the editor
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null) {
            foreach (PathNode n in grid) {
                Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}