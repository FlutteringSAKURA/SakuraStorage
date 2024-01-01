//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Update: //@ 2023.11.03 
// Update: //@ 2023.11.06 

// NOTE: //# 3D 게임 - 순찰 컨트롤 스크립트
//#          1) 순찰 적용 + 랜덤 순찰 적용

//~ ------------------------------------------------------------------------
public class MoveAgent : MonoBehaviour
{
    //% 선언 + 생성
    //public List<Transform> _wayPoints = new();  //& 모든 순찰 포인트
    public List<Transform> _wayPoints;
    //% 순찰 지점의 배열 인덱스 값
    int _nextIndex = 0;

    NavMeshAgent _naveMeshAgent;
    Transform _wayPointTransform;

    readonly float _patrolWalkSpeed = 1.5f;
    readonly float _traceRunSpeed = 3.7f;
    Animator _cyberCreatureAnimator;

    // TEMP:
    float 
    _rotDamping = 1.0f;


    //@ 순찰 유무 체크 
    private bool _isPatrolFlag;  //% 순찰 유무 체크
    public bool IsPatrolFlag    //% 프라퍼티화
    {
        get { return _isPatrolFlag; }   //# 순찰여부를 체크하여 값을 밖으로 리턴시킴.. [읽기 전용]
        set                             //# 순찰여부 값을 밖에서 받아서 셋팅..       [쓰기 전용]
        {
            _isPatrolFlag = value;

            if (_isPatrolFlag)
            {
                _naveMeshAgent.speed = _patrolWalkSpeed;
                _rotDamping = 1.0f;
                MoveWayPoint();     //# 다음 순찰 위치좌표로 이동
            }
        }
    }

    //@ 추적 유무 체크 
    private Vector3 _traceTarget;
    public Vector3 TraceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            _naveMeshAgent.speed = _traceRunSpeed;
            _rotDamping = 10.0f;     //& 추적상태의 계수
            TraceToTarget(_traceTarget);
        }
    }

    public void TraceToTarget(Vector3 traceTargetPos)
    {
        _naveMeshAgent.destination = traceTargetPos;    //% NaveMeshAgent의 목적지 설정
        _naveMeshAgent.isStopped = false;

        // Update: //@ 2023.11.06 
        _naveMeshAgent.stoppingDistance = this.gameObject.GetComponent<CyberCreatureController>()._attackDistance;

    }

    //@ 순찰 및 추적을 정지시키는 함수 
    public void Stop()
    {
        _naveMeshAgent.isStopped = true;
        _naveMeshAgent.velocity = Vector3.zero;
        _isPatrolFlag = false;
    }


    //@ NavMeshAgent 프라퍼티
    public float _blendSpeed
    {
        get { return _naveMeshAgent.velocity.magnitude; }       //& NveMeshAgent가 가진 움직임 값
    }


    //~ ------------------------------------------------------------------------
    private void Start()
    {
        // Legacy:
        // GameObject _group = GameObject.Find("WayPointGroups");
        var _group = GameObject.Find("WayPointGroups");
        if (_group != null)      //& _group이 없는 것이 아니라면 == 있다면
        {
            //^ _group의 자식들에게 있는 위치좌표값가져옴
            _group.GetComponentsInChildren<Transform>(_wayPoints);

            //^ _wayPoints 배열의 0번째를 제거
            _wayPoints.RemoveAt(0);
            // Update: 
            //# 각각의 병사들이 이동할 위치를 임의로 설정
            _nextIndex = Random.Range(0, _wayPoints.Count);
        }

        _wayPointTransform = GetComponent<Transform>();
        _naveMeshAgent = GetComponent<NavMeshAgent>();
        _cyberCreatureAnimator = GetComponent<Animator>();

        //% 자동회전 기능 비활성화
        //_naveMeshAgent.updateRotation = false;

        //% 목적지에 다다르면 속도를 줄이는 옵션을 비활성화
        _naveMeshAgent.autoBraking = false;

        //% 순찰목적지점 활성화 함수 콜백
        // Legacy:
        //MoveWayPoint();
        this.IsPatrolFlag = true;

        ////Debug.Log("순찰포인트 지점 : " + _wayPoints.Count);


    }

    //~ ------------------------------------------------------------------------

    //@ 순찰목적지점 활성화 함수
    private void MoveWayPoint()
    {
        //% 이동 목적지까지 경로 계산..isPathStale 현재경로가 유효하지 않은 경우 true..
        if (_naveMeshAgent.isPathStale) return;     //& 경로를 계산하는 동안은 움직이지 않음

        //% NaveMeshAgent의 목적지를 _wayPoints의 [_nextIndex].위치로 가는 코드
        _naveMeshAgent.destination = _wayPoints[_nextIndex].position;

        // Update: //@ 2023.11.06 
        _naveMeshAgent.speed = _patrolWalkSpeed;
        //# ---------------------------------------

        _naveMeshAgent.stoppingDistance = 0.0f;

        //% 이동하려면 stop을 false시킴
        _naveMeshAgent.isStopped = false;

    }

    //~ ------------------------------------------------------------------------
    private void Update()
    {
        //% NaveMeshAgent가 이동하는 방향 벡터를 쿼터니언 타입의 각도로 전환
        // if(!_naveMeshAgent.isStopped)
        // {
        //      Quaternion _rotation = Quaternion.LookRotation(_naveMeshAgent.desiredVelocity);
        //      gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, _rotation ,Time.deltaTime * _rotDamping);

        // }

        // Update: //@ 2023.11.06 
        //% 순찰모드가 아닐 때는 retuen;
        if (!_isPatrolFlag) return;
        //# -------------------------

        //if (_naveMeshAgent.remainingDistance <= 1.0f)
        //#  NOTE: sqrMagnitude는 두 값이 필요. // magnitude는 하나의 값 필요
        //% NaveMEshAgent의 velocity 값이 0.2 * 0.2 이상이고 .. 목적지점과 남은 거리가 0.5 이하
        //% velocity값은 어느정도 이동속도값이 있어야 조건을 만족하는데, remainingDistance가 0.05처럼 매우 가까울때는 velocity값이 0에 근접해 조건이 불성립할 수도 있다.
        //% 이 때는 velocity 조건 값을 빼면 된다.
        if (_naveMeshAgent.velocity.sqrMagnitude >= 0.2f * 0.2f &&
        _naveMeshAgent.remainingDistance <= 0.5f)
        {
            //& 0을 10으로 나누면 = 0 .. 1을 10으로 나누면 = 1.. 
            //& 나머지 연산을 의미 (= %) 0 % 10 = 0 // 1 % 10 = 1 // 8 % 8 =0
            // Legacy:
            //_nextIndex = ++_nextIndex % _wayPoints.Count;
            _nextIndex = Random.Range(0, _wayPoints.Count);

            MoveWayPoint();
            ////Debug.Log("순찰포인트 : " + _nextIndex);
        }
    }
}
