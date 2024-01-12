using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# 이 노드의 동작을 책임 지는 것은 m_action 델리게이트
//# 생성자는 NodeStates 열거형을 반환하는 시그니처에 맞는 메소드 전달을 요구
//# -> 조건만 만족하면 어떤 로직이던 구현 가능(기본상태는 FAILURE)
//# -> 기본상태를 SUCCESS or RUNNING으로 변경해도 무방

//# 이 클래스를 상속받아 확장하거나 클래스 자체를 수정해서 원하는대로 변경 가능
public class ActionNode : NodeForBTs
{

    //* 액션에 대한 메소드 시그니처
    public delegate NodeStates ActionNodeDelegate();

    //* 이 노드를 평가할 때 호출하는 델리게이트
    private ActionNodeDelegate m_action;

    //* 이 노드는 아무런 로직을 포함하지 않음
    //* 델리게이트 형태로 로직이 전달되어야 함
    //* 시그니처에 나와 있듯이 액션은 NodeState 열거형을 반환해야 함
    public ActionNode(ActionNodeDelegate action)
    {
        m_action = action;
    }

    //* 전달된 델리게이트로 노드를 평가하고 그에 맞는 상태 보고
    public override NodeStates Evaluate()
    {
        switch (m_action())
        {
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;
            case NodeStates.FAILURE:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;
            default:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
        }
    }
}
