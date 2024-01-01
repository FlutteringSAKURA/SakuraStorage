using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.20 
// Update: //@ 2023.10.23 
// Update: //@ 2023.10.26 
// Update: //@ 2023.10.27 

// NOTE: //# 3D 게임 - 공룡 유한상태머신
//#          1) 공룡 상태에 따른 애니메이션

public class DinosaurFSM : MonoBehaviour
{
    public enum DinosaurSTATE
    {
        IDLE, MOVE_WALK, HUNT, ATTACK, FIGHTIDLE, DAMAGED, DEAD, NOSTATE
    }

    public DinosaurSTATE _currentState = DinosaurSTATE.IDLE;
    DinosaurAnimationManager _dinosaurAnimation;

    float _attackDistance = 1.0f;       //& 공격거리
    float _tracingDistance = 3.0f;      //& 추적거리
    float _stopBattleBgmDistance = 5.0f;
    float _reTracingDistance = 8.0f;    //& 재추적 거리
    float _stopTracingDistance = 6.0f;      //& 추적 중단
    float _ruwAwayDistance = 7.0f;      //& 도망가는 거리

    public float _moveSpeed = 1.0f;
    public float _rotAnglePerSecond = 360f;

    public float _Hp = 100;
    //public float _runAwayHp = 40;

    Transform _sakuraObj;       //& 플레이어 위치좌표
    GameObject _currentPlayer;
    public float _betweenDistance;

    Vector3 _currentPlayerPsition;

    public ParticleSystem _bloodEffect;

    public float _attackTimer = 0.0f;
    public float _attackDelay = 1.0f;
    public float _idleMaxTime = 1.5f;

    public bool _isDinoAlive = false;
    public bool _attackWait = false;

    public bool _damaged = false;
    //public bool _attack = false;

    public AudioClip _attackSound = null;
    public AudioClip _deadSound = null;
    public AudioClip _damagedHitSound = null;
    AudioSource _audioBox;

    //% 부모(CharacterParams)와 연결되는 DinosaurParams 클래스
    DinosaurParams _dinosaurParams;
    //% 사쿠라의 Hp값이 여기에 들어있음
    SakuraParams _sakuraParams;

    // Legacy:.. //* 애니메이션 이벤트로 대체 .. 현재 사용안함.
    // 공룡의 사운드가 업데이트문에서 돌기 때문에 소리가 한번이 아니라 연속 발생하는 것을 막기 위해 시간값으로 조건을 주어 사운드 싱크를 맞춤
    public float _timeFlow = 0.0f;
    public float _soundTimerLimit = 4.5f;
    // --------------------------------------------

    // TEST: // SUCCESS:
    GameObject _battleSound;
    GameObject _noneBattleSound;

    public bool _battleBgmFlag = false;

    // Update: //@ 2023.10.26 
    //& 몬스터 생성 게임오브젝트
    GameObject _generaterObj;
    //& 몇번째 몬스터인지 저장
    public int _genrateID { get; set; }
    //& 공룡 생성시 좌표
    Vector3 _originalPosition;

    public GameObject _selectionMark;

    // Update: //@ 2023.10.27 
    CharacterController _dinoCharController;

    //~ ------------------------------------------------------------------------

    private void Start()
    {
        _sakuraObj = GameObject.FindWithTag("Player").transform;
        _currentPlayer = GameObject.FindWithTag("Player");
        _dinosaurAnimation = GetComponent<DinosaurAnimationManager>();
        _isDinoAlive = true;
        //// _attack = false;
        _audioBox = GetComponent<AudioSource>();

        ChangeState(DinosaurSTATE.IDLE, DinosaurAnimationManager.IDLE);     //& 초기 설정

        _bloodEffect.Stop();

        //% 할당 (공룡의 공격력을 가져옴)
        _dinosaurParams = GetComponent<DinosaurParams>();

        //% 데드이벤트 등록 .. AddListener() 함수..
        _dinosaurParams.deadEvent.AddListener(CallDeadEvent);
        ////_sakuraParams = GetComponent<SakuraParams>();
        //% 사쿠라의 Hp값이 들어있음 (사쿠라를 찾아서..)
        _sakuraParams = _sakuraObj.gameObject.GetComponent<SakuraParams>();

        // TEST:
        // SUCCESS: 
        _battleSound = GameObject.FindWithTag("BattleBGM");
        _noneBattleSound = GameObject.FindWithTag("NonBattleBGM");
        _battleBgmFlag = false;

        _generaterObj = GameObject.Find("GenerateSensor");

        // Update: //@ 2023.10.26 
        HideSelectionMark();

        // Update: //@ 2023.10.27 
        _dinoCharController = GetComponent<CharacterController>();


        //# 초기화(비전투 BGM PLAY)
        _battleSound.GetComponent<AudioSource>().enabled = false;
        _noneBattleSound.GetComponent<AudioSource>().enabled = true;


    }

