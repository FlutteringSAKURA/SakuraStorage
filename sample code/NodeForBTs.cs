using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//# This Script is the base Node for Behaviour Trees
//# 기반 기능 또는 최소 하나 이상의 시그니처를 제공하여 기능 확장을 가능하게 함
[System.Serializable]
public abstract class NodeForBTs
{

    //* 노드의 상태를 반환하는 델리게이트
    public delegate NodeStates NodeReturn();

    //* 노드의 현재 상태 (Failure, Success, Running 중 하나)
    protected NodeStates m_nodeState;

    // Protected인 m_nodeState를 위한 게터
    public NodeStates nodeState
    {
        get { return m_nodeState; }
    }

    //* 노드를 위한 생성자
    public NodeForBTs() { }

    //* 원하는 조건 세트를 평가하기 위해 이 메소드를 구현
    public abstract NodeStates Evaluate();

}

//! Additional Code
public enum  NodeStates
{
    SUCCESS,
    RUNNING,
    FAILURE
}