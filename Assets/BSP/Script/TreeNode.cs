using UnityEngine;
using System.Collections.Generic;

public class TreeNode
{
//-----------------------------------------------------------------------------------변수 및 프로퍼티 선언------------------------------------------------------------------------------------------------
    TreeNode _leftChild;
    TreeNode _rightChild;
    TreeNode _parentNode;

    Room _room;

    Bridge _leafNodeBridge;
    Bridge _parentNodeBridge;

    bool _howDivide = false;// false 위아래로 true 좌우로

    bool _visited = false;
    int _dir;
    

    public TreeNode LeftChild { get => _leftChild; set => _leftChild = value; }
    public TreeNode RightChild { get => _rightChild; set => _rightChild = value; }
    public TreeNode ParentNode { get => _parentNode; set => _parentNode = value; }
    public Room Room { get => _room; }    
    public Bridge LeafNodeBridge { get => _leafNodeBridge; set => _leafNodeBridge = value; }

    public bool HowDivide { get => _howDivide; set => _howDivide = value; }
    public bool Visited { get => _visited; set => _visited = value; }
    public int Dir { get => _dir; set => _dir = value; }
    public Bridge ParentNodeBridge { get => _parentNodeBridge; set => _parentNodeBridge = value; }
    
//-----------------------------------------------------------------------------------변수 및 프로퍼티 선언------------------------------------------------------------------------------------------------
    public TreeNode(Vector2 downLeft, Vector2 downRight, Vector2 upLeft, Vector2 upRight)
    {
        _room = new Room(downLeft, downRight, upLeft, upRight);
    }
    
    public int DividingNode(Vector2 downLeft, Vector2 downRight, Vector2 upLeft, Vector2 upRight, int afterDir) // afterDir = 이전 dir이 홀,짝에 따라 다음은 반대에 경우가 증가되게 하기 위함
    {
        int rate = Random.Range(4, 7);

        if (afterDir % 2 == 0)
            Dir = Random.Range(1,4);
        else
            Dir = Random.Range(0,3);

        int distance;
        if(Dir %2 ==0) //dir 이 짝수이면 위아래로, 홀수이면 좌우로 분할 == (howDivide = false 이면 위 아래 , true 이면 좌 우)
        {
            distance = (int)(Room.DownLeft.y)+(Room.GetVerticalDistance * rate / 10);
            LeftChild = new TreeNode(new Vector2(Room.DownLeft.x, distance), new Vector2(Room.DownRight.x, distance), new Vector2(Room.UpLeft.x, Room.UpLeft.y), new Vector2(Room.UpRight.x, Room.UpRight.y));
            RightChild = new TreeNode(new Vector2(Room.DownLeft.x, Room.DownLeft.y), new Vector2(Room.DownRight.x, Room.DownRight.y), new Vector2(Room.DownLeft.x, distance), new Vector2(Room.DownRight.x, distance));
            LeftChild.HowDivide = false;
            RightChild.HowDivide = false;
        }
        else
        {
            distance = (int)(Room.DownLeft.x)+(Room.GetHorizontalDistance * rate / 10);
            LeftChild = new TreeNode(new Vector2(Room.DownLeft.x, Room.DownLeft.y), new Vector2(distance, Room.DownLeft.y), new Vector2(Room.UpLeft.x, Room.UpLeft.y), new Vector2(distance, Room.UpLeft.y));
            RightChild = new TreeNode(new Vector2(distance, Room.DownLeft.y), new Vector2(Room.DownRight.x, Room.DownRight.y), new Vector2(distance, Room.UpLeft.y), new Vector2(Room.UpRight.x, Room.UpRight.y));
            LeftChild.HowDivide = true;
            RightChild.HowDivide = true;
        }

        LeftChild.ParentNode = this;
        RightChild.ParentNode = this;
        MapCreator.Map.RoomTree.Add(LeftChild);
        MapCreator.Map.RoomTree.Add(RightChild);
        MapCreator.Map.RoomQueue.Enqueue(LeftChild);
        MapCreator.Map.RoomQueue.Enqueue(RightChild);

        return Dir;
    }

