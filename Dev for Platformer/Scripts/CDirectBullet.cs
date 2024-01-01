using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDirectBullet : CBullet
{
    public string _targetTag;   // 공격 타겟 태그 이름

    // 직선 총알 초기화(방향)
    public override void Init(bool isRightDir)
    {
        // 직선 총알 초기화
        base.Init(isRightDir);
        Move(); // 총알 이동 
    }

    // 총알 이동을 처리함
    public override void Move()
    {
        // 지정한 방향과 속도로 총알이 이동함
        _rigidbody2d.velocity = new Vector2(_dirValue * _maxSpeed,
            _rigidbody2d.velocity.y);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어 피격시
        if (collision.gameObject.tag == _targetTag)
        {
            collision.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);

            // 총알을 파괴함
            Destroy(gameObject);
        }

        // 벽 충돌 시
        base.OnCollisionEnter2D(collision);
    }
}
