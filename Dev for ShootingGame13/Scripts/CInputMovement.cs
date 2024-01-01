using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// C++
// class CInputMovement : public MonoBehaviour
public class CInputMovement : MonoBehaviour
{
    // 경계 수치
    private const float LIMIT_POS_Y = 4.5f;

    private const float LIMIT_POS_X = 2.24f;

    // 비행 기 이동 속도
    public float _speed;

    private Vector2 direction;

    // 렌더러가 1프레임을 렌더링하기전에 호출해주는 이벤트 메소드
    // * 60프레임 게임의 경우 초당 60회 자동 호출됨
    // * 쉽게 렌더러가 화면을 그리기전에 할 무언가를 처리할 기회를 주는 메소드임
    private void Update()
    {
        // -1, 0, 1
        // 방향 키값 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 방향 벡터 생성
        direction.x = h;
        direction.y = v;

        // Transform.Translate(방향 벡터 * 속도 * 보간시간)
        // - 특정 위치로 지정한 방향 및 속도로 이동함

        // 비행기 이동
        transform.Translate(direction * _speed * Time.deltaTime);

        // 화면 경계 이동 제한 처리
        Vector2 pos = transform.position;
        if (Mathf.Abs(pos.x) > LIMIT_POS_X)
        {
            pos.x = Mathf.Sign(pos.x) * LIMIT_POS_X;
        }
        if (Mathf.Abs(pos.y) > LIMIT_POS_Y)
        {
            pos.y = Mathf.Sign(pos.y) * LIMIT_POS_Y;
        }
        transform.position = pos;
    }
}