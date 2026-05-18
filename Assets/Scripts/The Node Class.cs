using UnityEngine;

public class PathNode
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost; // Distance from starting node
    public int hCost; // Distance from end node
    public int fCost => gCost + hCost; // Total cost
    public PathNode parent;

    public PathNode(bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY) 
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}