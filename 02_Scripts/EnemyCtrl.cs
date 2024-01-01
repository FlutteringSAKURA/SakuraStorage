//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10
// Update: //@ 2023.10.12

// NOTE: //# 2D 게임 적 비행기 제어 스크립트
//#          1) 플레이어 움직이게 하기 + 뷰포트 좌표 내에서만 움직일 수 있도록 제한 하기
//#          2) 레이저 발사 구현
//#          3) 자동/수동모드 구현(기본 자동모드)
//#          4) 에너미 파괴시 랜덤 점수를 플레이어가 획득하게 구현
//#          5)

public class EnemyCtrl : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    Rigidbody2D _rigidbody2D;   //^ RigidBody2D를 코드로 붙이는 방법 활용 ((2) 리지바디 있어야 디스트로이존에서 반응해서 파괴가능
    int _enemyKilledScore = 100;

    private void Start()
    {
        //$ RigidBody2D를 코드로 붙이는 방법 활용 ((1)
        // gameObject.AddComponent<Rigidbody2D>();

        // ^ RigidBody 있는 상태로 RigidBody2D에 접근하여 Vector2값을 주어 이동시키는 법 ((2)
        // _rigidbody2D = GetComponent<Rigidbody2D>();
        // _rigidbody2D.velocity = new Vector2(0, -moveSpeed);

        //^ 에너미 파괴시 랜덤으로 점수 획득
        _enemyKilledScore = Random.Range(100, 201);     
    }
    private void Update()
    {
        EnemyMoveControl();     //% RigidBody 대신 물리력 표현 코드 ((3)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Lazer")
        {
            GameManagerCtrl.instance.SendMessage("AddScore", _enemyKilledScore);
            //! 레이저 재활용을 위해서는 비활성화 해야한다. .. Destroy를 하면 에러난다.
            other.gameObject.SetActive(false);  
        }
    }

    private void EnemyMoveControl()     
    {
        float yMove = moveSpeed * Time.deltaTime;
        transform.Translate(0, -yMove, 0);
    }
}
