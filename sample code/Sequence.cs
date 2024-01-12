using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# 시퀀스의 Evaluate()메소드는 전체 자식이 성공해야 참을 반환
//# -> 하나라도 실패하면 전체 시퀀스도 실패
//# -> FAILURE 조건을 가장 먼저 검사하여 보고
//# 만일 자식 노드 중 하나라도 RUNNING 상태라면 
//# -> 이를 해당 노드의 상태로 보고하고 부모 노드나 로직은 전체 트리를 다시 평가
public class Sequence : NodeForBTs
{

    //* 이 시퀀스에 속한 자식 노드들 
    private List<NodeForBTs> m_nodes = new List<NodeForBTs>();

    //* 초기 자식 목록을 반드시 제공해야 함
    public Sequence(List<NodeForBTs> nodes)
    {
        m_nodes = nodes;
    }

    //* 하나의 자식 노드라도 실패를 반환하면 전체 노드는 실패
    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach (NodeForBTs node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
            }
        }
        m_nodeState = anyChildRunning ? NodeStates.RUNNING :
        NodeStates.SUCCESS;
        return m_nodeState;
    }
}
