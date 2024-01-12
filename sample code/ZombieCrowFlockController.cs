using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# Flock System Basic Test Code
//# For Flock - Leader

//# 자신의 위치를 갱신해 자신을 따르는 개별 boid 객체들이 어디로 가야 할지 알 수 있게 함
//# 이 오브젝트는 ZombieFlock Script의 origin 변수에서 참조
public class ZombieCrowFlockController : MonoBehaviour
{

    public Vector3 offset;
    public Vector3 bound;
    public float speed = 4.0f;

    private Vector3 initialPosition;
    private Vector3 nextMovementPoint;

    private void Start()
    {
        initialPosition = transform.position;
        CalculateNextMovementPoint();
    }

    //* 컨트롤러 오브젝트가 목적지 근처인지 검사
    //* 목적지 근처라면 CalculateNextMovementPoint() 메소드 사용 -> NextMovementPoint 변수 갱신
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation,
        Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);

        if (Vector3.Distance(nextMovementPoint, transform.position) <= 10.0f)
        {
            CalculateNextMovementPoint();
        }
    }

	//* 현재 위치와 바운더리 벡터 사이의 범위에서 다음으로 이동할 임의의 목적지를 찾음
    private void CalculateNextMovementPoint()
    {
        float posX = Random.Range(initialPosition.x - bound.x,
        initialPosition.x + bound.x);

        float posY = Random.Range(initialPosition.y - bound.y,
        initialPosition.y + bound.y);

        float posZ = Random.Range(initialPosition.z - bound.z,
        initialPosition.z + bound.z);

        nextMovementPoint = initialPosition + new Vector3(posX, posY, posZ);
    }
}
