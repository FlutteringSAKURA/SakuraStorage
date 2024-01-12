using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    private static GridManager s_Instance = null;

    public static GridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (s_Instance == null)
                {
                    Debug.Log("Could not locate a Gridmanager " + "object. \n You have to have exactly " +
                    "one GridManager in the scene.");
                }
            }
            return s_Instance;
        }
    }

    //# Scene에서 GridManager 오브젝트를 찾아본 후 이미 있으면 이를 s_Instance static 변수에 할당해 관리

    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    private Vector3 origin = new Vector3();
    private GameObject[] obstacleList;
    public Node[,] nodes { get; set; }
    public Vector3 Origin
    {
        get { return origin; }
    }

    //# 열과 행의 수, 각 격자 타일의 크기, 격자와 장애물을 시각화하는데 필요한 bool변수, 격자에 있는 모든 node등 맵을 표현할 때 필요한 모든 변수 선언

    private void Awake()
    {
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }

    //맵 상의 모든 장애물을 찾는다
    private void CalculateObstacles()
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }
        if (obstacleList != null && obstacleList.Length > 0)
        {
            //맵에서 발견한 각 장애물을 리스트에 기록한다
            foreach (GameObject data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();
            }
        }
    }



    //# Obstacle 태그를 가진 모든 게임 오브젝트를 찾고 이를 obstacleList 속성에 추가
    //# CalculateObstacles 메소드에서 Node의 2차원 배열을 설정
    //# 일단 기본 속성으로 일반적인 node 오브젝트 생성 -> obstacleList 조사 후 이들의 위치를 열과 행 데이터로 변환 -> 해당 인덱스의 node를 장애물로 갱신
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.z += (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGird = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGird);
    }


    //# GetGridIndex 메소드는 주어진 좌표로부터 격자 셀 인덱스를 반환
    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
        {
            return -1;
        }
        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.z / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width && pos.x <= Origin.z + height && pos.z >= Origin.z);
    }

    //# GetRow, GetColumn 메소드는 주어진 인덱스로부터 격자 셀의 열과 행 데이터를 반환

    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }
    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }

    //# A star 클래스는 GetNeighbours 메소드를 사용해서 특정 node의 이웃 node들을 구함
    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);

        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);

        //아래
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //위
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //오른쪽
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //왼쪽
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }

    //# 현재 node의 왼쪽, 오른쪽, 위, 아래 4방향에 있는 이웃 Node를 가져온다
    //# AssignNeighbour 메소드 내부에서 장애물인지 검사
    //# 장애물이 아니면 해당 이웃 node를 참조 배열 목록인 neighbors에 추가

    private void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }

    //# 격자와 장애물 블록을 시각화 해줄 Debug 메소드

    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
        if (showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            if (obstacleList != null && obstacleList.Length > 0)
            {
                foreach (GameObject data in obstacleList)
                {
                    Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data.transform.position)), cellSize);
                }
            }
        }
    }

    public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);

        //수평 격자 라인을 그린다
        for (int i = 0; i < numRows; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
        //수직 격자 라인을 그린다
        for (int i = 0; i < numCols; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}
