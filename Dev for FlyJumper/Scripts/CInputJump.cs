using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInputJump : MonoBehaviour
{
    // 강체 참조
    private Rigidbody2D _rigidBody2d;

    // private일지라도 Inspector에 수정이 가능하도록 표시됨
    [SerializeField]
    private float _riseForce; // 수직 상승 힘

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // _rigidBody2d.gravityScale = 0f;

        // transform.Translate(Vector2.up * 3f);

        // Rigidbody2D.velocity :
        // _rigidBody2d.velocity : Vector2.up * 3f;

    }

    private void Update()
    {
        // 비행체가 화면 상단을 넘으면
        if (transform.position.y > 4.95f)
        {
            CGameController.IsGameStop = true;
        }
        // 화면 하단을 비행기가 넘어가면
        if (transform.position.y < - 4.95f)
        {
            CGameController.IsGameStop = true;
            GameObject.Find("GameController").
                GetComponent<CGameController>().GameEnd(0f); // 게임 종료
        }
        // Debug.Log("현재 속도 : " + _rigidBody2d.velocity); 

        if (Input.anyKeyDown && !CGameController.IsGameStop)
        {
            // 이동 속도 초기화 (수직 정지)
            // _rigidBody2d.velocity = new Vector2(0f, 0f);
            _rigidBody2d.velocity = Vector2.zero;

            // 점프

            // RigidBody2D.AddForce(방향 * 힘)
            // - 지정한 방향으로 물리적인 힘을 가함
            _rigidBody2d.AddForce(Vector2.up * _riseForce);
        }
    }

}
