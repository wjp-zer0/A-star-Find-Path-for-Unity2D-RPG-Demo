using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition
{
    //单例
    private static GridPosition instance; 
    
    public static GridPosition GetInstance()
    {
        if(instance == null)
        {
            instance = new GridPosition();
        }
        return instance;
    }

    /// <summary>
    /// 游戏对象位置网格化校正
    /// </summary>
    /// <param name="currentPosition">需要校正的坐标</param>
    /// <returns>校正后的坐标</returns>
    public Vector2 CorrectionPosition(Vector2 currentPosition)
    {
        float x;
        float y;
        if (currentPosition.x - Mathf.Floor(currentPosition.x) > 0.5f)
        {
            x = Mathf.Floor(currentPosition.x) + 1.0f;
        }
        else
        {
            x = Mathf.Floor(currentPosition.x);
        }
        if (currentPosition.y - Mathf.Floor(currentPosition.y) > 0.5f)
        {
            y = Mathf.Floor(currentPosition.y) + 1.0f;
        }
        else
        {
            y = Mathf.Floor(currentPosition.y);
        }
        return new Vector2(x, y);
    }
    
}
