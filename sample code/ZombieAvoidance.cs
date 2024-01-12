using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Update 메소드에서 Zombie Girl의 이동에 필요한 정보인 마우스 클릭 위치를 가져옴
//! 이를 위해 카메라가 바라보는 방향으로 광선을 쏜 후 광선이 지면과 충돌한 지점을 목표 지점으로 정함
//! (현재위치 벡터 - 목표지점 벡터) = 방향 벡터
//! AvoidObstacles 메소드 호출 => 방향 벡터를 인자로 전달

//! Layer Mask를 준비하면 현재 개체의 위치와 전방 정보를 가지고 Physics.Raycast 메소드를 호출
//! 일정 거리 이내에 대해서만 장애물 충돌 처리를 할 예정이므로 minimumDistToAvoid 변수를 광선의 길이로 사용
//! 충돌 광선의 법선 벡터를 구한 후 force 벡터를 곱하고 이를 개체의 현재 방향에 더하면 새로운 방향 벡터를 얻을 수 있음

//! Update 메소드에서 이렇게 구한 새 방향으로 Zombie Girl을 회전시키고 속력 값에 따라 위치를 갱신

//! Other Scripts and Sakura Object -> disable -> Only This Script Enable -> TEST

public class ZombieAvoidance : MonoBehaviour
{

    public float speed = 0.4f;
    public float mass = 5.0f;
    public float force = 50.0f;
    public float minimumDistToAvoid = 10.0f;

    //Zombie Girl의 실제 속도
    private float curSpeed;
    private Vector3 targetPoint;

    //초기화 수행
    private void Start()
    {
        mass = 5.0f;
        targetPoint = Vector3.zero;
    }

    private void OnGUI()
    {
        GUILayout.Label("Click anywhere to move the Zombie Girl");
    }

    private void Update()
    {
        //Zombie Girl은 마우스 클릭으로 이동
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100.0f))
        {
            targetPoint = hit.point;
        }
        //목표 지점을 향하는 방향 벡터
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();

        //장애물 회피 적용
        AvoidObstacles(ref dir);

        //목표 지점에 도착하면 Zombie Girl을 멈춤
        if (Vector3.Distance(targetPoint, transform.position) < 3.0f)
        {
            return;
        }

        //속도에 deltaTime을 적용
        curSpeed = speed * Time.deltaTime;

        //목표 방향 벡터로 Zombie Girl을 회전
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        // Zombie Girl을 전진 시킴
        transform.position += transform.forward * curSpeed;

    }

    //* 장애물 회피를 위해 새 방향 벡터를 계산
    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;
        //layer8 (Obstacles)만 검사
        int layerMask = 1 << 8;

        //회피 최소거리 이내에서 장애물과 Zombie Girl이 충돌했는지 검사 수행
        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMask))
        {
            //새 방향을 계산하기 위해 충돌 지점에서 법선을 구함
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f; //Don't want to move in Y-Space

            //Zombie Girl의 현재 전방 벡터에 force를 더해 새로운 방향 벡터를 얻음			
            dir = transform.forward + hitNormal * force;
        }
    }
}
