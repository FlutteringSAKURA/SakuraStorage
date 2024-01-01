using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Update: //@ 2023.11.03 
// Update: //@ 2023.11.06 
// Update: //@ 2023.11.07 

// NOTE: //# 3D 게임 - Cyber Creature 컨트롤러 .. 순찰 기능이 들어간 Creature(AI)
//#          1)


//~ ------------------------------------------------------------------------
public class CyberCreatureController : MonoBehaviour
{
    public enum CreatureState
    {
        IDLE, TRACE, PATROL, ATTACK, RELOAD, DAMAGED, DIE
    }
    public CreatureState _creatureState = CreatureState.IDLE;
    Transform _creatureTr;
    Transform _playerTr;
    NavMeshAgent _navMeshAgent;
    WaitForSeconds _waitSeconds;
    MoveAgent _moveAgent;   //% 각 포인트로 이동하는 클래스 스크립트
    Animator _creatureAnimator;
    public float _traceDistance = 16.0f;



    public float _attackDistance = 13.0f;
    //public float _roarDistance = 13.0f;
    //public bool _isRoar = false;
    public bool _isAlive = true;
    public bool _isAttack = false;
    public float _timeFlow = 0.0f;
    public float _coolTime = 5.0f;

    Rigidbody _creatureRigidBody;

    //# NOTE: 애니메이터컨트롤러에 설정되어 있는 파라미터 값 추출
    readonly int _hashWalk = Animator.StringToHash("isWalk");
    readonly int _hashAttack = Animator.StringToHash("isAttack");
    readonly int _hashHitted = Animator.StringToHash("isHitted");
    readonly int _hashDead = Animator.StringToHash("isDead");
    readonly int _hashDeadRandomIndex = Animator.StringToHash("DeadRandom");
    readonly int _hashShoot = Animator.StringToHash("isShoot");
    readonly int _hashCoolTime = Animator.StringToHash("isCoolTime");
    readonly int _hashSpeed = Animator.StringToHash("Speed");
    readonly int _hashWalkSpeed = Animator.StringToHash("walkSpeed");
    readonly int _hashPlayerDie = Animator.StringToHash("isPlayerDie");
    // Update: //@ 2023.11.07 
    readonly int _hashOffSet = Animator.StringToHash("offset");
    readonly int _hashMoveSpeed = Animator.StringToHash("isMoveSpeed");

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
    public bool _explosionFlag = false;

    // TEST: 
    public AudioClip _cyberCreatureHittedSound;


    GameObject _playerObj;


    public float _reloadTime = 1.3f;
    public bool _shootFlag = false;

    //~ ------------------------------------------------------------------------

    private void Awake()        //#  NOTE: Awake >> OnEnable >> Start
    {
        _creatureTr = this.gameObject.GetComponent<Transform>();
        // Legacy:
        /*
        var _player = GameObject.FindWithTag("Player");
        if (_player != null)    //# 플레이어 게임오브젝트를 찾으면 플레이어의 Transform컴포넌트를 Get하는 코드
            _playerTr = _player.GetComponent<Transform>();
        */
        // --------------------------------------------------            
        _playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _playerObj = GameObject.FindWithTag("Player");
        _moveAgent = GetComponent<MoveAgent>();
        _creatureAnimator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _creatureRigidBody = GetComponent<Rigidbody>();
        _waitSeconds = new WaitForSeconds(0.3f);

        // _isRoar = false;
        _isAlive = true;
        _isAttack = false;

        _explosionFlag = false;
        _shootFlag = false;

        // Update: //@ 2023.11.07 
        //# 게임 시작후 offset값 random
        _creatureAnimator.SetFloat(_hashOffSet, Random.Range(0.0f, 1.0f));
        //# 게임 시작후 moveSpeed값 random
        _creatureAnimator.SetFloat(_hashMoveSpeed, Random.Range(1.0f, 1.2f));

    }

