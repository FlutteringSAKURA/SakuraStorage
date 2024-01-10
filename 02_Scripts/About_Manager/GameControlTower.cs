using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

// Update: //@ 2023.11.21 
// Update: //@ 2023.11.30 
// Update: //@ 2023.12.01 
// Update: //@ 2023.12.05 
// Update: //@ 2023.12.06 
// Update: //@ 2023.12.13 
//# NOTE: Game의 전반을 관리하기 위한 컨트롤 타워 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class GameControlTower : MonoBehaviour
{
    [Header("Related Game Start Menu UI")]
    public Image _mainImg;

    [Header("Related Cinema Machine : Virtual Cameras")]
    public GameObject _mainCamera;
    public GameObject _virtualCamera01;
    public GameObject _virtualCamera02;
    public GameObject _virtualCamera07;


    [Header("Related Cinema Machine : Timeline")]
    public GameObject _timeLine;
    public GameObject _storyBook_Timeline;
    public GameObject _timeLine_TheDoorDarkness;

    public GameObject _timeline_LookBook;
    public GameObject _timeLine_SweetHomeSweetGoOut;


    [Header("Related UI References")]
    public GameObject _close_Mark_Canvas;



    [Header("Related Event's Sensor References")]
    [Tooltip("게임의 퀘스트 수행을 위한 퀘스트 센서 그룹 참조.. 초기 설정은 <비활성화>.. begin 시네마틱 재생 후 활성화 됨")]
    public GameObject _questSensorGroup;


    [Header("[ REALATED REFERENCE ABOUT BOOL CONDITIONS INFO ]")]
    [TextArea(2, 3)]
    public string _descripttion_About_Bool_Conditions;

    [Space(15f)]
    [Tooltip("Related Game Scene Load Conditions..")]
    public bool _timelineActiveFlag = true;
    [Tooltip("타임라인 재생시 마우스 커서 활성화 상태를 위한 조건")]
    public bool _timelineEndButMouseCursorDisplayFlag = false;
    [Tooltip("GoOutSweetHomeSweet타임라인 재생시 마우스 커서 비활성화 상태를 위한 조건")]
    public bool _theDarkForestReady = false;

    [Space(15f)]
    [Tooltip("마우스 커서 활성/비활성화를 위한 조건")]
    public bool _mouseCursorVisible = false;


    [Header("Related Game Back Ground Musics")]
    public GameObject _mainBgm;


    [Header("Related PatJi")]
    public GameObject _patJi;
    public GameObject _patJiCam;


    [Header("Related Dialogue Texts")]
    public GameObject _dialogueText;
    public GameObject _dialogueText01_StoryBook;
    public GameObject _dialogueText02_StoryVoiceContinue;
    public GameObject _dialogueText06_LookBookDevilVoice;
    public GameObject _dialogueText08_GoOutSweetHome;



    [Header("Related Interaction Object canvs")]
    [Tooltip("게임시작시 기본적으로 보여지는 인터렉션 표시 마크.. 초기 설정은 <비활성화>.. begin 시네마틱 재생 후 활성화 됨")]
    public GameObject[] _interaction_Obj_Group;

    [Header("Related Lightings")]
    public GameObject _candle;


    [Header("Related Reference Info")]
    [SerializeField]
    CandleController _candleController;

    [SerializeField]
    [Tooltip("팥쥐 연기 캐릭터의 좌표 초기화를 위한 참조")]
    GameObject _patJi_Actress;
    [SerializeField]
    [Tooltip("팥쥐 연기 캐릭터의 좌표 초기화를 위한 참조")]
    GameObject _patJi_Actress_StandbyOriginPos;

    [Space(15f)]
    [Header("Related Game Scene Load Conditions")]
    [Tooltip("게임씬 로드를 위한 조건")]
    public bool _homeSweetHomeClear = false;

    public static GameControlTower instance;


    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ -------------------------------------------------------------------------------
    void Start()
    {
        _mainImg.enabled = false;

        _timelineActiveFlag = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //~ -------------------------------------------------------------------------------
    void Update()
    {

        //% 마우스 커서 보임
        if (Input.GetKeyDown(KeyCode.Escape) && !_mouseCursorVisible)
        {
            _mouseCursorVisible = !_mouseCursorVisible;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MouseCursorActive();

            Debug.Log(Cursor.visible);
        }

        //% 마우스 커서 안보임
        else if (Input.GetKeyDown(KeyCode.Escape) && _mouseCursorVisible)
        {
            _mouseCursorVisible = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            MouseCursorInActive();

            Debug.Log(Cursor.visible);
        }


    }

    //~ ———————————————————————————————————————

    public void MouseCursorActive()
    {
        _mouseCursorVisible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log(Cursor.visible);
    }

    public void MouseCursorInActive()
    {
        _mouseCursorVisible = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log(Cursor.visible);

    }



    //@ CinemaChine Virtual Camera_0의 Begin Story에서 Wake Up 효과 를 주기 위한 Signal 함수 
    public void WakeUpCinema_PostProcessing()
    {
        //% begin story 처음에는 비활성화 되어 어둠속에서 팥쥐가 악몽을 꾸며 자고 있는 것을 표현
        //% PostProcessingController 스크립트가 활성화 되면 서서히 노출값이 줄어들면서 팥쥐가 어둠속에서 보이기 시작
        _virtualCamera01.GetComponent<PostProcessingController>().enabled = true;
    }

    // Update: //@ 2023.11.30 
    public void LookAtKongjiBedCinema_PostProcessing()
    {
        //% 침대쪽과 팥쥐쪽의 포커싱 조절 효과
        _virtualCamera02.GetComponent<PostProcessingController>().enabled = true;
    }

    // Update: //@ 2023.12.01 
    public void Twinkling_PostProcessing()
    {
        //% 눈 깜빡거리는 듯한 느낌의 카메라 연출 (화면 잠시 어두워졌다가 원래대로)
        _virtualCamera07.GetComponent<PostProcessingController>().enabled = true;
    }


    // Update: //@ 2023.11.30 
    public void SweetHomeSweetMainBGM_Play()
    {
        _mainBgm.SetActive(true);
    }



    // Update: //@ 2023.12.01 
    public void TimelineDisable()
    {
        _timelineActiveFlag = false;
        _timeLine.SetActive(false);

        _mainCamera.SetActive(false);

        _dialogueText.SetActive(false);

        _candle.SetActive(true);

        _candle.GetComponentInChildren<Light>().range = 3.0f;
        _candle.GetComponentInChildren<Light>().intensity = 5.0f;

        // Update: //@ 2023.12.05 
        _questSensorGroup.SetActive(true);

        for (int i = 0; i < _interaction_Obj_Group.Length; i++)
        {
            //% begin 시네마틱 재생 후, 기본적으로 보여지는 인터렉션 표시 마크.. 활성화
            _interaction_Obj_Group[i].SetActive(true);
        }

        _patJiCam.SetActive(true);

        //% 팥쥐 물리효과 영향 활성화
        _patJi.GetComponent<Rigidbody>().isKinematic = false;

    }

    //@ 타임라인 비활성화 시그널 호출을 위한 함수 
    public void TimeLine_StoryBook_Disable()
    {
        _timelineActiveFlag = false;
        _timelineEndButMouseCursorDisplayFlag = true;


        EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
        {

            _object_Have_EventSensorContorller[i]._nowListenStoyBookCheckFlag = false;
            _object_Have_EventSensorContorller[i]._eventActiveCheckFlag = false;
            _object_Have_EventSensorContorller[i]._storyVoiceFinishCheckFlag = true;
        }

        StartCoroutine(DelayTime());


        _close_Mark_Canvas.SetActive(true);

        _dialogueText01_StoryBook.SetActive(false);
        
        MouseCursorActive();
        Debug.Log("Event Disable");
    }

    IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(0.3f);

        _mainCamera.SetActive(false);
        _storyBook_Timeline.SetActive(false);
    }

    public void TimelineDisable_StoryVoiceContinue()
    {
        _dialogueText02_StoryVoiceContinue.SetActive(false);
    }


    public void TimelineDisable_TheDoorDarkness()
    {
        _timelineActiveFlag = false;

        //% 초기화
        _patJi_Actress.transform.position = _patJi_Actress_StandbyOriginPos.transform.position;
        _patJi_Actress.transform.rotation = _patJi_Actress_StandbyOriginPos.transform.rotation;


        StartCoroutine(DelayTime_TheDoorDarkness());
    }

    IEnumerator DelayTime_TheDoorDarkness()
    {
        _patJi.SetActive(true);
        _candleController.SendMessage("AttachedLeftHand");
        _candleController._fireFlame.SetActive(true);
        _timeLine_TheDoorDarkness.SetActive(false);

        EventSensorController.instance.UnlockMarkEnable();


        yield return new WaitForSeconds(0.05f);
        _patJiCam.SetActive(true);
        _mainCamera.SetActive(false);


    }
}
