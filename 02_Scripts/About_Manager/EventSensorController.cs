using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Update: //@ 2023.12.03 
// Update: //@ 2023.12.04 
//# NOTE: SweetHomeSweet의 Event Sensor의 제어를 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class EventSensorController : MonoBehaviour
{
    [Header("[ RELATED REFERENCE INFO ]")]
    [TextArea(3, 5)]
    public string _description_About_References;

    [SerializeField]
    [Tooltip("이야기 책이 있는 곳을 표시하는 UI.. 초기셋팅은 활성화")]
    GameObject _stoyObjectMark;

    [SerializeField]
    [Tooltip("이야기 책을 비추는 카메라.. 초기셋팅은 비활성화")]
    GameObject _firstPersion_Camera01_About_StoryBook;

    [SerializeField]

    [Tooltip("팥쥐 따라다니는 카메라.. 초기셋팅은 비활성화")]
    GameObject _sweetHomeSweet_Camera;

    [SerializeField]
    [Tooltip("스토리북의 책장 넘기는 부분을 표시해주는 UI.. 초기셋팅은 비활성화")]
    GameObject _clickPoint_Mark_Canvas;

    [SerializeField]
    [Tooltip("스토리북 이벤트 마무리를 위해 클로즈 UI 표시.. 초기셋팅은 비활성화")]
    GameObject _close_Mark_Canvas;

    [SerializeField]
    [Tooltip("이벤트 센서 감지시 특정 키를 누르라고 표시되는 UI.. 초기셋팅은 비활성화")]
    GameObject _keyMark_Label_StoryBook;

    [SerializeField]
    [Tooltip("문이 잠겨 있음을 표시하는 UI.. 초기셋팅은 활성화")]
    GameObject _lock_Mark;

    [SerializeField]
    [Tooltip("문이 열렸음을 표시하는 UI.. 초기셋팅은 활성화")]
    GameObject _unLock_Mark;

    [SerializeField]
    [Tooltip("이벤트 센서 감지시 특정 텍스트를 표시하는 UI.. 초기셋팅은 비활성화")]
    GameObject _keyMark_Label_cinemaDoor;

    [SerializeField]
    [Tooltip("이벤트 센서 감지시 특정 키를 누르라고 표시하는 UI.. 초기셋팅은 활성화")]
    GameObject _pressKeyCodeE_Text_cinemaDoor;

    [SerializeField]
    [Tooltip("이벤트 센서 감지시 특정 조건을 찾으라고 표시하는 UI.. 초기셋팅은 비활성화")]
    GameObject _findKey_Text_cinemaDoor;

    [SerializeField]
    [Tooltip("스토리북 이벤트의 signal작동을 위한 메인 카메라 참조")]
    GameObject _mainCamera;

    [SerializeField]
    [Tooltip("이벤트 센서의 조건 동일성 유지를 위한 참조")]
    GameObject[] _eventSensor_Group;

    //~

    [Header("[ REALATED REFERENCE ABOUT CINEMA MACHINE TIMELINE INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_CinemaMachin_Timeline_References;
    [SerializeField]
    [Tooltip("스토리북 이벤트 타임라인 작동을 위한 참조 GameObject.. 초기셋팅은 비활성화")]
    GameObject _storyBook_Timeline;

    [SerializeField]
    [Tooltip("스토리북 보고 난 뒤 이벤트 타임라인 작동(스토리 목소리 추가)을 위한 참조 .. 초기셋팅은 비활성화")]
    GameObject _storyVoiceContinueTimeline;

    //~

    [Header("[ REALATED BOOL CONDITIONS INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Bool_Conditions;

    [Tooltip("특정 이벤트가 발생할 경우 플레이어의 이동 조작이 되지 않도록 하는 것이 필요할 때 사용하기 위한 조건")]
    public bool _eventActiveCheckFlag = false;

    [Tooltip("이야기 책을 듣기 위해서 활성화가 되어 있어야 하는 조건")]
    public bool _listenStoryBookReadyCheckFlag = false;

    [Tooltip("이야기 책을 듣고 있음을 알려주는 조건.. 이야기를 듣고 있어야 이야기 종료도 가능")]
    public bool _nowListenStoyBookCheckFlag = false;

    [Tooltip("스토리북 보고 난 뒤 이벤트 타임라인 작동(스토리 목소리 추가)을 위한 조건")]
    public bool _storyVoiceContinueReadyFlag = false;


    [Tooltip("문을 열고 어두운 복도를 살피는 시네마를 재생시키기 위해 활성화가 되어 있어야 하는 첫번째 조건.. 이벤트 센서 감지시 활성화")]
    public bool _cinemaDoorCheckFlag = false;

    [Tooltip("방안의 조명(불)을 끄기 위해 활성화가 되어 있어야 하는 조건")]
    public bool _turnOffLightFindKeyCheckFlag = false;

    [Tooltip("조건이 필요할 때 사용하기 위해 임시로 만들어 놓은 조건")]
    public bool _etcCheckFlag = false;

    [Tooltip("문을 열고 어두운 복도를 살피는 시네마를 재생시키기 위해 활성화가 되어 있어야 하는 조건")]
    public bool _getKeyCheckFlag = false;

    [Tooltip("문이 열렸고, 문을 열고 어두운 복도를 살피는 시네마가 재생 될 경우 활성화가 되는 조건")]
    public bool _unLockCheckFlag = false;

    [Tooltip("스토리북의 책장 넘기는 부분을 클릭했다는 조건")]
    public bool _clickPoint_MarkCheckFlag = false;

    [Tooltip("이야기 책장을 넘기기 위해서 활성화가 되어 있어야 하는 조건")]
    public bool _storyVoiceFinishCheckFlag = false;

    [Tooltip("The Door Darkness Cinematic Timeline이 재생되는 동안에는 esc키 등이 작동하지 않게 하는 조건")]
    public bool _nowPlayingTimeline_TheDoorDarknessFlag = false;

    //~
    public static EventSensorController instance;

    //~
    [Header("[ REALATED EVENT TYPE INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Event_Type;

    public enum EventType
    {
        NONE, STORY_BOOK, CINEMA_DOOR, TURNOFF_LIGHT_FINDKEY, GET_KEY, ETC
    }
    public EventType _currentEventType = EventType.NONE;

    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ -------------------------------------------------------------------------------
    private void Start()
    {

    }

    //~ -------------------------------------------------------------------------------
    private void Update()
    {
        SelectionEventType();

        //% The Door Darkness 타임라인 재생 중에 ESC키 눌렀을 때 마우스 커서 비활성화
        if (Input.GetKeyDown(KeyCode.Escape) && _nowPlayingTimeline_TheDoorDarknessFlag)
        {
            GameControlTower.instance.MouseCursorInActive();
        }
        //% 이야기책 읽는 타임라인 재생 중에 ESC키 눌렀을 때 마우스 커서 항상 활성화 상태로 두기(책장을 넘겨야함)
        else if (Input.GetKeyDown(KeyCode.Escape) && _nowListenStoyBookCheckFlag)
        {
            GameControlTower.instance.MouseCursorActive();
        }
        //% 이야기책 읽는 타임라인 재생이 끝나고 마우스 커서 항상 활성화 상태로 두기(종료버튼 누를 수 있어야)
        else if (Input.GetKeyDown(KeyCode.Escape) && GameControlTower.instance._timelineEndButMouseCursorDisplayFlag)
        {
            GameControlTower.instance.MouseCursorActive();
        }
    }

    //~ -------------------------------------------------------------------------------
    private void SelectionEventType()
    {
        switch (_currentEventType)
        {
            case EventType.NONE:
                BoolConditionReset();

                break;

            case EventType.STORY_BOOK:
                _listenStoryBookReadyCheckFlag = true;

                _cinemaDoorCheckFlag = false;

                break;

            case EventType.CINEMA_DOOR:
                _cinemaDoorCheckFlag = true;

                _listenStoryBookReadyCheckFlag = false;

                break;

            case EventType.TURNOFF_LIGHT_FINDKEY:

                break;

            case EventType.GET_KEY:
                _getKeyCheckFlag = true;
                break;

            case EventType.ETC:

                break;

        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PatJi" && this.gameObject.name == "Story_Sensor")
        {
            ////Debug.Log("팥쥐 센서 In 확인");
            _currentEventType = EventType.STORY_BOOK;

            EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
            for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
            {
                _object_Have_EventSensorContorller[i]._currentEventType = EventType.STORY_BOOK;

            }

            _stoyObjectMark.SetActive(false);
            _keyMark_Label_StoryBook.SetActive(true);
        }



        if (other.gameObject.tag == "PatJi" && this.gameObject.name == "CinemaDoor_Sensor")
        {
            _currentEventType = EventType.CINEMA_DOOR;

            EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
            for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
            {
                _object_Have_EventSensorContorller[i]._currentEventType = EventType.CINEMA_DOOR;

            }

            _keyMark_Label_cinemaDoor.SetActive(true);
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PatJi" && this.gameObject.name == "Story_Sensor"
                && !_eventActiveCheckFlag && _storyVoiceContinueReadyFlag && Input.GetKeyDown(KeyCode.Escape)
                && !SweetHomeSweet_Quest_Manager.instance._quest_8_Completed)
        {

            EndStoryBookEnvent();
            ////Debug.Log("타임라인 재생중 ESC");
        }
        else if (other.gameObject.tag == "PatJi" && this.gameObject.name == "Story_Sensor"
            && _eventActiveCheckFlag && _storyVoiceContinueReadyFlag && _storyVoiceFinishCheckFlag && Input.GetKeyDown(KeyCode.Escape)
            && !SweetHomeSweet_Quest_Manager.instance._quest_8_Completed)
        {

            EndStoryBookEnvent();
            ////Debug.Log("타임라인 재생중 ESC");
        }

        else if (other.gameObject.tag == "PatJi" && this.gameObject.name == "Story_Sensor"
        && _storyVoiceFinishCheckFlag && _nowListenStoyBookCheckFlag && Input.GetKeyDown(KeyCode.Escape)
        && !SweetHomeSweet_Quest_Manager.instance._quest_8_Completed)
        {

            EndStoryBookEnvent();
            ////Debug.Log("타임라인 재생중 ESC");
        }

    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "PatJi" && this.gameObject.name == "Story_Sensor")
        {
            _currentEventType = EventType.NONE;

            EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
            for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
            {
                _object_Have_EventSensorContorller[i]._currentEventType = EventType.NONE;

            }
            ////Debug.Log("팥쥐 센서 Out 확인");

            GameControlTower.instance._timelineEndButMouseCursorDisplayFlag = false;

            _stoyObjectMark.SetActive(true);
            _keyMark_Label_StoryBook.SetActive(false);
        }

        if (other.gameObject.tag == "PatJi" && this.gameObject.name == "CinemaDoor_Sensor")
        {
            _currentEventType = EventType.NONE;

            //% EventSensorContoller 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
            EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
            for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
            {
                _object_Have_EventSensorContorller[i]._currentEventType = EventType.NONE;
                _object_Have_EventSensorContorller[i]._nowPlayingTimeline_TheDoorDarknessFlag = false;

            }
            //_cinemaDoorCheckFlag = false;

            _keyMark_Label_cinemaDoor.SetActive(false);
            _pressKeyCodeE_Text_cinemaDoor.SetActive(true);
            _findKey_Text_cinemaDoor.SetActive(false);



        }

    }


    //~ -----------------Event-------------------------------------------------------------

    public void NowNokeyEvent()
    {
        ////Debug.Log("문이 잠겨있습니다. 키를 찾으세요.");
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_01_UNLOCKDOOR);
        GameControlTower.instance.MouseCursorInActive();

        _pressKeyCodeE_Text_cinemaDoor.SetActive(false);
        _findKey_Text_cinemaDoor.SetActive(true);
    }

    public void GetKeyEvent()
    {
        ////Debug.Log("문이 열렸습니다. 시네마틱 타임라인을 재생합니다.");
        GameControlTower.instance.MouseCursorInActive();

        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        EventSensorController[] _object_Have_EventSensorContoller =
        GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContoller.Length; i++)
        {
            _object_Have_EventSensorContoller[i]._nowPlayingTimeline_TheDoorDarknessFlag = true;
            _object_Have_EventSensorContoller[i]._unLockCheckFlag = true;
        }

        _lock_Mark.SetActive(false);
        _unLock_Mark.SetActive(false);

        _findKey_Text_cinemaDoor.SetActive(false);

        _mainCamera.SetActive(true);

        //% 시네마틱 타임라인 재생!!!
        GameObject _gameContorlTower;
        _gameContorlTower = GameObject.Find("GameControlTower");
        _gameContorlTower.GetComponent<GameControlTower>()._patJi.SetActive(false);
        _gameContorlTower.GetComponent<GameControlTower>()._patJiCam.SetActive(false);
        
        _gameContorlTower.GetComponent<GameControlTower>()._timeLine_TheDoorDarkness.SetActive(true);
    }

    public void UnlockMarkEnable()
    {
        _unLock_Mark.SetActive(true);
    }


    public void BeginStoryBookReady()
    {
        GameControlTower.instance.MouseCursorActive();
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        EventSensorController[] _object_Have_EventSensorContoller =
        GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContoller.Length; i++)
        {
            _object_Have_EventSensorContoller[i]._eventActiveCheckFlag = true;

        }

        _eventActiveCheckFlag = true;
        _stoyObjectMark.SetActive(false);

        _clickPoint_Mark_Canvas.SetActive(true);

        _firstPersion_Camera01_About_StoryBook.SetActive(true);
        _sweetHomeSweet_Camera.SetActive(false);
    }

    public void BeginStoryBookEvent()
    {
        ////_nowListenStoyBookCheckFlag = true;

        GameControlTower.instance.MouseCursorActive();

        EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
        {
            _object_Have_EventSensorContorller[i]._storyVoiceContinueReadyFlag = true;
            _object_Have_EventSensorContorller[i]._eventActiveCheckFlag = true;
            _object_Have_EventSensorContorller[i]._nowListenStoyBookCheckFlag = true;
        }

    }

    public void EndStoryBookEnvent()
    {

        Debug.Log("스토리북.. 시네마틱 타임라인을 종료합니다.");
        GameControlTower.instance._timelineEndButMouseCursorDisplayFlag = false;

        GameControlTower.instance.MouseCursorInActive();
    
        _mainCamera.SetActive(false);

        EventSensorController[] _object_Have_EventSensorContorller =
                        GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
        {
            _object_Have_EventSensorContorller[i]._eventActiveCheckFlag = false;
            _object_Have_EventSensorContorller[i]._nowListenStoyBookCheckFlag = false;

        }

        _firstPersion_Camera01_About_StoryBook.SetActive(false);
        _keyMark_Label_StoryBook.SetActive(false);
        _stoyObjectMark.SetActive(true);
        _sweetHomeSweet_Camera.SetActive(true);


        _clickPoint_Mark_Canvas.SetActive(false);
        _close_Mark_Canvas.SetActive(false);
        _clickPoint_MarkCheckFlag = false;

        GameObject _gameControlTower;
        _gameControlTower = GameObject.FindWithTag("GameControlTower");
        _gameControlTower.GetComponent<GameControlTower>()._timelineActiveFlag = false;
        _gameControlTower.GetComponent<GameControlTower>()._dialogueText01_StoryBook.SetActive(false);


        //% 타임라인 초기화
        _storyBook_Timeline.SetActive(false);
        _storyBook_Timeline.GetComponent<PlayableDirector>().time = 0.0f;


        //% 마우스 클릭해서 스토리북을 읽은 것이 아니면 이어지는 이야기 타임라인 재생 코루틴 호출 안함.
        if (_storyVoiceContinueReadyFlag)
        {
            StartCoroutine(StoryVoiceContinue_AfterLookBookStory());
        }

    }

    IEnumerator StoryVoiceContinue_AfterLookBookStory()
    {

        yield return new WaitForSeconds(4.5f);
        GameControlTower.instance._dialogueText02_StoryVoiceContinue.SetActive(true);
        _storyVoiceContinueTimeline.SetActive(true);

    }

    //@ 타임라인 시그널 호출.. 타임라인 비활성화 함수 
    public void DisableStoryContinue_AfterLookBookStory()
    {
        //% 다이얼로그 텍스트 비활성화   
        GameControlTower.instance.TimelineDisable_StoryVoiceContinue();
        //% 타임라인 비활성화F
        _storyVoiceContinueTimeline.SetActive(false);

    }
    //@ 스토리 북 이야기 이벤트 타임라인 시작 호출 함수 
    public void StoryBook_TimelinePlay()
    {

        //% 스토리북 이야기 이벤트 발생시 필요한 조건들(스토리북을 듣고 있는 상태 체크 플래그) 셋팅을 위함 함수 호출
        BeginStoryBookEvent();

        StartCoroutine(WaitCameraActivation());
        _clickPoint_Mark_Canvas.SetActive(false);
        _storyBook_Timeline.SetActive(true);

        GameObject _gameControlTower;
        _gameControlTower = GameObject.FindWithTag("GameControlTower");
        _gameControlTower.GetComponent<GameControlTower>()._timelineActiveFlag = true;
        _gameControlTower.GetComponent<GameControlTower>()._dialogueText01_StoryBook.SetActive(true);

        ////Debug.Log("스토리북.. 시네마틱 타임라인을 재생합니다.");
    }

    IEnumerator WaitCameraActivation()
    {
        yield return new WaitForSeconds(45.0f);
        _mainCamera.SetActive(true);
        ////Debug.Log("Start Camera Activation");
    }

    public void BoolConditionReset()
    {
        _listenStoryBookReadyCheckFlag = false;
        _cinemaDoorCheckFlag = false;
        _turnOffLightFindKeyCheckFlag = false;
        _nowListenStoyBookCheckFlag = false;
    }

}
