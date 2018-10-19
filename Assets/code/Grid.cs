using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
    private Node[,] grid;
    public Vector2 gridsize;
    public float noder;
    private float noded;
    public LayerMask WhatLayer;//标识节点在可走层还是不可走层
    public int gridContX, gridContY;//保存每个方向上的网格数
    public Transform Player;
    public List<Node> path = new List<Node>();
	// Use this for initialization
	void Start () {
        noded = noder * 2;
        gridContX = Mathf.RoundToInt(gridsize.x / noded);
        gridContY = Mathf.RoundToInt(gridsize.y / noded);
        grid=new Node[gridContX,gridContY];
        CreatGrid();
	}
    private void CreatGrid() 
    {
        Vector3 StartPoint = transform.position - gridsize.x / 2 * Vector3.right - gridsize.y / 2 * Vector3.forward;
        for (int i = 0; i <gridContX; i++)
        {
            for (int j = 0; j < gridContY; j++)
            {
               Vector3 WorldPoint=StartPoint+Vector3.right*(i*noded+noder)+Vector3.forward*(j*noded+noder);
               bool walkable = !Physics.CheckSphere(WorldPoint,noder,WhatLayer);//发射一圆形射线检测到非碰撞即为可走
               grid[i, j] = new Node(walkable,WorldPoint,i,j);
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
    public Node GetFromPosition(Vector3 position) 
    {
        float percentX = (position.x + gridsize.x / 2) / gridsize.x;//使所有结点坐标从零开始
        float percentY = (position.z + gridsize.y / 2) / gridsize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridContX-1)*percentX);
        int y = Mathf.RoundToInt((gridContY-1)*percentY);
        return grid[x,y];
    }
    void OnDrawGizmos() //画出所有网格和结点
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(gridsize.x,1,gridsize.y));
        if (grid == null) return;
        Node playNode = GetFromPosition(Player.position);
        foreach(var node in grid)
        {
            Gizmos.color = node._canwalk ? Color.white : Color.red;
            Gizmos.DrawCube(node._worldPos,Vector3.one*(noded-.1f) );
        }
        if (path!= null) 
        {
            foreach (var node in path)
            {
                Debug.Log("bbbb");
                Gizmos.color = Color.black;
                Gizmos.DrawCube(node._worldPos, Vector3.one * (noded - .1f));
            }
        }
        if (playNode != null && playNode._canwalk) 
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(playNode._worldPos, Vector3.one * (noded - .1f));
        }
    }
    public List<Node> GetNeibourhood(Node nd) 
    {
        List<Node> neibourhood = new List<Node>();
        for (int i = -1; i <=1; i++)
        {
            for (int j = -1; j <=1; j++)
			{
			 if(i==0&&j==0)
             {
                 continue;
             }
             int tempX = nd._gridX + i;
             int tempY = nd._gridY + j;
             if (tempX < gridContX && tempX > 0 && tempY < gridContY && tempY > 0) 
             {
                 neibourhood.Add(grid[tempX, tempY]);
             }
			}
        }
        return neibourhood;//获取得到选定结点相邻的结点
    }
}
