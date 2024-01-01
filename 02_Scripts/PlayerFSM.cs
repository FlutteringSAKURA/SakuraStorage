using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update: //@ 2023.10.19 
// Update: //@ 2023.10.20 
// Update: //@ 2023.10.23 
// Update: //@ 2023.10.27 

// NOTE: //# 3D 게임 - 플레이어 유한상태머신
//#          1) 플레이어의 상태에 따른 애니메이션       // Completed:
//&          2) 대쉬 모드 만들기(run)..쉬프트 누를경우      // not Working:
//#          3) 칼질 사운드 + 데미지 받는 사운드        // Completed:
//#          4) 공룡에게 데미지를 전달                  // Completed:
//#          5) 전투, 비전투 사운드 구현                // Completed:
//#          6) 런 모드 구현
//#          7) 피격시 UI 효과 구현
//#          8) 타격할 때 스킬 게이지 구현
public class PlayerFSM : MonoBehaviour
{
    //@ 플레이어의 상태가 바뀌면 발생할 일들을 정의
    public enum SakuraState
    {
        IDLE, MOVE_WALK, MOVE_RUN, ATTACK, ATTACKWAIT, DEAD
    }

    public SakuraState _currentState = SakuraState.IDLE;    //% 현재상태
    Vector3 _curTargetPosition;     //% 이동할 목표지점의 좌표값

    Animator _sakuraAnimator;
    PlayerAnimationManager _playerAnim;     //% PlayerAnimationManager스크립트 접근을 통해 애니메이션 구동하기 위한 변수 선언

    public float _moveSpeed = 1.5f;
    public float _rotAnglePerSecond = 360f;
    GameObject _currentDino;        //% 넘어온 변수를 인식하기 위해 타입 변수 선언

    private float _attackDistance = 0.9f;       //^ 공격 거리
    float _tracingDistance = 2.5f;      //^ 추적 거리
    public float _attackTimer = 0.0f;
    public float _attackDelay = 2.3f;      //^ 공격 딜레이
    public ParticleSystem _bloodEffect;
    AudioSource _audioBox;
    public AudioClip _attackSound = null;
    public AudioClip _damagedSound = null;

    // Update:
    //! 플레이어가 데미지를 입을 때 사운드가 업데이트문에서 돌기 때문에 소리가 한번이 아니라 연속 발생하는 것을 막기 위한 조건을 위한 변수
    public bool _damaged = false;
    public float _timeFlow = 0.0f;
    public float _soundTimerLimit = 8.0f;
    //! -----------------------------------
    GameObject _clickManager;

    // Update: //@ 2023.10.23 
    SakuraParams _sakuraParams;
    DinosaurParams _dinosaurParams;

    // TEST: // SUCCESS:
    GameObject _battleSound;
    GameObject _noneBattleSound;

    // TEST:
    //public bool _runMode = false;

    // Update: //@ 2023.10.27 
    public GameObject _mainPanel;

    //~ ------------------------------------------------------------------------

    private void Start()
    {
        _sakuraAnimator = GetComponent<Animator>();
        _playerAnim = GetComponent<PlayerAnimationManager>();

        //_currentDino = GameObject.Find("Velociraptor");
        //_currentDino = GameObject.Find("Dinosaur_Velociraptor");

        _clickManager = GameObject.Find("CheckManager");

        //*_bloodEffect.Stop();

        _audioBox = GetComponent<AudioSource>();
        _damaged = false;

        _sakuraParams = GetComponent<SakuraParams>();
        //_dinosaurParams = GetComponent<DinosaurParams>();

        // TEST:
        // SUCCESS: 
        _battleSound = GameObject.FindWithTag("BattleBGM");
        _noneBattleSound = GameObject.FindWithTag("NonBattleBGM");

        _moveSpeed = 1.5f;

        // TEST:
        //_runMode = false;

        // TEST: //# 플레이어 죽을 때 이벤트
        Debug.Log("AddListener");
        _sakuraParams.deadEvent.AddListener(SakuraDeadEvent);

    }

    private void SakuraDeadEvent()
    {
        ////Debug.Log("SakuraDeadEvent");
        //& 상태 및 애니메이션 변화
        ChangeState(SakuraState.DEAD, PlayerAnimationManager.ANIM_DEAD);
        //& 플레이어 죽음 알리기
        //* _currentDino.GetComponent<DinosaurFSM>().SakuraIsDead();
        //& 클릭매니저 비활성화
        GameObject _clickManager = GameObject.Find("ClickManager");
        _clickManager.SetActive(false);

    }

