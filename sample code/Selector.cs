using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# 셀렉터는 합성노드 -> 하나 이상의 자식 노드를 가지고 있음
//# 이 자식 노드들은 m_nodes List<NodForBts> 변수에 저장

//# Evaluate() 메소드를 통하여 모든 자식 노드를 돌며 개별 결과를 평가
public class Selector : NodeForBTs
{

    //* 이 셀렉터를 위한 자식 노드들
    protected List<NodeForBTs> m_nodes = new List<NodeForBTs>();

    //* 생성자는 자식 노드의 목록을 필요로 함
    public Selector(List<NodeForBTs> nodes)
    {
        m_nodes = nodes;
    }
    //* 자식 중 하나가 성공을 보고하면 셀렉터는 즉시 상위로 성공을 보고 함
    //* 만일 모든 자식이 실패하면 실패를 보고 함
    public override NodeStates Evaluate()
    {
        foreach (NodeForBTs node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    return m_nodeState;
                case NodeStates.RUNNING:
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeStates.FAILURE;
        return m_nodeState;
    }
}
