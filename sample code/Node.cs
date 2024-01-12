using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! IComparable 상속을 위함
using System;
//! - - - - - - - - - - -

//# Node 클래스는 비용(G와 H), 장애물 여부 플래그, 위치, 부모 노드 등의 정보를 다루기 위한 속성을 포함
//# nodeTotalCost = G (시작 위치에서 현재 노드까지의 이동 비용)
//# estimatedCost = H (현재 노드에서 대상 목표 노드까지의 총 추정 비용)

//# 두 개의 간단한 생성자 메소드와 해당 노드의 장애물 여부 설정을 위한 wrapper 메소드를 하나 가짐
//# 그 후 CompareTo 메소드 구현

//# CompareTo 메소드를 override하기 위해 Node 클래스는 IComparable을 상속
//# 총 예상 비용을 기준으로 노드 배열의 목록을 정렬해야함

//# ArrayList 타입은 Sort 메소드를 포함
//# Sort는 기본적으로 리스트 내의 오브젝트(이 경우 Node 오브젝트)내에 구현된 CompareTo 메소드를 사용
//# 이 메소드는 esimatedCost 값에 기반을 두어 노드 오브젝트를 정렬하도록 구현

public class Node : IComparable
{
    public float nodeTotalCost;
    public float estimatedCost;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;

    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.parent = null;
        this.position = pos;
    }

    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        //음수 값은 오브젝트가 정렬된 상태에서 현재보다 앞에 있음을 의미
        if (this.estimatedCost < node.estimatedCost)
        {
            return -1;  //-1 반환
        }
        //양수 값은 오브젝트가 정렬된 상태에서 현재보다 뒤에 있음을 의미
        if (this.estimatedCost > node.estimatedCost)
        {
            return 1;   //1 반환
        }
        return 0;   //0 반환
    }

}
