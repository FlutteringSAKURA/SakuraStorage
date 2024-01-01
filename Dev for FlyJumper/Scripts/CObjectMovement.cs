using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObjectMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d; // 강체(물리엔진) 컴포넌트

    public float _speed; // 이동 속도

    public Vector2 _direction; // 이동 방향

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 지정한 방향으로 지정한 속도대로 이동시킴
        _rigidbody2d.velocity = _direction.normalized * _speed;
    }

    private void Update()
    {
        // 만약 게임이 종료된 상태라면
        if (CGameController.IsGameStop)
        {
            // 오브젝트의 이동을 중지함
            _rigidbody2d.velocity = Vector2.zero;
        }

        // 현재 기둥이 왼쪽 경계선을 넘어가면
        if (transform.position.x < -12f)
        {
            //파괴하라
            Destroy(gameObject);
        }
    }
}
