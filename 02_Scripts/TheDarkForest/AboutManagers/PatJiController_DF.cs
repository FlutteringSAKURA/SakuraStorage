using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.20 
// Update: //@ 2023.12.28 
// Update: //@ 2024.01.02 
//# NOTE: The Dark Forest - PatJi Character의 Control 전반을 관리하기 위한 컨트롤 타워 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class PatJiController_DF : MonoBehaviour
{
    [Header("[ PATJI MOVE REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Behaviour_Of_Patji_Move;

    public float _moveSpeed = 5.0f;
    public float _rotateSpeed_Y_Key = 360f;
    public float _mouseLotation_Y = 0.0f;       //& 마우스의 현재 값
    public float _rotation_Y_Mouse = 720f;      //& 마우스의 회전 값

    public float _rotateValue = 50.0f;

    public float _rotationSpeed = 100.0f;

    public bool _isAttack = false;
    public float _moveFront = 5.0f;     //& 전진
    public float _moveBack = -2.5f;     //& 후진

    // TEST:
    public float _directionRotateSpeed = 2.0f;
    public float _speed = 2.0f;


    //!

    [Header("[ PATJI JUMP REALATED INFO ]")]
    [Tooltip("Jump 관련")]
    public float _jumpForce = 5.0f;
    private Rigidbody _rigidBody;

    [Header("[ PATJI CLIMB REALATED INFO ]")]
    [Tooltip("기어오르기 관련")]
    RaycastHit _hit;
    public bool _climbMode = false;
    public float _climbSpeed = 1.3f;


    [Header("[ PATJI ANIMATION REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Patji_Animation;
    Animator _patJiAnimator;

    [Space(10f)]
    [Tooltip("Advanced Animation Activation Bool Condition")]
    public bool _walkMode_Flag = false;
    public bool _runMode_Flag = false;
    public bool _audioClipNotFinish = false;
    public bool _jumpMode_Flag = false;
    public bool _thisIsFallenWood = false;


    [Header("[ BOOL CONDITIONS REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _description_About_Bool_Conditions;

    public bool _isAlive = true;        //% 팥쥐 생존 조건 플래그
    public bool _mouseLockFlag = true;
    public bool _attackFlag = false;
    public bool _readyCompleted = false;
    public float _walkTimeFlow = 0.0f;
    public float _runTimeFlow = 0.0f;
    public float _footStepSound_PlayDelayTime = 1.0f;


    [Header("[ REFERENCES REALATED INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_References;
    GameObject _gameControlTower;

    public static PatJiController_DF instance;

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
        _rigidBody = GetComponent<Rigidbody>();

    }

    //~ -------------------------------------------------------------------------------
    private void Update()
    {
        //% 어둠의 숲이 시작되지 않으면 플레이어 작동 불가
        if (!GameControlTower_DF.instance._darkForestStartedFlag)
            return;
        //% 팥쥐가 죽으면 작동 불가
        if (!_isAlive)
            return;

        _walkTimeFlow += Time.deltaTime;
        _runTimeFlow += Time.deltaTime;

        MoveCheck();

        Action_Behaviour();

        //% 기본 애니메이션
        PatJi_Animation_Activation_BasicMode();

        //% 고급 애니메이션
        PatJi_Animation_Activation_AdvancedMode();


        if (Input.GetKeyDown(KeyCode.R))
        {
            _runMode_Flag = !_runMode_Flag;
        }

        if (_thisIsFallenWood)
        {
            _runMode_Flag = false;
            _moveSpeed = 2.5f;
            // _patJiAnimator.SetBool("onFallenWood", true);
        }

        if (!_climbMode)
        {
            _patJiAnimator.SetBool("isClimbIdle", false);
        }


    }

    //~ -------------------------------------------------------------------------------
    //@ 이벤트, 인터렉션 행동 관련 
    private void Action_Behaviour()
    {
        // Legacy:
        /*
                # 스토리북 이벤트 관련 
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
                    Debug.Log("StoryBook Play END");

                    EventSensorController.instance.EndStoryBookEnvent();


                    //// _clickCount = 0;

                }


                # The Door Darkness Cinema Event Related
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

        */
        //? 구현중
        //% 기어오르기 구현
        /*
        if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0),
            transform.TransformDirection(Vector3.forward), out _hit))
        {
            Debug.Log("기어오르기 함수 진입");
            if (_hit.distance < 0.5f && _hit.collider.tag == "ClimbObject")
            {
                transform.rotation = Quaternion.LookRotation(-_hit.normal);
                Debug.Log("기어오르기 시작");
            }
        }
        */
        if (_climbMode && Input.GetKey(KeyCode.W))
        {
            _runMode_Flag = false;
            _moveSpeed = 2.5f;

            transform.position = transform.position + Vector3.up * Time.deltaTime * _climbSpeed;
            //transform.Translate(0, _climbSpeed * Time.deltaTime, 0);

            _patJiAnimator.SetBool("isClimbUp", true);

            if (_walkTimeFlow > _footStepSound_PlayDelayTime)
            {
                _walkTimeFlow = 0.0f;

                AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_LADDER_CLIMB);
                AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_LADDER_PATJI_BREATH);
                ////Debug.Log("기어오르기 시작");
            }

        }
        if (_climbMode && Input.GetKey(KeyCode.S))
        {
            _runMode_Flag = false;
            _moveSpeed = 2.5f;

            transform.position = transform.position - Vector3.up * Time.deltaTime * _climbSpeed;
            //transform.Translate(0, -_climbSpeed * Time.deltaTime, 0);

            _patJiAnimator.SetBool("isClimbUp", true);

            if (_walkTimeFlow > _footStepSound_PlayDelayTime)
            {
                _walkTimeFlow = 0.0f;

                AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_LADDER_CLIMB);
                AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_LADDER_PATJI_BREATH);
                ////Debug.Log("기어오르기 시작");
            }

        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _patJiAnimator.SetBool("isClimbUp", false);
        }
    }

    //@ 이동 조작 관련
    private void MoveCheck()
    {
        // Legacy:
        /*
            if (EventSensorController.instance._eventActiveCheckFlag || EventSensorController_DF.instance._eventActiveCheckFlag)
                return;


            //# 회전 코드
            {
                //% 이 함수 내에서 움직이는 회전량
                float _addRotationY = 0f;
                if (Input.GetKey(KeyCode.Q))        //& 왼쪽 회전
                {
                    _addRotationY = -_rotateSpeed_Y_Key;
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
                transform.position += ((transform.rotation * _addMovePosition) * Time.deltaTime);
            }

            float _horizontal = Input.GetAxis("Horizontal");
            Vector3 _moveHorizontal_Direction = Vector3.right * _horizontal;
            transform.Translate(_moveHorizontal_Direction * _moveSpeed * Time.deltaTime);
        */

        //% 특정 이벤트가 발생 중일 때는 팥쥐의 이동과 회전이 모두 불가능하게 하는 코드
        // if (EventSensorController.instance._eventActiveCheckFlag || EventSensorController_DF.instance._eventActiveCheckFlag)
        //     return;

        if (Input.anyKeyDown)
            _readyCompleted = true;


        //# 회전 코드 (마우스 커서가 비활성화 상태 일 때만..)
        //if (!GameControlTower_DF.instance._mouseCursorVisible)
        if(!_climbMode)
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

        //# 이동 코드 
        // Update:

        float _vertical = Input.GetAxis("Vertical");
        float _horizontal = Input.GetAxis("Horizontal");


        Vector3 _moveDirection = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);

        Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _moveSpeed * Time.deltaTime);

        transform.Translate(_moveDirection.normalized * _moveSpeed * Time.deltaTime);

        // Legacy:
        /*
        //transform.rotation = Quaternion.Euler(0f, -154.176f, 0f);
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
        */
        //% 마우스의 이동에 따라 회전

        if (_climbMode)
            return;

        float _mouseXValue = Input.GetAxis("Mouse X");
        float _mousYValue = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * _rotateValue * Time.deltaTime * _mouseXValue);

    }

    private void PatJi_Animation_Activation_BasicMode()
    {

        //% 걷기(앞)
        if (Input.GetKey(KeyCode.W) && !_runMode_Flag)
        {
            if (!_thisIsFallenWood)
            {
                WalkForward_SetBool();
                FootStepSoundPlay_Walk();
                _patJiAnimator.SetBool("onFallenWood", false);
            }
            if (_thisIsFallenWood)
            {
                _patJiAnimator.SetBool("onFallenWood", true);
                FootStepSoundPlay_Walk();
            }


            //% 이동 점프
            if (Input.GetKeyDown(KeyCode.Space) && !_jumpMode_Flag)
            {
                JumpVoice_Play();
                _patJiAnimator.SetBool("jumpForward", true);
                _jumpMode_Flag = true;
                _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode.Impulse);
            }
        }
        //% 걷기(뒤)
        else if (Input.GetKey(KeyCode.S) && !_climbMode)
        {
            _moveSpeed = 1.7f;
            _footStepSound_PlayDelayTime = 1.0f;

            if (!_thisIsFallenWood)
            {
                WalkBack_SetBool();
                FootStepSoundPlay_Walk();
                _patJiAnimator.SetBool("onFallenWood", false);
            }

            if (_thisIsFallenWood)
            {
                _patJiAnimator.SetBool("onFallenWood", true);
                FootStepSoundPlay_Walk();
            }

            JumpForward();
        }
        //% 걷기(왼쪽)
        else if (Input.GetKey(KeyCode.A))
        {
            if (!_runMode_Flag)
            {
                WalkLeft_SetBool();
                FootStepSoundPlay_Walk();
            }
            //& 뛰기(왼쪽)
            if (_runMode_Flag)
            {
                RunLeft_SetBool();
                FootStepSoundPlay_Run();
            }

            JumpForward();
        }
        //% 걷기(오른쪽)
        else if (Input.GetKey(KeyCode.D))
        {
            if (!_runMode_Flag)
            {
                WalkRight_SetBool();
                FootStepSoundPlay_Walk();
            }
            //& 뛰기(오른쪽)
            if (_runMode_Flag)
            {
                RunRight_SetBool();
                FootStepSoundPlay_Run();
            }
            JumpForward();
        }

        //% 제자리 회전(왼쪽)
        else if (Input.GetKey(KeyCode.Q))
        {
            _patJiAnimator.SetBool("turnLeft", true);
            FootStepSoundPlay_Walk();


        }
        //% 제자리 회전(오른쪽)
        else if (Input.GetKey(KeyCode.E))
        {
            _patJiAnimator.SetBool("turnRight", true);
            FootStepSoundPlay_Walk();
        }

        else
        {
            DarkForest_Idle_AnimatinClip_Setting();

            if (_thisIsFallenWood)
            {
                _patJiAnimator.SetBool("onFallenWood", false);
            }


        }
    }

    private void WalkForward_SetBool()
    {
        _walkMode_Flag = true;
        _patJiAnimator.SetBool("walkForward", true);

        _patJiAnimator.SetBool("jumpForward", false);
        _patJiAnimator.SetBool("walkLeft", false);
        _patJiAnimator.SetBool("walkRight", false);
        _patJiAnimator.SetBool("walkBack", false);
        _moveSpeed = 2.5f;
    }
    private void WalkBack_SetBool()
    {
        _patJiAnimator.SetBool("walkBack", true);

        _patJiAnimator.SetBool("jumpForward", false);
        _patJiAnimator.SetBool("walkLeft", false);
        _patJiAnimator.SetBool("walkRight", false);
        _patJiAnimator.SetBool("walkForward", false);
    }
    private void WalkLeft_SetBool()
    {
        _patJiAnimator.SetBool("walkLeft", true);

        _patJiAnimator.SetBool("jumpForward", false);

        _patJiAnimator.SetBool("walkForward", false);
        _patJiAnimator.SetBool("walkRight", false);
        _patJiAnimator.SetBool("walkBack", false);
    }
    private void WalkRight_SetBool()
    {
        _patJiAnimator.SetBool("walkRight", true);

        _patJiAnimator.SetBool("jumpForward", false);

        _patJiAnimator.SetBool("walkForward", false);
        _patJiAnimator.SetBool("walkLeft", false);
        _patJiAnimator.SetBool("walkBack", false);
    }
    private void RunLeft_SetBool()
    {
        _patJiAnimator.SetBool("runLeft", true);

        _patJiAnimator.SetBool("jumpForward", false);

        _patJiAnimator.SetBool("runRight", false);
        _patJiAnimator.SetBool("runForward", false);
        //   _patJiAnimator.SetBool("runBack", false);
        _moveSpeed = 4.5f;
    }
    private void RunRight_SetBool()
    {
        _patJiAnimator.SetBool("runRight", true);

        _patJiAnimator.SetBool("jumpForward", false);

        _patJiAnimator.SetBool("runLeft", false);
        _patJiAnimator.SetBool("runForward", false);
        //   _patJiAnimator.SetBool("runBack", false);

        _moveSpeed = 4.5f;
    }
    private void DarkForest_Idle_AnimatinClip_Setting()
    {
        _moveSpeed = 2.5f;
        _walkMode_Flag = false;
        _patJiAnimator.SetBool("walkForward", false);
        _patJiAnimator.SetBool("runForward", false);
        _patJiAnimator.SetBool("jump", false);
        _patJiAnimator.SetBool("walkBack", false);
        _patJiAnimator.SetBool("walkLeft", false);
        _patJiAnimator.SetBool("walkRight", false);
        _patJiAnimator.SetBool("runLeft", false);
        _patJiAnimator.SetBool("runRight", false);
        _patJiAnimator.SetBool("turnLeft", false);
        _patJiAnimator.SetBool("turnRight", false);
    }



    //@ 걷는 상황에서의 Foot Step AudioSound 
    private void FootStepSoundPlay_Walk()
    {
        if (_jumpMode_Flag)
            return;

        if (FloorCheck.instance._grassType && !_runMode_Flag)
        {
            _footStepSound_PlayDelayTime = 1.0f;
            if (_walkTimeFlow > _footStepSound_PlayDelayTime)
            {
                _walkTimeFlow = 0.0f;
                if (!_thisIsFallenWood && !_climbMode)
                {
                    AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_WALK_STEP_SOUND_GRASS_FLOOR_01);
                }
                else if (_thisIsFallenWood)
                {
                    AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_RUN_STEP_SOUND_FALLEN_WOOD);
                }


            }
            AudioSoundSystem.instance.Recovery();
            ////Debug.Log("걷기");
        }
        else if (FloorCheck.instance._grassType && _runMode_Flag && Input.GetKey(KeyCode.S))
        {
            if (_walkTimeFlow > _footStepSound_PlayDelayTime)
            {
                _walkTimeFlow = 0.0f;
                if (!_thisIsFallenWood)
                {
                    AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_WALK_STEP_SOUND_GRASS_FLOOR_01);
                }
                else if (_thisIsFallenWood)
                {
                    AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_RUN_STEP_SOUND_FALLEN_WOOD);
                }


            }
            AudioSoundSystem.instance.Recovery();

        }
        else if (FloorCheck.instance._grassType && _runMode_Flag && Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.W))
                return;
            if (_walkTimeFlow > _footStepSound_PlayDelayTime)
            {
                _walkTimeFlow = 0.0f;
                if (!_thisIsFallenWood)
                {
                    AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_WALK_STEP_SOUND_GRASS_FLOOR_01);
                }
                else if (_thisIsFallenWood)
                {
                    AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_RUN_STEP_SOUND_FALLEN_WOOD);
                }

            }
            AudioSoundSystem.instance.Recovery();

        }
    }

    //@ 뛰는 상황에서의 Foot Step AudioSound 
    private void FootStepSoundPlay_Run()
    {
        if (_jumpMode_Flag)
            return;

        if (FloorCheck.instance._grassType && _runMode_Flag)
        {
            _footStepSound_PlayDelayTime = 1.8f;
            if (_runTimeFlow > _footStepSound_PlayDelayTime)
            {
                _runTimeFlow = 0.0f;
                AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_RUN_STEP_SOUND_GRASS_FLOOR);
            }
        }

    }
    //@ jump voice 
    private void JumpVoice_Play()
    {
        AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_JUMP_VOICE);
    }


    public void PatJi_Animation_Activation_AdvancedMode()
    {
        //% 달리기
        if (Input.GetKey(KeyCode.W) && _runMode_Flag && !_walkMode_Flag)
        {
            _walkMode_Flag = false;
            _patJiAnimator.SetBool("runForward", true);
            _patJiAnimator.SetBool("jumpForward", false);
            _moveSpeed = 4.5f;

            FootStepSoundPlay_Run();

            //% 이동 점프
            if (Input.GetKeyDown(KeyCode.Space) && !_jumpMode_Flag)
            {
                ////Debug.Log("이동 점프");
                JumpVoice_Play();
                _patJiAnimator.SetBool("jumpForward", true);
                _jumpMode_Flag = true;
                _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode.Impulse);
            }
            //?.. 뛰는 소리가 바로 멈추지 않는 경우가 있음.. 테스트 중
            if (Input.GetKey(KeyCode.S))
            {
                AudioSoundSystem.instance.PlayAudioSound_Stop();
            }

        }
        //% 제자리 점프
        else if (Input.GetKeyDown(KeyCode.Space) && !_jumpMode_Flag && !_walkMode_Flag)
        {
            Debug.Log("제자리 점프");
            JumpVoice_Play();
            _patJiAnimator.SetBool("jump", true);
            _jumpMode_Flag = true;
            //!_rigidBody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
            _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode.Impulse);
        }

    }

    private void JumpForward()
    {
        //% 이동 점프
        if (Input.GetKeyDown(KeyCode.Space) && !_jumpMode_Flag)
        {
            JumpVoice_Play();
            _patJiAnimator.SetBool("jumpForward", true);
            _jumpMode_Flag = true;
            _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode.Impulse);
        }
    }



    //~ --------------------------------------------On Collision Enter---------------------------------
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.name == "FallenWood")
        {
            _jumpMode_Flag = false;
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            _patJiAnimator.SetBool("onGround", true);
            _patJiAnimator.SetBool("onFallenWood", false);
        }

        if (other.gameObject.name == "FallenWood")
        {
            _thisIsFallenWood = true;
            //AudioSoundSystem.instance.PlayAudioSound_DF(AudioSoundId_DF.THE_DARK_FOREST_RUN_STEP_SOUND_FALLEN_WOOD);

            _patJiAnimator.SetBool("onFallenWood", true);
            _patJiAnimator.SetBool("onGround", false);

            _moveSpeed = 2.5f;
        }

        if (other.gameObject.tag == "ClimbObject")
        {
            _climbMode = true;
            _rigidBody.useGravity = false;
            _patJiAnimator.SetBool("isClimbUp", true);
        }

    }
    //~ --------------------------------------------On Collision Exit---------------------------------    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "FallenWood")
        {
            _thisIsFallenWood = false;
            _patJiAnimator.SetBool("onFallenWood", false);

        }
        if (other.gameObject.tag == "ClimbObject")
        {
            _climbMode = false;
            _rigidBody.useGravity = true;
            _patJiAnimator.SetBool("isClimbUp", false);
            _patJiAnimator.SetBool("isClimbIdle", false);
        }

    }
    //~ --------------------------------------------On Trigger Enter---------------------------------    
    private void OnTriggerEnter(Collider other)
    {


    }

    //~ --------------------------------------------On Trigger Exit---------------------------------    
    private void OnTriggerExit(Collider other)
    {


    }

}
