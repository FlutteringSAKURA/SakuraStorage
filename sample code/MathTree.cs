using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//# 참고 - Enum에서 SUCCESS는 첫 번째 항목이므로 평가가 되지 않은 노드의 기본값은 변하지 않음
//# -> 기본값 = 파란색 -> (평가가 이루어지지 않아도 = 파란색 유지)
public class MathTree : MonoBehaviour
{

    //* RUNNING = 노란색, SUCCESS = 파란색, FAILED = 빨간색
    public Color m_evaluating;
    public Color m_succeeded;
    public Color m_failed;

    //* 실제 노드 선언
    //# 루트 노드의 셀렉터 테스트 코드 (자식 노드 중 하나라도 조건을 만족하면 성공)
    public Selector m_rootNode;
    //# - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    //! 루트 노드의 시퀀스 테스트 코드 (전체 자식 노드 모두 조건을 만족할 때만 성공)
    //@public Sequence m_rootNode;
    //! - - - - - - - - - - - - - - - - - - - - - - - - - - - - =

    public ActionNode m_node2A;
    public Inverter m_node2B;
    public ActionNode m_node2C;
    public ActionNode m_node3;

    //* 박스에 대한 참조
    public GameObject m_rootNodeBox;
    public GameObject m_node2aBox;
    public GameObject m_node2bBox;
    public GameObject m_node2cBox;
    public GameObject m_node3Box;

    public int m_targetValue = 20;
    private int m_currentValue = 0;

    //* 테스트 중 필요한 내용을 출력하는데 사용할 UI Text
    [SerializeField]
    private Text m_valueLabel;


    //# 노드는 아래쪽부터 인스턴스화하고 차례로 자식들을 할당
    //# -> 자식 노드를 전달하지 않고는 부모를 인스턴스화 할 수 없기 때문
    //# m_node2A, m_node2C, m_node3 = 액션노드이므로 델리게이트를 전달해야 함
    //# m_node2B는 셀렉터가 되어야 하므로 하나의 자식 필요 -> m_node3이 자식 역할

    //# 이들 티어가 준비되면 모든 티어2 노드를 하나의 목록으로 만들어서 티어1 노드인 루트노드에 전달해야 함
    //# -> 루트 노드는 셀렉터로 인스턴스화 할 때 자식 목록을 필요로 하기 때문

    //# 모든 노드의 인스턴스화를 마치면 Evaluate() 메소드를 사용
    //# 루트 노드의 평가 -> UpdateBoxes() 메소드로 Box오브젝트의 색깔 갱신
    private void Start()
    {
        //* 가장 깊은 레벨의 노드는 Node 3으로 자식을 갖지 않음
        m_node3 = new ActionNode(NotEqualToTaget);

        //* 레벨2 노드 생성
        m_node2A = new ActionNode(AddTen);

        //* Node 2B는 셀렉터로 Node3을 자식으로 가지므로 생성자에 이를 전달
        m_node2B = new Inverter(m_node3);
        m_node2C = new ActionNode(AddTen);

        //* 마지막은 루트 노드로, 일단 여기에 전달할 자식 목록을 만듬
        List<NodeForBTs> rootChildren = new List<NodeForBTs>();
        rootChildren.Add(m_node2A);
        rootChildren.Add(m_node2B);
        rootChildren.Add(m_node2C);

        //* 그런 후 루트 노드 오브젝트를 만들고 여기에 목록 전달

        //# 루트 노드의 셀렉터 테스트 코드 (자식 노드 중 하나라도 조건을 만족하면 성공)
        m_rootNode = new Selector(rootChildren);
        //# - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        //! 루트 노드의 시퀀스 테스트 코드 (전체 자식이 모두 조건을 만족할 때만 성공)
        //@ m_rootNode = new Sequence(rootChildren);
        //! - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        m_valueLabel.text = m_currentValue.ToString();
        m_rootNode.Evaluate();
        UpdateBoxes();
    }


    private void UpdateBoxes()
    {
        //* 루트 노드 박스 갱신
        if (m_rootNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_rootNodeBox);
        }
        else if (m_rootNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_rootNodeBox);
        }

        //* 2A 노드 박스 갱신
        if (m_node2A.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2aBox);
        }
        else if (m_node2A.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2aBox);
        }

        //* 2B 노드 박스 갱신
        if (m_node2B.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2bBox);
        }
        else if (m_node2B.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2bBox);
        }

        //* 2C 노드 박스 갱신
        if (m_node2C.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node2cBox);
        }
        else if (m_node2C.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node2cBox);
        }

        //* 3 노드 박스 갱신
        if (m_node3.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(m_node3Box);
        }
        else if (m_node3.nodeState == NodeStates.FAILURE)
        {
            SetFailed(m_node3Box);
        }
    }

    //* 데코레이터의 자식 액션 노드에 전달하는 메소드
    //# 현재의 값이 목표값과 일지하지 않으면 성공 반환 -> 그 반대일 때는 실패 반환
    //* -> 부모 인버터 데코레이터는 노드가 반환한 것의 반대로 평가
    //* -> 만일 값이 일치하지 않으면 인버터 노드는 실패하고 반대면 성공함
    private NodeStates NotEqualToTaget()
    {
        if (m_currentValue != m_targetValue)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    //* 다른 두 액션 노드에 전달되는 메소드
    //* m_currentValue 변수에 10을 더함
    //# -> 그 값이 m_targetValue와 일치하는지 검사 
    //# -> 일치하면 SUCCESS, 불일치하면 FAILURE 평가
    private NodeStates AddTen()
    {
        m_currentValue += 10;
        m_valueLabel.text = m_currentValue.ToString();
        if (m_currentValue == m_targetValue)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    //@ Not Implementation / Not Using - - - - - - - - - - - - - - - - - - - 
    private void SetEvaluationg(GameObject m_rootNodeBox)
    {
        m_rootNodeBox.GetComponent<Renderer>().material.color = m_evaluating;
    }
    //@ - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    private void SetSucceeded(GameObject m_rootNodeBox)
    {
        m_rootNodeBox.GetComponent<Renderer>().material.color = m_succeeded;
    }

    private void SetFailed(GameObject m_rootNodeBox)
    {
        m_rootNodeBox.GetComponent<Renderer>().material.color = m_failed;
    }

    //! Not Implementation - - - - - - - - - - - - -
    public void Reset()
    {
        int currentLevel = Application.loadedLevel;
        Application.LoadLevel(currentLevel);
    }
    //! - - - - - - - - - - - - - - - - - - - - - - -
}
