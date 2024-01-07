using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MapCreator : MonoBehaviour
{
//-----------------------------------------------------------------------------------변수 및 프로퍼티 선언------------------------------------------------------------------------------------------------
    public static MapCreator _instance; //싱글톤
    public static MapCreator Map { get { return _instance; } }
    GameObject floorParent;
    GameObject bridgeParent;    

    [SerializeField]
    Material floorMaterial;
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    List<GameObject> tilePrefabs;

    [SerializeField]
    int _maxDistance; //맵 전체 범위
    [SerializeField]
    int maxIterate; // 최대 반복 횟수
    int nowIterate=0;

    int dir = 0;
    int nodeNum = 0;

    Vector3 _startPos = new Vector3(0, 0, 0); // 맵 생성 위치에 시작점(기준점)

    private List<TreeNode> _roomTree = new List<TreeNode>();
    private Queue<TreeNode> _roomQueue = new Queue<TreeNode>(); // Queue를 통한 Dividing 노드 실시
    private List<TreeNode> _roomInfo = new List<TreeNode>();
    private List<Bridge> _bridgeInfo = new List<Bridge>();
    private List<GameObject> _list = new List<GameObject>();

    public List<TreeNode> RoomTree { get => _roomTree; private set => _roomTree = value; }    
    public Queue<TreeNode> RoomQueue { get => _roomQueue; private set => _roomQueue = value; }    
    public List<TreeNode> RoomInfo { get => _roomInfo; private set => _roomInfo = value; }        
    public List<Bridge> BridgeInfo { get => _bridgeInfo; private set => _bridgeInfo = value; }

//-----------------------------------------------------------------------------------변수 및 프로퍼티 선언------------------------------------------------------------------------------------------------  

    public void Awake()
    {
        if (_instance == null)
            _instance = this;

    }

    public void Start()
    {        
        Init();
        InitRoom();
        InitBridge();
        InitWallTile();

        StaticBatchingUtility.Combine(_list.ToArray<GameObject>(),gameObject);
    }

    public void Init()
    {
        floorParent = new GameObject("FloorParent");
        floorParent.transform.parent = gameObject.transform;
        bridgeParent = new GameObject("BridgeParent");
        bridgeParent.transform.parent = gameObject.transform;
        

        maxIterate = UnityEngine.Random.Range(3, maxIterate)*2;
        RoomQueue.Enqueue(new TreeNode(new Vector2(0, 0), new Vector2(_maxDistance, 0), new Vector2(0, _maxDistance), new Vector2(_maxDistance, _maxDistance)));
    }

    public void InitRoom()
    {
        while (nowIterate <= maxIterate) // 공간 분할
        {
            TreeNode divide = RoomQueue.Dequeue();
            dir = divide.DividingNode(divide.Room.DownLeft, divide.Room.DownRight, divide.Room.UpLeft, divide.Room.UpRight, dir);
            nowIterate++;
        }

        while (RoomQueue.Count > 0) // 방 생성
        {
            TreeNode node = RoomQueue.Dequeue();
            RoomInfo.Add(node);

            MakeRoom(node, nodeNum);
            nodeNum++;
        }
    }

    public void InitBridge()
    {
        foreach (TreeNode node in RoomInfo) // 형제 리프노드 간 연결
            node.MakeLeafNodeBridge();

        for (int i = 0; i < RoomTree.Count - RoomInfo.Count; i += 2) // 상위 형제노드 간 연결
        {
            TreeNode.MakeParentNodeBridge(RoomTree[i], RoomTree[i + 1], RoomTree[i].HowDivide);
        }


        foreach (Bridge bridge in BridgeInfo) // 다리 생성
            MakeBridge(bridge, bridgeParent.transform);
    }

    public void InitWallTile()
    {
        foreach (TreeNode node in RoomInfo)
        {
            Room room = node.Room;
            GenerateWall.MakeWall(wallPrefab, room.OutLine.VerticalLineL, room.OutLine.VerticalLineR, room.OutLine.HorizontalLineU, room.OutLine.HorizontalLineD, room.Go);
            GenerateTile.MakeTile(room, tilePrefabs);
        }

        foreach (Bridge bridge in BridgeInfo)
        {
            GenerateWall.MakeWall(wallPrefab, bridge.OutLine.VerticalLineL, bridge.OutLine.VerticalLineR, bridge.OutLine.HorizontalLineU, bridge.OutLine.HorizontalLineD, bridge.Go);
            GenerateTile.MakeTile(bridge, tilePrefabs);
        }
    }

    public void MakeBridge(Bridge bridge, Transform parent)
    {
        string s = String.Join("_", bridge.Connect);
        CreateMesh<Bridge>(bridge, parent,s);
        bridge.MakeOutLine();
        bridge.OutLine.DeleteBridgeWall(bridge);
    }

    public void MakeRoom(TreeNode node,int nodeNum) 
    {
        int rate = UnityEngine.Random.Range(8, 10);

        node.Room.ReduceRoom(rate);

        node.Room.RoomNum = nodeNum;
        string s = nodeNum.ToString();

        CreateMesh<Room>(node.Room,floorParent.transform,s);

        node.Room.MakeOutLine();
    }

    public void CreateMesh<T>(T room,Transform parent,string nodeNum) where T : Square // Room 정보를 토대로 Mesh작업 
    {
        Vector3 upLeft = new Vector3(room.UpLeft.x, 0, room.UpLeft.y);
        Vector3 upRight = new Vector3(room.UpRight.x, 0, room.UpRight.y);
        Vector3 downLeft = new Vector3(room.DownLeft.x, 0, room.DownLeft.y);
        Vector3 downRight = new Vector3(room.DownRight.x, 0, room.DownRight.y);

        Vector3[] vertices = new Vector3[]
        {
            upLeft,
            upRight,
            downLeft,
            downRight,
            
        };

        Vector2[] uvs = new Vector2[]
        {
            room.DownLeft,
            room.DownRight,
            room.UpLeft,
            room.UpRight,
        };

        int[] triangles = new int[]
        {
            0,1,2,2,1,3,
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        
        


        GameObject floor = new GameObject($"{typeof(T).Name}{nodeNum}", typeof(MeshFilter), typeof(MeshRenderer));        
        _list.Add(floor);
        
        floor.transform.position = _startPos;
        floor.transform.localScale = Vector3.one;
        floor.GetComponent<MeshFilter>().mesh = mesh;
        floor.GetComponent<MeshRenderer>().material = floorMaterial;
        floor.transform.parent = parent;
        floor.gameObject.layer = LayerMask.NameToLayer("Floor");

        GameObject wall = new GameObject("Wall");
        wall.transform.parent = floor.transform;
        room.Go = wall;

        
        GameObject tile = new GameObject("Tile");
        tile.transform.parent = floor.transform;
        room.TileGo = tile;        
    }

    
}
