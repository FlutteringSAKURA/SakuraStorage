using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMovement : MonoBehaviour
{
    public Transform tr; // 운석의 변환 객체(컴포넌트)를 참조

    // 플레이어쉽을 향한 방향 벡터 정보 (정규화)
    public Vector3 nrDirection;

    public float speed; // 운석 이동 속도

    // 파괴 되는 기준 좌표 const 상수
    private const float DESTROY_POS_X = 7.5F;

    private const float DESTROY_POS_Y = 6F;

    // 최고 렌더링 되기 바로 전에 자동 호출되는 메소드 
    private void Start()
    {
        // 플레이어 게임 오브젝트를 검색함
        GameObject playerShip = GameObject.Find("PlayerShip");

        // 기본 이동 방향은 가운데(원점)
        Vector3 pos = Vector3.zero;

        // 플레이어쉽을 찾으면
        if (playerShip != null)
        {
            // 플레이어쉽의 위치를 저장함
            pos = playerShip.transform.position;
        }

        // 플레이어쉽을 향한 방향 벡터를 구함
        Vector3 direction = pos - tr.position;
        // 크기를 제거한(크기가1인) 방향 벡터를 만들어줌 (정규화 - 단위 벡터)
        nrDirection = direction.normalized;
    }

    private void Update()
    {
        // 운석 이동 (플레이어를 향해)
        // Transform.Translate(방향 * 속도(크기) * Time.deltaTime);
        tr.Translate(nrDirection * speed * Time.deltaTime);

        // 운석이 화면을 넘어가면
        if (tr.position.x < -DESTROY_POS_X ||
            tr.position.x > DESTROY_POS_X ||
            tr.position.y < -DESTROY_POS_Y ||
            tr.position.y > DESTROY_POS_Y)
        {
            // 운석을 파괴함
            Destroy(gameObject); // delete gameObject;
        }
    }
}