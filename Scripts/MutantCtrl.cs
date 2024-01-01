using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public class MutantAnim
//{
//    public AnimationClip idle;
//    public AnimationClip walk;
//    public AnimationClip run;
//    public AnimationClip shout;
//    public AnimationClip attack;
//    public AnimationClip gotHit;
//    public AnimationClip die;

//}

public class MutantCtrl : MonoBehaviour
{
    public enum MutantState { idle, traceW, traceR, attack, shout, die };

    public MutantState mutantState = MutantState.idle;

    private Transform monsterTr; // 몬스터 위치
    private Transform playerTr; // 플레이어 위치
    private NavMeshAgent nvAgent;
    //private Animation mutantAnim;
    private Animator anim;
    //public MutantAnim anim;

    // Ragdoll Setting Code
    public Rigidbody[] rbody;
    // =========================== end

    public float traceWalk = 8.0f;
    public float traceRun = 15.0f;
    public float attackDist = 2.0f;
    //public float shoutDist = 32f;
    public bool _isAttack = false;
   // public bool _isShout = false;
    public bool _isIdle = true;
    public bool _isWalk = false;
    public bool _isRun = false;

    

    public GameObject bloodEffect;
    public GameObject bloodDecal;
    public GameObject fireEffect;

    private bool isDie = false;
    private int hp = 1000;

    private GameUI gameUI;

    public Material normalMonster;
    public Material fireMonster;
    public Transform firePivot;
    private SkinnedMeshRenderer[] skinMesh;
    private bool expChk = false;

    private void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
        //mutantAnim = GetComponent<Animation>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        skinMesh = GetComponentsInChildren<SkinnedMeshRenderer>();

        anim = GetComponent<Animator>();
        //mutantAnim.clip = anim.idle;
        //mutantAnim.Play();

        StartCoroutine(this.CheckMutantState());
        StartCoroutine(this.MutantAction());
    }

    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    private IEnumerator CheckMutantState()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while (!isDie)
        {
            yield return waitTime;
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            //if(dist <= shoutDist)
            //{
            //    Shout();
            //}

            if (dist <= attackDist)
            {
                mutantState = MutantState.attack;
                _isAttack = true;
                _isIdle = false;
            }

            else if (dist <= traceRun && _isWalk)
            {

                _isRun = true;
                _isIdle = false;
                mutantState = MutantState.traceR;
            }
            else if (dist <= traceWalk && !_isRun)
            {              
                _isIdle = false;
                _isWalk = true;
                mutantState = MutantState.traceW;
            }
            //else if (dist < traceWalk)
            //{
            //    Shout();
            //}
          
            else
            {
                _isAttack = false;
                _isIdle = true;
                _isWalk = false;
                _isRun = false;
                mutantState = MutantState.idle;
            }
            
        }
    }
    //private void Shout()
    //{
    //    anim.SetBool("isShout", true);
               
    //}

    private IEnumerator MutantAction()
    {
        while (!isDie)
        {
            switch (mutantState)
            {
                case MutantState.idle:
                    nvAgent.isStopped = true;
                    // idle
                    anim.SetBool("TraceW", false);
                    
                   // mutantAnim.CrossFade(anim.idle.name, 0.1f);
                    break;
                case MutantState.traceW:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    nvAgent.speed = 3.5f;
                    anim.SetBool("TraceW", true);
                    // walk
                    //mutantAnim.CrossFade(anim.walk.name, 0.1f);                    
                    break;
                case MutantState.traceR:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    nvAgent.speed = 9.9f; 
                    
                    anim.SetBool("TraceR", true);
                  
                    anim.SetBool("isAttack", false);
                    // run
                   //mutantAnim.CrossFade(anim.run.name, 0.1f);
                    break;
                case MutantState.attack:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = true;
                    // attack
                    anim.SetBool("isAttack", true);
                    //mutantAnim.CrossFade(anim.attack.name, 0.1f);
                    break;
                case MutantState.shout:
                    nvAgent.isStopped = true;
                    anim.SetBool("isShout", true);
                    break;
                case MutantState.die:
                    // setRagdoll
                    break;                                  
            }
            yield return null;
        }
    }

    // Rag Doll setting Code
    private void SetRagdoll(bool isEnable)
    {
        foreach (Rigidbody _rbody in rbody)
        {
            _rbody.isKinematic = !isEnable;
        }
    }
    // ====================== end



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BULLET")
        {
            CreateBloodEffect(collision.transform.position);
            Destroy(collision.gameObject);
            anim.SetTrigger("isHit");

            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0)
            {
                MutantDie();
                
            }
        }
    }

    private void MutantDie()
    {
        if (!expChk)
        {
            // gameObject.tag = "Untagged";
            StopAllCoroutines();
            isDie = true;
            mutantState = MutantState.die;
            nvAgent.isStopped = true;
            anim.SetTrigger("isDie");
            SetRagdoll(true);
            
            //setRagdoll
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        gameUI.DispScore(500);
        StartCoroutine(this.DeleteMutant());
        
    }

    private IEnumerator DeleteMutant()
    {
        yield return new WaitForSeconds(5.0f);
        SetRagdoll(false);
        foreach (Collider coll in gameObject.GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }
        Destroy(gameObject, 8.0f);
    }

    //private IEnumerator PushObjectPool()
    //{
    //    yield return new WaitForSeconds(3.0f);

    //    isDie = false;
    //    hp = 100;
    //    gameObject.tag = "MONSTER";
    //    monState = MonsterState.idle;

    //    if (expChk)
    //    {
    //        foreach (SkinnedMeshRenderer skin in skinMesh)
    //        {
    //            skin.material = normalMonster;
    //        }
    //        nvAgent.enabled = true;
    //        gameObject.GetComponent<Animator>().enabled = true;
    //        expChk = false;
    //    }

    //    gameObject.GetComponent<CapsuleCollider>().enabled = true;
    //    foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
    //    {
    //        coll.enabled = true;
    //    }
    //    gameObject.SetActive(false);
    //}

    private void CreateBloodEffect(Vector3 pos)
    {
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(blood1, 2.0f);
        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.02f);
        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        GameObject blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);
        float scale = Random.Range(1.5f, 3.5f);
        blood2.transform.localScale = Vector3.one * scale;
        Destroy(blood2, 5.0f);
    }

    private void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgent.isStopped = true;
        anim.SetTrigger("isPlayerDie");
        // Monster win
    }

    private void OnDamage(object[] _params)
    {
        CreateBloodEffect((Vector3)_params[0]);
        hp -= (int)_params[1];
        if ( hp <= 0)
        {
            MutantDie();
        }
        // Monster hit
    }

    private void OnExpDamage()
    {
        // 뮤턴트 스피어 콜라이더 비활성화 -> Rag Doll의 Collider들과의 충돌로 인하여 부자연스러움 방지를 위해
        gameObject.GetComponent<CapsuleCollider>().enabled = false; 
              
        GameObject fire = (GameObject)Instantiate(fireEffect, firePivot.position,
            Quaternion.identity);
        fire.transform.parent = firePivot;
        Destroy(fire, 8.0f);
        foreach (SkinnedMeshRenderer skin in skinMesh)
        {
            skin.material = fireMonster;
        }
        nvAgent.enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Animator>().enabled = false;
        expChk = true;
        // gameObject.tag = "Untagged";
        StopAllCoroutines();
        isDie = true;
        mutantState = MutantState.die;
        StartCoroutine(MutantExpDie());
    }

    private IEnumerator MutantExpDie()
    {
        yield return new WaitForSeconds(10.0f);
        MutantDie();
    }
}
