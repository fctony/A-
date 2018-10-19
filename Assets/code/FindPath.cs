using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath : MonoBehaviour {
    private Grid _grid;
    public Transform player, EndPoint;
    void Start () {
        
        _grid =GetComponent<Grid>();
	}
	
	void Update () {
        
       

        Findingpath(player.position, EndPoint.position);
	}
    void Findingpath(Vector3 Startpos,Vector3 EndPos) 
    {
       Node startNode = _grid.GetFromPosition(Startpos);
        Node endNode = _grid.GetFromPosition(EndPos);
        List<Node> openSet = new List<Node>();//创建开启列表
        HashSet<Node> closeSet = new HashSet<Node>();//创建关闭列表
        openSet.Add(startNode);

        while(openSet.Count>0)
        {
            Node currentNode = openSet[0];//当前结点为最优结点
            for (int i = 0; i <openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || 
                    openSet[i].fCost == currentNode.fCost &&
                    openSet[i].hCost < currentNode.hCost) //判断最优结点
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closeSet.Add(currentNode);
            if (currentNode == endNode) 
            {
                GeneratePath(startNode,endNode);
                return;
            }
            foreach (var node in _grid.GetNeibourhood(currentNode)) 
            {
                if (!node._canwalk || closeSet.Contains(node)) continue;
                int newcost = currentNode.gCost + GetDistanceNodes(currentNode, node);//当前相邻格子与开始格子的距离
                if (newcost < node.gCost ||!openSet.Contains(node)) 
                {
                    node.gCost = newcost;
                    node.hCost = GetDistanceNodes(node, endNode);
                    node.parent = currentNode;
                    if (!openSet.Contains(node)) 
                    {
                        openSet.Add(node);
                    }
                }
            }
        }
    }
    private void GeneratePath(Node startnode,Node endnode) 
    {
        List<Node> path = new List<Node>();
        Node temp = endnode;
        while (temp != startnode) 
        {
            path.Add(temp);
            temp = temp.parent;//更新当前结点为父节点
        }
        path.Reverse();//反转
        _grid.path = path;
    }
    int GetDistanceNodes(Node a,Node b) //获取两结点之间的距离
    {
        int cntX = Mathf.Abs(a._gridX-b._gridX);
        int cntY = Mathf.Abs(a._gridY-b._gridY);
        if (cntX > cntY) 
        {
        return 14*cntY+10*(cntX-cntY);
        }
        else 
        {
        return 14*cntX+10*(cntY-cntX);
        }
    }
}