    //? 공룡의 사망사실을 플레이어에게 알려주는 함수 
    public void DinoIsDead()
    {
        ////Debug.Log("dino Die");
        ChangeState(SakuraState.IDLE, PlayerAnimationManager.ANIM_IDLE);
        _currentDino = null;
    }

    //~ ------------------------------------------------------------------------
    private void Update()
    {
        UpdateState();

        // // if (Input.GetKeyDown(KeyCode.R))
        // // {
        // //     _runMode = !_runMode;

        // //     _moveSpeed = 4.0f;
        // //     Debug.Log("Run Mode");

        // // }
        // // else
        // // {
        // //      _runMode = false;
        // //     _moveSpeed = 1.5f;
        // // }

    }
    //~ ------------------------------------------------------------------------

    //@ 애니메이션 이벤트를 위한 함수 (플레이어의 데미지 이펙트 효과) + 공룡의 피를 깎는 코드 
    public void AttackCalculate()
    {
        if (_currentDino == null)
        {
            return;
        }
        //& 칼질 애니메이션 이벤트 함수 심어둔 곳에서 블러드 효과 발생하게 호출하는 코드
        _currentDino.GetComponent<DinosaurFSM>().PlayDamagedEffect();
        _audioBox.PlayOneShot(_attackSound);

        //? 공룡의 피를 깍는 코드 
        //& 사쿠라의 공격력만큼 공룡의 Hp값 감소
        //% 사쿠라의 공격력을 가져옴
        int _sakuraAttackPower = _sakuraParams.GetRandomAttack();

        //& 공룡이 데미지 입을 떄 사운드 재생
        _currentDino.GetComponent<DinosaurFSM>().DamagedHitSoundPlay();

        ////Debug.Log("사쿠라 공격력" + _sakuraAttackPower);

        //% 공격력을 전달하는 방법
        // Legacy: 
        //* _currentDino.GetComponent<DinosaurParams>().Damaged(_sakuraAttackPower);

        // Update: //# 플레이어의 공격력으로 공룡의 피를 깎음
        _dinosaurParams.SetCounterAttack(_sakuraAttackPower);

        // Update: //# 공격시 스킬 게이지 채우기
        // float _sakuraSkillGauge = _sakuraParams.ChargeSkillGauge();
        // _sakuraParams.ChargeSkillGauge(_sakuraSkillGauge);

        //SakuraUIManager.instance.SkillGaugeBar(_sakuraParams);

        //// Debug.Log("블러드");
        //// Debug.Log("이벤트 함수 테스트" + _currentDino.name);
    }


    //@ 캐릭터의 상태변환, 애니메이션 동기를 맞추는데 활용하는 함수
    void ChangeState(SakuraState _newState, int _anyNumber)
    {
        if (_currentState == _newState)     //% 기존의 상태가 새로운 상태와 동일하다면 굳이 이하를 다시 할 필요 없으니 조건으로 준 코드
            return;
        _playerAnim.ChangeAnimation(_anyNumber);        //& ChangeAnimaiont()함수에 접근..
        _currentState = _newState;      //& _newState를 _CurrentState로 치환

    }

    //@ 이동을 위한 함수(위치 좌표를 받아옴)
    public void MoveTo(Vector3 _tPoint)
    {
        ////Debug.Log("마우스 클릭 위치 : " + _tPoint);
        //& 플레이어 사망시 마우스클릭으로 움직이는 현상 방지
        if (_currentState == SakuraState.DEAD)
        {
            return;
        }
        //& ----------------------------------------
        _currentDino = null;
        _curTargetPosition = _tPoint;       //% 마우스 클릭좌표의 값(_tPoint)을 받아 넣은 변수_curTargetPosition(=치환)

        ChangeState(SakuraState.MOVE_WALK, PlayerAnimationManager.ANIM_WALK);

    }

    //@ 목적방향으로 몸을 회전하는 함수
    void TurnToDestination()
    {
        //% 목표지점 - 자기자신의 위치 =  목표 방향
        Quaternion _lookRotation = Quaternion.LookRotation(_curTargetPosition - transform.position);

        //! 부드럽게 회전할 필요.
        // transform.rotation = _lookRotation; 

        // Update:  //# RotateTowards(현재회전, 목표방향회전. 초당 회전각도값)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * _rotAnglePerSecond);

        ////Debug.Log("목표지점의 좌표 방향 : " + _curTargetPosition);
    }

