using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! ZombieGirl이 현재 목적지에 도달할 때마다 새로운 임의의 지점을 지정된 영역 내에서 생성
//! 이후 Update메소드는 ZombieGirl을 회전시키고 새로운 목적지를 향해 이동시킴

public class Wander : MonoBehaviour
{

    private Vector3 tarPos;

    private float movementSpeed = 0.15f;
    private float rotSpeed = 1.0f;
    private float minX, maxX, minZ, maxZ;

    //초기화에 사용
    private void Start()
    {
        minX = -45.0f;
        maxX = 45.0f;

        minZ = -45.0f;
        maxZ = 45.0f;
        //돌아다닐 위치 얻기
        GetNextPosition();
    }
    private void Update()
    {
        //목적 지점 근처인지 검사
        if (Vector3.Distance(tarPos, transform.position) <= 5.0f)
        {
            GetNextPosition(); // generate new random position
        }
        //목적지 방향으로의 회전을 위한 Quaternion설정
        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);
        //회전과 Translate갱신
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
    }

    private void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
    }
}
