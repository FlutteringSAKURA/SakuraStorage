using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.01 
// Update: //@ 2023.12.04 
// Update: //@ 2023.12.22 
//# NOTE: 팥쥐 행동 및 제어를 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class PatJi_Controller : MonoBehaviour
{
    [Header("[ PATJI MOVE REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Behaviour_Of_Patji_Move;

    public float _moveSpeed = 5.0f;
    public float _rotateSpeed_Y_Key = 360f;
    public float _mouseLotation_Y = 0.0f;       //& 마우스의 현재 값
    public float _rotation_Y_Mouse = 720f;      //& 마우스의 회전 값

    public float _rotationSpeed = 100.0f;

    public bool _isAttack = false;
    public float _moveFront = 5.0f;     //& 전진
    public float _moveBack = -2.5f;     //& 후진

    [Header("[ PATJI ANIMATION REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Patji_Animation;
    Animator _patJiAnimator;


    [Header("[ BOOL CONDITIONS REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _description_About_Bool_Conditions;

    public bool _isAlive = true;        //% 팥쥐 생존 조건 플래그

    public bool _mouseLockFlag = true;

    public bool _attackFlag = false;

    public float _timeFlow = 0.0f;
    public float _footStepSound_PlayDelayTime = 1.0f;
    ////public int _clickCount = 0;

    [Header("[ REFERENCES REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_References;
    GameObject _gameControlTower;


    public static PatJi_Controller instance;

    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ -------------------------------------------------------------------------------
    private void Start()
    {
        _gameControlTower = GameObject.FindWithTag("GameControlTower");
        _patJiAnimator = GetComponent<Animator>();
    }

    //~ -------------------------------------------------------------------------------
    private void Update()
    {
        _timeFlow += Time.deltaTime;
        //% 타임라인이 실행중일 때 혹은 팥쥐가 죽어있을 때, 팥쥐가 이동이 불가하도록하는 코드 
        if (_gameControlTower.GetComponent<GameControlTower>()._timelineActiveFlag || !_isAlive)
            return;

        MoveCheck();

        Action_Behaviour();

        PatJi_Animation_Activation();
    }

    //~ -------------------------------------------------------------------------------
    //@ 이벤트, 인터렉션 행동 관련 
    private void Action_Behaviour()
    {
        //# 스토리북 이벤트 관련 
        if (Input.GetKeyDown(KeyCode.L) && !EventSensorController.instance._nowListenStoyBookCheckFlag
                && EventSensorController.instance._listenStoryBookReadyCheckFlag)
        {
            ////Debug.Log("StoryBook input TEST");
            EventSensorController.instance.BeginStoryBookReady();


        }
        else if (Input.GetKeyDown(KeyCode.Escape) && EventSensorController.instance._listenStoryBookReadyCheckFlag
        && EventSensorController.instance._eventActiveCheckFlag && !EventSensorController.instance._nowListenStoyBookCheckFlag
        && !EventSensorController.instance._storyVoiceContinueReadyFlag && !SweetHomeSweet_Quest_Manager.instance._quest_8_Completed)
        {
            EventSensorController.instance.EndStoryBookEnvent();
        }

        //# The Door Darkness Cinema Event Related
        if (Input.GetKeyDown(KeyCode.U) && EventSensorController.instance._cinemaDoorCheckFlag
                        && !EventSensorController.instance._getKeyCheckFlag
                        && EventSensorController.instance._currentEventType == EventSensorController.EventType.CINEMA_DOOR)
        {
            EventSensorController.instance.NowNokeyEvent();

        }

        else if (Input.GetKeyDown(KeyCode.U) && EventSensorController.instance._getKeyCheckFlag
        && EventSensorController.instance._currentEventType == EventSensorController.EventType.CINEMA_DOOR
        && EventSensorController.instance._cinemaDoorCheckFlag)
        {
            EventSensorController.instance.GetKeyEvent();

        }

    }

    //@ 이동 조작 관련
    private void MoveCheck()
    {
        EventSensorController _eventSensorController;
        _eventSensorController = FindObjectOfType<EventSensorController>();
        //% 특정 이벤트가 발생 중일 때는 팥쥐의 이동과 회전이 모두 불가능하게 하는 코드
        if (EventSensorController.instance._eventActiveCheckFlag)
            return;

        //% 팥쥐 이동행동시 마우스 커서 비활성화
        GameControlTower.instance.MouseCursorInActive();

        if (GameControlTower.instance._timelineEndButMouseCursorDisplayFlag)
        {
            GameControlTower.instance.MouseCursorActive();
        }

        //# 회전 코드
        {
            //% 이 함수 내에서 움직이는 회전량
            float _addRotationY = 0f;
            if (Input.GetKey(KeyCode.Q))        //& 왼쪽 회전
            {
                _addRotationY = -_rotateSpeed_Y_Key;
                ////Debug.Log("회전값" + _addRotationY);
            }
            else if (Input.GetKey(KeyCode.E))       //& 오른쪽 회전
            {
                _addRotationY = _rotateSpeed_Y_Key;
            }

            //% 마우스 이동량에 따른 회전량
            if (_mouseLockFlag)
            {   //& 마우스의 이동량을 얻어 각도값을 반영하는 것 (=_addRotationY)변수에 넣어줌
                _addRotationY += (Input.GetAxis("Mouse X") * _rotation_Y_Mouse);
                ////Debug.Log("마우스 회전 좌표값" + _addRotationY);
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

            // TEMP: _patJiAnimator.SetFloat("SpeedZ", _addMovePosition.z);

            //* NOTE: 오른쪽의 값을 왼쪽으로 넘겨주는 개념 
            //*       Vector3는 zero+ 방향으로 생각함
            //!       ((짐벌락)) : 축과 축이 꼬이는 현상
            //% 이동 수행 코드 (좌표값에 _addMovePosition 값을 자연시간에 따른 값을 산출해 좌표값에 적용)
            transform.position += ((transform.rotation * _addMovePosition) * Time.deltaTime);


        }

        float _horizontal = Input.GetAxis("Horizontal");
        Vector3 _moveHorizontal_Direction = Vector3.right * _horizontal;
        transform.Translate(_moveHorizontal_Direction * _moveSpeed * Time.deltaTime);


        // Legacy:
        /*
                //% 특정 이벤트가 발생 중일 때는 팥쥐의 이동과 회전이 모두 불가능하게 하는 코드
                if (EventSensorController.instance._eventActiveCheckFlag)
                    return;

                //# 회전 코드 (마우스 커서가 비활성화 상태 일 때만..)
                if (!GameControlTower.instance._mouseCursorVisible)
                {
                    float _rotationY = transform.rotation.eulerAngles.y;
                    if (Input.GetKey(KeyCode.Q))
                    {
                        _rotationY -= _rotationSpeed * Time.deltaTime;
                    }
                    else if (Input.GetKey(KeyCode.E))
                    {
                        _rotationY += _rotationSpeed * Time.deltaTime;
                    }

                    //% 각도 보정
                    if (_rotationY > 180f)
                    {
                        _rotationY -= 360f;
                    }

                    _rotationY = Mathf.Clamp(_rotationY, -180, 180);
                    transform.rotation = Quaternion.Euler(0, _rotationY, 0);
                }
                else if (GameControlTower.instance._mouseCursorVisible)
                {

                    //% 이 함수 내에서 움직이는 회전량
                    float _addRotationY = 0f;
                    if (Input.GetKey(KeyCode.Q))        //& 왼쪽 회전
                    {
                        _addRotationY = -_rotateSpeed_Y_Key;
                        ////Debug.Log("회전값" + _addRotationY);
                    }
                    else if (Input.GetKey(KeyCode.E))       //& 오른쪽 회전
                    {
                        _addRotationY = _rotateSpeed_Y_Key;
                    }

                    //% 마우스 이동량에 따른 회전량
                    if (_mouseLockFlag)
                    {   //& 마우스의 이동량을 얻어 각도값을 반영하는 것 (=_addRotationY)변수에 넣어줌
                        _addRotationY += (Input.GetAxis("Mouse X") * _rotation_Y_Mouse);
                    }
                    //% 현재각도에 이 값을 더함
                    _mouseLotation_Y += (_addRotationY * Time.deltaTime);
                    //% 오일러각으로 입력 .. Y축 회전으로 캐릭터가 측면을 향하게 하는 코드
                    transform.rotation = Quaternion.Euler(0, _mouseLotation_Y, 0);


                }

                //# 이동 코드 
                // Update:
                float _vertical = Input.GetAxis("Vertical");
                float _horizontal = Input.GetAxis("Horizontal");
                Vector3 _moveDirection = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);
                transform.Translate(_moveDirection.normalized * _moveSpeed * Time.deltaTime);

        */
    }

    //@ 타일 바닥 걷는 소리 재생 및 시간 조정 함수 
    private void FootStepSoundPlay()
    {
        if (FloorCheck.instance._woodType)
        {
            if (_timeFlow > _footStepSound_PlayDelayTime)
            {
                _timeFlow = 0.0f;
                AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.SWEETHOMESWEET_WALK_STEP_SOUND_WOODEN_FLOOR);
                StartCoroutine(SoundDelay());

            }
            ////Debug.Log("나무바닥");
        }
        else if (FloorCheck.instance._tileType)
        {
            if (_timeFlow > _footStepSound_PlayDelayTime)
            {
                _timeFlow = 0.0f;
                AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.SWEETHOMESWEET_WALK_STEP_SOUND_TILE_FLOOR);
            }
            ////Debug.Log("타일바닥");
        }

    }

    IEnumerator SoundDelay()
    {
        yield return new WaitForSeconds(0.4f);
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.SWEETHOMESWEET_WALK_STEP_SOUND_WOODEN_FLOOR_02);
    }

    //@ 팥쥐의 애니메이션 호출 함수 
    private void PatJi_Animation_Activation()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _patJiAnimator.SetBool("walkForward", true);
            FootStepSoundPlay();

        }
        else if (Input.GetKey(KeyCode.S))
        {
            _patJiAnimator.SetBool("walkBack", true);
            FootStepSoundPlay();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _patJiAnimator.SetBool("walkLeft", true);
            FootStepSoundPlay();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _patJiAnimator.SetBool("walkRight", true);
            FootStepSoundPlay();
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _patJiAnimator.SetBool("turnLeft", true);
            FootStepSoundPlay();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            FootStepSoundPlay();
            _patJiAnimator.SetBool("turnRight", true);

        }

        else
        {
            CandleHold_Idle_AnimatinClip_Setting();

        }
    }

    public void CandleHold_Idle_AnimatinClip_Setting()
    {
        _patJiAnimator.SetBool("walkForward", false);
        _patJiAnimator.SetBool("walkBack", false);
        _patJiAnimator.SetBool("walkLeft", false);
        _patJiAnimator.SetBool("walkRight", false);
        _patJiAnimator.SetBool("turnLeft", false);
        _patJiAnimator.SetBool("turnRight", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Story_Sensor")
        {
            EventSensorController.instance._currentEventType = EventSensorController.EventType.STORY_BOOK;
        }
        else if (other.gameObject.name == "CinemaDoor_Sensor")
        {
            EventSensorController.instance._currentEventType = EventSensorController.EventType.CINEMA_DOOR;
        }
    }
}
