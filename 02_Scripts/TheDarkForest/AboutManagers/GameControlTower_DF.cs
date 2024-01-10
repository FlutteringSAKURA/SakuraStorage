using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.18 
// Update: //@ 2024.01.04 
//# NOTE: The Dark Forest Scene의 전반을 관리하기 위한 컨트롤 타워 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class GameControlTower_DF : MonoBehaviour
{
    [Header("[ REALATED REFERENCE ABOUT GAMEOBJECT INFO ]")]
    [TextArea(2, 4)]
    public string _descripttion_About_Object_References;
    [SerializeField]
    [Tooltip("어둠의 숲 스타트 포인트 좌표 참조")]
    private Transform _startingPoint;

    [SerializeField]
    [Tooltip("Back Ground Decoration Model = House")]
    private GameObject _sweetHomeSweet_BG_DecoModel;


    [Header("[ REALATED REFERENCE ABOUT CINEMA MACHINE VIRTUAL CAMERA INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_CinemaMachin_Virtual_Cameara_References;

    [Header("Related Cinema Machine : Virtual Cameras")]
    // public GameObject _mainCamera;
    // public GameObject _virtualCamera01;
    // public GameObject _virtualCamera02;
    // public GameObject _virtualCamera07;


    [Header("Related Cinema Machine : Timeline")]
    // public GameObject _timeLine;
    // public GameObject _storyBook_Timeline;
    // public GameObject _timeLine_TheDoorDarkness;

    // public GameObject _timeline_LookBook;
    // public GameObject _timeLine_SweetHomeSweetGoOut;


    [Header("Related UI References")]
    // public GameObject _close_Mark_Canvas;



    [Header("Related Event's Sensor References")]
    [Tooltip("게임의 퀘스트 수행을 위한 퀘스트 센서 그룹 참조.. 초기 설정은 <비활성화>.. begin 시네마틱 재생 후 활성화 됨")]
    // public GameObject _questSensorGroup;


    [Header("Related Bool Flag References")]
    // public bool _timelineActiveFlag = true;

    [Header("[ REALATED REFERENCE ABOUT MAIN BGM INFO ]")]
    [TextArea(2, 3)]
    public string _descripttion_About_MainBGM_References;
    [Header("Related Game Back Ground Musics")]
    public GameObject _mainBgm;

    [Header("[ REALATED REFERENCE PATJI(PLAYER) INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_PatJi_References;
    [Header("Related PatJi")]
    public GameObject _patJi;
    public GameObject _patJiCam;


    [Header("Related Dialogue Texts")]
    // public GameObject _dialogueText;
    // public GameObject _dialogueText01_StoryBook;
    // public GameObject _dialogueText02_StoryVoiceContinue;
    // public GameObject _dialogueText06_LookBookDevilVoice;
    // public GameObject _dialogueText08_GoOutSweetHome;


    //  [Header("Related Interaction Object canvs")]
    //  [Tooltip("게임시작시 기본적으로 보여지는 인터렉션 표시 마크.. 초기 설정은 <비활성화>.. begin 시네마틱 재생 후 활성화 됨")]
    // public GameObject[] _interaction_Obj_Group;

    // [Header("Related Lightings")]
    // public GameObject _candle;


    // [Header("Related Reference Info")]
    // [SerializeField]
    // CandleController _candleController;

    // [SerializeField]
    // [Tooltip("팥쥐 연기 캐릭터의 좌표 초기화를 위한 참조")]
    // GameObject _patJi_Actress;
    // [SerializeField]
    // [Tooltip("팥쥐 연기 캐릭터의 좌표 초기화를 위한 참조")]
    // GameObject _patJi_Actress_StandbyOriginPos;
    [Header("[ REALATED REFERENCE ABOUT LOAD GAME SCENE INFO ]")]
    [TextArea(2, 3)]
    public string _descripttion_About_LoadGameScene_References;
    [Space(15f)]
    [Header("Related Game Scene Load Conditions")]
    [Tooltip("게임씬 로드를 위한 조건")]
    public bool _homeSweetHomeClear = false;
    [Tooltip("게임씬 로드가 될 때 일정시간 후 플레이어를 카메라가 바라보게 하기 위한 조건")]
    public bool _darkForestStartedFlag = false;

    [Header("[ REALATED REFERENCE ABOUT BOOL CONDITIONS INFO ]")]
    [TextArea(2, 3)]
    public string _descripttion_About_Bool_Conditions;
    [Space(15f)]
    [Header("Related Game Scene Load Conditions")]
    [Tooltip("마우스 커서 활성/비활성화를 위한 조건")]
    public bool _mouseCursorVisible = false;

    public static GameControlTower_DF instance;


    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ -------------------------------------------------------------------------------
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // _patJi.SetActive(false);

        StartCoroutine(InitializeCameraDampValue());
        _patJiCam.GetComponent<TheDarkForestCamera>().enabled = false;
    }

    //~ -------------------------------------------------------------------------------
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !_mouseCursorVisible)
        {
            _mouseCursorVisible = !_mouseCursorVisible;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _mouseCursorVisible)
        {
            _mouseCursorVisible = !_mouseCursorVisible;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


    }

    //~ -------------------------------------------------------------------------------

    IEnumerator InitializeCameraDampValue()
    {
        yield return new WaitForSeconds(3.0f);
        TheDarkForestMainBGM_Play();
        yield return new WaitForSeconds(2.0f);
        _patJiCam.GetComponent<TheDarkForestCamera>().enabled = true;

        yield return new WaitForSeconds(7.0f);
        _darkForestStartedFlag = true;
        _sweetHomeSweet_BG_DecoModel.SetActive(true);


        EventSensorController_DF.instance._eventActiveCheckFlag = false;

        _patJi.SetActive(true);
        _patJiCam.GetComponent<TheDarkForestCamera>()._dampingValue = 1.0f;
    }

    // Update: //@ 2023.11.30 
    public void TheDarkForestMainBGM_Play()
    {
        _mainBgm.SetActive(true);
    }


}
