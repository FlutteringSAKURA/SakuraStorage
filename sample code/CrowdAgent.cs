using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//# NavMeshAgent 타입의 컴포넌트를 필요로함
//# Start()함수에서 agent 변수에 지정

[RequireComponent(typeof(NavMeshAgent))]
public class CrowdAgent : MonoBehaviour
{

    public Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(1.5f, 3.5f);
        agent.SetDestination(target.position);
    }
}