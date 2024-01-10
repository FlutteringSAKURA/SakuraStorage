using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.05 
// Update: //@ 2023.12.06 
// Update: //@ 2023.12.07 
// Update: //@ 2023.12.08 
// Update: //@ 2023.12.11 
// Update: //@ 2023.12.13 
// Update: //@ 2023.12.15 

//# NOTE: 퀘스트 제어를 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class SweetHomeSweet_Quest_Manager : MonoBehaviour
{
    [Header("[ RELATED REFERENCE INFO ]")]
    [SerializeField]
    GameObject _patJi;

    [SerializeField]
    [Tooltip("시네마신 타임라인 재생을 위한 메인 카메라 참조")]
    GameObject _mainCamera;


    [Tooltip("퀘스트 진행 상황에 따라 각각 활성화 되는 인터렉션 표시 마크.. 초기 설정은 <비활성화>.. ")]
    public GameObject[] _interaction_Indicator_Mark_Group;
    public GameObject[] _interaction_Indicator_Mark_Group_02;

    [SerializeField]
    [Tooltip("퀘스트의 SendMessage발송을 통해 Quest_Receiver가 수신 할 수 있도록 참조")]
    GameObject _quest_Receiver;

    [Header("[ Quest 02 ]")]

    [SerializeField]
    [Tooltip("퀘스트의 SendMessage발송을 통해 Light Off가 수신 할 수 있도록 참조")]
    GameObject _lightOff;

    [Header("[ Quest 03 - 1 ]")]

    [SerializeField]
    [Tooltip("화장실 물내리는 퀘스트(3.1)가 진행 되기 전 카메라 시점 변화를 위한 참조")]
    GameObject _firstPersonCamera_Toilet;

    [SerializeField]
    [Tooltip("화장실 물내리는 퀘스트(3.1)가 진행 되기 전 카메라 시점 변화를 위한 참조")]
    GameObject _homeSweetHome_Camera;

    [Header("[ Quest 04 ]")]

    [SerializeField]
    [Tooltip("입욕제 넣기 퀘스트(4)가 진행 되기 전 카메라 시점 변화를 위한 참조")]
    GameObject _firstPersonCamera_BathWater;

    [SerializeField]
    [Tooltip("Quest4(욕조에 입욕제 넣기)를 수행할 때, 입욕제가 아닐 경우 표시해주는 경고 텍스트 참조")]
    GameObject _q_4_BathBombWaterText_Canvas;

    [SerializeField]
    [Tooltip("Quest4(욕조에 입욕제 넣기)를 수행할 때, 입욕제가 맞을 경우 줌 카메라 활성화를 위한 참조")]
    GameObject _q_4_irstPerson_Camera_04_BathWater_Pass_Zoom_In;

    [Header("[ Quest 05 ]")]

    [SerializeField]
    [Tooltip("열쇠 찾기 퀘스트 (5)가 진행 되기 전 카메라 시점 변화를 위한 카메라 참조")]
    GameObject _firstPersonCamera_FindKey;

    [Space(10f)]
    [Header("[ Quest 06 ]")]
    [SerializeField]
    [Tooltip("열쇠 찾기 퀘스트 (6)의 진행 카메라 시점 변화를 위한 참조")]
    GameObject _firstPersonCamera_LookBook;

    [Space(10f)]
    [Header("[ Quest 07 ]")]
    [SerializeField]
    [Tooltip("열쇠 찾기 퀘스트 (7)의 진행 카메라 시점 변화를 위한 참조")]
    GameObject _firstPersonCamera_Picture;


    [Space(10f)]
    [Header("[ RELTED QUEST INFO ]")]
    [TextArea(3, 5)]
    public string _description_About_Quest_Name;

    public enum Quest_Name
    {
        NONE,
        Q_1_OPEN_THE_DOOR,
        Q_2_TURN_ON_LIGHT,
        Q_3_OPEN_CAP_AND_CLEAN,
        Q_3_1_OPEN_CAP_AND_CLEAN,
        Q_4_BATH_WATER,
        Q_5_FIND_KEY,
        Q_6_AFTER_OPEN_DOOR_EVENT_LOOK_BOOK,
        Q_7_LOOK_PICTURE,
        Q_8_GO_OUT
    }
    public Quest_Name _currentQuest = Quest_Name.NONE;


    [Tooltip("퀘스트의 진행상황에 따라 진행할 퀘스트를 위해 발동될 센서를 위한 참조 배열")]
    public GameObject[] _questSensor_Group_Child;

    //~
    [Space(10f)]
    [Header("[ BOOL CONDITIONS INFO RELATED READY FOR QUESTS ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Bool_Conditions01;


    [Tooltip("화장실 문 접근시 활성화 되는 조건")]
    public bool _quest_1_Ready = false;

    [Tooltip("화장실 불을 키기 위한 조건")]
    public bool _quest_2_Ready = false;

    [Tooltip("변기 뚜껑을 열기 위한 조건")]
    public bool _quest_3_Ready = false;

    [Tooltip("변기 물을 내리기 위한 조건")]
    public bool _quest_3_1_Ready = false;


    [Tooltip("입욕제를 넣기 위한 조건")]
    public bool _quest_4_Ready = false;

    [Tooltip("열쇠를 찾기 위한 조건")]
    public bool _quest_5_Ready = false;

    [Tooltip("복도 문을 열어본 뒤 드레스룸의 책을 보기 위한 조건")]
    public bool _quest_6_Ready = false;

    [Tooltip("그림을 보기 위한 조건")]
    public bool _quest_7_Ready = false;

    [Tooltip("창문으로 나가기 위한 조건")]
    public bool _quest_8_Ready = false;

    //~
    [Space(15f)]
    [Tooltip("문을 열고 닫는 키를 누르기 위한 조건")]
    public bool _enableKey_door = false;

    [Tooltip("화장실 불을 작동시키는 키를 누르기 위한 조건")]
    public bool _enableKey_Light = false;

    [Tooltip("화장실 불을 작동시 코루틴이 실행되는 동안 중복해서 키가 눌리는 것을 막기 위한 조건")]
    public bool _alreadyGetkeyDown_T = false;

    [Tooltip("화장실 변기 뚜껑을 열 때, 닫는 R 키가 먼저 눌리는 것을 막기 위한 조건")]
    public bool _alreadyGetkeyDown_P = false;

    [Tooltip("화장실 변기커버를 올렸다 내리는 키를 누르기 위한 조건")]
    public bool _enableKey_ToiletCap = false;

    [Tooltip("화장실 변기 중복해서 키를 눌러 작동할 때, 이미 사라진 퀘스트 마커를 다시 찾아 Null값 에러가 나는 것을 방지하기 위한 조건 ")]
    public bool _alreadyDisable_Marker = false;

    [Space(10f)]
    [Tooltip("입욕제를 선택할 수 있는 숫자 키를 누르기 위한 조건")]
    public bool _enableNumKey_BathWater = false;


    //~
    [Space(10f)]
    [Header("[ BOOL CONDITIONS INFO RELATED READY FOR QUESTS ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Bool_Conditions02;

    [Tooltip("퀘스트 2 발동을 위한 조건")]
    public bool _quest_1_Completed = false;

    [Tooltip("퀘스트 3 발동을 위한 조건")]
    public bool _quest_2_Completed = false;

    [Tooltip("퀘스트 3.1 발동을 위한 조건")]
    public bool _quest_3_Completed = false;

    [Tooltip("퀘스트 4 발동을 위한 조건")]
    public bool _quest_3_1_Completed = false;

    [Tooltip("퀘스트 5 발동을 위한 조건")]
    public bool _quest_4_Completed = false;

    [Tooltip("퀘스트 6 발동을 위한 조건")]
    public bool _quest_5_Completed = false;

    [Tooltip("퀘스트 7 발동을 위한 조건")]
    public bool _quest_6_Completed = false;

    [Tooltip("퀘스트 8 발동을 위한 조건")]
    public bool _quest_7_Completed = false;

    [Tooltip("숲(필드) 출발..Scene Change 발동을 위한 조건")]
    public bool _quest_8_Completed = false;

    //~
    [Space(15f)]
    [Tooltip("퀘스트 3.1 (변기 물내리기) 카메라 시점 전환을 위한 조건")]
    public bool _toiletFirstPersonCamActiveFlag = false;

    [Tooltip("퀘스트 4 (입욕제 욕조에 넣기) 카메라 시점 전환을 위한 조건")]
    public bool _bathWaterFirstPersonCamActiveFlag = false;

    [Tooltip("퀘스트 4 (욕조 입욕제 넣기) 센서 발동시 퀘스트 인디케이터 마크가 _FirstPersonCmera_Toilet을 향하는 것을 막기 위한 퀘스트 임시 성공 조건")]
    public bool _already_Disable_Q_3_1_IndicatorMark = false;

    [Tooltip("퀘스트 4 (욕조 입욕제 넣기) 입욕제 선택시 해당 입욕제 확대보기 조건")]
    public bool _bathBomb_Number01 = false;
    public bool _bathBomb_Number02 = false;
    public bool _bathBomb_Number03 = false;
    public bool _bathBomb_Number04 = false;
    public bool _bathBomb_Number05 = false;

    [Space(10f)]
    [Tooltip("퀘스트 4 (욕조 입욕제 넣기) 입욕제 선택시 해당 입욕제 휠스크롤 확대보기 조건")]
    public bool _bathBombSelectionCamActiveFlag = false;
    [Tooltip("퀘스트 4 (욕조 입욕제 넣기) 입욕제 선택시 해당 입욕제 넣는 장면 연출 동안 ESC 비활성화 조건")]
    public bool _bathWaterPassZoominCameraFlag = false;


    [Space(10f)]
    [Tooltip("퀘스트 5 (열쇠 찾기) 카메라 시점 전환을 위한 조건")]
    public bool _findKeyFirstPersonCamActiveFlag = false;

    [Tooltip("퀘스트 5 (열쇠 찾기) 열쇠를 집어 확대보기 위한 조건")]
    public bool _readyToGetKeyFlag = false;

    [Space(10f)]
    [Tooltip("퀘스트 6 (드레스룸 책 보기) 카메라 시점 전환을 위한 조건")]
    public bool _lookBookFlag = false;
    [Tooltip("퀘스트 6 (드레스룸 책 보기) 벽 사진 카메라로 반복해서 보여지지 않게 하기 위한 조건")]
    public bool _lookBookDevilVoiceFinished = false;

    [Space(10f)]
    [Tooltip("퀘스트 7(픽처 액션) 픽처 액션이 반복해서 발생하지 않게 하기 위한 조건")]
    public bool _pictureFlag = false;

    [Space(10f)]
    [Tooltip("퀘스트 7(픽처 액션) 퀘스트8 인디케이터 마크가 곧바로 활성화 되지 않도록 조정해주기 주기 위한 조건")]
    public bool _lookAgainPicturOntheWallFlag = false;

    [Space(10f)]
    [Tooltip("퀘스트 8(집 나가기)) 퀘스트8 인디케이터 마크 비활성화 위한 조건")]
    public bool _go_Out_SweetHomeSweetFlag = false;


    [Space(10f)]
    [Header("[ RELATED TIME INFO ]")]
    [TextArea(3, 5)]
    public string _description_About_Timer;

    [Tooltip("키 작동의 딜레이를 주기 위한 시간 설정.. ex(변기 물내리기)")]
    public float _timeFlow = 0.0f;

    [Tooltip("키 작동의 딜레이를 주기 위한 시간 설정")]
    public float _inputKeyValue_delayTime;

    public AnimationClip _toilet_Water_Refill_Anim;

    [Space(10f)]
    [Header("[ RELATED OBJECT REFERENCE INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Object_Reference_Info;

    [SerializeField]
    [Tooltip("화장실 불을 키기 위한 라이트 참조")]
    GameObject _bathLookAtBathBombBottle_PoinfLight;

    [SerializeField]
    [Tooltip("열쇠에 집중하기 위한 불을 켜기 위한 참조")]
    GameObject _key_Look_At_FindKey_PointLight;

    [SerializeField]
    [Tooltip("드레스룸 그림책에 집중하기 위한 불을 켜기 위한 라이트 참조")]
    GameObject _lookBook_PointLight;

    [SerializeField]
    [Tooltip("벽 그림에F 집중하기 위한 불을 켜기 위한 라이트 참조")]
    GameObject _picture_PointLight;


    [Space(10f)]
    [Header("[ RELATED CONTROL INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Contorl_Info;

    public float _mouseWhellScrolSpeed = 5.0f;


    [Space(10f)]
    //~
    public static SweetHomeSweet_Quest_Manager instance;


    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ -------------------------------------------------------------------------------
    private void Start()
    {
        //% 기본 시작 시간과 딜레이 타임을 특정 애니메이션의 길이로 초기 설정   
        _timeFlow = _toilet_Water_Refill_Anim.length;
        _inputKeyValue_delayTime = _toilet_Water_Refill_Anim.length;

    }

    //~ -------------------------------------------------------------------------------

    private void Update()
    {
        //% 팥쥐가 죽어있으면 리턴
        if (_patJi.GetComponent<PatJi_Controller>()._isAlive == false)
            return;

        WhatAreYouDoingNow();
        WhatAreYouCompletedNow();

        _timeFlow += Time.deltaTime;

        //% 입욕제 확대 보기시
        if (_bathBombSelectionCamActiveFlag)
        {
            float _scroll = Input.GetAxis("Mouse ScrollWheel") * _mouseWhellScrolSpeed;
            //& 최대 줌인
            if (_firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView <= 25.0f && _scroll < 0)
            {
                _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView = 25.0f;
            }
            //& 최대 줌아웃
            else if (_firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView >= 75.0f && _scroll > 0)
            {
                _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView = 75.0f;
            }
            //& 줌인 아웃 하기
            else
            {
                _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView += _scroll;
            }
        }

        //% 열쇠F 확대 보기시
        else if (_quest_Receiver.GetComponent<Qest_Receiver>()._closeUpKeyFlag)
        {
            float _scroll = Input.GetAxis("Mouse ScrollWheel") * _mouseWhellScrolSpeed;
            //& 최대 줌인
            if (_firstPersonCamera_FindKey.GetComponent<Camera>().fieldOfView <= 25.0f && _scroll < 0)
            {
                _firstPersonCamera_FindKey.GetComponent<Camera>().fieldOfView = 25.0f;
            }
            //& 최대 줌아웃
            else if (_firstPersonCamera_FindKey.GetComponent<Camera>().fieldOfView >= 75.0f && _scroll > 0)
            {
                _firstPersonCamera_FindKey.GetComponent<Camera>().fieldOfView = 75.0f;
            }
            //& 줌인 아웃 하기
            else
            {
                _firstPersonCamera_FindKey.GetComponent<Camera>().fieldOfView += _scroll;
            }
        }

        //% Look Book Devil Voice 타임라인 재생 중에 ESC키 눌렀을 때 마우스 커서 비활성화
        if (Input.GetKeyDown(KeyCode.Escape) && _lookBookFlag)
        {
            GameControlTower.instance.MouseCursorInActive();
        }
        //% 입욕제 넣는 시네마틱 애니메이션 재생 중에 ESC키 눌렀을 때 마우스 커서 비활성화 유지
        else if (Input.GetKeyDown(KeyCode.Escape) && _bathWaterPassZoominCameraFlag)
        {
            GameControlTower.instance.MouseCursorInActive();
        }
        //% 집을 나가는 마지막 시네마틱 타임라인 재생 중에 ESC키 눌렀을 때 마우스 커서 비활성화 유지
        else if (Input.GetKeyDown(KeyCode.Escape) && GameControlTower.instance._theDarkForestReady)
        {
            GameControlTower.instance.MouseCursorInActive();
        }


    }


    //~ -------------------------------------------------------------------------------
    //@ 현재 수행중인 퀘스트가 무엇인지 체크해주는 함수 
    private void WhatAreYouDoingNow()
    {
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager
                        = GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();

        switch (_currentQuest)
        {

            case Quest_Name.NONE:
                QuestBoolFlag_Reset();

                break;

            case Quest_Name.Q_1_OPEN_THE_DOOR:
                _quest_1_Ready = true;

                if (Input.GetKeyDown(KeyCode.O) && _enableKey_door)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _quest_Receiver.SendMessage("Open_BathDoor_HomeSweetHome", SendMessageOptions.DontRequireReceiver);
                }
                else if (Input.GetKeyDown(KeyCode.C) && _enableKey_door)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _quest_Receiver.SendMessage("Close_BathDoor_HomeSweetHome", SendMessageOptions.DontRequireReceiver);
                }
                Q_1_OpenTheDoorBoolCondition();

                break;

            case Quest_Name.Q_2_TURN_ON_LIGHT:
                _quest_2_Ready = true;

                //% 문 열고 닫기
                if (Input.GetKeyDown(KeyCode.O) && _enableKey_door)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _quest_Receiver.SendMessage("Open_BathDoor_HomeSweetHome", SendMessageOptions.DontRequireReceiver);


                }
                else if (Input.GetKeyDown(KeyCode.C) && _enableKey_door)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _quest_Receiver.SendMessage("Close_BathDoor_HomeSweetHome", SendMessageOptions.DontRequireReceiver);

                }

                //% 불 다시 켜기
                else if (Input.GetKeyDown(KeyCode.T) && _enableKey_Light && !_quest_3_Ready && !_alreadyGetkeyDown_T)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _lightOff.SendMessage("TryingToLightOn", SendMessageOptions.DontRequireReceiver);

                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_02_TURNONLIGHT);

                    //# 퀘스트 3(변기뚜껑 올렸다 내리기) 센서가 활성화 되기 위한 _quest_2_Completed = true; 조건이 코루틴 후에 발동되게 하기 위한 코드
                    StartCoroutine(WaitForLightOffScriptCorutine());

                    //# Indicator Mark 비활성화
                    GameObject _lightOnMark_Text;
                    _lightOnMark_Text = GameObject.Find("LightOnMark_Text");
                    _lightOnMark_Text.SetActive(false);

                    //# T키가 중복해서 눌리는 것을 방지
                    _alreadyGetkeyDown_T = true;

                    ////Debug.Log("화장실의 불을 다시 켜봅니다.");
                }
                Q_2_TurnOnLightBoolCondition();

                break;

            case Quest_Name.Q_3_OPEN_CAP_AND_CLEAN:
                _quest_3_Ready = true;

                if (Input.GetKeyDown(KeyCode.P) && _enableKey_ToiletCap && !_alreadyGetkeyDown_P)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _alreadyGetkeyDown_P = true;

                    _quest_Receiver.SendMessage("LiftUp_Toilet_Cap", SendMessageOptions.DontRequireReceiver);
                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_03_TOILET_CAP_OPEN);
                    ////Debug.Log("팥쥐가 변기통의 커버를 올립니다.");

                }
                else if (Input.GetKeyDown(KeyCode.R) && _enableKey_ToiletCap && _alreadyGetkeyDown_P)
                {
                    GameControlTower.instance.MouseCursorInActive();
                    _alreadyGetkeyDown_P = false;

                    _quest_Receiver.SendMessage("LiftDown_Toilet_Cap", SendMessageOptions.DontRequireReceiver);
                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_03_TOILET_CAP_CLOSE);
                    ////Debug.Log("팥쥐가 변기통의 커버를 내립니다.");

                    //# 퀘스트3 성공 플래그 true;
                    _quest_3_Completed = true;
                    _questSensor_Group_Child[_questSensor_Group_Child.Length - 8].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_Completed = true;

                    //# 중복해서 키가 눌렀을 때 이미 비활성한 상태의 퀘스트 마커를 다시 찾아 비활성을 시도하는 것(Null)을 방지하는 방어코드
                    if (!_alreadyDisable_Marker)
                    {
                        //# Quest Indicator Mark 비활성화
                        GameObject _toilet_Object_Mark;
                        _toilet_Object_Mark = GameObject.Find("Toilet_Object_Mark");
                        _toilet_Object_Mark.SetActive(false);

                        _alreadyDisable_Marker = true;
                    }

                }

                else if (Input.GetKeyDown(KeyCode.Escape) && _toiletFirstPersonCamActiveFlag
                && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag && !SweetHomeSweet_Quest_Manager.instance._quest_8_Completed)
                {
                    ReturnCameraOriginalView_Toilet();
                    Debug.Log("화장실 변기 카메라 시점을 원상 복귀합니다");

                }
                Q_3_OpenCapAndCleanBoolCondition();

                break;

            case Quest_Name.Q_3_1_OPEN_CAP_AND_CLEAN:
                _quest_3_1_Ready = true;

                if (Input.GetKeyDown(KeyCode.Escape) && _toiletFirstPersonCamActiveFlag
                    && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag)
                {
                    ReturnCameraOriginalView_Toilet();
                    //// Debug.Log("카메라 시점을 원상 복귀합니다");

                }

                else if (Input.GetKeyDown(KeyCode.K) && _toiletFirstPersonCamActiveFlag)
                {

                    GameControlTower.instance.MouseCursorInActive();
                    //# 물이 내려갈 때까지 키를 눌러도 반복 작동 하지 않는다.. 애니메이션이 모두 재생될 때까지
                    if (_timeFlow >= _inputKeyValue_delayTime)
                    {
                        _quest_Receiver.SendMessage("Toilet_Water_Refill", SendMessageOptions.DontRequireReceiver);
                        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_03_TOILET);
                        ////Debug.Log("변기 레버를 내렸습니다.");
                    }


                    //# 이전 미션과 조건 동기화는 미리 어기서 해줌;
                    _quest_3_1_Completed = true;
                    _questSensor_Group_Child[_questSensor_Group_Child.Length - 7].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_1_Completed = true;
                    _questSensor_Group_Child[_questSensor_Group_Child.Length - 8].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_1_Completed = true;

                }
                Q_3_1_OpenCapAndCleanBoolCondition();

                break;

            case Quest_Name.Q_4_BATH_WATER:
                _quest_4_Ready = true;

                //% SendMessage를 보내기 위한 선언 할당
                ShowMarkText _showMarkText;
                _showMarkText = GameObject.Find("Q_04_BathWater_Sensor").GetComponent<ShowMarkText>();

                if (Input.GetKeyDown(KeyCode.Escape) && _bathWaterFirstPersonCamActiveFlag && !_enableNumKey_BathWater
                && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag && !_quest_8_Completed)
                {

                    ReturnCameraOriginalView_BathWater();
                    ////Debug.Log("욕조 카메라 시점을 원상 복귀합니다");
                }

                else if (Input.GetKeyDown(KeyCode.Escape) && _bathWaterFirstPersonCamActiveFlag && _enableNumKey_BathWater
                && !_bathWaterPassZoominCameraFlag && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag
                && !_quest_8_Completed)
                {
                    GameControlTower.instance.MouseCursorActive();

                    _bathBombSelectionCamActiveFlag = false;
                    _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView = 75.0f;

                    EventSensorController.instance._eventActiveCheckFlag = false;

                    _enableNumKey_BathWater = false;
                    _q_4_BathBombWaterText_Canvas.SetActive(false);

                    //% 제자리에 돌려 놓기
                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB_BOTTLE_SOUND);
                    _quest_Receiver.SendMessage("ReturnBathBombObjectOriginalTransform", SendMessageOptions.DontRequireReceiver);
                    ReturnOrigninalObjectPositionForTEXT();
                    ReturnAllBathBomb_NumberBoolCondition();
                    ////Debug.Log("욕조 카메라 시점은 그대로 두고 오브젝트만 원상 복귀합니다");
                }

                else if (Input.GetKeyDown("1") && _bathWaterFirstPersonCamActiveFlag && !_bathBomb_Number01 && !_bathBomb_Number02
                && !_bathBomb_Number03 && !_bathBomb_Number04 && !_bathBomb_Number05)
                {

                    GameControlTower.instance.MouseCursorActive();
                    _bathBombSelectionCamActiveFlag = true;

                    EventSensorController.instance._eventActiveCheckFlag = true;

                    ////Debug.Log("1번 입욕제 선택됨");
                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB_BOTTLE_SOUND);

                    for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
                    {
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number01 = true;

                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number02 = false;
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number03 = false;
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number04 = false;
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number05 = false;

                    }

                    _showMarkText.SendMessage("BathBottleNumberTextDisable", SendMessageOptions.DontRequireReceiver);

                    //% 확대 보기
                    _quest_Receiver.SendMessage("LookAtBathBomb", SendMessageOptions.DontRequireReceiver);
                    _enableNumKey_BathWater = true;

                }
                else if (Input.GetKeyDown("2") && _bathWaterFirstPersonCamActiveFlag && !_bathBomb_Number02 && !_bathBomb_Number01
                && !_bathBomb_Number03 && !_bathBomb_Number04 && !_bathBomb_Number05)
                {
                    GameControlTower.instance.MouseCursorActive();
                    _bathBombSelectionCamActiveFlag = true;

                    EventSensorController.instance._eventActiveCheckFlag = true;

                    ////Debug.Log("2번 입욕제 선택됨");
                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB_BOTTLE_SOUND);

                    for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
                    {

                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number02 = true;

                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number01 = false;
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number03 = false;
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number04 = false;
                        _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number05 = false;
                    }

                    _showMarkText.SendMessage("BathBottleNumberTextDisable", SendMessageOptions.DontRequireReceiver);
                    _quest_Receiver.SendMessage("LookAtBathBomb", SendMessageOptions.DontRequireReceiver);
                    _enableNumKey_BathWater = true;
                }

                else if (Input.GetKeyDown("3") && _bathWaterFirstPersonCamActiveFlag && !_bathBomb_Number03 && !_bathBomb_Number01
                && !_bathBomb_Number02 && !_bathBomb_Number04 && !_bathBomb_Number05)
                {
                    GameControlTower.instance.MouseCursorActive();
                    _bathBombSelectionCamActiveFlag = true;

                    EventSensorController.instance._eventActiveCheckFlag = true;
                    ////Debug.Log("3번 입욕제 선택됨");
                    _bathBomb_Number03 = true;

                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB_BOTTLE_SOUND);

                    _showMarkText.SendMessage("BathBottleNumberTextDisable", SendMessageOptions.DontRequireReceiver);
                    _quest_Receiver.SendMessage("LookAtBathBomb", SendMessageOptions.DontRequireReceiver);

                    _enableNumKey_BathWater = true;
                }

                else if (Input.GetKeyDown("4") && _bathWaterFirstPersonCamActiveFlag && !_bathBomb_Number04 && !_bathBomb_Number01
                && !_bathBomb_Number02 && !_bathBomb_Number03 && !_bathBomb_Number05)
                {
                    GameControlTower.instance.MouseCursorActive();
                    _bathBombSelectionCamActiveFlag = true;

                    EventSensorController.instance._eventActiveCheckFlag = true;
                    ////Debug.Log("4번 입욕제 선택됨");
                    _bathBomb_Number04 = true;

                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB_BOTTLE_SOUND);

                    _showMarkText.SendMessage("BathBottleNumberTextDisable", SendMessageOptions.DontRequireReceiver);
                    _quest_Receiver.SendMessage("LookAtBathBomb", SendMessageOptions.DontRequireReceiver);

                    _enableNumKey_BathWater = true;
                }
                else if (Input.GetKeyDown("5") && _bathWaterFirstPersonCamActiveFlag && !_bathBomb_Number05 && !_bathBomb_Number03
                && !_bathBomb_Number02 && !_bathBomb_Number03 && !_bathBomb_Number04)
                {
                    GameControlTower.instance.MouseCursorActive();
                    _bathBombSelectionCamActiveFlag = true;

                    EventSensorController.instance._eventActiveCheckFlag = true;
                    ////Debug.Log("5번 입욕제 선택됨");
                    _bathBomb_Number05 = true;

                    AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB_BOTTLE_SOUND);
                    _showMarkText.SendMessage("BathBottleNumberTextDisable", SendMessageOptions.DontRequireReceiver);
                    _quest_Receiver.SendMessage("LookAtBathBomb", SendMessageOptions.DontRequireReceiver);

                    _enableNumKey_BathWater = true;
                }

                Q_4_BathWaterBoolCondition();

                break;

            case Quest_Name.Q_5_FIND_KEY:
                _quest_5_Ready = true;

                //% SendMessage를 보내기 위한 선언 할당
                _showMarkText = GameObject.Find("Q_05_FindKey_Sensor").GetComponent<ShowMarkText>();
                //_showMarkText.SendMessage("",SendMessageOptions.DontRequireReceiver);

                if (Input.GetKeyDown(KeyCode.Escape) && _findKeyFirstPersonCamActiveFlag
                    && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag && !_quest_8_Completed)
                {

                    ReturnCameraOriginalView_FindKey();
                    Debug.Log("열쇠찾기 카메라 시점을 원상 복귀합니다");
                    if (EventSensorController.instance._getKeyCheckFlag)
                    {

                        //% 열쇠 비활성화
                        _quest_Receiver.GetComponent<Qest_Receiver>().DisableKey();
                    }
                }
                Q_5_FindKeyBoolConditions();

                break;

            case Quest_Name.Q_6_AFTER_OPEN_DOOR_EVENT_LOOK_BOOK:
                _quest_6_Ready = true;

                if (Input.GetKeyDown(KeyCode.Escape) && _lookBookFlag && _pictureFlag
                    && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag
                    && !_quest_8_Completed)
                {

                    ReturnCameraOriginalView_LookBook();
                    Debug.Log("그림책 보기 카메라 시점을 원상 복귀합니다");

                }

                Q_6_AfterOpenDoorEventLookBookBoolCondition();

                break;

            case Quest_Name.Q_7_LOOK_PICTURE:
                _quest_7_Ready = true;
                _pictureFlag = true;


                if (Input.GetKeyDown(KeyCode.Escape) && _quest_6_Completed
                    && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag && !EventSensorController.instance._nowListenStoyBookCheckFlag
                    && !EventSensorController.instance._eventActiveCheckFlag && !_quest_8_Completed)
                {

                    ReturnCameraOriginalView_PictureOntheWall();
                    Debug.Log("벽 그림 카메라 시점을 원상 복귀합니다");

                }
                else if (Input.GetKeyDown(KeyCode.Escape) && _quest_7_Completed && !EventSensorController.instance._nowPlayingTimeline_TheDoorDarknessFlag
                && !EventSensorController.instance._nowListenStoyBookCheckFlag && !EventSensorController.instance._eventActiveCheckFlag && !_quest_8_Completed)
                {
                    ReturnCameraOriginalView_TheDoorDarkness();
                    Debug.Log("도어 다크 시네마 타임라인을 끝내고 원래의 카메라로 복귀합니다.");
                }

                Q_7_LookPictureBoolCondition();

                break;

            case Quest_Name.Q_8_GO_OUT:
                Q_8_GoOutBoolCondition();


                break;
        }

    }

    private void Q_8_GoOutBoolCondition()
    {
        _quest_8_Ready = true;

        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
        _quest_3_Completed = true;
        _quest_3_1_Completed = true;
        _quest_4_Completed = true;
        _quest_5_Completed = true;
        _quest_6_Completed = true;
        _quest_7_Completed = true;
    }

    private void Q_7_LookPictureBoolCondition()
    {
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
        _quest_3_Completed = true;
        _quest_3_1_Completed = true;
        _quest_4_Completed = true;
        _quest_5_Completed = true;
        _quest_6_Completed = true;
    }

    private void Q_6_AfterOpenDoorEventLookBookBoolCondition()
    {
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
        _quest_3_Completed = true;
        _quest_3_1_Completed = true;
        _quest_4_Completed = true;
        _quest_5_Completed = true;
    }

    private void Q_5_FindKeyBoolConditions()
    {
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
        _quest_3_Completed = true;
        _quest_3_1_Completed = true;
        _quest_4_Completed = true;
    }

    private void Q_4_BathWaterBoolCondition()
    {
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
        _quest_3_Completed = true;
        _quest_3_1_Completed = true;
    }

    private void Q_3_1_OpenCapAndCleanBoolCondition()
    {
        _quest_3_Ready = false;
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
        _quest_3_Completed = true;
    }

    private void Q_3_OpenCapAndCleanBoolCondition()
    {
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
        _quest_2_Completed = true;
    }

    private void Q_2_TurnOnLightBoolCondition()
    {
        _quest_1_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

        _quest_1_Completed = true;
    }

    private void Q_1_OpenTheDoorBoolCondition()
    {
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;
    }


    //~ -------------------------------------------------------------------------------
    private void ReturnCameraOriginalView_TheDoorDarkness()
    {
        GameControlTower.instance.MouseCursorInActive();
        //% EventSensorContoller 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        EventSensorController[] _object_Have_EventSensorContorller =
            GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
        {
            _object_Have_EventSensorContorller[i]._nowPlayingTimeline_TheDoorDarknessFlag = false;
        }

        GameObject _gameControlTower = GameObject.Find("GameControlTower");
        _gameControlTower.GetComponent<GameControlTower>().TimelineDisable_TheDoorDarkness();

        _lookBookFlag = false;

        _picture_PointLight.SetActive(false);
        _firstPersonCamera_Picture.SetActive(false);

    }

    private void ReturnAllBathBomb_NumberBoolCondition()
    {
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager
                        = GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();

        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number01 = false;

            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number02 = false;
            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number03 = false;
            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number04 = false;
            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathBomb_Number05 = false;
        }

    }

    private void ReturnOrigninalObjectPositionForTEXT()
    {
        Debug.Log("모든 입욕제를 제자리에 놓습니다.");
        //% SendMessage를 보내기 위한 선언 할당
        ShowMarkText _showMarkText;
        _showMarkText = GameObject.Find("Q_04_BathWater_Sensor").GetComponent<ShowMarkText>();

        Debug.Log("입욕제를 선택할 수 있는 번호를 표시하는 UI 텍스트를 다시 활성화 합니다. ");
        _showMarkText.SendMessage("BathBottleNumberTextEnable", SendMessageOptions.DontRequireReceiver);

    }

    IEnumerator WaitForLightOffScriptCorutine()
    {
        //# Light_Off 스크립트의 TotallyElectronicFailure코루틴 시간 동안 기다린후  _quest_2_Completed = true 발동
        yield return StartCoroutine(_lightOff.GetComponent<Light_Off>().TotallyElectronicFailure());
        _quest_2_Completed = true;
    }

    //~ -----------------------------------------On Trigger Enter--------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("지금 발생한 퀘스트는 : " + _currentQuest + "입니다.");

        //@ 퀘스트 1 
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_01_OpenBathRoomDoor_Sensor")
        {

            _enableKey_door = true;


        }

        //@ 퀘스트 2 
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_02_TurnOnLight_Sensor")
        {

            _enableKey_door = true;
            _enableKey_Light = true;

        }

        //@ 퀘스트 3 
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_03_OpenToiletCap_And_Clen_Sensor")
        {

            _enableKey_ToiletCap = true;

            //% 퀘스트 진행을 위한 카메라 시점 변환
            if (!_toiletFirstPersonCamActiveFlag)
            {
                ChangedCameraVeiw_Toilet();

            }

        }
        //@ 퀘스트 3.1 
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_03_1_OpenToiletCap_And_Clen_Sensor")
        {
            //% 퀘스트 진행을 위한 카메라 시점 변환
            if (!_toiletFirstPersonCamActiveFlag)
            {
                ChangedCameraVeiw_Toilet();

            }

        }
        //@ 퀘스트 4 
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_04_BathWater_Sensor")
        {
            ChangedCameraVeiw_BathWater();


        }

        //@ 퀘스트 5
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_05_FindKey_Sensor")
        {
            if (other.gameObject.tag == "PatJi_Temp")
                return;

            ChangedCameraVeiw_FindKey();

        }

        //@ 퀘스트 6

        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_06_LookBook_And_Picture_On_the_Wall_Sensor")
        {
            if (other.gameObject.tag == "PatJi_Temp")
                return;

            ChangedCameraView_LookBook();

        }

        //@ 퀘스트 7
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_07_LookPicture_Sensor")
        {
            if (other.gameObject.tag == "PatJi_Temp")
                return;

            ChangedCameraView_PictureOntheWall();

            //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
            SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
            GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
            for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
            {
                //& 퀘스트7 완료
                _object_Have_SweetHomeSwett_Quest_Manager[i]._lookAgainPicturOntheWallFlag = true;
            }

        }

        //@ 퀘스트 8
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_08_SweetHomeSweet_Go_Out_Sensor")
        {
            if (other.gameObject.tag == "PatJi_Temp")
                return;

            Q_08_HomeSweetHomeGoOut_Cinemascene_Play();

            //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
            SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
            GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
            for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
            {
                //& 퀘스트7 완료
                _object_Have_SweetHomeSwett_Quest_Manager[i]._quest_8_Completed = true;

            }

            DisableAllIndicatorMark();



        }


    }

    private void DisableAllIndicatorMark()
    {
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            //& 퀘스트7 완료..Indicator Mark 비활성화
            _object_Have_SweetHomeSwett_Quest_Manager[i]._go_Out_SweetHomeSweetFlag = true;
        }

        for (int i = 0; i < _interaction_Indicator_Mark_Group.Length; i++)
        {
            _interaction_Indicator_Mark_Group[i].SetActive(false);
        }
        GameObject _interaction_Obj_canvas_Cinema_02;
        _interaction_Obj_canvas_Cinema_02 = GameObject.Find("Interaction_Obj_Canvas_Cinema_02");
        _interaction_Obj_canvas_Cinema_02.SetActive(false);

        Debug.Log("Mark Disable TEST");

    }

    private void Q_08_HomeSweetHomeGoOut_Cinemascene_Play()
    {

        GameControlTower.instance.MouseCursorInActive();
        _patJi.SetActive(false);
        _homeSweetHome_Camera.SetActive(false);
        _mainCamera.SetActive(true);

        GameObject _gameControlTowerObject = GameObject.Find("GameControlTower");
        GameControlTower _gameContorlTowerScript = GameObject.FindObjectOfType<GameControlTower>();
        _gameControlTowerObject.GetComponent<GameControlTower>()._homeSweetHomeClear = true;
        _gameContorlTowerScript._timeLine_SweetHomeSweetGoOut.SetActive(true);
        _gameContorlTowerScript._timelineActiveFlag = true;
        _gameContorlTowerScript._theDarkForestReady = true;

        _gameContorlTowerScript._dialogueText08_GoOutSweetHome.SetActive(true);

    }

    //~ -------------------------------------------------------------------------------
    private void ChangedCameraView_PictureOntheWall()
    {
        GameControlTower.instance.MouseCursorInActive();
        _firstPersonCamera_Picture.SetActive(true);
        _homeSweetHome_Camera.SetActive(false);
        _picture_PointLight.SetActive(true);

        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._quest_6_Completed = true;
        }

        _patJi.SetActive(false);
        _patJi.tag = "PatJi_Temp";
    }

    private void ReturnCameraOriginalView_PictureOntheWall()
    {
        GameControlTower.instance.MouseCursorInActive();
        _patJi.SetActive(true);
        _lookBookFlag = false;

        _picture_PointLight.SetActive(false);

        _firstPersonCamera_Picture.SetActive(false);
        _homeSweetHome_Camera.SetActive(true);

    }

    private void ChangedCameraView_LookBook()
    {
        GameControlTower.instance.MouseCursorInActive();
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._lookBookFlag = true;
        }

        GameControlTower.instance._timeline_LookBook.SetActive(true);
        GameControlTower.instance._dialogueText06_LookBookDevilVoice.SetActive(true);

        _firstPersonCamera_LookBook.SetActive(true);
        _homeSweetHome_Camera.SetActive(false);
        _lookBook_PointLight.SetActive(true);

        _patJi.SetActive(false);
        _patJi.tag = "PatJi_Temp";

        //% 벽에 걸린 그림이 비뚤어지는 액션이 반복되지 않도록 하는 방어 코드
        if (_pictureFlag)
            return;

        StartCoroutine(PictureCameraAction());

    }

    IEnumerator PictureCameraAction()
    {
        //% Devil Voice LookBook Timeline Play가 끝난 후
        yield return new WaitForSeconds(17.5f);
        GameControlTower.instance._dialogueText06_LookBookDevilVoice.SetActive(false);



        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._pictureFlag = true;
        }

        _firstPersonCamera_LookBook.SetActive(false);
        _homeSweetHome_Camera.SetActive(false);
        _picture_PointLight.SetActive(true);
        _firstPersonCamera_Picture.SetActive(true);

        _quest_Receiver.SendMessage("PictureAnimationPlay", SendMessageOptions.DontRequireReceiver);
        //% 사운드 재생
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_06_07_LOOKBOOK_AND_PICTURE_ON_THE_WALL);
        yield return new WaitForSeconds(0.05f);
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_06_07_LOOKBOOK_AND_PICTURE_ON_THE_WALL_01);


    }

    private void ReturnCameraOriginalView_LookBook()
    {
        GameControlTower.instance.MouseCursorInActive();
        _patJi.SetActive(true);
        _lookBookFlag = false;
        //% 타임라인 초기화를 위한 비활성화 코드
        GameControlTower.instance._timeline_LookBook.SetActive(false);
        GameControlTower.instance._dialogueText06_LookBookDevilVoice.SetActive(false);

        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._lookBookFlag = false;
        }

        _picture_PointLight.SetActive(false);

        _firstPersonCamera_LookBook.SetActive(false);
        _firstPersonCamera_Picture.SetActive(false);

        _homeSweetHome_Camera.SetActive(true);
        _lookBook_PointLight.SetActive(false);

        if (_quest_6_Completed)
        {
            Debug.Log("퀘스트 6 완료.. 퀘스트 7이 활성화 됐습니다.");

            //% 퀘스트 7(그림 처다보기) 센서 활성화
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 2].SetActive(true);
            //% 퀘스트 7(그림 처다보기) 인디케이터 마크 활성화
            _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 2].SetActive(true);
        }


    }

    private void ChangedCameraVeiw_FindKey()
    {
        GameControlTower.instance.MouseCursorActive();
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._findKeyFirstPersonCamActiveFlag = true;
        }

        _firstPersonCamera_FindKey.SetActive(true);
        _homeSweetHome_Camera.SetActive(false);
        _key_Look_At_FindKey_PointLight.SetActive(true);

        _patJi.SetActive(false);

        _patJi.tag = "PatJi_Temp";

    }

    private void ReturnCameraOriginalView_FindKey()
    {
        GameControlTower.instance.MouseCursorInActive();
        _patJi.SetActive(true);


        _findKeyFirstPersonCamActiveFlag = false;
        _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView = 75.0f;

        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._findKeyFirstPersonCamActiveFlag = false;
        }

        //% SendMessage를 보내기 위한 선언 할당
        ShowMarkText _showMarkText;
        _showMarkText = GameObject.Find("Q_05_FindKey_Sensor").GetComponent<ShowMarkText>();

        ////Debug.Log("입욕제를 선택할 수 있는 번호를 표시하는 UI 텍스트를 비활성화 합니다. ");
        _showMarkText.SendMessage("ClickText_On_Mark_Towel_Q5_FindKey_Disable", SendMessageOptions.DontRequireReceiver);
        _showMarkText.SendMessage("ClickText_On_Mark_Key_Q5_FindKey_Disable", SendMessageOptions.DontRequireReceiver);

        _firstPersonCamera_FindKey.SetActive(false);
        _homeSweetHome_Camera.SetActive(true);
        _key_Look_At_FindKey_PointLight.SetActive(false);

    }

    //@ 수건을 클릭할 때.. GravityTrue_Button 명칭의 버튼 이벤트로 호출되는 함수.. 
    public void RigidBodyActive_Towel()
    {
        GameObject _towel;
        _towel = GameObject.Find("RigidBody_Gravity_Towel");

        //% RigidBody ... Gravity 적용을 위한 코드
        _towel.GetComponent<Rigidbody>().isKinematic = false;

        _readyToGetKeyFlag = true;

        //% 타월 떨어지는 소리
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_05_FINDKEY_TOWEL_DROP_SOUND);

        //% 열쇠 반짝거림
        _quest_Receiver.SendMessage("EmissionEffectKey", SendMessageOptions.DontRequireReceiver);

        //% SendMessage를 보내기 위한 선언 할당
        ShowMarkText _showMarkText;
        _showMarkText = GameObject.Find("Q_05_FindKey_Sensor").GetComponent<ShowMarkText>();

        _showMarkText.SendMessage("ClickText_On_Mark_Towel_Q5_FindKey_Disable", SendMessageOptions.DontRequireReceiver);
        _showMarkText.SendMessage("ClickText_On_Mark_Key_Q5_FindKey_Enable", SendMessageOptions.DontRequireReceiver);

    }

    //@ 퀘스트 5 (열쇠 찾기) 완료에 필요한 열쇠를 획득하는 함수 

    public void CloseUpKey()
    {
        Debug.Log("열쇠를 획득했습니다.");

        ShowMarkText _showMarkText;
        _showMarkText = GameObject.Find("Q_05_FindKey_Sensor").GetComponent<ShowMarkText>();
        _showMarkText._getKey_button.SetActive(false);

        _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 4].SetActive(false);

        //% 열쇠 획득 사운드 
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_05_FINDKEY);

        _quest_Receiver.SendMessage("LookAtKeyAndGetKey", SendMessageOptions.DontRequireReceiver);

        //% 열쇠 획득 .. 문 열 수 있음..
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        EventSensorController[] _object_Have_EventSensorContoller =
        GameObject.FindObjectsOfType<EventSensorController>();
        for (int i = 0; i < _object_Have_EventSensorContoller.Length; i++)
        {
            _object_Have_EventSensorContoller[i]._getKeyCheckFlag = true;
        }
    }

    private void ChangedCameraVeiw_BathWater()
    {
        GameControlTower.instance.MouseCursorInActive();
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathWaterFirstPersonCamActiveFlag = true;
        }

        _bathLookAtBathBombBottle_PoinfLight.SetActive(true);

        _firstPersonCamera_BathWater.SetActive(true);
        _homeSweetHome_Camera.SetActive(false);
    }

    //@ 현재 수행할 퀘스트의 활성을 위해 수행 완료한 퀘스트를 체크해주는 함수 
    private void WhatAreYouCompletedNow()
    {
        //# 퀘스트1 + 퀘스트2 = 화장실 문열고 들어가기 + 화장실 불 켜보기
        if (_quest_1_Completed && !_quest_2_Ready)
        {
            if (_quest_2_Completed)
                return;

            //% 퀘스트 2 준비 완료.. 퀘스트 센서 활성화
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 8].SetActive(true);
            //% 퀘스트 2 표시 마크.. 활성화
            _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 8].SetActive(true);


            //% 퀘스트1 완료.. 비활성화
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 9].SetActive(false);

            Debug.Log("퀘스트 1 완료.. 퀘스트 2가 활성화 됐습니다.");
        }

        //# 퀘스트 3 = 변기 뚜껑 열기 준비
        if (_quest_2_Completed && !_quest_3_Ready)
        {

            //% 퀘스트 2완료.. 화장실 문열림 닫힘 유지를 위해 비활성화시키지 않고 조건만 바꿔 유지함
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 8].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_2_Ready = false;
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 8].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_Ready = true;

            //% 퀘스트 3 준비완료.. 퀘스트 센서 활성화
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 7].SetActive(true);
            //% 퀘스트 3 표시 마크.. 활성화
            _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 7].SetActive(true);

            Debug.Log("퀘스트 2 완료.. 퀘스트 3이 활성화 됐습니다.");

        }

        //# 퀘스트 3.1 = 변기 물 내리기 준비
        if (_quest_1_Completed && _quest_2_Completed && _quest_3_Completed && _quest_3_Ready && !_quest_3_1_Ready)
        {

            //% 퀘스트 3.1 준비완료.. 퀘스트 센서 활성화
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 6].SetActive(true);
            //% 퀘스트 3.1 표시 마크.. 활성화
            _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 6].SetActive(true);
            Debug.Log("퀘스트 3 완료.. 퀘스트 3.1이 활성화 됐습니다.");

        }

        //# 퀘스트 4 = 욕조에 입욕제 타기 준비
        // if (_quest_1_Completed && _quest_2_Completed && _quest_3_1_Completed && _quest_3_Ready && !_quest_4_Ready)
        {
            //% 이전 퀘스트 3(변기 뚜껑 올리고 내리는)의 조건 값을 다음 퀘스트 3.1에 맞게끔 동기화(싱크 맞춤)
            //_questSensor_Group_Child[_questSensor_Group_Child.Length - 7].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_Ready = false;
            //_questSensor_Group_Child[_questSensor_Group_Child.Length - 7].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_1_Ready = true;

            //% 퀘스트 3완료.. 비활성화
            //_questSensor_Group_Child[_questSensor_Group_Child.Length - 7].SetActive(false);
            //% 퀘스트 2완료.. 화장실 문열림 닫힘 유지를 위해 비활성화시키지 않고 조건만 바꿔 유지함
            //_questSensor_Group_Child[_questSensor_Group_Child.Length - 7].GetComponent<SweetHomeSweet_Quest_Manager>()._currentQuest = Quest_Name.Q_3_OPEN_CAP_AND_CLEAN;

        }

        //# 퀘스트 5 = 열쇠 얻기 준비
        // if (_quest_1_Completed && _quest_2_Completed && _quest_3_1_Completed && _quest_4_Completed && !_quest_5_Ready)
        {
            // //% 퀘스트 5 준비완료.. 퀘스트 센서 활성화
            // _questSensor_Group_Child[_questSensor_Group_Child.Length - 4].SetActive(true);
            // //% 퀘스트 5 표시 마크.. 활성화
            // _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 4].SetActive(true);
            // Debug.Log("퀘스트 4 완료.. 퀘스트 5가 활성화 됐습니다.");
        }

        //# 마지막 Go Out Sweet Home Sweet 준비
        if (_quest_7_Completed && EventSensorController.instance._unLockCheckFlag && EventSensorController.instance._storyVoiceContinueReadyFlag && !_go_Out_SweetHomeSweetFlag)
        {
            Debug.Log("퀘스트 7 완료.. 퀘스트 8이 활성화 됐습니다.");

            //% 퀘스트 8(그림 처다보기) 센서 활성화
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 1].SetActive(true);
            //% 퀘스트 8(그림 처다보기) 인디케이터 마크 활성화
            _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 1].SetActive(true);
            for (int i = 0; i < _interaction_Indicator_Mark_Group_02.Length; i++)
            {
                _interaction_Indicator_Mark_Group_02[i].SetActive(true);
            }

        }


    }

    //@ 퀘스트 3.1(화장실 물내리는 퀘스트)이 완료 되는 경우 퀘스트 마크를 비활성화 해주는 함수 
    //# 애니메이션 이벤트로 함수 호출함.
    public void Disable_ToiletWater_Quest_IndicatorMark()
    {
        //% Null값 방어코드 .. K키(변기 물을 여러번 작동시켜도 괜찮음)
        if (!_already_Disable_Q_3_1_IndicatorMark)
        {
            GameObject _toiletWater_Mark;
            _toiletWater_Mark = GameObject.Find("ToiletWater_Mark");
            _toiletWater_Mark.SetActive(false);

            //& 퀘스트 3.1 성공
            _questSensor_Group_Child[_questSensor_Group_Child.Length - 5].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_3_1_Completed = true;

            _already_Disable_Q_3_1_IndicatorMark = true;

        }

        ////Debug.Log("편지 받았어요");
    }

    private void ChangedCameraVeiw_Toilet()
    {
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._toiletFirstPersonCamActiveFlag = true;
        }

        _firstPersonCamera_Toilet.tag = "MainCamera";
        _firstPersonCamera_Toilet.SetActive(true);
        _homeSweetHome_Camera.SetActive(false);
    }
    private void ReturnCameraOriginalView_Toilet()
    {
        GameControlTower.instance.MouseCursorInActive();
        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._toiletFirstPersonCamActiveFlag = false;
        }

        ShowMarkText.instance.Esc_Toilet();
        _firstPersonCamera_Toilet.SetActive(false);
        _homeSweetHome_Camera.SetActive(true);

    }


    //~ --------------------------------------------On trigger Exit---------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("PatJi") && _quest_1_Completed)
        { _quest_1_Ready = false; }

        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_01_OpenBathRoomDoor_Sensor")
        {
            _enableKey_door = false;

        }
        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_02_TurnOnLight_Sensor")
        {

            _enableKey_door = false;
            _enableKey_Light = false;

        }

        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_03_OpenToiletCap_And_Clen_Sensor")
        {
            _enableKey_ToiletCap = false;

            ReturnCameraOriginalView_Toilet();
            ////Debug.Log("카메라 시점을 원상 복귀합니다");

        }

        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_03_1_OpenToiletCap_And_Clen_Sensor")
        {
            ReturnCameraOriginalView_Toilet();

            // NOTE: //# 퀘스트 4 (욕조 입욕제 넣기) 센서 발동시 퀘스트 인디케이터 마크가 _FirstPersonCmera_Toilet을 향하고 있어 
            //# 이 상태로 _FirstPersonCmera_Toilet이 비활성화 되면 퀘스트4 인디케이터 마크가 타겟을 잃어 굳어버리는 현상이 발생
            //# 따라서 퀘스트 4 센서가 생성되는 시점을 OntriggerExit시점으로 한다.
            //# 퀘스트3.1 성공 플래그 true
            if (_quest_3_1_Completed && !_quest_4_Completed)
            {
                //% 퀘스트 4 준비완료.. 퀘스트 센서 활성화
                _questSensor_Group_Child[_questSensor_Group_Child.Length - 5].SetActive(true);
                //% 퀘스트 4 표시 마크.. 활성화
                _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 5].SetActive(true);
                ////Debug.Log("퀘스트 3.1 완료.. 퀘스트 4가 활성화 됐습니다.");
            }
            ////Debug.Log("카메라 시점을 원상 복귀합니다");

        }

        if (other.gameObject.tag.Contains("PatJi") && this.gameObject.name == "Q_04_BathWater_Sensor")
        {
            ReturnCameraOriginalView_BathWater();

            if (_quest_4_Completed && !_quest_5_Completed)
            {

                //% 퀘스트 5 준비완료.. 퀘스트 센서 활성화
                _questSensor_Group_Child[_questSensor_Group_Child.Length - 4].SetActive(true);
                //% 퀘스트 5 표시 마크.. 활성화
                _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 4].SetActive(true);
                ////Debug.Log("퀘스트 4 완료.. 퀘스트 5가 활성화 됐습니다.");
            }

        }

        if (other.gameObject.tag.Contains("PatJi_Temp") && this.gameObject.name == "Q_05_FindKey_Sensor")
        {
            ReturnCameraOriginalView_FindKey();
            _patJi.tag = "PatJi";


            if (EventSensorController.instance._getKeyCheckFlag)
            {
                //% 퀘스트 6 준비완료.. 퀘스트 5센서 비활성화
                _questSensor_Group_Child[_questSensor_Group_Child.Length - 4].SetActive(false);

                //% 퀘스트 6 표시 마크.. 활성화
                //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
                SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
                GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
                for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
                {
                    _object_Have_SweetHomeSwett_Quest_Manager[i]._quest_5_Completed = true;
                }

                _questSensor_Group_Child[_questSensor_Group_Child.Length - 3].SetActive(true);
                _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 3].SetActive(true);
            }
        }

        if (other.gameObject.tag.Contains("PatJi_Temp") && this.gameObject.name == "Q_06_LookBook_And_Picture_On_the_Wall_Sensor")
        {
            ReturnCameraOriginalView_LookBook();
            _patJi.tag = "PatJi";

            //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
            SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
            GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
            for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
            {
                _object_Have_SweetHomeSwett_Quest_Manager[i]._lookBookFlag = false;
            }
        }

        if (other.gameObject.tag.Contains("PatJi_Temp") && this.gameObject.name == "Q_07_LookPicture_Sensor")
        {
            _patJi.tag = "PatJi";
            //% EventSensorContoller 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
            EventSensorController[] _object_Have_EventSensorContorller =
                GameObject.FindObjectsOfType<EventSensorController>();
            for (int i = 0; i < _object_Have_EventSensorContorller.Length; i++)
            {
                _object_Have_EventSensorContorller[i]._nowPlayingTimeline_TheDoorDarknessFlag = false;
            }

            if (_lookAgainPicturOntheWallFlag)
            {
                //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
                SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
                GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
                for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
                {
                    //& 퀘스트7 완료
                    _object_Have_SweetHomeSwett_Quest_Manager[i]._quest_7_Completed = true;
                }
            }
        }
    }


    private void ReturnCameraOriginalView_BathWater()
    {
        GameControlTower.instance.MouseCursorInActive();

        _bathBombSelectionCamActiveFlag = false;
        _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView = 75.0f;

        //% SweetHomeSweet_Quest_Manager 스크립트를 가지고 있는 오브젝트들을 모두 찾아 그 오브젝트들의 특정 조건값을 동기화 시키기 위한 코드
        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._bathWaterFirstPersonCamActiveFlag = false;
        }

        //% SendMessage를 보내기 위한 선언 할당
        ShowMarkText _showMarkText;
        _showMarkText = GameObject.Find("Q_04_BathWater_Sensor").GetComponent<ShowMarkText>();

        ////Debug.Log("입욕제를 선택할 수 있는 번호를 표시하는 UI 텍스트를 비활성화 합니다. ");
        _showMarkText.SendMessage("BathBottleNumberTextDisable", SendMessageOptions.DontRequireReceiver);

        _enableNumKey_BathWater = false;

        _q_4_BathBombWaterText_Canvas.SetActive(false);

        _bathLookAtBathBombBottle_PoinfLight.SetActive(false);

        _firstPersonCamera_BathWater.SetActive(false);
        _homeSweetHome_Camera.SetActive(true);
    }


    private void QuestBoolFlag_Reset()
    {
        _quest_1_Ready = false;
        _quest_2_Ready = false;
        _quest_3_Ready = false;
        _quest_3_1_Ready = false;
        _quest_4_Ready = false;
        _quest_5_Ready = false;
        _quest_6_Ready = false;
        _quest_7_Ready = false;
        _quest_8_Ready = false;

    }

    //~ -------------------------------------------------------------------------------
    //@ Quest4 (욕조에 입욕제 넣기) 수행중 입욕제가 아닐 경우 경고 텍스트를 보여주는 함수.. SendMessage로 호출됨 
    public void DisplayWaning_itIsNotBathBomb()
    {
        _q_4_BathBombWaterText_Canvas.SetActive(true);
    }
    public void ItIsBathBomb()
    {
        if (_quest_4_Completed)
            return;
        Debug.Log("이것은 입욕제가 맞습니다.");
        _bathWaterPassZoominCameraFlag = true;

        _q_4_irstPerson_Camera_04_BathWater_Pass_Zoom_In.SetActive(true);
        _firstPersonCamera_BathWater.SetActive(false);

        StartCoroutine(Completed_Quest_04_BathWater());
    }

    IEnumerator Completed_Quest_04_BathWater()
    {
        //% 퀘스트 완료 = 퀘스트4 표시 마크 비활성화 
        _interaction_Indicator_Mark_Group[_interaction_Indicator_Mark_Group.Length - 5].SetActive(false);

        //% 입욕제 그림자 욕조 물에 비추는 것을 없애기 위해 잠시 입욕제 비활성화
        _quest_Receiver.GetComponent<Qest_Receiver>().TempDisablePurpleBottle();
        //% 물 색깔 변화
        _quest_Receiver.GetComponent<Qest_Receiver>().BathWaterColorChanged();

        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_04_BATHWATERBOMB);

        yield return new WaitForSeconds(4.5f);
        _bathWaterPassZoominCameraFlag = false;
        _q_4_irstPerson_Camera_04_BathWater_Pass_Zoom_In.SetActive(false);

        //% 입욕제 원상복귀 활성화
        _quest_Receiver.GetComponent<Qest_Receiver>().TempDisablePurpleBottle();

        _firstPersonCamera_BathWater.SetActive(true);
        _firstPersonCamera_BathWater.GetComponent<Camera>().fieldOfView = 75.0f;

        //% 퀘스트4 완료 퀘스트5 센서 활성화를 위한 조건 미리 동기화
        _quest_4_Completed = true;
        _questSensor_Group_Child[_questSensor_Group_Child.Length - 6].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_4_Completed = true;
        _questSensor_Group_Child[_questSensor_Group_Child.Length - 7].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_4_Completed = true;
        _questSensor_Group_Child[_questSensor_Group_Child.Length - 8].GetComponent<SweetHomeSweet_Quest_Manager>()._quest_4_Completed = true;

    }


}
