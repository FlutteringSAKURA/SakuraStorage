using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Additional Code for NavMesh TEST
using UnityEngine.AI;
//! - - - - - - - - - - - - - - - - -
//# 게임을 시작하면 모든 NavMeshAgent 타입 개체를 찾아서 이를 NavMeshAgent배열에 저장
//# 마우스 클릭 이벤트 발생 -> RayCast로 광선과 충돌한 첫 오브젝트를 탐색 
//# 광선이 오브젝트와 충돌하면 마커의 위치 갱신 -> 각 NavMesh 에이전트의 destination 속성을 새로운 위치로 갱신

public class Target : MonoBehaviour
{
    //! Additional Code for NavMesh TEST
    private NavMeshAgent[] navAgents;
    //! - - - - - - - - - - - - - - - - - 

    public Transform targetMarker;

    //! Additional Code for NavMesh TEST    
    private void Start()
    {
        navAgents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
    }
    private void UpdateTargets(Vector3 targetPosition)
    {
        foreach (NavMeshAgent agent in navAgents)
        {
            agent.destination = targetPosition;
        }
    }
    //! - - - - - - - - - - - - - - - - -    
    private void Update()
    {
        int button = 0;

        //마우스를 클릭하면 충돌 지점을 얻는다.
        if (Input.GetMouseButtonDown(button))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                //! Additional and Change Code for NavMesh TEST
                UpdateTargets(targetPosition);
                //targetMarker.position = targetPosition + new Vector3(0, 5, 0);
                //! - - - - - - - - - - - - - - - - -
                targetMarker.position = targetPosition;
            }
        }
    }
}
