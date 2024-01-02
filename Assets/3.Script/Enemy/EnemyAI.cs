using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Node
{
    /*
        1. 이 노드가 벽인지 확인
        2. 부모노드
        3. 이 노드의 x, y 좌표값
        4. f, h, g 값 (f = h + g)
           h : 추정값. 즉 가로 세로 장애물을 무시하여 목표까지 거리
           g : 시작노드부터 이동했던 거리
     */
    public bool IsWall;
    public Node Parentnode;
    public int x, y;
    public int G;
    public int H;
    public int F { get { return G + H; } }

    public Node(bool IsWall, int x, int y)
    {
        this.IsWall = IsWall;
        this.x = x;
        this.y = y;
    }
}
public class EnemyAI : MonoBehaviour
{

    public GameObject StartPos, EndPos;
    public GameObject BottomLeft_obj, TopRight_obj;

    [SerializeField] private Vector2Int bottom_left, top_right, start_pos, end_pos;

    public List<Node> Final_NodeList;

    //대각선 이용할 것인지?
    public bool AllowDigonal = true;

    //코너를 가로질러가지 않을 경우 이동 중 수직 수평 장애물이 있는지 판단
    public bool DontCrossCorner = true;

    private int SizeX, SizeY;
    Node[,] node_Array;

    Node StartNode, EndNode, CurrentNode;

    List<Node> OpenList, CloseList;

    //public GameObject Player;

    public void SetPosition()       //맵 사이즈
    {
        bottom_left = new Vector2Int((int)BottomLeft_obj.transform.position.x, (int)BottomLeft_obj.transform.position.y);
        top_right = new Vector2Int((int)TopRight_obj.transform.position.x, (int)TopRight_obj.transform.position.y);
        start_pos = new Vector2Int((int)StartPos.transform.position.x, (int)StartPos.transform.position.y);
        end_pos = new Vector2Int((int)EndPos.transform.position.x, (int)EndPos.transform.position.y);
    }

    public void PathFinding()
    {
        SetPosition();
        //개수
        SizeX = top_right.x - bottom_left.x + 1;
        SizeY = top_right.y - bottom_left.y + 1;

        node_Array = new Node[SizeX, SizeY];

        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                bool IsWall = false;

                //벽인지 확인
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottom_left.x, j + bottom_left.y), 0.4f))
                {
                    if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
                    {
                        IsWall = true;
                    }
                }
                node_Array[i, j] = new Node(IsWall, i + bottom_left.x, j + bottom_left.y);
            }
        }
        //시작과 끝 노드 열린리스트, 닫힌리스트 최종경로 리스트 초기화 작업
        StartNode = node_Array[start_pos.x - bottom_left.x, start_pos.y - bottom_left.y];
        EndNode = node_Array[end_pos.x - bottom_left.x, end_pos.y - bottom_left.y];

        OpenList = new List<Node>();
        CloseList = new List<Node>();
        Final_NodeList = new List<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];
            for (int i = 0; i < OpenList.Count; i++)
            {
                //OpenListF가  CurrentNode F보다 작거나 같다면
                //H가 작은 것을 현재 노드로 설정
                if (OpenList[i].F <= CurrentNode.F && OpenList[i].H < CurrentNode.H)
                {
                    CurrentNode = OpenList[i];
                }
                //열린리스트에서 닫힌리스트로 옮기기
                OpenList.Remove(CurrentNode);
                CloseList.Add(CurrentNode);

                //CurrentNode가 도착지라면
                if (CurrentNode == EndNode)
                {
                    Node targetNode = EndNode;
                    while (targetNode != StartNode)
                    {
                        Final_NodeList.Add(targetNode);
                        targetNode = targetNode.Parentnode;
                    }
                    Final_NodeList.Add(StartNode);
                    Final_NodeList.Reverse();
                    return;
                }
                if (AllowDigonal)
                {
                    //대각선으로 움직이는 cost계산     ↗↘↖↙   
                    OpenListAdd(CurrentNode.x + 1, CurrentNode.y + 1);
                    OpenListAdd(CurrentNode.x + 1, CurrentNode.y - 1);
                    OpenListAdd(CurrentNode.x - 1, CurrentNode.y + 1);
                    OpenListAdd(CurrentNode.x - 1, CurrentNode.y - 1);
                }
                //직선으로 움직이는 cost계산    ↑ ↓ → ← 
                OpenListAdd(CurrentNode.x + 1, CurrentNode.y);
                OpenListAdd(CurrentNode.x - 1, CurrentNode.y);
                OpenListAdd(CurrentNode.x, CurrentNode.y + 1);
                OpenListAdd(CurrentNode.x, CurrentNode.y - 1);
            }


        }
    }

    public void OpenListAdd(int checkX, int checkY)
    {
        //상하좌우 범위를 벗어나지 않고 벽도 아니면서 닫힌리스트에 없어야 한다.
        if (checkX >= bottom_left.x && checkX < top_right.x + 1         //x가 bottomleft와 topright안에 있고
            && checkY >= bottom_left.y && checkY < top_right.y + 1
            && !node_Array[checkX - bottom_left.x, checkY - bottom_left.y].IsWall
            && !CloseList.Contains(node_Array[checkX - bottom_left.x, checkY - bottom_left.y]))
        {
            //대각선 허용 시(벽 사이로는 통과가 되지않음)
            if (AllowDigonal)
            {
                if (node_Array[CurrentNode.x - bottom_left.x, checkY - bottom_left.y].IsWall &&
                    node_Array[checkX - bottom_left.x, CurrentNode.y - bottom_left.y].IsWall)
                {
                    return;
                }
            }
            //코너를 가로질러 가지 않을 시 (이동 중 수직 수평 장애물이 하나라도 있으면 안됨)
            if (DontCrossCorner)
            {
                if (node_Array[CurrentNode.x - bottom_left.x, checkY - bottom_left.y].IsWall ||
                    node_Array[checkX - bottom_left.x, CurrentNode.y - bottom_left.y].IsWall)
                {
                    return;
                }
            }
            //check하는 노드를 이웃 노드에 넣고 직선은 10, 대각선은 14로 계산
            Node neighborNode = node_Array[checkX - bottom_left.x, checkY - bottom_left.y];
            int movecost = CurrentNode.G + (CurrentNode.x - checkX == 0 || CurrentNode.y - checkY == 0 ? 10 : 14);

            //이동 비용이 이웃노드 G보다 작거나, 열린리스트에 이웃노드가 없다면
            //G, H, parentNode 설정 후 열린리스트 주가
            if (movecost < neighborNode.G || !OpenList.Contains(neighborNode))
            {
                neighborNode.G = movecost;
                neighborNode.H = (Mathf.Abs(neighborNode.x - EndNode.x) + Mathf.Abs(neighborNode.y - EndNode.y)) * 10;

                neighborNode.Parentnode = CurrentNode;

                OpenList.Add(neighborNode);
            }
        }
    }

    public float gameTime = 0;

    private void Update()
    {
        gameTime += Time.deltaTime;
        if (gameTime > 0.2f)
        {
            PathFinding();
            gameTime = 0;
        }
    }
    private void OnDrawGizmos()
    {
        //씬 뷰의 Debug용으로 그림을 그릴 때 사용(Update처럼 계속 돌아감)
        if (Final_NodeList != null)
        {
            for (int i = 0; i < Final_NodeList.Count - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(Final_NodeList[i].x, Final_NodeList[i].y), new Vector2(Final_NodeList[i + 1].x, Final_NodeList[i + 1].y));
            }
        }
    }
}