    // // private void PlayBattleBGM()
    // // {
    // //     _battleSound.GetComponent<AudioSource>().enabled = true;
    // //     _noneBattleSound.GetComponent<AudioSource>().enabled = false;
    // // }

    //~ ------------------------------------------------------------------------
    private void Update()
    {
        UpdateState();

        // // 비전투 음악
        // // if (GetDistanceFromPlayer() > _stopBattleBgmDistance)
        // // {
        // //     _battleBgmFlag = false;
        // //     _battleSound.GetComponent<AudioSource>().enabled = false;
        // //     _noneBattleSound.GetComponent<AudioSource>().enabled = true;
        // // }

        // //  전투음악
        // // else if (GetDistanceFromPlayer() <= _attackDistance)
        // // {
        // //     _battleBgmFlag = true;
        // //     _battleSound.GetComponent<AudioSource>().enabled = true;
        // //     _noneBattleSound.GetComponent<AudioSource>().enabled = false;
        // // }
        // // BgmPlayer();
        ////_timeFlow += Time.deltaTime;

    }

    //~ ------------------------------------------------------------------------

    //@ 상태 변화를 위한 함수 
    void ChangeState(DinosaurSTATE _newState, string _anyString)
    {
        if (_currentState == _newState)
            return;
        _dinosaurAnimation.ChangeAnimation(_anyString);
        _currentState = _newState;
    }

    //@ 플레이어와의 거리 산출을 위한 함수 
    float GetDistanceFromPlayer()
    {   //% 리턴 타입이 있는 함수 ----- 거리 산출
        float _distance = Vector3.Distance(transform.position, _sakuraObj.position);
        return _distance;
    }

    //@ 플레이어 쪽으로 바라보는 함수 
    private void TurnToPlayer()
    {
        Quaternion _lookRotation = Quaternion.LookRotation(transform.position - _sakuraObj.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * _rotAnglePerSecond);

        ////Debug.Log("랩터가 쳐다본다");
    }

    //@ 플레이어 쪽으로 이동시키는 함수 
    private void MoveToPlayer()
    {
        // Legacy:
        //* transform.position = Vector3.MoveTowards(transform.position, _sakuraObj.position, _moveSpeed * Time.deltaTime);
        // Update: //@ 2023.10.27 
        //# 캐릭터 컨트롤러를 사용하는 방식으로 업데이트..
        _dinoCharController.Move(transform.forward * -_moveSpeed * Time.deltaTime);
    }

    //@ 데미지를 입을 때 발생하는 이펙트 효과 함수 
    public void PlayDamagedEffect()
    {
        _bloodEffect.Play();

        DamagedState();
    }

    // Update: //@ 2023.10.26 
    public void ShowSelectionMark()
    {
        _selectionMark.SetActive(true);
    }

    public void HideSelectionMark()
    {
        _selectionMark.SetActive(false);
    }

    // Update:
    //@ 공룡이 사쿠라를 공격하면 사쿠라의 HP 차감
    public void AttackCalculate()
    {
        //& 공격데미지를 입은 플레이어의 애니메이션 변화
        _sakuraObj.GetComponent<Animator>().SetTrigger("Damaged");

        //& 공룡 어택 사운드
        _audioBox.PlayOneShot(_attackSound);

        //& 플레이어 데미지 입는 사운드
        _sakuraObj.GetComponent<PlayerFSM>().Damaged();

        //$ 부모(CharacterParams)스크립트 내의 함수(SetCounterAttack()) 하나로 공유해서 쓰는 방법.
        //% 몬스터의 공격력(오른쪽)만큼 플레이어의 Hp값(왼쪽) 감소
        _sakuraParams.SetCounterAttack(_dinosaurParams.GetRandomAttack());
        // TEST:  //# 전투시 .. 배틀 사운드 재생
        // SUCCESS: 
        _dinosaurParams._isAttack = true;
        _currentPlayer.GetComponent<PlayerFSM>()._timeFlow = 0.0f;
        _battleSound.GetComponent<AudioSource>().enabled = true;
        _noneBattleSound.GetComponent<AudioSource>().enabled = false;

    }

