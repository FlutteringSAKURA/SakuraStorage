using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 플레이어 유니티짱 제어 스크립트
//#          1) 이동 구현
//#          2) 포인트 구슬 먹기 + 먹을 때 사운드 재생
//#          3) 게임 오버시 게임 오버 씬 호출
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class UnityChangCtrl : MonoBehaviour
{
    public float _moveSpeed = 5.0f;
    public float _rotateSpeed = 360f;
    CharacterController _characterCtrl;
    Animator _unityChanAnimator;

    List<GameObject> _CoinBox = new List<GameObject>();

    // public bool _isStageClear = false;
    // public static UnityChangCtrl instance;

    // private void Awake() {
    //     if (UnityChangCtrl.instance == null)
    //     {
    //         instance = this;
    //     }    

    // }

//~ ---------------------------------------------------------
    private void Start()
    {
        _characterCtrl = GetComponent<CharacterController>();
        _unityChanAnimator = GetComponent<Animator>();
    }
//~ ---------------------------------------------------------
    private void Update()
    {
        //& X값 = Horizontal .. Y값 = 0.. Z값 = Vertical
        Vector3 _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //# NOTE: sqrMagnitude .. 벡터의 이동량이 ((가져와서, 입력값의 합) == 0.01f보다 크다는 것은 이동하고 있다는 뜻. .. 함수 X.. Only read = 읽기 전용
        //#       Slerp() .. 보간법.. 조작에 따라 입력되는 두 벡터 사이의 값을 새로 계산 .. ((= 키 입력을 받아 새로운 이동량을 산출) .. 두 지점을 연결해서 두 지점 사이의 거리를 파악하는 것
        //#       Angle() .. 각도 계산
        //#       LookAt() .. 월드포지션에서 벡터포인트로 회전해서 해당 방향을 바라보게하여 방향을 바꾸는 함수

        if (_direction.sqrMagnitude > 0.01f)    //^ 이동하고 있다는 조건
        {
            Vector3 _forward = Vector3.Slerp(transform.forward, _direction, _rotateSpeed * Time.deltaTime / Vector3.Angle(transform.forward, _direction));
            transform.LookAt(transform.position + _forward);


        }
        _characterCtrl.Move(_direction * _moveSpeed * Time.deltaTime);
        _unityChanAnimator.SetFloat("Speed", _characterCtrl.velocity.magnitude);

        if (GameObject.FindGameObjectsWithTag("Coin").Length == 0 && !GameManagerCtrl.instance._isStageClear)
        {
            GameManagerCtrl.instance.StageClear();
            GameManagerCtrl.instance._isStageClear = true;
        }
    }
//~ ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);

            SoundManagerCtrl.instance.GetCoinSoundPlay();

            Debug.Log("유니티짱이 동전을 획득했습니다.");
        }
        if (other.gameObject.tag == "Creature")
        {
            //& 게임 오버 씬 전환
            GameManagerCtrl.instance.GameOver();

        }
    }

}
