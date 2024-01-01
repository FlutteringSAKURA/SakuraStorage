using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent를 사용하기 위한 유니티 엔진 코드

public class MonsterCtrl : MonoBehaviour
{
    // [ 3차 추가 수정 코드 ] 개발자 정의 선언 
    public enum MonsterState { idle, trace, attack, kickAttack, roar, die };  // kickAttack은 TEST CODE
    // Monster State를 idle 상태로 초기화
    public MonsterState monState = MonsterState.idle;
    // ==================================================== 3차 코드 END

    // [ TEST CODE ] -> 발차기 공격 유무 
    public bool _iskickAttack = false;
    // 기본 공격 유무
    public bool _isAttack = false;
    // =================================== Test Code END 

    // [ 1차 기본 코드 ]
    private Transform monsterTr; // 몬스터 위치 인스턴스 선언
    private Transform playerTr;  // 플레이어 위치 인스턴스 선언
    private NavMeshAgent nvAgent; // NavMeshAgent 인스턴스 선언

    // [ 4차 추가 수정 코드 ] Animator 인스턴스 선언
    private Animator anim;

    // [3차 추가 수정 코드]
    public float traceDist = 10.0f; // 추격거리 10미터
    public float attackDist = 2.0f; // 공격 거리 2미터 
    // [ TEST CODE ]
    public float roarDist;
    // ================================================= 3차 코드 END

    // [ 7차 추가 수정 코드 ]
    public GameObject bloodEffect; // 피흘리는 효과를 위한 인스턴스 선언
    public GameObject bloodDecal;  // 혈흔 효과를 위한 인스턴스 선언

    // [ 파이어 몬스터 코드 1 ] 
    public GameObject fireEffect; // 불타는 효과를 위한 인스턴스 선언
    // ================================================파이어 몬스터 코드 END

    // [3차 추가 수정 코드]
    private bool isDie = false; // 죽은 상태가 아님 (초기화)
    // ============================ 3차 추가 수정 코드 END

    // [ 사망 구현 추가 코드 ]
    public int hp = 100;
    // =============================== END

    // [ 감마 코드 1 ]
    private GameUI gameUI;
    // ========================= END

    // [ 파이어 몬스터 코드 1 ]
    public Material normalMonster;
    public Material fireMonster;
    public Transform firePivot;
    private SkinnedMeshRenderer[] skinMesh;
    public bool expChk = false;
    // =================================== 파이어 몬스터 코드 END

    // [ TEST CODE]
    public bool _isRoar = false;
    // ========================== TEST CODE END



    // [ 풀 베타 코드 1 ] -> Start 함수를 Awake 함수로 변형
    private void Awake()
    {
        // 몬스터의 위치 캐시처리
        monsterTr = this.gameObject.GetComponent<Transform>();
        // Player 태그를 가진 게임오브젝트의 위치 캐시처리 (트랜스폼을 가져옴)
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
        // [ 1차 코드 ] Player의 위치를 목적지로 
        // nvAgemt.destination = playerTr.position;

        // [ 4차 추가 수정 코드 ] -> Animator 캐시 처리
        anim = GetComponent<Animator>();

        // [ 감마 코드 2 추가 ] -> 캐시 처리
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        // ====================================================== 감마 코드 END

        // [ 파이어 몬스터 코드 1 ]
        skinMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        // ============================================== 파이어 몬스터 코드 END


        // [ 3차 추가 수정 코드 ] -> [ 풀 베타 코드 ] -> Awake함수로 바꿔줄 경우 주석 처리
        // Awake 함수에서는 코루틴 함수 호출 불가
        // StartCoroutine(this.CheckMonsterState());
        // StartCoroutine(this.MonsterAction());




    }

    // [ +++ 베타 추가 코드 ] -> 해당 게임오브젝트가 활성화 되면 무조건 한번 수행
    // Start 함수의 경우는 게임오브젝트가 Destroy된 후가 아니면 다시 수행 불가
    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlyerDie; // 플레이어 사망 이벤트를 호출 활성화

