using UnityEngine;
using System.Collections;

public class Node
{
    public bool _canwalk;
    public Vector3 _worldPos;
    public int _gridX, _gridY;
    public int gCost;
    public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public Node parent;//指向父对象的指针
    public Node(bool canWalk, Vector3 position, int x, int y)
    {
        _canwalk = canWalk;
        _worldPos = position;
        _gridX = x;
        _gridY = y;
    }
}