    //@ 목적지로 이동하는 함수
    void MoveToDestination()
    {

        transform.position = Vector3.MoveTowards(transform.position, _curTargetPosition, Time.deltaTime * _moveSpeed);


        if (_currentDino == null)        //&  현재 공룡이 없다고 판단되면.. 땅을 클릭한 것으로 봄
        {
            ////ChangeState(SakuraState.MOVE_WALK, PlayerAnimationManager.ANIM_WALK);

            //% 목표지점과 현재 위치가 같아지면 상태전환
            if (transform.position == _curTargetPosition)
            {
                ChangeState(SakuraState.IDLE, PlayerAnimationManager.ANIM_IDLE);
                ////Debug.Log("idle TEST");
            }
        }

        else
        {
            if (Vector3.Distance(transform.position, _curTargetPosition) < _attackDistance)
            {
                ChangeState(SakuraState.ATTACK, PlayerAnimationManager.ANIM_ATTACK);
                ////Debug.Log("attack TEST");
            }
            // // else
            // // {
            // //     ChangeState(SakuraState.MOVE_WALK, PlayerAnimationManager.ANIM_WALK);
            // //     Debug.Log("walkTEST");
            // // }
        }

        ////transform.position = Vector3.MoveTowards(transform.position, _curTargetPosition, Time.deltaTime * _moveSpeed);
    }

    //@ 공격을 위한 이동 함수 
    public void AttackDinosaur(GameObject _dinoSaur)
    {
        //! 공룡을 계속해서 선택이 되어 불필요한 애니메이션 동작이 하지 않게 방어코드 (공룡 클릭한후 반복 클릭 무시)
        if (_currentDino == _dinoSaur && _currentDino != null)
            return;
        //!-------------------------------------------------------

        //& 사쿠라가 공격시 공룡의 파라미터 값을 가져옴
        _dinosaurParams = _dinoSaur.GetComponent<DinosaurParams>();

        //% 공룡이 살아있는 경우
        if (_dinosaurParams._isAlive)
        {
            //& 공룡(GameObject)을 가져와 인자(_dinoSaur)에 담음
            _currentDino = _dinoSaur;
            ////Debug.Log("공룡이름과 좌표값 : " + _currentDino + _currentDino.transform.position);
            //& 공룡의 위치좌표를 가져와 _curTargetPosition에 담음
            _curTargetPosition = _currentDino.transform.position;

            // Update: //@ 2023.10.27 
            //#        선택한 것이 살아있는 공룡인 경우 함수 실행..(선택 마크)표시
            GameManagerController.instance.ChangeTargetMarkToDinosaur(_currentDino);

            ChangeState(SakuraState.MOVE_WALK, PlayerAnimationManager.ANIM_WALK);
        }

        //% 공룡이 죽은 경우
        else
        {
            _dinosaurParams = null;
        }

    }

    void UpdateState()
    {
        switch (_currentState)
        {
            case SakuraState.IDLE:
                // //BgmControl();
                // // _battleSound.GetComponent<AudioSource>().enabled = false;
                // // _noneBattleSound.GetComponent<AudioSource>().enabled = true;

                IdleState();
                NoneBattleBgmPlay();

                break;
            case SakuraState.ATTACKWAIT:

                AttackWaitState();


                break;

            case SakuraState.ATTACK:
                // TEST:  //# 전투시 .. 배틀 사운드 재생
                // SUCCESS: 
                if (_dinosaurParams._isAttack == true)
                {
                    _battleSound.GetComponent<AudioSource>().enabled = true;
                    _noneBattleSound.GetComponent<AudioSource>().enabled = false;
                }
                AttackState();

                break;

            case SakuraState.MOVE_WALK:

                MoveState();
                NoneBattleBgmPlay();

                break;
            case SakuraState.MOVE_RUN:

                //? MoveRunToDestination();

                break;

            case SakuraState.DEAD:

                DeadState();
                NoneBattleBgmPlay();

                //& 게임 종료 UI  
                SakuraUIManager.instance.ShowGameOver();

                //& 리스타트 게임
                if (Input.anyKeyDown)
                {
                    ////Debug.Log("Re-Strart");
                    SakuraUIManager.instance.ReStartGame();
                }
                break;

        }
    }

    private void NoneBattleBgmPlay()
    {
        _timeFlow += Time.deltaTime;

        if (_timeFlow >= _soundTimerLimit)
        {
            _battleSound.GetComponent<AudioSource>().enabled = false;
            _noneBattleSound.GetComponent<AudioSource>().enabled = true;
        }
    }

