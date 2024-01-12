using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar
{
    //* PriorityQueue인 openList와 closedList를 선언
    public static PriorityQueue closedList, openList;
    
    //* 두 node 사이의 비용 계산을 위한 HeuristicEstimateCost 메소드 
    //* (하나의 위치 벡터 - 나머지) = 둘 사이의 방향 벡터 = 현재 node와 목적지 node 사이의 거리
    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }
    
    //# 1.openList에서 첫 node를 가져옴 (openList의 node는 항상 정렬된 상태임 -> 첫 node는 항상 목적지 node까지의 추정 비용이 가장 적음)
    //# 2.현재 node가 이미 목적지 node인지 검사 -> 만일 그렇다면 while 반복문을 탈출하고 path 배열을 만든다.
    //# 3.현재 node의 이웃 node를 저장할 배열 리스트를 생성 -> 격자에서 이웃을 가져오기 위해서는 GetNeighbours 메소드를 사용
    //# 4.이웃 배열의 모든 node에 대해 이미 closedList에 있는지 검사 
    //! -> 만일 없다면 비용을 계산하고 node 속성을 새로 계산한 값으로 부모 노드 데이터와 함께 갱신하고 openList에 추가
    //# 5.현재 node를 closedList에 넣고 이를 openList에서 제거 -> 1단계로 돌아감

    public static ArrayList FindPath(Node start, Node goal)
    {
        //열린 리스트와 닫힌 리스트를 초기화하고 시작 노트 부터 시작하면서 이를 열린 리스트에 넣는다.
        //그 후 열린 리스트를 대상으로 처리 시작
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        closedList = new PriorityQueue();
        Node node = null;
        while (openList.Length != 0)
        {
            node = openList.First();
            //현재 node가 목적지 node인지 확인
            if (node.position == goal.position)
            {
                return CalculatePath(node);
            }
            //이웃 node를 저장하기 위해 ArrayList를 생성
            ArrayList neighbours = new ArrayList();

            GridManager.instance.GetNeighbours(node, neighbours);

            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];
                if (!closedList.Contains(neighbourNode))
                {
                    float cost = HeuristicEstimateCost(node, neighbourNode);

                    float totalCost = node.nodeTotalCost + cost;
                    float neighbournodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost + neighbournodeEstCost;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }
            //현재 node를 closedList에 추가
            closedList.Push(node);
            //openList에서는 제거
            openList.Remove(node);
        }
        if (node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }
        return CalculatePath(node);
    }

    //# openList에 더 이상 node가 없고 유효한 경로가 존재한다면 현재 node는 대상 node위치에 놓임
    //# 현재 node를 매개변수로 CalculatePath 메소드 호출

    //# CalculatePath 메소드는 각 node의 부모 node 오브젝트를 추적해 배열 리스트를 만듬
    //# 리스트는 대상 node ~ 시작 node까지의 배열 목록 (그러나 실제 필요한 것은 시작 ~ 대상이므로 => Reverse 메소드 호출)
    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }
}
