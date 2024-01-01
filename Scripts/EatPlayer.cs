using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EatPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject _canvasBoxes;
    [SerializeField]
    Text _escapeText;

   // private Transform IntheSarkTr;
    public GameObject IntheSharkCam;
    public bool OffCam = true;
    public bool OnCam = false;
    SharkCtrl sharkCtrlScript;
    public GameObject sharkObject;

    private GameObject _playerObject;
    // private NavMeshAgent navShark;

    //public float eatTime = 0.0f;
    //public float escapeTime = 5.0f;
    public bool escapeShark;
    public bool TailAttackChk = false;

    // 탈출 게이지바 이미지
    public Image escapeBarImg;
      
    // 탈출 조건을 위한 설정 값
    int keydownNum = 0;
    float escapeNum = 10;


    private void Start()
    {
       // IntheSarkTr = GetComponent<Transform>();
        _playerObject = GameObject.FindGameObjectWithTag("Player");
   //     navShark = GetComponent<NavMeshAgent>();
        sharkCtrlScript = GetComponent<SharkCtrl>();

        // 탈출 게이지 바를 0으로 초기화
        escapeBarImg.fillAmount = 0;     
    }

    private void Update()
    {
        //eatTime += Time.deltaTime; // 정규화

        //[ 인트값인 fillAmpount값을 Float형으로 사용하기 위한 방법 ]
        // => (float)탈출숫자에서 (float)스페이스바 누른횟수 => (float) 값으로 받는다
        // => 즉, 1/10 => 0.1 Float값이 되는 것이다.
        float escapeBar = (float)keydownNum / (float)escapeNum; 
        escapeBarImg.fillAmount = escapeBar;

        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스버튼을 누르면
        {
            keydownNum++; // 버튼다운 숫자가 1씩 증가
            if (keydownNum >= escapeNum) // 버튼 다운 숫자가 탈출숫자와 같거나 커지면
            {
                escapeShark = true; 
                EscapeShark(); // 탈출시켜라
            }
        }
    }

    private void EscapeShark()
    {
        OffCam = true;
        OnCam = false;
        _playerObject.SetActive(true); // 플레이어 활성화
        IntheSharkCam.SetActive(false); // 뱃속 캠 비활성화
        _canvasBoxes.SetActive(true); // 메인 UI 활성화
        _escapeText.enabled = false; // 탈출 메세지 비활성화

        //  sharkCtrlScript.GetComponent<SharkCtrl>().enabled = true;
        // sharkObject.GetComponent<NavMeshAgent>().isStopped = false;
        // sharkObject.GetComponentInChildren<SphereCollider>().isTrigger = true;
        //sharkCtrlScript.sharkStat = SharkCtrl.SharkState.idle;
        escapeBarImg.enabled = false;

       // navShark.isStopped = false;
        StartCoroutine(VomitPlayer());
    }

    private void OnTriggerEnter(Collider coll)
    {              
        keydownNum = 0; // 다운버튼 횟수 초기화

        if (coll.gameObject.tag == "Player" && _playerObject.GetComponent<PlayerCtrl>().hp <= 30.0f
            && !sharkCtrlScript._swingTail)
        {
            OffCam = false;
            OnCam = true;
            _playerObject.SetActive(false); // 플레이어 비활성화
            IntheSharkCam.SetActive(true); // 뱃속 캠 활성화
            _canvasBoxes.SetActive(false); // 메인 UI 비활성화
            _escapeText.enabled = true; // 탈출 메세지 활성화            
            escapeBarImg.enabled = true; // 탈출 게이지바 UI 활성화

            // sharkCtrlScript.GetComponent<SharkCtrl>().enabled = false;
            //     sharkObject.GetComponent<NavMeshAgent>().enabled = false;
   //         IntheSharkCam.transform.position = IntheSarkTr.position;
            //sharkObject.GetComponent<Animator>().SetBool("isBiteAttack", false);
            // sharkObject.GetComponent<Animator>().SetBool("isTailAttack", false);         
         //   sharkObject.GetComponent<SharkCtrl>().monState = MonsterCtrl.MonsterState.idle;

            //_playerObject.GetComponent<CapsuleCollider>().isTrigger = false;

            sharkObject.GetComponentInChildren<SphereCollider>().isTrigger = false;
            _playerObject.layer = LayerMask.NameToLayer("BODY");
            // sharkCtrlScript.sharkStat = SharkCtrl.SharkState.idle;
       //     _playerObject.GetComponent<PlayerCtrl>()._isJump = false;

          //  navShark.speed = 2.0f;
        }                 
          //  StartCoroutine(VomitPlayer());        
    }


    private IEnumerator VomitPlayer()
    {
        yield return new WaitForSeconds(0.8f);
      //  Debug.Log("vomit");
      //  OffCam = true;
      //  OnCam = false;
      //  IntheSharkCam.SetActive(false);


     //   _playerObject.SetActive(true);
       // IntheSharkCam.GetComponent<Camera>().enabled = true;
       
        // _playerObject.GetComponent<CapsuleCollider>().isTrigger = true;
        sharkObject.GetComponentInChildren<SphereCollider>().isTrigger = true;
        _playerObject.layer = LayerMask.NameToLayer("PLAYER");
        // sharkCtrlScript.GetComponent<SharkCtrl>().enabled = true;
        // sharkObject.GetComponent<NavMeshAgent>().enabled = true;

      //  navShark.speed = 5.0f;
    }
}
