using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveObj : MonoBehaviour
{
    public int _speed = 10;
    public float _lifeTime = 1f;

    public Vector2 _dir;

    // 총알이 활성화 됨
    private void OnEnable()
    {
        // 이동속도 설정
        GetComponent<Rigidbody2D>().velocity = _dir.normalized * _speed;
        Invoke("Die", _lifeTime);
    }

    // 총알이 비활성화됨
    private void OnDisable()
    {
        CancelInvoke("Die");
    }

    void Die()
    {
        // 발포 후 1초뒤에 오브젝트 풀에 오브젝트를 추가함
        CObjectPool.current.PoolObject(gameObject);
    }
}