    public void MakeLeafNodeBridge()//같은 부모노드에 자식들이 둘다 리프 노드일 경우만 우선적으로 생성
    {
        if (this.ParentNode.LeftChild.LeftChild != null) // Queue(FIFO)로 작업했기 때문에 왼쪽 자식노드가 null인데 오른쪽 자식노드가 null이 아닌 경우는 없으니 왼쪽 자식 노드만 체크
            return;

        if (Visited)
            return;

        Bridge bridge = new Bridge(ParentNode.LeftChild.Room,ParentNode.RightChild.Room,ParentNode.LeftChild.HowDivide);

        this.ParentNode.LeftChild.LeafNodeBridge = bridge;
        this.ParentNode.RightChild.LeafNodeBridge = bridge;
        this.ParentNode.LeftChild.Visited = true;
        this.ParentNode.RightChild.Visited = true;

        MapCreator.Map.BridgeInfo.Add(bridge);
    }

    public static void MakeParentNodeBridge(TreeNode leftTree, TreeNode rightTree,bool howDivide) // LeafNode를 제외한 상위 형제노드들 연결
    {
        Queue<TreeNode> q = new Queue<TreeNode>();
        List<TreeNode> rightChild = new List<TreeNode>();

        TreeNode leftLeafNode= leftTree;
        TreeNode rightLeafNode= null;

        q.Enqueue(rightTree);        

        while (leftLeafNode.RightChild != null)
            leftLeafNode = leftLeafNode.RightChild;

        while(q.Count>0)
        {
            TreeNode node = q.Dequeue();
            if (node.LeftChild == null)
            {
                rightChild.Add(node);
                continue;
            }                

            q.Enqueue(node.LeftChild);
            q.Enqueue(node.RightChild);
        }

        for (int i = 0; i < rightChild.Count; i++) //(howDivide = false 이면 위 아래 , true 이면 좌 우)
        {
            if (!howDivide)
            {
                if ((rightChild[i].Room.DownRight.x >= leftLeafNode.Room.DownLeft.x && rightChild[i].Room.DownRight.x <= leftLeafNode.Room.DownRight.x) ||
                        (rightChild[i].Room.DownLeft.x >= leftLeafNode.Room.DownLeft.x && rightChild[i].Room.DownLeft.x <= leftLeafNode.Room.DownRight.x) ||
                        (leftLeafNode.Room.DownLeft.x >= rightChild[i].Room.UpLeft.x && leftLeafNode.Room.DownLeft.x <= rightChild[i].Room.UpRight.x) ||
                        (leftLeafNode.Room.DownRight.x >= rightChild[i].Room.UpLeft.x && leftLeafNode.Room.DownRight.x <= rightChild[i].Room.UpRight.x))
                {
                    rightLeafNode = rightChild[i];
                    break;
                }
            }
            else
            {
                if ((rightChild[i].Room.UpLeft.y >= leftLeafNode.Room.DownRight.y && rightChild[i].Room.UpLeft.y <= leftLeafNode.Room.UpRight.y) ||
                    (rightChild[i].Room.DownLeft.y >= leftLeafNode.Room.DownRight.y && rightChild[i].Room.DownLeft.y <= leftLeafNode.Room.UpRight.y) ||
                    (leftLeafNode.Room.DownRight.y >= rightChild[i].Room.DownLeft.y && leftLeafNode.Room.DownRight.y <= rightChild[i].Room.UpLeft.y) ||
                    (leftLeafNode.Room.UpRight.y >= rightChild[i].Room.DownLeft.y && leftLeafNode.Room.UpRight.y <= rightChild[i].Room.UpLeft.y))
                {
                    rightLeafNode = rightChild[i];
                    break;
                }
            }
        }
        

        Bridge bridge = new Bridge(leftLeafNode.Room, rightLeafNode.Room, howDivide);

        MapCreator.Map.BridgeInfo.Add(bridge);
    }

    
}
