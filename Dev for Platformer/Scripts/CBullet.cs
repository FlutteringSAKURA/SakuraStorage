using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CBullet : MonoBehaviour
{
    public float _dirValue;                 // 총알 발사 방향
    public float _maxSpeed;                 // 총알 속도
    protected Rigidbody2D _rigidbody2d;     // 물리 엔진 컴포넌트 참조

    public GameObject _destroyEffectPrefab; // 총알이 파괴될때 생성할 이펙트
    public float _destroyEffectDestroyTime;

    // 총알 초기화
    public virtual void Init(bool isRightDir)
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _dirValue = (isRightDir) ? 1 : -1;
    }

    // 이동 메소드(강제)
    public abstract void Move();

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (_destroyEffectPrefab != null)
            {
                GameObject effect = Instantiate(_destroyEffectPrefab, transform.position,
                    Quaternion.identity);
                Destroy(effect, _destroyEffectDestroyTime);
            }

            Destroy(gameObject);
        } 
    }
}
