using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# 데코레이터는 자식이 하나 -> List<NodeForBTs>가 아닌 하나의 노드 변수 m_node를 가짐
//# ->이 노드는 생성자를 통해 전달 (빈 생성자를 먼저 부르고 이후에 별도의 메소드를 통해 자식노드를 전달해도 무방)
public class Inverter : NodeForBTs
{
    //* 평가할 자식 노드
    private NodeForBTs m_node;
    public NodeForBTs node
    {
        get { return m_node; }
    }
    
    //* 생성자는 이 인버터 데코레이터가 감쌀 자식 노드를 필요로 함
    public Inverter(NodeForBTs node)
    {
        m_node = node;
    }

    //* 자식이 실패하면 성공을 보고하고 자식이 성공하면 실패를 보고, RUNNING은 그대로 보고
    public override NodeStates Evaluate()
    {
        switch (m_node.Evaluate())
        {
            case NodeStates.FAILURE:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;
        }
        m_nodeState = NodeStates.SUCCESS;
        return m_nodeState;
    }
}