    // Update: // NOTE: //@ 공룡 사망시 자동실행하는 함수
    void CallDeadEvent()
    {
        //& 1. 애니메이션 변화
        ChangeState(DinosaurSTATE.DEAD, DinosaurAnimationManager.DEAD);

        //& 2. 플레이어에게 공룡 사망 사실을 알려줌
        // Legacy: 
        //* _sakuraObj.gameObject.SendMessage("DinoIsDead");

        _sakuraObj.GetComponent<PlayerFSM>().DinoIsDead();

        //% 7. 공룡이 죽었을 때, 공룡의 위치에 랜덤 골드를 떨어뜨리는 코드 (공룡의 파람스 스크립트를 통해)
        ObjectManager.instance.DropToGoldPosition(transform.position, _dinosaurParams._rewardGold);

        //& 3. 죽음 사운드 재생
        _audioBox.PlayOneShot(_deadSound);

        //& 4. 콜라이더 비활성화 .. > DeadState()에서 처리함

        // Update: //@ 2023.10.26 
        //& 5. 5초후 시체 사라짐
        // Legacy:
        //Destroy(gameObject, 5.0f);

        //& 6. 공룡 사망후 재소환을 위해서
        _dinosaurParams._isAttack = false;
        StartCoroutine(RemoveFromWorld());

    }

    IEnumerator RemoveFromWorld()
    {
        yield return new WaitForSeconds(3.0f);
        //! ----
        //& _generateObj에 가서 (_generateID)의 자기 자신을 제거해달라는 요첨의 함수
        _generaterObj.GetComponent<Generator>().RemoveDinosaur(_genrateID);
    }

    // Update: //@ 2023.10.26 
    //# 공룡 생성시 그 객체의 생성넘버, 좌표값의 정보
    public void SetGenerateObj(GameObject genObj, int genID, Vector3 originPos)
    {
        this._generaterObj = genObj;
        this._genrateID = genID;
        this._originalPosition = originPos;
    }

    // Update: //@ 2023.10.26 초기화 셋팅값 함수 ..
    public void ReturnTotheWorld()
    {
        //& 사망 후 애니메이션 상태를 IDLE 상태로 초기화
        ChangeState(DinosaurSTATE.IDLE, DinosaurAnimationManager.IDLE);
        // TEST: //! ---
        //# HP Bar 초기화 + 부활
        gameObject.GetComponent<DinosaurParams>().InitParams();
        // // _dinosaurParams._textColor.color = Color.white;
        // // gameObject.GetComponent<DinosaurParams>()._textColor.color = Color.white;
        // // _dinosaurParams._currentHp = _dinosaurParams._maxHp;
        // // _dinosaurParams._dinoHpBar.rectTransform.localScale =
        // // new Vector3((float)_dinosaurParams._currentHp / (float)_dinosaurParams._maxHp, 1f, 1f);
        //# 위치값 초기화
        transform.position = _originalPosition;
        //// 부활
        ////_dinosaurParams._isAlive = true;
        //# 콜라이더 활성화
        gameObject.GetComponent<Collider>().enabled = true;


    }

    //@ 사쿠라 죽음 알림 함수 
    public void SakuraIsDead()
    {
        ////Debug.Log("Sakura Die");
        ChangeState(DinosaurSTATE.IDLE, DinosaurAnimationManager.IDLE);
        _currentPlayer = null;
    }


    //@ 아이들 상태 
    private void IdleState()
    {
        //& 추적거리 이내로 플레이어와 근접시. 추적모드 + 걷기
        if (_sakuraParams._isAlive == false)
        {
            ChangeState(DinosaurSTATE.IDLE, DinosaurAnimationManager.IDLE);
            _dinosaurParams._isAttack = false;
            return;
        }


        if (GetDistanceFromPlayer() < _tracingDistance)
        {
            ChangeState(DinosaurSTATE.HUNT, DinosaurAnimationManager.WALK);
        }
    }

