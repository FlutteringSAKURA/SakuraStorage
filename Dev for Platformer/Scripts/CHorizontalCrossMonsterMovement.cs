using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHorizontalCrossMonsterMovement : CMonsterMovement
{
    // 이동 메소드 재정의
    public override void Move()
    {
        base.Move(); // 기본 이동 로직 수행

        // 시선 방향에 맞게 지정한 속도로 이동함
        _rigidbody2d.velocity = new Vector2((_characterState._isRightDir) ?
            _moveSpeed : -_moveSpeed, _rigidbody2d.velocity.y);
    }

    // Trigger 충돌 이벤트 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 터닝 포인트랑 충돌했다면
        if (collision.tag == "TurnPoint")
        {
            Flip(); // 방향 전환
        }
    }


}
