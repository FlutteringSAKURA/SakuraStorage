using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.16 
// Update: //@ 2023.10.17 

// NOTE: //# 3D 게임 플레이어 유니티짱 제어 스크립트 - Random_Maze_Target_Goal
//#          1) 이동 구현                                 // Completed:
//#          2) 애니매이션 구현                            // Completed:
//#          3) 공격 구현 (손 공격) + (발차기 공격)          // Completed:
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
// NOTE: (1)if .. (2)else if .. (3) else if // (1)[if .. else] ... (2)[if .. else] ...이 둘은 두가지 조건 뭉치
public class UnityChanController : MonoBehaviour
{
    public float _moveSpeed = 5.0f;
    public float _rotateSpeed_Y_Key = 360f;
    public float _mouseLotation_Y = 0.0f;       //& 마우스의 현재 값
    public float _rotation_Y_Mouse = 720f;      //& 마우스의 회전 값
    public bool _isAttack = false;
    public float _moveFront = 5.0f;     //& 전진
    public float _moveBack = -2.5f;     //& 후진

    public Transform _bulletGenPosition;        //& 총알이 발사될 좌표 위치 변수 선언
    public GameObject _bulletPrefab = null;       //& 총알 프리팹
    public GameObject _playerObject = null;       //& 플레이어 모델 프리팹
    public GameObject _bulletEffect = null;     //& 총알 충돌시 효과 프리팹
    // CharacterController _characterCtrl;
    Animator _unityChanAnimator;
    public bool _mouseLockFlag = true;

    public bool _attackFlag = false;

    public float _timeFlow = 0.0f;
    public float _coolTime = 1.0f;


    //~ ---------------------------------------------------------
    private void Start()
    {
        //_characterCtrl = GetComponent<CharacterController>();
        _unityChanAnimator = GetComponentInChildren<Animator>();
        //^ 위와 동일한 코드 
        //^ Animator _unityChanAnim = _playerObject.GetComponent<Animator>();
        //_bulletPrefab = GameObject.Find("Bullet");
    }
    //~ ---------------------------------------------------------
    private void Update()
    {

        _timeFlow += Time.deltaTime;

        //% 이동
        CheckMove();


        //@ 승리조건 (1).. TargetManager에서 승리조건 (1-1)사용
        // SUCCESS:
        // if (GameObject.FindGameObjectsWithTag("Target").Length <= 0)
        // {
            
        //     GameManagerSrc.SetStageClear();
        // }


    }
    //~ ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        //@ 승리조건 (2)
        if(other.gameObject.tag.Contains("Goal"))
        {
            GameManagerSrc.SetStageClear();
        }
    }

    //@ 인풋값을 가져와서 이동과 회전을 담당
    void CheckMove()
    {
        //# 회전 코드
        {
            //% 이 함수 내에서 움직이는 회전량
            float _addRotationY = 0f;
            if (Input.GetKey(KeyCode.Q))        //& 왼쪽 회전
            {
                _addRotationY = -_rotateSpeed_Y_Key;
                //Debug.Log("회전값" + _addRotationY);
            }
            else if (Input.GetKey(KeyCode.E))       //& 오른쪽 회전
            {
                _addRotationY = _rotateSpeed_Y_Key;
            }
            //% 마우스 이동량에 따른 회전량
            if (_mouseLockFlag)
            {   //& 마우스의 이동량을 얻어 각도값을 반영하는 것 (=_addRotationY)변수에 넣어줌
                _addRotationY += (Input.GetAxis("Mouse X") * _rotation_Y_Mouse);
                //Debug.Log("마우스 회전 좌표값" + _addRotationY);
            }
            //% 현재각도에 이 값을 더함
            _mouseLotation_Y += (_addRotationY * Time.deltaTime);
            //% 오일러각으로 입력 .. Y축 회전으로 캐릭터가 측면을 향하게 하는 코드
            transform.rotation = Quaternion.Euler(0, _mouseLotation_Y, 0);
        }
        //# 이동 코드
        Vector3 _addMovePosition = Vector3.zero;        //& Vector3.zero == new Vector3(0,0,0);
        {
            //& GetaxisRaw : 하나의 좌표값에서의 입력값 사용시 사용하는 함수.. ((축자체를 넣어줄때 사용하는 함수)
            Vector3 _vectorInput = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));       //^ 전진, 후진
            if (_vectorInput.z > 0)       //& 이동량이 들어왔다는 조건
            {
                _addMovePosition.z = _moveFront;
            }
            else if (_vectorInput.z < 0)
            {
                _addMovePosition.z = _moveBack;
            }

            _unityChanAnimator.SetFloat("SpeedZ", _addMovePosition.z);

            //* NOTE: 오른쪽의 값을 왼쪽으로 넘겨주는 개념 
            //*       Vector3는 zero+ 방향으로 생각함
            //!       ((짐벌락)) : 축과 축이 꼬이는 현상
            //% 이동 수행 코드 (좌표값에 _addMovePosition 값을 자연시간에 따른 값을 산출해 좌표값에 적용)
            transform.position += ((transform.rotation * _addMovePosition) * Time.deltaTime);
        }

        //# 공격 구현
        {
            //!bool _stopCheck = true;
            //% 발차기 총알
            if (Input.GetKeyDown(KeyCode.F) && _timeFlow > _coolTime && _addMovePosition.z == 0.0f)
            {
                _timeFlow = 0.0f;
                _attackFlag = true;
                //^ 애니메이션 설정(적용)
                _unityChanAnimator.SetBool("Attack", _attackFlag);
                //!_unityChanAnimator.SetBool("StopCheck", _stopCheck);
                _unityChanAnimator.SetInteger("StopCheck_int", 1);
                //_unityChanAnimator.SetTrigger("Attack_Trigger");
                Invoke("ShootBullet", 0.5f);
                //ShootBullet();
            }
            else
            {
                _attackFlag = false;
              //!  _stopCheck = false;
                //print("발차기 플래그 체크" + _attackFlag);
            }
            _unityChanAnimator.SetBool("Attack", _attackFlag);

            //% 손 총알
            // bool _shootAttackFlag;
            if (Input.GetButtonDown("Fire1"))
            {
                ShootBullet();
            }
            else
            {
                _attackFlag = false;
                //print("손 공격 플래그 체크" + _attackFlag);
            }
            _unityChanAnimator.SetBool("Shoot", _attackFlag);

        }

    }

    void ShootBullet()
    {
        if (_bulletGenPosition != null)      //& 총알 발사 위치가 있다면
        {
            _attackFlag = true;     //& 공격 개시
                                    //^ 총알 생성 위치 지정 (총알이 생성될 위치의 좌표값을 (_vecBulletPos)변수에 넣음)
            Vector3 _vecBulletPos = _bulletGenPosition.transform.position;
            _vecBulletPos += (transform.rotation * Vector3.forward);
            _vecBulletPos.y = 0.5f;

            //^ 총알 생성
            Instantiate(_bulletPrefab, _vecBulletPos, transform.rotation);

            print(_attackFlag);
        }
        else
        {
            _attackFlag = false;
            print(_attackFlag);
        }

    }


}
