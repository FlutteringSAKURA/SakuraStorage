using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkCtrl : MonsterCtrl
{
    public enum SharkState { idle, trace, biteAttack, tailAttack, isPlayerDie, die };
    public SharkState sharkStat = SharkState.idle;

    private Transform sharkTr;
    private Transform playerTrm;
    private NavMeshAgent nvAgentShark;

    private SkinnedMeshRenderer[] skinMeshShark;

    private Animator animShark;

    private GameUI gameUIShark;

    private bool _isDie = false;
    public bool _tailAttack = false;
    public bool _biteAttack = false;
    public bool _isPlayerDie = false;
   public bool _swingTail = false;



    // public bool _biteAttackChk = false;

    private void Awake()
    {
        sharkTr = gameObject.GetComponent<Transform>();
        playerTrm = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgentShark = GetComponent<NavMeshAgent>();

        animShark = GetComponent<Animator>();

        gameUIShark = GameObject.Find("GameUI").GetComponent<GameUI>();
        skinMeshShark = GetComponentsInChildren<SkinnedMeshRenderer>();
    }


    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }


    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    private void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgentShark.isStopped = true;

        animShark.SetTrigger("isPlayerDie");
    }
    

    private IEnumerator CheckMonsterState()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while (!_isDie)
        {
            yield return waitTime;
            float dist = Vector3.Distance(playerTrm.position, sharkTr.position);

            if (dist <= attackDist)
            {

                sharkStat = SharkState.biteAttack;
                _biteAttack = true;
                _tailAttack = false;
           
         
                //            _swingTail = true;
                //   _swingTail = true;
            }
             
            else if (_biteAttack && !_tailAttack)
            {
                _biteAttack = false;
             
                TailAttack();

            }
            else if (dist <= traceDist)
            {
                sharkStat = SharkState.trace;
                _biteAttack = false;
                _tailAttack = false;
               _swingTail = false;


            }
            else
            {
                sharkStat = SharkState.idle;
                _biteAttack = false;
                _tailAttack = false;
           //     _swingTail = false;
            }            
        }
    }
    
    private void TailAttack()
    {
       
        _tailAttack = true;
        animShark.SetBool("isTailAttack", true);      
   //     _swingTail = true;
    }

    private IEnumerator MonsterAction()
    {
        while (!_isDie)
        {
            switch (sharkStat)
            {
                case SharkState.idle:
                    nvAgentShark.isStopped = true;
                    animShark.SetBool("isTrace", false);
                    //     _biteAttackChk = false;
                    //_tailAttack = false;
                    //_biteAttack = false;

                    break;
                case SharkState.trace:
                    nvAgentShark.destination = playerTrm.position;
                    nvAgentShark.isStopped = false;
                    animShark.SetBool("isTrace", true);
                    animShark.SetBool("isBiteAttack", false);
                    _biteAttack = false;
                    _tailAttack = false;
                    break;
                case SharkState.biteAttack:
                    nvAgentShark.destination = playerTrm.position;
                    nvAgentShark.isStopped = true;
                    //_biteAttack = true;
                  
                    animShark.SetBool("isBiteAttack", true);
                    break;
                case SharkState.tailAttack:
                    nvAgentShark.destination = playerTrm.position;
                    nvAgentShark.isStopped = true;
                    animShark.SetBool("isTailAttack", true);
                    //_biteAttack = false;
                    _tailAttack = true;
                  //  _swingTail = true;
                    break;
                case SharkState.isPlayerDie:
                    nvAgentShark.isStopped = true;
                    animShark.SetBool("isPlayerDie", true);
                    //_biteAttack = false;
                    //_tailAttack = false;
                    break;
                case SharkState.die:
                    //_biteAttack = false;
                    //_tailAttack = false;
                    break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BULLET")
        {
            CreateBloodEffect(collision.transform.position);
            Destroy(collision.gameObject);
            // animShark.SetTrigger("isHit");

            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if (hp <= 0)
            {
                MonsterDie();
            }
        }

    }
    private void MonsterDie()
    {
        if (!expChk)
        {
            StopAllCoroutines();
            _isDie = true;
            sharkStat = SharkState.die;

            nvAgentShark.isStopped = true;
            animShark.SetTrigger("isDie");
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }

        gameUIShark.DispScore(200);

        //StartCoroutine(PushObjectPool());
    }

    //private IEnumerator PushObjectPool()
    //{
    //    yield return new WaitForSeconds(3.0f);
    //    _isDie = false;
    //    hp = 700;
    //    gameObject.tag = "MONSTER";
    //    sharkStat = SharkState.idle;
    //    if (expChk)
    //    {
    //        foreach (SkinnedMeshRenderer skin in skinMeshShark)
    //        {
    //            skin.material = normalMonster;
    //        }
    //        nvAgentShark.enabled = true;
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

    private void CreateBloodEffect(Vector3 position)
    {
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, position, Quaternion.identity);
        Destroy(blood1, 2.0f);

        Vector3 decalPos = sharkTr.position + (Vector3.up * 0.09f);
        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        GameObject blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);
        float scale = Random.Range(3.5f, 6.5f);

        blood2.transform.localScale = Vector3.one * scale;
        Destroy(blood2, 7.0f);
    }
    private void OnplayerDie()
    {
        StopAllCoroutines();
        nvAgentShark.isStopped = true;

        animShark.SetTrigger("isPlayerDie");
    }

    private void OnDamage(object[] _params)
    {
        // Debug.Log(string.Format("Hit ray {0} : {1}", _params[0], _params[1]));
        CreateBloodEffect((Vector3)_params[0]); // hitPoint
        hp -= (int)_params[1]; // damage

        //   animShark.SetTrigger("isHit");

        if (hp <= 0)
        {
            MonsterDie();
        }
    }

    private void OnExpDamage()
    {
        StopAllCoroutines();
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }

        GameObject fire = Instantiate(fireEffect, firePivot.position, Quaternion.identity);
        fire.transform.parent = firePivot;
        Destroy(fire, 7.9f);
        foreach (SkinnedMeshRenderer skin in skinMeshShark)
        {
            skin.material = fireMonster;
        }
        nvAgentShark.enabled = false;

        // gameObject.GetComponent<Rigidbody>().isKinematic = false; // 폭탄에 맞으면 물리효과를 받게 해줌
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        gameObject.GetComponent<Animator>().enabled = false;
        expChk = true;

        StartCoroutine(MonsterExpDie());
    }

    private IEnumerator MonsterExpDie()
    {
        yield return new WaitForSeconds(10.0f);
        MonsterDie();
    }
}
