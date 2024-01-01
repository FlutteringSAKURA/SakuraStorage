//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

// Update: //@ 2023.11.02 
// Update: //@ 2023.11.03 
// Update: //@ 2023.11.09 
// Update: //@ 2023.11.13 

// NOTE: //# 3D 게임 - Creature 컨트롤러
//#          1) 유한상태머신 구현       Completed:
//#          2) NavMeshAgent적용     Completed:
//#          3) 피격시 BloodEffect구현   Completed:
//#          4) 배럴 영향에 크리처도 폭살되는 효과 구현 (+ 숯덩이) Completed:
//#          5) 크리처 사망 구현

//~ ------------------------------------------------------------------------
public class CreatureController : MonoBehaviour
{
    public enum CreatureState
    {
        IDLE, TRACE, ATTACK, ROAR, DAMAGED, DIE
    }
    public CreatureState _creatureState = CreatureState.IDLE;
    Transform _creatureTr;
    Transform _playerTr;
    NavMeshAgent _navMeshAgent;
    Animator _creatureAnimator;
    public float _traceDistance = 10.0f;

    public float _attackDistance = 2.8f;
    public float _roarDistance = 13.0f;
    public bool _isRoar = false;
    public bool _isAlive = true;
    public bool _isAttack = false;
    public float _timeFlow = 0.0f;
    public float _coolTime = 5.0f;

    Rigidbody _creatureRigidBody;

    //# NOTE: 애니메이터컨트롤러에 설정되어 있는 파라미터 값 추출
    readonly int _hashTrace = Animator.StringToHash("isTrace");
    readonly int _hashAttack = Animator.StringToHash("isAttack");
    readonly int _hashHitted = Animator.StringToHash("isHitted");
    readonly int _hashDead = Animator.StringToHash("isDead");
    readonly int _hashRoar = Animator.StringToHash("isRoar");
    readonly int _hashSpeed = Animator.StringToHash("animationSpeed");
    readonly int _hashPlayerDie = Animator.StringToHash("isPlayerDie");
    //# ------------------------------------------------

    GameObject _bloodEffect;
    public GameObject _bloodDecalFx;
    public GameObject _fireEffect;
    public Transform _firePivot;

    //# 텍스처
    private SkinnedMeshRenderer[] _skinMesh;
    public Material _fireBurnMat;

    // Update: //@ 2023.11.03 
    public float _creatureHp = 100;
    public float _currentHp;
    public bool _explosionFlag = false;

    // TEST: //# 빛반응 
    GameObject _playerObj;

    // Update: //@ 2023.11.08 
    // Update: //@ 2023.11.09 
    public AudioClip _roarSoundClip;
    public AudioClip _hittedSoundClip;
    public AudioClip _dieSoundClip;
    public AudioClip _attackSoundClip;
    public float _roarTime = 0.0f;
    public float _roarCoolTime = 1.5f;
    public bool _noDamageFlag = false;

    // Update: //@ 2023.11.10 
    //# creature Hpbar 구현
    public Image _hpBar;
    public TMP_Text _nameText;
    Color _basicColor = new Vector4(0.0f, 1.0f, 0, 1.0f);   //& 초록
    Color _changedColor;
    public int _nameTextChangeHpValue = 55;

    public GameObject _canvasRigidBody;
    public Transform _initUiTr;

    public static CreatureController instance;

    // Update: //@ 2023.11.13 
    //# 크치러 체력바 어느 방향에서든 플레이어를 바라보게 하기 구현
    //! TEMP: 크리처 하위에 체력바가 있기 때문에 크리처 자체도 돌아가는 문제 있음.

    //Transform _mainCamTr;

    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        if (instance != null) instance = this;


        _creatureTr = this.gameObject.GetComponent<Transform>();
        _playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _creatureAnimator = GetComponent<Animator>();

        _creatureRigidBody = GetComponent<Rigidbody>();

        _playerObj = GameObject.FindWithTag("Player");
        //_canvas = GameObject.FindWithTag("Canvas");

        _isRoar = false;
        _isAlive = true;
        _isAttack = false;

        _explosionFlag = false;
        _noDamageFlag = false;

