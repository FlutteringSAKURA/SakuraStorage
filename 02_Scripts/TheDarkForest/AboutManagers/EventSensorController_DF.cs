using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.18 
// Update: //@ 2023.12.21 
// Update: //@ 2023.12.26 

//# NOTE: The Dark Forest의 Event Sensor의 제어를 위한 스크립트
//#       path : TheDarkForest_Starting_Story_Sensor / CCTV_Sensor_01 ~ / OFF_THE_COURSE ~ /

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class EventSensorController_DF : MonoBehaviour
{
    [Header("[ REALATED BOOL CONDITIONS INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_Bool_Conditions;

    [Tooltip("특정 이벤트가 발생할 경우 플레이어의 이동 조작이 되지 않도록 하는 것이 필요할 때 사용하기 위한 조건")]
    public bool _eventActiveCheckFlag = false;

    [Tooltip("특정 지역을 지나간 후 플레이어가 다시 왔던 길을 되돌아 가려할 때, 자칫 카메라가 너무 멀어지는 것을 방지하기 위해 플레이어를 따라다니는 카메라 시야로 변경하기 위한 조건")]
    public bool _ifPlayerTurnWayBack_Flag = false;
    [Tooltip("무엇인가 이미 존재하는 경우 무엇인가를 다시 발동시키지 않게 하기 위한 조건")]
    public bool _alreadyExistanceFlag = false;

    //~

    [Header("[ REALATED GAMEOBJECT REFERENCES INFO ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_GameObject_References;
    [SerializeField]
    [Tooltip("팥쥐를 따라다니는 카메라 구역 생성을 위한 센서 게임오브젝트 참조")]
    private GameObject _off_The_Course_If_Patji_Back;

    //~
    [Header("[ REALATED TIMELINE REFERENCES INFO ]")]
    [TextArea(2, 4)]
    public string _descripttion_About_Timeline_References;
    [SerializeField]
    [Tooltip("어둠의 숲 타임라인 01번 - Timeline_TheDarkForestStory 참조")]
    private GameObject _timeline_TheDarkForestStory;
    [SerializeField]
    [Tooltip("어둠의 숲 타임라인 01번 다이얼로그 - TheDarkForest_StoryDialogue_Text_Group_01 참조")]
    private GameObject _theDarkForest_StoryDialogue_Text_Group_01;


    public static EventSensorController_DF instance;

    //~ -------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    //~ --------------------------------------------On trigger Enter---------------------------------
    private void OnTriggerEnter(Collider other)
    {
        //@ 어둠의 숲 시작.. 스토리 재생 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "TheDarkForest_Starting_Story_Sensor")
        {
            Debug.Log("어둠의 숲 스토리 타임라인이 재생됩니다.");
            _theDarkForest_StoryDialogue_Text_Group_01.SetActive(true);
            _timeline_TheDarkForestStory.SetActive(true);
        }

        //@ CCTV 1번 지역 센서 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "CCTV_Sensor_01")
        {
            CCTV_Manager.instance.CCTV_01_ON();

            CCTV_Manager.instance.CCTV_02_OFF();
            CCTV_Manager.instance.CCTV_03_OFF();
            CCTV_Manager.instance.CCTV_04_OFF();
            CCTV_Manager.instance.CCTV_05_OFF();
        }

        //@ CCTV 2번 지역 센서 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "CCTV_Sensor_02")
        {
            CCTV_Manager.instance.CCTV_02_ON();

            CCTV_Manager.instance.CCTV_01_OFF();
            CCTV_Manager.instance.CCTV_03_OFF();
            CCTV_Manager.instance.CCTV_04_OFF();
            CCTV_Manager.instance.CCTV_05_OFF();

        }

        //@ CCTV 3번 지역 센서 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "CCTV_Sensor_03")
        {

            if (_alreadyExistanceFlag)
                return;

            CCTV_Manager.instance.CCTV_03_ON();

            CCTV_Manager.instance.CCTV_01_OFF();
            CCTV_Manager.instance.CCTV_02_OFF();
            CCTV_Manager.instance.CCTV_04_OFF();
            CCTV_Manager.instance.CCTV_05_OFF();

        }

        //@ CCTV 4번 지역 센서 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "CCTV_Sensor_04")
        {
            //% 자유지역 센서가 존재하지 않는다면..
            if (!_alreadyExistanceFlag)
            {
                // NOTE: //# 플레이어가 온길을 다시 돌아갈 때, 카메라 시야에 문제가 생기지 않도록 임시로 자유 지역 센서를 활성화
                StartCoroutine(Generate_OffTheCourse());

            }

            CCTV_Manager.instance.CCTV_04_ON();

            CCTV_Manager.instance.CCTV_01_OFF();
            CCTV_Manager.instance.CCTV_02_OFF();
            CCTV_Manager.instance.CCTV_03_OFF();
            CCTV_Manager.instance.CCTV_05_OFF();
        }

        //@ CCTV 5번 지역 센서 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "CCTV_Sensor_05")
        {


            CCTV_Manager.instance.CCTV_05_ON();

            CCTV_Manager.instance.CCTV_01_OFF();
            CCTV_Manager.instance.CCTV_02_OFF();
            CCTV_Manager.instance.CCTV_03_OFF();
            CCTV_Manager.instance.CCTV_04_OFF();
        }


        //@ 자유 지역 센서 
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.CompareTag("OFF_THE_COURSE"))
        {
            CCTV_Manager.instance.All_CCTV_OFF();

            // if (_ifPlayerTurnWayBack_Flag)
            // {
            //     _ifPlayerTurnWayBack_Flag = false;
            //     _alreadyExistanceFlag = false;

            // }
        }

    }


    //~ --------------------------------------------On trigger Exit---------------------------------
    //@ 임시로 생성된 자유지역 센서 비활성화 
    // NOTE: //# 플레이어가 온길을 다시 돌아갈 때, 카메라 시야에 문제가 생기지 않도록 임시로 자유 지역 센서를 활성화하는데,
    //# 다시 정상적으로 정해진 길을 가게 될 때 (= 최초의 카메라 연출 복원)을 위해 임시로 생성된 자유지역의 센서는 비활성화 되어야 한다. 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.name == "OFF_THE_COURSE_If_Patji_Back")
        {
            //% 2.0초 뒤에 _alreadyExistanceFlag를 false로 초기화하여 다시 CCTV 03번 작동 가능하게 한다. 
            //StartCoroutine(DisableOffTheCourse_Patji_BackWay());
            Invoke("DisableOffTheCourse_Patji_BackWay", 2.0f);
            this.gameObject.SetActive(false);


        }
    }

    //@ 2.0초 뒤에 _alreadyExistanceFlag를 flase로 초기화해 다시 CCTV 03번 작동이 가능하게 하는 함수 
    private void DisableOffTheCourse_Patji_BackWay()
    {
        //yield return new WaitForSeconds(2.0f);

        EventSensorController_DF[] _object_Have_EventSensorController_DF =
                        GameObject.FindObjectsOfType<EventSensorController_DF>();
        for (int i = 0; i < _object_Have_EventSensorController_DF.Length; i++)
        {
            //_object_Have_EventSensorController_DF[i]._ifPlayerTurnWayBack_Flag = false;
            _object_Have_EventSensorController_DF[i]._alreadyExistanceFlag = false;
        }
    }

    IEnumerator Generate_OffTheCourse()
    {
        yield return new WaitForSeconds(1.5f);

        EventSensorController_DF[] _object_Have_EventSensorController_DF =
                    GameObject.FindObjectsOfType<EventSensorController_DF>();
        for (int i = 0; i < _object_Have_EventSensorController_DF.Length; i++)
        {
            _object_Have_EventSensorController_DF[i]._ifPlayerTurnWayBack_Flag = false;
            _object_Have_EventSensorController_DF[i]._alreadyExistanceFlag = true;
        }

        _off_The_Course_If_Patji_Back.SetActive(true);

    }
}
