using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Astar 클래스를 사용 -> 시작 node ~ 목적지 node까지의 경로를 찾음
public class AstarTestCode : MonoBehaviour
{

    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray; //Astar의 FindPath 메소드가 반환하는 node배열을 저장하는데 사용

    GameObject objStartCube, objEndCube;
    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;   //경로탐색 사이의 시간 간격


    //Start와 End 태그를 가진 오브젝트 탐색 및 pathArray 배열 초기화 
    private void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        pathArray = new ArrayList();
        FindPath();
    }

    //시작과 도착 node의 위치가 변경된 경우 intervalTime속성에서 정의한 주기마다 새로운 경로 탐색
    //그런 후 FindPath 메소드 호출
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }

    //시작과 도착 게임 오브젝트를 가져옴 
    //GridManager, GetGridIndex 메소드를 사용 -> 새로운 node 생성
    //이후 시작 node와 도착 node 정보를 가지고 Astar.FindPath 메소드를 호출
    //결과로 반환된 배열 목록을 지역 변수 pathArray에 저장
    private void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        startNode = new Node(GridManager.instance.GetGridCellCenter(
            GridManager.instance.GetGridIndex(startPos.position)));

        goalNode = new Node(GridManager.instance.GetGridCellCenter(
            GridManager.instance.GetGridIndex(endPos.position)
        ));

        pathArray = Astar.FindPath(startNode, goalNode);
    }

    //경로를 시각적으로 보여주기 위함
    //pathArray배열의 정보를 순회하며 각 node를 연결하는 선을 그림
    private void OnDrawGizmos()
    {
        if (pathArray == null)
        {
            return;
        }
        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.yellow);
                    index++;
                }
            }
        }
    }
}
