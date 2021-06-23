using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public AStarFindPath findPath;
    private Vector2 target;
    public Vector2 curAimPos;
    private int index = 0;

    private void Awake()
    {
        findPath = new AStarFindPath();
    }
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        StartFindPathCoroutine();
        
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    /// <summary>
    /// 检测点击的目标点位置
    /// </summary>
    void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    /// <summary>
    /// 启动A star寻路协程
    /// </summary>
    void StartFindPathCoroutine()
    {
        if (curAimPos != GridPosition.GetInstance().CorrectionPosition(target))
        {
            StopCoroutine(findPath.FindPath(GridPosition.GetInstance().CorrectionPosition(transform.position), curAimPos));
            index = 0;
            curAimPos = GridPosition.GetInstance().CorrectionPosition(target);
            findPath.ClearData();
            StartCoroutine(findPath.FindPath(GridPosition.GetInstance().CorrectionPosition(transform.position), curAimPos));
        }
    }

    /// <summary>
    /// 角色移动至目标点
    /// </summary>
    void MoveToTarget()
    {
        if (findPath.pathFounded)
        {
            if (findPath.callBackList.Count!=0)
            {
                Vector2 pointTarget = findPath.callBackList[index];

                if (Vector2.Distance(transform.position, findPath.callBackList[index]) != 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, findPath.callBackList[index], Time.deltaTime);
                }
                else
                {
                    if (index < findPath.callBackList.Count - 1)
                    {
                        index++;
                    }
                    else
                    {
                        Debug.Log("到达目标点");
                        return;
                    }
                }
            }
        }
    }
}
