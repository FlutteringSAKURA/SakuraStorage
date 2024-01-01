using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
// C++
class InputMovement : public MonoBehaviour
{
 public:
   Transform* tr;

   private:
        void Update();
}

void InputMoveMent::Update() {
   float h = Input::GetAxisRaw("Horizontal");
   float v = Input::GetAxisRaw("Vertical");

   tr->Translate(new Vector3(h, v, 0) * 5 * Time::deltaTime);
}
    
*/

public class InputMovement : MonoBehaviour
{
    // Transform 객체 참조

    // [차이점]
    // C#은 메소드 마다 접근지정자를 표시해 줘야 함
    public Transform tr;

    //경계 좌표 const 상수
    const float LIMIT_POS_X = 6f;

    const float LIMIT_POS_Y = 4.5f;

    public float speed;

    // [차이점]
    // C#은 메소드를 선언과 정의를 동시에 수행함
    // C#은 메소드 마다 접근 지정자를 표시해 줘야 함
    private void Update() // Update() : 렌더링 타이밍에 자동 호출되는 메소드
    {
        // 키입력을 받음
        // [차이점]
        // C#은 클래스명.메소드 형태로 Static 메소드를 호출함
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Transform 컴포넌트(객체)한테 이동을 부탁함 -> 이동 메소드 호출
        tr.Translate(new Vector3(h, v, 0f) * speed * Time.deltaTime);

        // 비행기의 x,y, 값 읽기
        //Debug.Log("현재 x위치 = > " + tr.position.x + ", y위치 => " + tr.position.y);

        // 비행기의 x, y값 저장
        // x, y를 3만큼 이동함
        // tr.position = new Vector2(3, 3);

        // [퀴즈]
        // 비행기를 이동하다 경계선을 넘으면 더이상 경계 위치를 벗어나지 않게 하시오.
        // (경계값 => x : -6 ~ 6, y : -4.5 ~ 4.5)
        // * 힌트 : 조건문(if)을 이용하시오.

        MoveAreaLimit();

    }

    private void MoveAreaLimit()
    {
        //경계 처리 수행 코드

        /* [ 첫번째 ]
         Vector3 pos = tr.position;
        if (pos.x < -LIMIT_POS_X) // -6f
        {
            tr.position = new Vector2(-LIMIT_POS_X, tr.position.y);
        }
        else if (pos.x > LIMIT_POS_X) // 6f
        {
            tr.position = new Vector2(LIMIT_POS_X, tr.position.y);
        }
        if (pos.y > LIMIT_POS_Y) // 4.5f
        {
            tr.position = new Vector2(tr.position.x, LIMIT_POS_Y);
        }
        else if (pos.y < -LIMIT_POS_Y)
        {
            tr.position = new Vector2(tr.position.x, -LIMIT_POS_Y);
        }
        */

        /* [ 두번째 ]
        if (tr.position.x < -LIMIT_POS_X)
        {
            tr.position = new Vector2(-LIMIT_POS_X, tr.position.y);
        }
        else if (tr.position.x > LIMIT_POS_X)
        {
            tr.position = new Vector2(LIMIT_POS_X, tr.position.y);
        }
        if (tr.position.y > LIMIT_POS_Y)
        {
            tr.position = new Vector2(tr.position.x, LIMIT_POS_Y);
        }
        else if (tr.position.y < -LIMIT_POS_Y)
        {
            tr.position = new Vector2(tr.position.x, -LIMIT_POS_Y);
        }
        */


        /* [ 세번째 ]
         Vector2 temp = tr.position;
        if (tr.position.x < -6)
        {
            temp.x = -6f;
        }
        else if (tr.position.x > 6)
        {
            temp.x = 6f;
        }
        if (tr.position.y < -4.5)
        {
            temp.y = -4.5f;
        }
        else if (tr.position.y > 4.5)
        {
            temp.y = 4.5f;
        }
        tr.position = temp;
    }
    */

        // Mathf.Abs(값) : 지정한 값을 절대값으로 반환함
        // Mathf.Sign(값) : 지정된 값이 음수면 -1을 0또는 양수면 1을 반환함

        Vector2 pos = tr.position;
        
        // - 6.4 -> Abs -> if (6.4 > 6)
        // 6.4 -> Abs -> if (6.4 > 6)
        if (Mathf.Abs(pos.x) > LIMIT_POS_X)
        {
            // -6.5 -> Sign -> -1 * 6 => -6
            // 6.4 -> Sign -> 1 * 6 => 6
            pos.x = Mathf.Sign(pos.x) * LIMIT_POS_X;
        }
        if (Mathf.Abs(pos.y) > LIMIT_POS_Y)
        {
            pos.y = Mathf.Sign(pos.y) * LIMIT_POS_Y;
        }
        if (Mathf.Abs(pos.y) > LIMIT_POS_Y)
        {
            pos.y = Mathf.Sign(pos.y) * LIMIT_POS_Y;
        }
        tr.position = pos;
        }
    }
