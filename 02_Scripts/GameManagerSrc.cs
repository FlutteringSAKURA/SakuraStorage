//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

// Update: //@ 2023.10.02
// Update: //@ 2023.10.05
// Update: //@ 2023.10.06
// Update: //@ 2023.10.09

// NOTE: //# Game Manager Script
//#          1) 괴물을 처치하면 점수 획득 + (UI)적용
//#          2) 괴물 스폰
//#          3) 플레이어 상태바
//#          4) 플레이어 pause기능
//#          5) 게임스타트 화면 구현
//?          6) 게임 정지일 때 마우스 커서 작동 제대로 되게 구현하기 
//?            >> 게임 시작 후 인게임에서 마우스 락 + 사라지게 구현하기 // 정지일 때는 다시 마우스 커서 나오게 구현
//?          7) 게임오버 구현((UI) 후 마무리..

public class GameManagerSrc : MonoBehaviour
{
    public int initScore = 0;
    public Text scoreText;
    public static GameManagerSrc instance;
    public GameObject creatures;       //& 괴물 프리팹
    GameObject creatureAttackPosCollider;
    GameObject playerObj;

    public int creaturesType = 0;       //& 괴물 종류 초기값
    public int maxNumCreatures = 10;        //^ 괴물 스폰 최대치
    public int minNumCreatures = 0;         //^ 괴물 스폰 최소치

    //public AudioClip gameEndSound;
    //AudioSource audioBox;

    public Slider playerHpbar;      //$ 플레이어의 체력바
    public bool isGamePause = false;
    public GameObject gamePanel;    //$ 게임패널
    public GameObject startPanel;   //$ 게임스타트 패널
    public Image viewFinderTargetMark;
    public GameObject targetMark;
    public GameObject gameCanvas;
    public GameObject pausePanel;   //$ 일시정지 구현을 위한 패널    
    public bool isGameStarted = false;    //$ 게임스타트를 구현하기 위한 불 변수
    //public bool m_cursorIsLocked = true;
    //public bool lockCursor = true;

