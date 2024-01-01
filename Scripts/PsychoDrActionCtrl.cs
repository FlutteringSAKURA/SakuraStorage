using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 테스트 코드임 


public class PsychoDrActionCtrl : MonoBehaviour
{
    public enum PsychoDrState { idle, moveA ,moveB, gunShot, isPlayerDie, die };
    public PsychoDrState psychoStat = PsychoDrState.idle;

    private Transform psychoTr; 
    private Transform PlayerTr;
    private NavMeshAgent navAgentPsycho;

     // 순찰 구역 A and B
     [SerializeField]
    Transform ChkDistrictA;  
    [SerializeField]
    Transform ChkDistrictB;

   //  private SkinnedMeshRenderer[] skinMeshPsycho;
    private Animator animPsycho;
  //  private GameUI gameUIPsycho;

    public PsychoDrCtrl _psychoDrCtrlScript;
    public float chkDist = 2.0f;

    public bool _isDie = false;
    public bool _gunShot = false;
    public bool _isPlayerDie = false;
    public bool _move = false;

    public bool _rightChk = false;
    public bool _leftChk = false;
    public float moveSpeed = 0.0f;
    int attackRange = 5;

    private void Awake()
    {
        psychoTr = GetComponent<Transform>(); 
        PlayerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navAgentPsycho = GetComponent<NavMeshAgent>();
        animPsycho = GetComponent<Animator>();
        //    gameUIPsycho = GameObject.Find("GameUI").GetComponent<GameUI>();
        //   skinMeshPsycho = GetComponentsInChildren<SkinnedMeshRenderer>();

        ChkDistrictA = GetComponent<Transform>();
        ChkDistrictB = GetComponent<Transform>();

    }
    private void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += OnPlayerDie;
       // StartCoroutine(ChkMonsterState());
        StartCoroutine(MonsterAction());
    }

    private void Update()
    {
        moveSpeed += Time.deltaTime; // 정규화 

        float distB = Vector3.Distance(ChkDistrictA.position, psychoTr.position);
        //float distA = Vector3.Distance(ChkDistrictB.position, psychoTr.position.normalized * moveSpeed);

        //Vector3 moveDir = (Vector3.right * moveSpeed);
        //psychoTr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

    //    if (distB <= chkDist && !_rightChk)
    //    {

            //  Vector3 forward = Vector3.Slerp(transform.forward, )
            //psychoTr.LookAt(transform.position + Vector3.forward );
            //Vector3 moveDir = (Vector3.forward * moveSpeed);
            //psychoTr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
            psychoTr.Rotate(Vector3.up * moveSpeed);
        
        for (int i = 1; i < attackRange; i++)
        {
         
            psychoTr.Rotate(Vector3.up * i);
          //  i++;
        }
           // psychoStat = PsychoDrState.moveB;

      //  }
        //else if (distA <= chkDist)
        //{
        //    _leftChk = true;
        //    psychoStat = PsychoDrState.moveA;
        //}
    }
    //private IEnumerator ChkMonsterState()
    //{
    //    WaitForSeconds waitTime = new WaitForSeconds(0.2f);
    //    while (!_isDie)
    //    {
    //        yield return waitTime;
    //        //float distB = Vector3.Distance(ChkDistrictA. position, psychoTr.position);
    //        //float distA = Vector3.Distance(ChkDistrictB.position, psychoTr.position);

    //        //if (distB <= chkDist && !_rightChk)
    //        //{
    //        //    _rightChk = true;

    //        //    psychoStat = PsychoDrState.moveB;

    //        //}

    //        //if (distA <= chkDist && _rightChk)
    //        //{
    //        //    _leftChk = true;
    //        //    psychoStat = PsychoDrState.moveA;
    //        //}
    //    }
    //}

    private IEnumerator MonsterAction()
    {
        while (!_isDie)
        {
            switch (psychoStat)
            {
                case PsychoDrState.idle:
                    break;

                case PsychoDrState.moveA: // A로 이동
                    navAgentPsycho.destination = ChkDistrictA.position;
                    animPsycho.SetBool("isMove", true);
                    break;

                case PsychoDrState.moveB: // B로 이동
                    navAgentPsycho.destination = ChkDistrictB.position;
                    animPsycho.SetBool("isMove", true);
                    break;

                case PsychoDrState.gunShot:
                    navAgentPsycho.isStopped = true;

                    break;
                case PsychoDrState.isPlayerDie:
                    break;
                case PsychoDrState.die:
                    break;
            
            }
            yield return null;
        }
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= OnPlayerDie;
    }
    private void OnPlayerDie()
    {
        StopAllCoroutines();
        navAgentPsycho.isStopped = true;

    }

}
