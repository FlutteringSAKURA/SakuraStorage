using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Steer 메소드는 이동할 목표지점 Vector3와 경로의 최종 웨이포인트 여부를 매개변수로 받음
//! 현재위치와 목표 위치 사이의 거리를 통해 남은 거리를 계산해야 함 
//! (목표위치 벡터 - 현재 위치 벡터) = 목표지점을 향하는 벡터 
//! 벡터의 크기 = 남은 거리

//! direction 속성만 남기기 위해 벡터를 정규화
//! 만일 최종 웨이포인트이면서 근접한 거리가 미리 지정한 10 이내라면 서서히 속도를 줄여 멈추도록 함
//! 또는 간단히 목표 폭소들 지정된 속력으로 갱신할 수도 있음
//! (목표 속도 - 현재 속도 벡터) = 새로운 조향 벡터
//! 새로운 조향 벡터 / 질량 = 가속도

//! Wonder Script -> disable -> This Script Enalbe -> TEST!!!
public class ZombieFollowing : MonoBehaviour
{

    public Path path;
    public float speed = 0.15f;
    public float mass = 5.0f;
    public bool isLooping = true;

    //Zombie Girl의 실제 속도
    private float curSpeed;

    private int curPathIndex;
    private float pathLength;
    private Vector3 targetPoint;

    Vector3 velocity;

    private void Start()
    {
        pathLength = path.Length;
        curPathIndex = 0;

        //Zombie girl의 현재 속도를 얻는다.
        velocity = transform.forward;
    }
    private void Update()
    {
        //속도를 통일
        curSpeed = speed * Time.deltaTime;
        targetPoint = path.GetPoint(curPathIndex);
        //목적지의 반지름 내에 들어오면 경로의 다음 지점으로 이동
        if (Vector3.Distance(transform.position, targetPoint) < path.Radius)
        {
            //경로가 끝나면 정지
            if (curPathIndex < pathLength - 1)
            {
                curPathIndex++;
            }
            else if (isLooping)
            {
                curPathIndex = 0;
            }
            else
            {
                return;
            }
        }
        //최종 지점에 도착하지 않았다면 계속 이동
        if (curPathIndex >= pathLength)
        {
            return;
        }
        //경로를 따라 다음 Velocity를 계산
        if (curPathIndex >= pathLength - 1 && !isLooping)
        {
            velocity += Steer(targetPoint, true);
        }
        else
        {
            velocity += Steer(targetPoint);
        }
        //속도에 따라 Zombie girl이동
        transform.position += velocity;
        //원하는 Velocity로 Zombie girl을 회전
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    //목적지로 벡터의 방향을 바꾸는 조향 알고리즘
    public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
    {
        //현재 위치에서 목적지 방향으로 방향 벡터를 계산한다
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        //원하는 Velocity를 정규화
        desiredVelocity.Normalize();

        //속력에 따라 속도를 계산
        if (bFinalPoint && dist < 10.0f)
        {
            desiredVelocity *= (curSpeed * (dist / 10.0f));
        }
        else
        {
            desiredVelocity *= curSpeed;
        }

        //힘 Vector 계산
        Vector3 steeringForce = desiredVelocity - velocity;
        Vector3 acceleration = steeringForce / mass;

        return acceleration;
    }
}
