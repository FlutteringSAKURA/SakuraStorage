using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.11

// NOTE: //# 2D 게임 플레이어의 스타트 포인트 강제 이동
//#          1) 게임 시작시 특정한 위치로 플레이어를 이동시킨 후 해당 위치에서 시작하기 구현
//#          2) 
//#          3) 
//#          4) 
//#          5)

public class MoveToInitalPoint : MonoBehaviour
{
    // public GameObject _playerObj;
    public float moveSpeed = 5.0f;
    public Vector3 _initTargetPosition;
    public bool _getReady = false;

    private void Start()
    {
        _initTargetPosition = new Vector3(-0.05f, -6.0f, -0.18f);
    }

    public void MoveTo(Vector3 position)
    {
        _initTargetPosition = position;
    }

    private void Update()
    {
        if (!_getReady)
        {
            transform.position = Vector3.MoveTowards(transform.position, _initTargetPosition, moveSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)     //^ 스타트포인트 도달시 체크용 박스콜라이더 비활성화
    {
        if (other.gameObject.name == "StartPoint")
        {
            _getReady = true;
            Debug.Log("GET READY");
            //other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
