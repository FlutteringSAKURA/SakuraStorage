using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CParbolaBullet : CBullet
{
    public float _damageRange;      // 공격범위
    public float _shotForce;        // 발포 힘

    public LayerMask _targetMask;   // 공격 타겟 충돌 레이어

    // 직선 총알 초기화(방향)
    public override void Init(bool isRightDir)
    {
        // 직선 총알 초기화
        base.Init(isRightDir);
        Move(); // 총알이동
    }

    // 포탄 이동을 처리함
    public override void Move()
    {
        // 지정한 방향과 속도로 총알이 이동함
        _rigidbody2d.AddForce(new Vector2(1f * _dirValue, 1f) * _shotForce);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 스플레시 데미지를 처리함
        Collider2D collider = Physics2D.OverlapCircle(transform.position,
            _damageRange, _targetMask);
        if (collider != null)
        {
            collider.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
        }
        // 충돌 이펙트 처리함
        base.OnCollisionEnter2D(collision);
    }

}