    //@ 추적, 사양 상태 
    void HuntingState()
    {
        //// var _distanceOfSakuraAndDinosaur = Vector3.Distance(_sakuraObj.position, transform.position);

        if (_dinosaurParams._isAlive)
        {
            ////  _distanceOfSakuraAndDinosaur = Vector3.Distance(_sakuraObj.position, transform.position);

            if (GetDistanceFromPlayer() <= _attackDistance && !_damaged)
            {
                ChangeState(DinosaurSTATE.ATTACK, DinosaurAnimationManager.ATTACK);
                _dinosaurParams._isAttack = true;
            }
            // TEST:
            else if (GetDistanceFromPlayer() > _stopTracingDistance)
            {
                ChangeState(DinosaurSTATE.IDLE, DinosaurAnimationManager.IDLE);
                _dinosaurParams._isAttack = false;
            }
            //! --------------------------------------------------------
            else
            {
                TurnToPlayer();     //& 플레이어를 향해 회전
                MoveToPlayer();     //& 플레이어를 향해 이동

            }

        }
    }

    //@ 공격 상태 
    private void AttackState()
    {
        // Legacy:
        // //if (_sakuraParams._isAlive == false)            
        // //  return;

        if (_sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.DEAD)
        {
            ChangeState(DinosaurSTATE.IDLE, DinosaurAnimationManager.IDLE);
            _dinosaurParams._isAttack = false;
            return;
        }

        //& 플레이어가 재추적 거리 범위 밖으로 나가면 추적
        if (GetDistanceFromPlayer() > _tracingDistance && !_damaged)
        {
            _attackTimer = 0;       //^ 시간 값 초기화
            ChangeState(DinosaurSTATE.HUNT, DinosaurAnimationManager.WALK);     //^ 추적모드 + 이동 애니메이션
        }

        else
        {
            if (_attackTimer > _attackDelay && !_damaged)
            {
                ////_attack = true;
                //^플레이어를 바라봄
                //transform.LookAt(_sakuraObj.position);     

                //% 플레이어 데미지함수 호출
                _sakuraObj.GetComponent<PlayerFSM>().DamageCheck();

                //^ 공격 애니매이션
                _dinosaurAnimation.ChangeAnimation(DinosaurAnimationManager.ATTACK);
                _dinosaurParams._isAttack = true;

                StartCoroutine(ResetConditions());

                if (_attackTimer > _idleMaxTime && !_damaged)
                {
                    _dinosaurAnimation.ChangeAnimation(DinosaurAnimationManager.FIGHTIDLE);
                }
                //! ------------------------------------------
            }

            _attackTimer += Time.deltaTime;

        }
    }

    IEnumerator ResetConditions()
    {
        yield return new WaitForSeconds(1.0f);
        _attackTimer = 0;
    }

    //@ 데미지 입는 경우 
    public void DamagedState()
    {

        //& 공룡의 hp가 10보다 크거나 같은 경우 if()
        _dinosaurAnimation.ChangeAnimation(DinosaurAnimationManager.DAMAGED);
        _damaged = true;

        StartCoroutine(DamagedFalse());

        //& 피가 0이 될 때 사망 .. 0이거나 이보다 작은 경우 .. 이벤트로 처리 (UnityEngine.Event)

        // // if (_dinosaurParams._isAlive == false)
        // // {
        // //     DeadState();
        // // }

    }

    IEnumerator DamagedFalse()
    {
        ////_dinosaurAnimation.ChangeAnimation(DinosaurAnimationManager.DAMAGED);
        yield return new WaitForSeconds(0.65f);
        _damaged = false;
    }


    //@ 상태 변화를 위한 함수 
    private void UpdateState()
    {
        switch (_currentState)
        {

            case DinosaurSTATE.IDLE:
                IdleState();    //& 거리함수 < 추적함수

                break;

            case DinosaurSTATE.FIGHTIDLE:
                FightIdleState();

                break;

            case DinosaurSTATE.ATTACK:
                AttackState();

                break;

            case DinosaurSTATE.MOVE_WALK:
                // MoveState();

                break;

            case DinosaurSTATE.HUNT:
                HuntingState();

                break;

            case DinosaurSTATE.DAMAGED:
                DamagedState();

                break;

            case DinosaurSTATE.DEAD:
                DeadState();

                break;

            case DinosaurSTATE.NOSTATE:

                break;

        }
    }

    private void FightIdleState()
    {

    }

    //@ 사망 
    private void DeadState()
    {
        // // _audioBox.PlayOneShot(_deadSound);
        // // _dinosaurAnimation.ChangeAnimation(DinosaurAnimationManager.DEAD);
        // // _dinosaurParams._isDinoAlive = false;

        //& 콜라이더 비활성화
        gameObject.GetComponent<Collider>().enabled = false;
    }
    public void DamagedHitSoundPlay()
    {
        AudioSource.PlayClipAtPoint(_damagedHitSound, transform.position);
        ////_audioBox.PlayOneShot(_damagedHitSound);
    }

}