        // [ TEST CODE ] disable Monster's SphereCollider 
        //  PlayerCtrl.OffCollider += this.MonsterDie; 
        // ============================================= test code end
        // [ 풀 베타 코드 2 ] -> Awake 함수에선 코루틴 함수를 사용할 수 없으므로 OnEnable 함수에서 실행
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
        // =============================================
    }
    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlyerDie; // 비활성화
                                                   // PlayerCtrl.OffCollider -= this.OffCollider; // 비활성화
    }
    // ====================== +++ 베타 코드 END

    private IEnumerator CheckMonsterState()
    {
        // [ 3 - 1차 추가 수정 코드 ]
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while (!isDie)
        {
            // 0.2 초 기다렸다가 몬스터 상태를 체크
            // [ 3차 추가 수정 코드] 
            // yield return new WaitForSeconds(0.2f); // 메모리 누수 현상이 생긴다.(권장하지 않는 중급 코드)
            // [ 3 - 1차 추가 수정 코드]
            yield return waitTime;

            // 작은 값부터 비교해야 한다.
            // attackDist와 traceDist 각각의 값을 비교하면 attackDist의 값이 작기 때문에
            // 먼저 연산하지 않으면 traceAttackDist의 연산에 포함되므로
            // AttackDist가 수행되지 않음에 주의

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            // [ TEST CODE ]
            if (dist <= roarDist) // 몬스터와 플레이어의 거리가 추적거리와 같다면
            {
                // 표효하라
                Roar();
            }
            // =================================== TEST CODE END

            if (dist <= attackDist) // Player와 monster의 거리가 공격거리와 같거나 작다면
            {
                // 공격 해라
                monState = MonsterState.attack;
                _isAttack = true;

                // [ TEST CODE ]  공격을 했고 발차기를 하지 않은 상태라면
                if (_isAttack && !_iskickAttack)
                {
                    KickAttack();

                }
                // ===================== Test Code End

            }
            else if (dist <= traceDist) // 추적거리와 같거나 작다면
            {

                // 추적해라
                monState = MonsterState.trace;
            }
            // 그 밖의 경우는 대기해라
            else { monState = MonsterState.idle; }
        }
    }
    // [ TEST CODE ]
    private void Roar()
    {
        anim.SetBool("isFind", true);
        _isRoar = true;
    }
    // ========================== TEST CODE END

    // [ TEST CODE ] -> 발차기 애니메이션 구현
    private void KickAttack()
    {
        anim.SetBool("kickAttack", true);
        _iskickAttack = true;
    }

    // =============================== Test Code End

    private IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monState)
            {
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    // [ 4차 추가 수정 코드] -> Animator의 isTrace 조건이 false인 경우의 Animation을 수행
                    anim.SetBool("isTrace", false);
                    break;
                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    // [ 4차 추가 수정 코드] -> Animator의 isTrace조건이 true인 경우의 Animation을 수행
                    anim.SetBool("isTrace", true);
                    // [ 5차 추가 수정 코드 ]
                    anim.SetBool("isAttack", false);
                    break;
                case MonsterState.attack:
                    // [ 5차 추가 수정 코드 ]
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = true;
                    anim.SetBool("isAttack", true);
                    break;
                // ============================= 5차 수정 코드 END
                // [ TEST CODE ]
                case MonsterState.kickAttack:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = true;
                    anim.SetBool("kickAttack", true);
                    break;
                // [ TEST CODE ]                    
                case MonsterState.roar:
                    nvAgent.isStopped = true;
                    anim.SetBool("isFind", true);
                    break;
                // ====================================== Test code END
                case MonsterState.die:
                    break;
            }
            yield return null;
        }
    }

    // [ 6차 추가 수정 코드 ]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BULLET")
        {
            // [ 7차 추가 수정 코드 ] -> 충돌 위치에 BloodEffect를 구현
            CreateBloodEffect(collision.transform.position);
            // [ 6차 추가 수정 코드 ]
            Destroy(collision.gameObject);
            anim.SetTrigger("isHit");

            //[ 사망 구현 추가 코드 ] -> 
            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0)
            {
                MonsterDie();
            }
        }
    } // ========================================== 6차 추가 수정코드 END

    // [ 사망 구현 추가 코드 ] 
    private void MonsterDie()
    {
        // [ 플러스 알파 코드 ] -> 몬스터 사망시 UnTag 설정
        gameObject.tag = "Untagged";
        // =================================== 플러스 알파 코드 End

        StopAllCoroutines();
        isDie = true;
        monState = MonsterState.die;
        // [ 파이어 몬스터 코드 2 ] 추가시 변경 작성 if문으로 옮겨줌 (주석처리)
        if (!expChk)
        {
            nvAgent.isStopped = true;
            anim.SetTrigger("isDie");
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        // nvAgemt.isStopped = true;
        // anim.SetTrigger("isDie");

        // 캡슐콜라이더 비활성화 +++ 추가 코드
        gameObject.GetComponent<CapsuleCollider>().enabled = false; // 몬스터 몸통 캡슐콜라이더 비활성화
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        // ======================== +++ 추가 코드 END

        // [ 감마 코드 3 추가] -> 몬스터 사망시 기본 50포인트 획득
        gameUI.DispScore(50);
        // ========================= 감마 코드 END

        // [ TEST CODE ] -> 몬스터 이름에 따라 획득 포인트 차등화
        if (gameObject.name == "HellMonster")
        {
            // 지옥 괴물은 50포인트 추가 획득 (총 100포인트)
            gameUI.DispScore(50);
        }
        else if (gameObject.name == "CrawlerMonster")
        {
            // 갈고리 괴물은 150포인트 추가 획득 (총 200포인트)
            gameUI.DispScore(150);
        }
        // =============================== TEST CODE END

        // [ 풀 베타 코드 3 ]

        StartCoroutine(this.PushObjectPool());
        // ======================================== 풀 베타 코드 3 End
    }

    // [ 풀 베타 코드 4 ] -> 몬스터 오브젝트의 재사용을 위해 초기 상태 값으로 환원 후 비활성화
    private IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(3.0f);

        isDie = false;
        hp = 100;
        gameObject.tag = "MONSTER";
        monState = MonsterState.idle;
        // [ 파이어 몬스터 코드 3 ]
        if (expChk)
        {
            foreach (SkinnedMeshRenderer skin in skinMesh)
            {
                skin.material = normalMonster;
            }
            nvAgent.enabled = true;
            gameObject.GetComponent<Animator>().enabled = true;
            expChk = false;
        }
        // ================================== 파이어 몬스터 코드 END

        gameObject.GetComponent<CapsuleCollider>().enabled = true; // 몬스터 캡슐콜라이더 활성화
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }
        gameObject.SetActive(false); // 오브젝트 비활성화
    }
    // ======================================= 풀 베타 코드 End

    // ========================= 사망 구현 추가 코드 END

    // [ 7차 추가 수정코드 ]
    private void CreateBloodEffect(Vector3 position)
    {
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, position, Quaternion.identity);
        Destroy(blood1, 2.0f);

        // [ 8차 추가 수정 코드 ] -> 바닥의 Quad와 BloodDecal의 높이를 고려하여 위로 0.2정도 높이를 더해줌
        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.05f);
        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        GameObject blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);
        float scale = Random.Range(1.5f, 3.5f);
        // blood2.transform.localScale = new Vector3(scale, scale, 1); [ 별로 좋지 않은 코드]
        blood2.transform.localScale = Vector3.one * scale; // x, y, z 모두 1로 정규화 [ 권장 코드 ]
        Destroy(blood2, 5.0f);
        // ========================================= 8차 추가 수정 코드 END
    }
    // ======================= 7차 추가 수정 코드 END

    // [ +++ 알파 추가 코드 1번]
    private void OnPlyerDie()
    {
        StopAllCoroutines();
        //   nvAgent.isStopped = true; // 네비게이션 비활성화

        anim.SetTrigger("isPlayerDie");
    }
    // =========================== End

    // [ TEST CODE ] disable Monster's SphereCollider 
    //private void OffCollider()
    //{

    //    StopAllCoroutines();
    //    nvAgent.isStopped = true;
    //    foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
    //    {
    //        coll.enabled = false;

    //    }
    //}
    // ================================== test code end

    // [ 레이 알파 코드 ]
    private void OnDamage(object[] _params)
    {
        // 디버그는 확인 후 주석처리
        // Debug.Log(string.Format("Hit ray {0} : {1}", _params[0], _params[1]));

        // [ 레이 베타 코드 ] 
        CreateBloodEffect((Vector3)_params[0]); // hitPoint
        hp -= (int)_params[1];  // damage

        anim.SetTrigger("isHit");

        if (hp <= 0)
        {
            MonsterDie();
        }
        // =============================== 레이 베타 코드 End
    }

    // [ 파이어 몬스터 코드 2 ]
    private void OnExpDamage()
    {

        // [ TEST CODE ] -> Nav Mash 관련 에러 막기 위함
        StopAllCoroutines();
        // ======================== TEST CODE END

        // [ TEST CODE ] -> 몬스터 폭살시 시체에 걸려 플레이어 피가 줄어드는 현상
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        // === TEST CODE END===
        GameObject fire = (GameObject)Instantiate(fireEffect, firePivot.position, Quaternion.identity);
        fire.transform.parent = firePivot; // 파이어 피봇에서 불꽃 발생
        Destroy(fire, 7.9f);
        foreach (SkinnedMeshRenderer skin in skinMesh)
        {
            skin.material = fireMonster;
        }
        nvAgent.enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false; // 물리속성 영향을 받게 하기 위함
        gameObject.GetComponent<Animator>().enabled = false;
        expChk = true;

        StartCoroutine(MonsterExpDie());
    }

    private IEnumerator MonsterExpDie()
    {
        // 10초 후 몬스터 사망
        yield return new WaitForSeconds(10.0f);
        MonsterDie();
    }

    // ================================================ 파이어 몬스터 코드 END

    private void Update()
    {
        // [ 2차 추가 수정 코드 ]
        // 좋은 코드는 아님 >> 매 60프레임마다 업데이트를 수행하므로 불필요한 낭비가 됨
        // nvAgemt.destination = playerTr.position;
    }


}

