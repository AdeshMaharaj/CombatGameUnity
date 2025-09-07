using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition; // Adjust to Vector3 if needed
    public int gridX;
    public int gridY;
    public int GCost;
    public int HCost;
    public Node parent;

    public int FCost
    {
        get { return GCost + HCost; }
    }

    // New property for position (returns the same as worldPosition)
    public Vector2 position
    {
        get { return worldPosition; }
    }

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
