using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Update: //@ 2023.10.02
// Update: //@ 2023.10.04
// Update: //@ 2023.10.05
// Update: //@ 2023.10.06

// NOTE: //# 괴물 제어 스크립트
//#          1) 총알에 맞으면 체력 감소
//#          2) 괴물 총알에 맞으면 애니매이션 구현 (데미지 입을 경우 / 죽을 경우)
//#          3) 괴물 사망시 점수 값 넘겨주기 (= 점수획득 구현)
//#          4) 총에 맞으면 괴물 소리 구현
//#          5) 피격시 블러드 Effect 구현 
//#          6) 플레이어 감지 구현
//#          7) NavMeshAgent 구현(AI)
//#          8) 괴물 사망시 손에 있는 콜라이더 비활성화

public class CreaturesController : MonoBehaviour
{
    public int creatureHp = 1000;
    // bool isCreatureAlive = true;    //$ 아래와 동일 코드
    public bool isCreatureAlive;    //^ 인스펙터에서 확인을 위해 퍼블릭으로 선언함 >> 인스팩터에서 체크박스에 체크하면 동일코드
    Animator creatureAnim;
    public AudioClip creatureHittedSound;   //^ 괴물 피격시 울부짖음 재생
    public AudioClip creatureDieSound;  //^ 괴물 사망시 사운드 재생
    public ParticleSystem bloodEffect;  //^ 괴물 피격시 혈흔 효과
    public Transform bloodFxPos;
    public GameObject playerObject; //^ 플레이어 감지 구현을 위한 변수    
    Transform playerPos;    //^ 플레이어 위치
    public float betweenDistance;   //^ 플레이어와 괴물 사이의 거리 값 계산을 위한 변수선언
    public NavMeshAgent creatureNavMeshAgent;   //& NavMeshAgent 구현을 위한 변수 선언
    //GameObject attackPos;

    public Collider[] childColliders;   //% 게임오브젝트의 부모부터 자식까지 모든 콜라이더를 담는 변수 선언

    /*
    public static CreaturesController instanceCreatures;
    private void Awake()
    {
        if (instanceCreatures == null)
        {
            instanceCreatures = this;
        }
        else
        {
            //Destroy(this);
        }
    }
    */


    private void Start()
    {
        creatureAnim = GetComponent<Animator>();
        // bloodEffect = GameObject.Find("bloodFx");
        playerObject = GameObject.Find("Player_Capsule");   //$ 게임오브젝트 자체를 찾아서 그 위치값 접근
        // NOTE: var
       //^ playerPos = GameObject.FindWithTag("Player").transform;     //$ 위치값 자체 접근
        creatureNavMeshAgent = GetComponent<NavMeshAgent>();
        //attackPos = GameObject.FindGameObjectWithTag("CreatureAttackPos");

        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();     //^ 위치값 접근 동일 코드
        //creatureNavMeshAgent.destination = playerPos.position;      //& 플레이어의 위치값으로 괴물의 도착값을 동일화함
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 20, Color.yellow);

        var betweenDistance = Vector3.Distance(playerPos.position, transform.position);
        // print(betweenDistance);

        if (isCreatureAlive)
        {
            betweenDistance = Vector3.Distance(playerPos.position, transform.position);
            if (betweenDistance <= 3.0f && PlayerState.instance.isPlayerAlive)
            {
                //% 공격
                creatureAnim.SetTrigger("Attack");
            }
            // if (betweenDistance < 9.0f)
            // {
            //     //% 울부짖음
            //     creatureAnim.SetInteger("State", 2);
            // }
            else if (betweenDistance < 7.0f && PlayerState.instance.isPlayerAlive)
            {

                //% 울부짖음
                // creatureAnim.SetInteger("State", 1);

                //% 추격
                creatureAnim.SetInteger("State", 1);
                //creatureNavMeshAgent.SetDestination(playerPos.position);
                //^ 위와 동일한 효과를 구현함 .. 다만 아래 코드는 게임오브젝트 자체의 위치값을 찾아서.
                //^ creatureNavMeshAgent.SetDestination(playerObject.transform.position);   //^ 아래와 동일 코드
                creatureNavMeshAgent.SetDestination(playerPos.position);

            }
            else
            {   //% 아이들
                creatureAnim.SetInteger("State", 0);
            }
        }
        // NOTE: //? NavMesh 없이 인공지능 구현하는 방법 중 하나 보간법. 플레이어 쪽으로 방향 자연스럽게 바꾸는 코드
        Vector3 dirPos = playerPos.position - transform.position;
        // Debug.Log(dirPos);
        // NOTE: //# Lerp : 보간법, 시작과 끝을 계산하여 자연스럽게 .. // LookRotation : 플레이어 위치와 그 차이 만큼 방향을 회전시켜달라는 함수
        /* dirPos.y = 0.0f;
        dirPos.Normalize();     //^ 정규화 함수
        
         transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirPos), 60f * Time.deltaTime);   
         */

    }


    void Damaged(int creatureDam)   //# BulletScript에서 주입된 값을 받음
    {
        creatureHp -= creatureDam;
        Debug.Log("Damaged Creature Insect -> Hp : " + creatureHp);
        if (creatureHp <= 0)
        {
            creatureHp = 0;
            isCreatureAlive = false;
            AudioSource.PlayClipAtPoint(creatureDieSound, transform.position);  //$ 괴물 사망시 사운드 재생 구현
            creatureAnim.SetTrigger("Die");
            GameManagerSrc.instance.SendMessage("addScore", 200);   //% 괴물 처치시 점수 주입

            this.gameObject.GetComponentInChildren<SphereCollider>().enabled = false;

            childColliders = gameObject.GetComponentsInChildren<CapsuleCollider>();     //# 자식의 모든 캡슐콜라이더를 찾아 childCollider 변수에 넣음
            foreach (Collider coll in childColliders)       //# childColliders에 담긴 것들 중 Collider타입(=coll)을 찾아 enable = false로 만듬 (=비활성화)
            {
                coll.enabled = false;
            }

            // if(gameObject.name.Contains("Insect"))
            // {
            //     attackPos.SetActive(false);
            // }
            // if (gameObject.name.Contains("Creature_DevilTeeth"))
            // {
            //     attackPos.SetActive(false);
            // }
        }

        else
        {
            creatureAnim.SetTrigger("Hit");
            // bloodEffect.GetComponent<ParticleSystem>().Play();      //& 괴물 피격시 혈흔효과 구현
            AudioSource.PlayClipAtPoint(creatureHittedSound, transform.position);   //& 괴물 피격시 울부짖음 재생 구현
            ParticleSystem bloodFx = Instantiate(bloodEffect, bloodFxPos.transform.position, transform.rotation);
            Destroy(bloodFx, 1.5f);
            // SoundManagerSrc.instanceSounds.CreautreHittedSound(gameObject.transform);
            //! 위 코드는 싱글톤으로 사운드 매니저를 만들어 게임오브젝트의 위치에서 사운드 재생하려고 시도했으나 실패

        }

    }

}