    private void OnEnable()     //& 이벤트 발생시
    {
        StartCoroutine(CheckCreatureState());
        StartCoroutine(CreatureAction());

        // Update: //@ 2023.11.03 
        //% PlayerController 스크립트의 OnPlayerDie 이벤트 발생시 이 스크립트의 PlayerIsDead 실행
        PlayerController.OnPlayerDie += this.PlayerIsDead;      //^ 연결
    }

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        //% Resources폴더의 게임오브젝트(이름)을 불러오겠다는 코드
        _bloodEffect = Resources.Load<GameObject>("SplashBlood");
        _skinMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    //~ ------------------------------------------------------------------------

    private void OnDisable()        //& 이벤트 비활성화
    {
        PlayerController.OnPlayerDie -= this.PlayerIsDead;      //^ 해제
    }

    IEnumerator CheckCreatureState()
    {
        WaitForSeconds _waitTime = new WaitForSeconds(0.2f);


        while (_isAlive)        //% CyberCreature가 사망할 때까지
        {
            yield return _waitTime;
            float _distance = (_playerTr.position - _creatureTr.position).sqrMagnitude;
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
                ////Debug.Log("크리처의 상태 : " + _creatureState);
            }

            else if (_betweenDistance <= _traceDistance)
            {

                _creatureState = CreatureState.TRACE;

                _isAttack = false;
                ////Debug.Log("크리처의 상태 : " + _creatureState);
            }

            else
            {
                _creatureState = CreatureState.PATROL;
                _isAttack = false;

                ////Debug.Log("크리처의 상태 : " + _creatureState);
            }
        }

    }

    IEnumerator CreatureAction()
    {
        while (_isAlive)
        {
            switch (_creatureState)
            {
                case CreatureState.IDLE:


                    _creatureAnimator.SetBool(_hashCoolTime, true);

                    _timeFlow += Time.deltaTime;
                    if (_timeFlow >= _reloadTime)
                    {
                        _creatureState = CreatureState.ATTACK;
                        _timeFlow = 0.0f;
                    }

                    break;

                case CreatureState.TRACE:
                    _timeFlow += Time.deltaTime;
                    // Legacy:


                    _navMeshAgent.destination = _playerTr.position;
                    _navMeshAgent.isStopped = false;
                    _creatureAnimator.SetBool(_hashWalk, true);
                    _creatureAnimator.SetBool(_hashAttack, false);

                    _shootFlag = false;

                    // _creatureAnimator.SetBool(_hashRoar, true);
                    // _creatureAnimator.SetInteger("roarCheck", 0);
                    //_isRoar = true;

                    //  StartCoroutine(StartTrace());

                    //& 속도 증가 (Run)
                    _navMeshAgent.speed = 4.5f;

                    //_creatureRigidBody.isKinematic = true;
                    ////Debug.Log("추적상태");
                    break;

                case CreatureState.ATTACK:
                    // _timeFlow = 0.0f;
                    //  _navMeshAgent.destination = _playerTr.position;
                    //& 순찰 및 추적 정지
                    // Legacy: _navMeshAgent.isStopped = true;
                    _moveAgent.Stop();

                    // TEST://# 크리처가 플레이어를 계속적으로 바라보게 방향을 회전 // SUCCESS:
                    // NOTE: //^ Lerp : 보간법, 시작과 끝을 계산하여 자연스럽게 .. // LookRotation : 플레이어 위치와 그 차이 만큼 방향을 회전시켜달라는 함수
                    Vector3 _dirPos = _playerTr.position - transform.position;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_dirPos),
                    60f * Time.deltaTime);

                    _creatureAnimator.SetBool(_hashAttack, true);
                    _creatureAnimator.SetBool(_hashWalk, false);
                    _creatureAnimator.SetTrigger(_hashShoot);

                    AttackState();

                    _shootFlag = true;

                    _timeFlow += Time.deltaTime;
                    if (_timeFlow >= _reloadTime)
                    {
                        _timeFlow = 0.0f;
                        _creatureState = CreatureState.IDLE;

                        //gameObject.GetComponent<CyberCreatureFireCtrl>().SendMessage("GunFire");
                        //  _creatureAnimator.SetBool(_hashReload, false);
                    }

                    ////Debug.Log("공격상태");
                    break;

                case CreatureState.RELOAD:

                    break;

                case CreatureState.PATROL:

                    PatrolState();

                    break;

                case CreatureState.DAMAGED:
                    //SoundManager.instance.CyberCreatureHitSound();

                    // TEST:
                    //? Hitted Sound 적용중..
                    SoundManager.instance.PlaySfx(gameObject.transform.position, _cyberCreatureHittedSound);

                    //  _timeFlow = 0.0f;

                    //& 속도 감소
                    //_navMeshAgent.speed = 2.0f;

                    ////Debug.Log("데미지 입는 상태");
                    break;


                case CreatureState.DIE:
                    //  _timeFlow = 0.0f;
                    //   _navMeshAgent.isStopped = true;
                    _creatureAnimator.SetTrigger(_hashDead);
                    DeadState();

                    //_creatureRigidBody.isKinematic = false;
                    ////Debug.Log("죽음상태");
                    break;
            }
            yield return null;
        }
    }

    private void DeadState()
    {
        //this.gameObject.tag = "Untagged";
        _isAlive = false;
        _moveAgent.Stop();
        CyberCreatureFireCtrl _cyberCreatureFireCtrl = gameObject.GetComponent<CyberCreatureFireCtrl>();
        _cyberCreatureFireCtrl.enabled = true;
        _creatureAnimator.SetInteger(_hashDeadRandomIndex, Random.Range(0, 3));
        _creatureAnimator.SetTrigger(_hashDead);
    }

    private void AttackState()
    {

    }

    private void PatrolState()
    {
        //% MoveAgent스크립트에 접근해서 순찰모드로 전환
        _moveAgent.IsPatrolFlag = true;
        _creatureAnimator.SetBool(_hashWalk, true);
        _creatureAnimator.SetBool(_hashAttack, false);
        _navMeshAgent.isStopped = false;

        _timeFlow = 0.0f;
        _shootFlag = false;

        //& 속도 원상회복 (Walk)
        _navMeshAgent.speed = 1.5f;
    }


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
            _creatureHp -= other.gameObject.GetComponent<Bullet>()._damage;
            Debug.Log("creature의 생명력 : " + _creatureHp);
            if (_creatureHp <= 0)
            {
                _creatureHp = 0;
                CreatureDie();
                Debug.Log("creature의 생명력 : " + _creatureHp);

                float _point = Random.Range(130f, 200f);
                GameManagerScript.instance.DisplaySocre(_point);
                GameManagerScript.instance.AddKillCount(1);
            }

            //Destroy(other.gameObject);
        }

    }

    private void CreatureDie()
    {
        gameObject.tag = "Untagged";

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
        //% 크리처 몸통 캡슐콜라이더 비활성화
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        Debug.Log("creature 사망 ");
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
        // _navMeshAgent.isStopped = true;

        _creatureAnimator.SetFloat(_hashSpeed, Random.Range(0.7f, 1.2f));
        //_creatureAnimator.SetTrigger(_hashPlayerDie);

        //& 다시 순찰 모드..
        PatrolState();


        Debug.Log("플레이어가 Cyber Creature에 의해 죽었습니다.");
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

    // Update: //@ 2023.11.07 
    //#        델리게이트 이벤트식으로 코드 변경 >> OnplayerDie ==>> PlayerIsDead
    //% 플레이어 사망. 이벤트 발생
    public void OnPlayerDie()
    {
        if (!_isAlive)      //& 방어코드 :  죽은 크리처에게는 아무 영향 주지 않음
            return;
        _moveAgent.Stop();
        _isAttack = false;
        StopAllCoroutines();
        _creatureAnimator.SetTrigger(_hashPlayerDie);

    }

    //~ ------------------------------------------------------------------------
    private void Update()
    {

        if (_creatureState == CreatureState.TRACE)
            _creatureAnimator.SetFloat(_hashWalkSpeed, _moveAgent._blendSpeed);
    }
}
