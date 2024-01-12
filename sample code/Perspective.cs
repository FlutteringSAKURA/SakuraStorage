using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 센서 구현 관련 - 시각

//! 부모 Sense 클래스의 Start와 Update 메소드에서 각기 호출할 Initialize와 UpdateSense구현
//! DetectAspect 메소드에서 일단 플레이어와 Zombie Girl의 현재 방향 간의 각도를 검사
//! 만일 시야에 들어왔다면 플레이어 방향으로 광선을 발사한다.
//! 광선의 길이는 Zombie Girl이 볼 수 있는 거리 속성이다.
//! Raycast 메소드는 다른 오브젝트와 충돌하면 반환하고 특성 컴포넌트를 통해 특성 이름을 검사한다.
//! 플레이어가 볼 수 있는 범위 내에 있더라도 벽으로 막혀 있으면 보지 못함.

//! Test동안 OnDrawGizmos 메소드는 시야 범위를 표시하는 선을 그려 Zombie Girl이 어디까지 볼 수 있는지를 에디터 창에 표시

public class Perspective : Sense
{

    public int FieldOfView = 45;
    public int ViewDistance = 100;

    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialize()
    {
        //플레이어의 위치 찾기
        playerTrans = GameObject.FindGameObjectWithTag("Sakura").transform;
    }
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;
        //검출 범위에 있으면 시각 검사를 수행한다.
        if (elapsedTime >= detectionRate)
        {
            DetectAspect();
        }
    }

    //Zombie Girl의 시야 검사
    private void DetectAspect()
    {
        RaycastHit hit;
        //현재 위치로부터 플레이어 위치로의 방향
        rayDirection = playerTrans.position - transform.position;
        //Zombie Girl의 전방 벡터와 플레이어와 Zombie Girl사이의 방향 벡터간의 각도 검사
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            //플레이어가 시야에 들어왔는지 검사
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();
                if (aspect != null)
                {
                    //특성 검사
                    if (aspect.aspectName == aspectName)
                    {
                        print("I Can see The fresh meat Detected");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerTrans == null) return;
        Debug.DrawLine(transform.position + Vector3.up * 1, playerTrans.position, Color.red);
        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        //대략적인 시야 범위 시각화
        Vector3 leftRayPoint = frontRayPoint;
        leftRayPoint.x += FieldOfView * 0.5f;

        Vector3 rightRayPoint = frontRayPoint;
        rightRayPoint.x -= FieldOfView * 0.5f;

        Debug.DrawLine(transform.position + Vector3.up * 1, frontRayPoint, Color.blue);
        Debug.DrawLine(transform.position + Vector3.up * 1, leftRayPoint, Color.blue);
        Debug.DrawLine(transform.position + Vector3.up * 1, rightRayPoint, Color.blue);
    }
}