        // Update: //@ 2023.11.10 
        _hpBar.color = _basicColor;     //& 체력바 색이 기본 색이 된다.
        _changedColor = _basicColor;    //& 변화한 색이 기본 색이 된다.
        _currentHp = _creatureHp;



    }
    //~ ------------------------------------------------------------------------
    private void OnEnable()     //& 게임오브젝트가 활성화 될 때 .. 유니티 콜백 함수
    {
        StartCoroutine(CheckCreatureState());       //^ 크리처의 현 상태
        StartCoroutine(CreatureAction());       //^ 코루틴 활용한 상태별 액션 체크

        // Update: //@ 2023.11.03 
        //% PlayerController 스크립트의 OnPlayerDie 이벤트 발생시 이 스크립트의 PlayerIsDead 실행
        PlayerController.OnPlayerDie += this.PlayerIsDead;      //^ 연결
    }
    private void OnDisable()        //& 이벤트 비활성화 .. 오브젝트 비활성화할 때 작동하는 함수
    {
        PlayerController.OnPlayerDie -= this.PlayerIsDead;      //^ 해제
    }

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        //% Resources폴더의 게임오브젝트(이름)을 불러오겠다는 코드
        _bloodEffect = Resources.Load<GameObject>("SplashBlood");
        _skinMesh = GetComponentsInChildren<SkinnedMeshRenderer>();

        //! TEMP: 크리처 하위에 체력바가 있기 때문에 크리처 자체도 돌아가는 문제 있음.
        //_mainCamTr = Camera.main.transform;



    }

    //~ ------------------------------------------------------------------------
    //@ 유한 상태 머신 (FSM) 
    IEnumerator CheckCreatureState()
    {
        WaitForSeconds _waitTime = new WaitForSeconds(0.1f);

        while (_isAlive)
        {
            yield return _waitTime;
            // Update: //@ 2023.11.03 
            //# 크리처가 사망상태이면 코루틴 중단
            if (_creatureState == CreatureState.DIE)
                yield break;
            //# ------------

            float _betweenDistance = Vector3.Distance(_playerTr.position, _creatureTr.position);
            if (_betweenDistance <= _attackDistance)
            {
                _creatureState = CreatureState.ATTACK;
                _isAttack = true;
                Debug.Log("공격");

                _roarTime = 0.0f;
            }

            else if (_betweenDistance <= _traceDistance)
            {
                _creatureState = CreatureState.TRACE;
                _isRoar = true;

                _roarTime += Time.deltaTime;

                //% roar시간 체크
                StartCoroutine(Roar());

                _isAttack = false;
                Debug.Log("추적");
            }

            else
            {
                _creatureState = CreatureState.IDLE;
                _isAttack = false;
                _roarTime = _roarCoolTime;
                Debug.Log("아이들");
            }
        }

    }

    IEnumerator Roar()
    {
        if (_roarTime >= _roarCoolTime)
        {
            SoundManager.instance.PlaySfx(gameObject.transform.position, _roarSoundClip);
            _navMeshAgent.isStopped = true;
            //& roar시간 동안은 물리영향 받지 않도록..(피 깎이지 않도록)
            // TEST:  
            //? _creatureRigidBody.isKinematic = true;
            _noDamageFlag = true;

        }
        yield return new WaitForSeconds(1.5f);
        _isRoar = false;
        _roarTime = 0.0f;
        _navMeshAgent.isStopped = false;
        //? _creatureRigidBody.isKinematic = false;
        _noDamageFlag = false;
    }

    IEnumerator CreatureAction()
    {
        while (_isAlive)
        {
            switch (_creatureState)
            {
                case CreatureState.IDLE:
                    _navMeshAgent.isStopped = true;
                    _creatureAnimator.SetBool(_hashTrace, false);

                    //_creatureRigidBody.isKinematic = true;

                    _timeFlow += Time.deltaTime;
                    if (_timeFlow > _coolTime)      //& 3초가 지나면..
                    {
                        _creatureAnimator.SetTrigger("isReset");
                        _creatureAnimator.SetInteger("roarCheck", 1);
                        //_isRoar = true;

                    }
                    ////Debug.Log("아이들 상태");
                    break;

                case CreatureState.TRACE:
                    _timeFlow = 0.0f;
                    // Legacy:
                    //_navMeshAgent.destination = _playerTr.position;

                    //* _navMeshAgent.isStopped = false;

                    _creatureAnimator.SetBool(_hashRoar, true);
                    _creatureAnimator.SetInteger("roarCheck", 0);
                    // _isRoar = false;

                    StartCoroutine(StartTrace());


                    //& 속도 원상 복구
                    //_navMeshAgent.speed = 3.5f;

                    //_creatureRigidBody.isKinematic = true;
                    ////Debug.Log("추적상태");
                    break;

                case CreatureState.ATTACK:

                    _timeFlow = 0.0f;
                    _navMeshAgent.velocity = Vector3.zero;
                    _navMeshAgent.isStopped = true;
                    _creatureAnimator.SetBool(_hashAttack, true);
                    _isRoar = false;

                    // Update: //@ 2023.11.07 
                    LookTarget();

                    // _creatureRigidBody.isKinematic = true;
                    ////Debug.Log("공격상태");
                    break;

                case CreatureState.ROAR:
                    _timeFlow = 0.0f;
                    _navMeshAgent.isStopped = true;
                    _creatureAnimator.SetBool(_hashRoar, true);
                    _isRoar = true;


                    // _creatureRigidBody.isKinematic = false;
                    ////Debug.Log("울부짖음상태");
                    break;

                case CreatureState.DAMAGED:
                    _timeFlow = 0.0f;
                    _isRoar = false;
                    //& 속도 감소
                    //_navMeshAgent.speed = 2.0f;

                    ////Debug.Log("데미지 입는 상태");
                    break;


                case CreatureState.DIE:
                    _timeFlow = 0.0f;
                    _navMeshAgent.isStopped = true;
                    _creatureAnimator.SetTrigger(_hashDead);
                    _isRoar = false;
                    _isAlive = false;

                    // yield return new WaitForSeconds(3.0f);

                    //? TEST


                    //_creatureRigidBody.isKinematic = false;
                    ////Debug.Log("죽음상태");
                    break;
            }
            yield return null;
        }
    }

    private void LookTarget()
    {
        Vector3 _dirPos = _playerTr.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_dirPos),
        60f * Time.deltaTime);

    }

    IEnumerator StartTrace()
    {
        //_isRoar = true;

        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1.875f);
        _isRoar = false;

        _creatureAnimator.SetBool(_hashTrace, true);
        _creatureAnimator.SetBool(_hashAttack, false);
        _navMeshAgent.isStopped = false;
        // Update:
        _navMeshAgent.SetDestination(_playerTr.position);

    }

    //@ 충돌 함수 
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Bullet"))
        {
            _creatureState = CreatureState.DAMAGED;
            //% 애니메이션은 따로 분리.. 데미지상태에서 계속 애니메이션 반복되는 현상을 방지하기 위해 
            _creatureAnimator.SetTrigger("isHitted");

            //% 총알 맞을 때 너무 밀려나는 것을 방지하기 위해
            //_creatureRigidBody.isKinematic = true;

            //% Vec3값의 변수에 충돌된 부분의 값을 담음
            Vector3 _position = other.GetContact(0).point;
            //% Quaternion 회전값의 변수에 충돌된 부분의 정규화 값을 빼주어 회전값 넣어줌
            Quaternion _rotation = Quaternion.LookRotation(-other.GetContact(0).normal);    //& 충돌지점 법선벡터
            //% 위 위치좌표와 회전값으로 함수 실행
            CreateBloodEffect(_position, _rotation);


            // Update: //@ 2023.11.03 
            //# NOTE: Creature 데미지 + 사망 코드 적용

            if (_noDamageFlag)   //& 울부짖는 동안은 피가 달지 않게
                return;

            _currentHp -= other.gameObject.GetComponent<Bullet>()._damage;
            Debug.Log("creature의 생명력 : " + _currentHp);

            // Update: //@ 2023.11.10 
            //# creature hpBar 구현 
            //% 크리처 체력바 업데이트 함수 호출
            UpdateCreatureUI();

            // Update: //@ 2023.11.09 
            SoundManager.instance.PlaySfx(gameObject.transform.position, _hittedSoundClip);

            if (_currentHp <= 0)
            {
                _currentHp = 0;
                CreatureDie();
                // TEST:
                _creatureState = CreatureState.DIE;

                float _point = Random.Range(50f, 100f);
                GameManagerScript.instance.DisplaySocre(_point);

                GameManagerScript.instance.AddKillCount(1);
                Debug.Log("creature의 생명력 : " + _creatureHp);

                if (gameObject.tag.Contains("CreatureTeeth"))
                {
                    Invoke("ReGenCreture", 4.5f);
                    Debug.Log("ready to regen Creature");

                }
            }

            // Destroy(other.gameObject);
        }

    }

    private void UpdateCreatureUI()
    {
        float _hp = _currentHp / _creatureHp;
        if (_hp > 0.5f)
        {
            _changedColor.r = (1 - _hp) * 2.0f;

        }
        else
        {
            _changedColor.g = _hp * 2.0f;
        }
        _hpBar.color = _changedColor;
        _hpBar.fillAmount = _hp;

        if (_currentHp <= _nameTextChangeHpValue)
        {
            _nameText.color = Color.red;
        }
        // if (_creatureHp <= _nameTextChangeHpValue)
        // {
        //     _nameText.color = Color.red;
        // }
    }

    private void CreatureDie()
    {
        if (gameObject.tag == "Creature")
        {
            gameObject.tag = "Untagged";
        }

        // Update: //@ 2023.11.09 
        SoundManager.instance.PlaySfx(gameObject.transform.position, _dieSoundClip);

        //# 코루틴 자체에서 죽은 경우 yield break; 로 대체함
        StopAllCoroutines();

        _isAlive = false;
        _creatureState = CreatureState.DIE;

        if (!_explosionFlag)
        {
            _navMeshAgent.isStopped = true;
            _creatureAnimator.SetTrigger(_hashDead);
        }

        // TEST:
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Invoke("RigidBodyKinematic", 0.5f);


        //% 크리처 몸통 캡슐콜라이더 비활성화
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }

        // Update: //@ 2023.11.10 
        gameObject.GetComponentInChildren<Rigidbody>().isKinematic = false;      //& 사망시 크리처 UI 바닥으로 떨어짐


        Debug.Log("creature 사망 ");



    }
    void RigidBodyKinematic()
    {
        //_canvasRigidBody.AddComponent<Rigidbody2D>();
        _canvasRigidBody.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //_canvasRigidBody.GetComponent<FallDownUI>().enabled = true;
        Invoke("SetActiveFalse", 2.5f);
    }

    void SetActiveFalse()
    {
        _canvasRigidBody.SetActive(false);

        if (gameObject.tag.Contains("CreatureTeeth"))
        {
            //ReGenCreture();
            //!Debug.Log("ready to regen Creature");

        }
    }

    void ReGenCreture()     //% 3구역 크리처이면 리젠준비. (비활성화).. 오브젝트 풀 방식의 크리처이기 때문에
    {
        Debug.Log("ready to regen Creature");
        _isAlive = true;
        _creatureState = CreatureState.IDLE;
        //gameObject.tag = "CreatureTeeth";
        _currentHp = 100;

        _canvasRigidBody.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //_canvasRigidBody = GameObject.FindGameObjectWithTag("CreatureCanvas");

        _canvasRigidBody.transform.position = _initUiTr.transform.position;

        _hpBar.color = _basicColor;
        //_changedColor = _basicColor;


        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

        this.gameObject.SetActive(false);
        _canvasRigidBody.SetActive(true);


    }

    //@ 피격시 혈흔 효과 함수 
    private void CreateBloodEffect(Vector3 position, Quaternion rotation)
    {
        GameObject _blood = Instantiate(_bloodEffect, position, rotation, _creatureTr) as GameObject;
        Destroy(_blood, 1.5f);

        // Update: //# 바닥 혈흔 효과 구현
        Vector3 _decalPos = _creatureTr.position + (Vector3.up * 0.02f);
        Quaternion _decalRot = Quaternion.Euler(90.0f, 0, Random.Range(0, 360.0f));
        GameObject _bloodDecal = (GameObject)Instantiate(_bloodDecalFx, _decalPos, _decalRot);

        float _bloodDecalscale = Random.Range(1.5f, 3.5f);
        _bloodDecal.transform.localScale = Vector3.one * _bloodDecalscale;
        Destroy(_bloodDecal, 5.0f);
    }

    //@ 트리거 함수 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

    void PlayerIsDead()
    {
        //& 모든 코루틴 정지
        StopAllCoroutines();
        //& 추적 정지
        _navMeshAgent.isStopped = true;

        if (!_isAlive)      //& 방어코드 :  죽은 크리처에게는 아무 영향 주지 않음
            return;
        _creatureAnimator.SetFloat(_hashSpeed, Random.Range(0.7f, 1.2f));

        _creatureAnimator.SetTrigger(_hashPlayerDie);


        Debug.Log("플레이어가 Creature에 의해 죽었습니다.");
    }

    //@ 추적시 시각적 효과..추적시 사정거리 표시
    private void OnDrawGizmos()
    {
        //% 추적시 사정거리 표시
        if (_creatureState == CreatureState.TRACE)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _traceDistance);
        }

        //% 공격 사정거리 표시
        if (_creatureState == CreatureState.ATTACK)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
        }

    }

    // Update: //@ 2023.11.09 
    //# 애니메이션 이벤트로 호출
    private void AttackSoundPlay()
    {
        SoundManager.instance.PlaySfx(gameObject.transform.position, _attackSoundClip);
    }


    //@ 폭발 데미지 함수 
    //# NOTE: Barrel 스크립트에서 SendMessage로 호출하고 있음. 호출하는 함수명이 동일해야 작동함에 주의
    private void OnExpDamage()
    {
        _explosionFlag = true;
        _creatureState = CreatureState.DIE;

        //% Nav Mesh 관련 에러 막기 위한 코드
        StopAllCoroutines();

        //% Creature 폭살시 시체에 걸려 플레이어 피가 줄어드는 현상 방지를 위한 코드
        // NOTE: //# 다만 Creature의 AttackPos가 손에 있으므로 그 손에 있는 SphereCollider만 비활성화. 
        //#  모든 콜라이더를 비활성화하면 배럴폭발시 물리효과를 받지 못하는 문제가 있기 때문에
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            //& 크리처의 collider 비활성화
            coll.enabled = false;
        }

        //# 불 옮겨 붙는 효과 코드
        //% _firePivot 위치좌표의 Quaternion회전값으로 _fireFeefect 프리팹을 생성해 _burningFire 함수에 넣음
        GameObject _burningFire = (GameObject)Instantiate(_fireEffect, _firePivot.position, Quaternion.identity);
        //% _firePivot을 부모로 삼아 생성된 _burningFire가 하위로 들어감 (= 불 옮겨 붙는 효과)
        _burningFire.transform.parent = _firePivot;

        Destroy(_burningFire, 8.0f);

        //# 물리속성 영향 받게 하기 위함 .. barrel스크립트에서 이미 해주고 있음.
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _creatureAnimator.enabled = false;
        _navMeshAgent.enabled = false;

        //# Creature가 몸이 타버린 텍스처 효과 구현
        foreach (SkinnedMeshRenderer skin in _skinMesh)
        {
            skin.material = _fireBurnMat;
        }

        Destroy(gameObject, 8.0f);


    }

    // TEST: 플레이어의 라이트가 비춰지면 사운드가 바뀌는 함수
    private void Detected()
    {
        // 음악 재생
        // SoundManager.instance.CreatureDetection();
    }
    private void Undetected()
    {
        // 음악 정지
        SoundManager.instance.CreatureUndetection();
    }

    //?


    private void Update()
    {
        // Update: //@ 2023.11.13 
        //# 크리처 체력바 어느 방향에서든 플레이어를 바라보게 하기 구현
        //! TEMP: 크리처 하위에 체력바가 있기 때문에 크리처 자체도 돌아가는 문제 있음.

        // transform.LookAt(transform.position +
        //     _mainCamTr.rotation * Vector3.forward, _mainCamTr.rotation * Vector3.up);

    }
}
