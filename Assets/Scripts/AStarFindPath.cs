using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AStarFindPath
{
    private Vector2 minCostPos;
    private Dictionary<Vector2, (float, Vector2)> openDic = new Dictionary<Vector2, (float, Vector2)>();
    private Dictionary<Vector2, Vector2> closeDic = new Dictionary<Vector2, Vector2>();
    public List<Vector2> callBackList = new List<Vector2>();
    public bool pathFounded;

    /// <summary>
    /// 计算一个节点的路径消耗
    /// </summary>
    /// <param name="position">节点位置</param>
    /// <param name="startPos">路径起点</param>
    /// <param name="endPos">路径终点</param>
    /// <returns></returns>
    public float Cost(Vector2 position, Vector2 startPos, Vector2 endPos)
    {
        float f, g, h;
        g = Vector2.Distance(position, startPos);
        h = Vector2.Distance(position, endPos);
        f = g + h;
        return f;
    }

    /// <summary>
    /// 找到一个节点附近所有可行走的节点，添加至开放列表里，并将该节点从开放列表删除
    /// </summary>
    /// <param name="position">节点位置</param>
    /// <param name="startPos">路径起点</param>
    /// <param name="endPos">路径终点</param>
    public void FindAroundGrid(Vector2 position, Vector2 startPos, Vector2 endPos)
    {
        Vector2 around = position;
        around += Vector2.up;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.left;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.down;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.down;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.right;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.right;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.up;
        OpenDicAddItem(around, position, startPos, endPos);
        around += Vector2.up;
        OpenDicAddItem(around, position, startPos, endPos);
        openDic.Remove(minCostPos);
    }

    /// <summary>
    /// 将满足条件的节点添加至开放列表
    /// </summary>
    /// <param name="around">需要判断的节点</param>
    /// <param name="parentGrid">节点的父节点</param>
    /// <param name="startPos">路径起点</param>
    /// <param name="endPos">路径终点</param>
    void OpenDicAddItem(Vector2 around, Vector2 parentGrid, Vector2 startPos, Vector2 endPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(around, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground") && !openDic.ContainsKey(around) && !closeDic.ContainsKey(around))
        {
            if (CheckDiagonalGrid(around, parentGrid))
            {
                float cost = Cost(around, startPos, endPos);
                openDic.Add(around, (cost, parentGrid));
                Debug.Log("ADD" + around + " Grid Cost:" + cost);
            }
        }
    }

    /// <summary>
    /// 判断和父节点呈对角关系的节点能否添加至开放列表里,如果不需要角色斜向移动，可以注释该函数
    /// </summary>
    /// <param name="around">需判断的节点</param>
    /// <param name="parent">父节点</param>
    /// <returns></returns>
    public bool CheckDiagonalGrid(Vector2 around, Vector2 parent)
    {
        Vector2 diagonal = new Vector2(around.x - parent.x, around.y - parent.y);
        if (diagonal == new Vector2(1, 1))
        {
            RaycastHit2D hit = Physics2D.Raycast(around + Vector2.left, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition1 = hit.collider;
            hit = Physics2D.Raycast(around + Vector2.down, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition2 = hit.collider;
            if (condition1 && condition2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (diagonal == new Vector2(1, -1))
        {
            RaycastHit2D hit = Physics2D.Raycast(around + Vector2.left, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition1 = hit.collider;
            hit = Physics2D.Raycast(around + Vector2.up, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition2 = hit.collider;
            if (condition1 && condition2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (diagonal == new Vector2(-1, -1))
        {
            RaycastHit2D hit = Physics2D.Raycast(around + Vector2.right, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition1 = hit.collider;
            hit = Physics2D.Raycast(around + Vector2.up, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition2 = hit.collider;
            if (condition1 && condition2)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        else if (diagonal == new Vector2(-1, 1))
        {
            RaycastHit2D hit = Physics2D.Raycast(around + Vector2.right, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition1 = hit.collider;
            hit = Physics2D.Raycast(around + Vector2.down, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));
            bool condition2 = hit.collider;

            if (condition1 && condition2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 找到路径消耗最小的节点
    /// </summary>
    /// <returns>路径消耗最小的节点</returns>
    public Vector2 FindMinCostGrid()
    {
        float minValue = Mathf.Infinity;
        Vector2 minCostPosParent = minCostPos;
        foreach (var gridPos in openDic)
        {
            if (gridPos.Value.Item1 < minValue)
            {
                minValue = gridPos.Value.Item1;
                minCostPos = gridPos.Key;
                minCostPosParent = gridPos.Value.Item2;
            }
        }
        if (!closeDic.ContainsKey(minCostPos))
        {
            closeDic.Add(minCostPos, minCostPosParent);
        }
        return minCostPos;
    }

    /// <summary>
    /// A star寻路协程
    /// </summary>
    /// <param name="startPos">路径起点</param>
    /// <param name="endPos">路径终点</param>
    /// <returns></returns>
    public IEnumerator FindPath(Vector2 startPos, Vector2 endPos)
    {
        minCostPos = startPos;
        do
        {
            if (startPos != endPos)
            {
                FindAroundGrid(minCostPos, startPos, endPos);
                minCostPos = FindMinCostGrid();
                yield return null;
            }
            else
            {
                yield break;
            }
        } while (minCostPos != endPos && openDic.Count != 0);

        if (minCostPos == endPos)
        {
            Debug.Log("找到了最短路径");
            Vector2 temp = endPos;
            callBackList.Add(endPos);
            while (closeDic[temp] != startPos)
            {
                Debug.Log(closeDic[temp]);
                temp = closeDic[temp];
                callBackList.Add(temp);
                yield return null;
            }
            callBackList.Reverse();
            pathFounded = true;
            yield return pathFounded;
        }
        else if (openDic.Count == 0)
        {
            Debug.Log("无法到达指定点" + openDic.Count);
        }
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void ClearData()
    {
        if (openDic != null)
        {
            openDic.Clear();
        }
        if (closeDic != null)
        {
            closeDic.Clear();
        }
        if (callBackList != null)
        {
            callBackList.Clear();
        }
        pathFounded = false;
        minCostPos = Vector2.zero;
    }
}