    // // private void BgmControl()
    // // {
    // //      TEST: 비전투시 .. 평화로운 사운드 재생
    // //     if (_currentDino.GetComponent<DinosaurFSM>()._battleBgmFlag == false)
    // //     {
    // //         _battleSound.GetComponent<AudioSource>().enabled = false;
    // //         _noneBattleSound.GetComponent<AudioSource>().enabled = true;
    // //     }
    // //     else if (_currentDino.GetComponent<DinosaurFSM>()._battleBgmFlag == true)
    // //     {
    // //         _battleSound.GetComponent<AudioSource>().enabled = true;
    // //         _noneBattleSound.GetComponent<AudioSource>().enabled = false;
    // //     }

    // // }


    //@ 아이들 상태 함수 
    private void IdleState()
    {

    }

    //@ 이동 함수 
    private void MoveState()
    {
        TurnToDestination();
        MoveToDestination();
    }

    //@ 사망 함수 

    private void DeadState()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    //@ 공격 함수 
    private void AttackState()
    {
        // _attackTimer += Time.deltaTime;

        if (_dinosaurParams._isAlive == false)
        {
            ChangeState(SakuraState.IDLE, PlayerAnimationManager.ANIM_IDLE);
            return;
        }

        ChangeState(SakuraState.MOVE_WALK, PlayerAnimationManager.ANIM_WALK);

        _attackTimer = 0.0f;
        //& 목표지점의 위치로 회전
        transform.LookAt(_curTargetPosition);
        //& 일정 시간 후 재공격
        ChangeState(SakuraState.ATTACKWAIT, PlayerAnimationManager.ANIM_ATTACKWAIT);

        ////SakuraAttack();
    }

    //@ 공격 대기 함수 
    private void AttackWaitState()
    {

        _attackTimer += Time.deltaTime;
        ////_attackTimer = 0f;
        if (_attackTimer > _attackDelay)
        {
            ChangeState(SakuraState.ATTACK, PlayerAnimationManager.ANIM_ATTACK);
            //& 칼질 후 공룡의 애니메이션 변화
        }

    }

    //@ 데미지 받는 경우의 함수 
    public void DamageCheck()
    {

        // Update: //! 사운드 재생 싱크를 맞추기 위한 조건 코드
        _damaged = true;
        _timeFlow += Time.deltaTime;

        // // if (_timeFlow < _soundTimerLimit && _damaged)
        // // {
        // //     // DinosaurFSM 스크립트의 AttackCalculate()로 대체
        // //     // _sakuraAnimator.SetTrigger("Damaged");
        // //     // _audioBox.PlayOneShot(_damagedSound);

        // //     //DamagedSound();

        // // }
        StartCoroutine(DamagedFalse());
        //! ------------------------------------------------

        ////Debug.Log("DAMAGED");
    }

    IEnumerator DamagedFalse()
    {
        yield return new WaitForSeconds(1.5f);
        _damaged = false;
        _timeFlow = 0.0f;
    }

    public void Damaged()
    {
        _bloodEffect.Play();
        _audioBox.PlayOneShot(_damagedSound);

        // Update: //@ 2023.10.27 
        _mainPanel.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        StartCoroutine(ChangeMainFrameColor());
    }

    IEnumerator ChangeMainFrameColor()
    {
        yield return new WaitForSeconds(0.5f);
        _mainPanel.GetComponent<Image>().color = new Color(0, 227, 225, 100);
        yield return new WaitForSeconds(0.5f);
        _mainPanel.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);
        _mainPanel.GetComponent<Image>().color = new Color(0, 227, 225, 100);
        yield return new WaitForSeconds(0.5f);
        _mainPanel.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);
        _mainPanel.GetComponent<Image>().color = new Color(0, 227, 225, 100);
    }
    
    //@ 발소리 사운드 관련 
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.tag.Contains("Ground") && _currentState == SakuraState.MOVE_WALK)
    //     {
    //         //SoundManager.instance.GroundWalkSoundPlay();
    //         SoundManager.instance._groundWalkSound.enabled = true;
    //         SoundManager.instance._waterWalkSound.enabled = false;
    //         Debug.Log("sound Ground");
    //     }

    //     if (other.gameObject.tag.Contains("Water") && _currentState == SakuraState.MOVE_WALK)
    //     {
    //         //SoundManager.instance.WaterWalkSoundPlay();
    //         SoundManager.instance._groundWalkSound.enabled = false;
    //         SoundManager.instance._waterWalkSound.enabled = true;
    //         Debug.Log("sound water");
    //     }
    // }
    // private void OnTriggerEnter(Collider other)
    // {


    // }
}
