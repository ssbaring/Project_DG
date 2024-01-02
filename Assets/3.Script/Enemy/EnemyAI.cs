using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Node
{
    /*
        1. �� ��尡 ������ Ȯ��
        2. �θ���
        3. �� ����� x, y ��ǥ��
        4. f, h, g �� (f = h + g)
           h : ������. �� ���� ���� ��ֹ��� �����Ͽ� ��ǥ���� �Ÿ�
           g : ���۳����� �̵��ߴ� �Ÿ�
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

    //�밢�� �̿��� ������?
    public bool AllowDigonal = true;

    //�ڳʸ� ������������ ���� ��� �̵� �� ���� ���� ��ֹ��� �ִ��� �Ǵ�
    public bool DontCrossCorner = true;

    private int SizeX, SizeY;
    Node[,] node_Array;

    Node StartNode, EndNode, CurrentNode;

    List<Node> OpenList, CloseList;

    //public GameObject Player;

    public void SetPosition()       //�� ������
    {
        bottom_left = new Vector2Int((int)BottomLeft_obj.transform.position.x, (int)BottomLeft_obj.transform.position.y);
        top_right = new Vector2Int((int)TopRight_obj.transform.position.x, (int)TopRight_obj.transform.position.y);
        start_pos = new Vector2Int((int)StartPos.transform.position.x, (int)StartPos.transform.position.y);
        end_pos = new Vector2Int((int)EndPos.transform.position.x, (int)EndPos.transform.position.y);
    }

    public void PathFinding()
    {
        SetPosition();
        //����
        SizeX = top_right.x - bottom_left.x + 1;
        SizeY = top_right.y - bottom_left.y + 1;

        node_Array = new Node[SizeX, SizeY];

        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                bool IsWall = false;

                //������ Ȯ��
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
        //���۰� �� ��� ��������Ʈ, ��������Ʈ ������� ����Ʈ �ʱ�ȭ �۾�
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
                //OpenListF��  CurrentNode F���� �۰ų� ���ٸ�
                //H�� ���� ���� ���� ���� ����
                if (OpenList[i].F <= CurrentNode.F && OpenList[i].H < CurrentNode.H)
                {
                    CurrentNode = OpenList[i];
                }
                //��������Ʈ���� ��������Ʈ�� �ű��
                OpenList.Remove(CurrentNode);
                CloseList.Add(CurrentNode);

                //CurrentNode�� ���������
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
                    //�밢������ �����̴� cost���     �֢٢آ�   
                    OpenListAdd(CurrentNode.x + 1, CurrentNode.y + 1);
                    OpenListAdd(CurrentNode.x + 1, CurrentNode.y - 1);
                    OpenListAdd(CurrentNode.x - 1, CurrentNode.y + 1);
                    OpenListAdd(CurrentNode.x - 1, CurrentNode.y - 1);
                }
                //�������� �����̴� cost���    �� �� �� �� 
                OpenListAdd(CurrentNode.x + 1, CurrentNode.y);
                OpenListAdd(CurrentNode.x - 1, CurrentNode.y);
                OpenListAdd(CurrentNode.x, CurrentNode.y + 1);
                OpenListAdd(CurrentNode.x, CurrentNode.y - 1);
            }


        }
    }

    public void OpenListAdd(int checkX, int checkY)
    {
        //�����¿� ������ ����� �ʰ� ���� �ƴϸ鼭 ��������Ʈ�� ����� �Ѵ�.
        if (checkX >= bottom_left.x && checkX < top_right.x + 1         //x�� bottomleft�� topright�ȿ� �ְ�
            && checkY >= bottom_left.y && checkY < top_right.y + 1
            && !node_Array[checkX - bottom_left.x, checkY - bottom_left.y].IsWall
            && !CloseList.Contains(node_Array[checkX - bottom_left.x, checkY - bottom_left.y]))
        {
            //�밢�� ��� ��(�� ���̷δ� ����� ��������)
            if (AllowDigonal)
            {
                if (node_Array[CurrentNode.x - bottom_left.x, checkY - bottom_left.y].IsWall &&
                    node_Array[checkX - bottom_left.x, CurrentNode.y - bottom_left.y].IsWall)
                {
                    return;
                }
            }
            //�ڳʸ� �������� ���� ���� �� (�̵� �� ���� ���� ��ֹ��� �ϳ��� ������ �ȵ�)
            if (DontCrossCorner)
            {
                if (node_Array[CurrentNode.x - bottom_left.x, checkY - bottom_left.y].IsWall ||
                    node_Array[checkX - bottom_left.x, CurrentNode.y - bottom_left.y].IsWall)
                {
                    return;
                }
            }
            //check�ϴ� ��带 �̿� ��忡 �ְ� ������ 10, �밢���� 14�� ���
            Node neighborNode = node_Array[checkX - bottom_left.x, checkY - bottom_left.y];
            int movecost = CurrentNode.G + (CurrentNode.x - checkX == 0 || CurrentNode.y - checkY == 0 ? 10 : 14);

            //�̵� ����� �̿���� G���� �۰ų�, ��������Ʈ�� �̿���尡 ���ٸ�
            //G, H, parentNode ���� �� ��������Ʈ �ְ�
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
        //�� ���� Debug������ �׸��� �׸� �� ���(Updateó�� ��� ���ư�)
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