    public int creatureHp = 1000;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenCreatures());
        playerObj = GameObject.FindWithTag("Player");
        gamePanel.SetActive(false);

        playerObj.GetComponent<Rigidbody>().isKinematic = true;     //^ 게임 시작 전까지 이동 불가 상태 만들기 위함
        // m_cursorIsLocked = false;
        Cursor.visible = true;
        //Time.timeScale = 0.0f;

        //audioBox = GetComponent<AudioSource>();
        //UnityStandardAssets.Characters.FirstPerson.mouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.mouseLook();
        MouseLook mouseLockCtrl = new MouseLook();
        
        ////mouseLockCtrl.m_cursorIsLocked = false;
        

    }

    IEnumerator GenCreatures()
    {
        if (minNumCreatures < maxNumCreatures && isGameStarted)       //# 최대치가 될 때까지 조건 OK
        {
            GameObject creatureInsect = Instantiate(creatures) as GameObject;
            float posX = Random.Range(43, 81.5f);
            float posZ = Random.Range(49, 63.5f);
            creatureInsect.transform.position = new Vector3(posX, 0, posZ);     //& new .. 생성자 키워드
            minNumCreatures++;      //% 괴물 순차적으로 하나 씩 증가
        }
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(GenCreatures());     //^ 반복
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted && PlayerState.instance.isPlayerAlive)
        {
            playerObj.GetComponent<Rigidbody>().isKinematic = false;
        }
        // if(Input.GetMouseButtonDown(0) && !isGamePause)
        // {
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isGameStarted)
            {
                UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
                mouseLockCtrl.lockCursor = false;
                mouseLockCtrl.m_cursorIsLocked = false;
                Cursor.lockState = CursorLockMode.None;
                Debug.Log("마우스값 확인");
            }
            // else if(Input.GetMouseButtonUp(0))
            // {
            //     UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
            //     mouseLockCtrl.lockCursor = true;
            // }

        }
        if (Input.GetMouseButtonDown(0) && isGameStarted && !isGamePause)
        {
            // UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
            // mouseLockCtrl.lockCursor = true;
            // mouseLockCtrl.m_cursorIsLocked = true;

            // mouseLockCtrl.SetCursorLock(mouseLockCtrl.lockCursor);
              Cursor.lockState = CursorLockMode.Locked;
              Cursor.visible = false;
              
                Debug.Log("인게임 마우스 사라지기 테스트");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isGameStarted)
        {
            isGamePause = true;
            if (isGamePause)
            {
                Time.timeScale = 0.0f;
                gamePanel.SetActive(false);  //^ 게임패널 비활성화
                pausePanel.SetActive(true);    //^ 정지패널 활성화
                
                // UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
                // mouseLockCtrl.lockCursor = true;
            }

            else if (Input.GetMouseButtonUp(0))
            {
                // UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
                // mouseLockCtrl.lockCursor = true;
                // Cursor.lockState = CursorLockMode.Locked;
                // Cursor.visible = false;
                Debug.Log("인게임 마우스 사라지기 테스트22");
            }

            else
            {
                isGamePause = false;
                Time.timeScale = 1.0f;
                gamePanel.SetActive(true);  //^ 게임패널 활성화
                pausePanel.SetActive(false);    //^ 정지패널 비활성화
                //Cursor.visible = true;
                // m_cursorIsLocked = false;

                // UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLockCtrl = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
                // mouseLockCtrl.lockCursor = false;
                // Cursor.lockState = CursorLockMode.Locked;
                // Cursor.visible = false;

                Debug.Log("인게임 마우스 사라지기 테스트22333");

            }
        }

        // if (Input.GetKeyUp(KeyCode.Escape))
        // {
        //     m_cursorIsLocked = false;
        // }
        // else if (Input.GetMouseButtonUp(0) && !isGamePause)
        // {
        //     m_cursorIsLocked = true;
        // }

        // if (m_cursorIsLocked)
        // {
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Cursor.visible = false;
        //     Debug.Log("mouse LOCK!!");
        // }
        // else if (!m_cursorIsLocked)
        // {
        //     Cursor.lockState = CursorLockMode.None;
        //     Cursor.visible = true;
        // }
    }

    void addScore(int getPoint)     //% 괴물 처치시 점수 획득 함수
    {
        initScore = initScore + getPoint;
        scoreText.text = initScore.ToString() + "\n POINT";
        Debug.Log("socre : " + getPoint);
    }

    public void PlayerDie()
    {
        // creatureAttackPosCollider = GameObject.FindWithTag("CreatureAttackPos");
        // creatureAttackPosCollider.GetComponentInChildren<SphereCollider>().enabled = false;
        //creatures.GetComponentInChildren<SphereCollider>().enabled = false;
        
        PlayerState.instance.isPlayerAlive = false;

        playerObj.GetComponent<Rigidbody>().isKinematic = true;
        
        playerObj.GetComponent<CapsuleCollider>().enabled = false;

        //creatures.GetComponent<CapsuleCollider>().enabled = false;
        // creatureAttackPosCollider.GetComponent<SphereCollider>().enabled = false;
        // creatureAttackPosCollider.GetComponent<CapsuleCollider>().enabled = false;
        Debug.Log("Player Dead!!");

        //GameEndSoundPlay();
    }


    // TEMP: //! 사용안함
    // // public void GameEndSoundPlay()
    // // {
    // //    //audioBox.PlayOneShot(gameEndSound);
    // //     Debug.Log("GameEndSoundPlay");
    // // }

    void PauseActivation()
    {
        isGamePause = false;
        Time.timeScale = 1.0f;  //$ 유니티 내 시간흐름의 재생속도를 1.0f로 
        Debug.Log("Pause버튼 작동");
    }
    void GameStart()
    {
        isGameStarted = true;
        gameCanvas.SetActive(true);
        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        Time.timeScale = 1.0f;
        Debug.Log("GAME START!!!!");
    }

}

