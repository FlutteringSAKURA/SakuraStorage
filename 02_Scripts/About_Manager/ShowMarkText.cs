using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.05 
//# NOTE: Indicator Text를 보여주기 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class ShowMarkText : MonoBehaviour
{
    [Header("[ REALATED REFERENCE INFO ]")]
    [SerializeField]
    [Tooltip("퀘스트의 리시버의 현재 조건 정보를 얻기 위한 참조")]
    private Qest_Receiver _quest_Receiver;
    [SerializeField]
    [Tooltip("스윗홈스윗 퀘스트 매니저의 현재 조건 정보를 얻기 위한 참조")]
    private SweetHomeSweet_Quest_Manager _sweetHomeSweet_Quest_Manager;

    //# 화장실 문 닫기 열기 관련 텍스트
    public GameObject _markText_O;
    public GameObject _markText_C;

    //# 변기 커버 올리기 내리기 관련 텍스트
    public GameObject _toiletCap_Text_P;
    public GameObject _toiletCap_Text_R;

    //# 변기 물 내리기 관련 텍스트
    public GameObject _toiletWater_Text_K;
    public GameObject _lightOnMark_Text;

    //# 입욕제 집기 관련 텍스트
    //! public GameObject _pickUp_Bottle_Text_G;
    [Tooltip("입욕제 넣기 퀘스트(4) - 선택할 입욕제 병들의 넘버 UI를 보여주기 위한 참조")]
    public GameObject[] _bathBottleNuberText_Group;

    //# 열쇠찾기 관련 텍스트
    [Tooltip("열쇠찾기 퀘스트(5) - 열쇠찾는 과정에서 수건을 클릭하라는 UI를 보여주기 위한 참조")]
    public GameObject _clickText_On_Mark_Towel;

    [Tooltip("열쇠찾기 퀘스트(5) - 열쇠찾는 과정에서 열쇠를 클릭하라는 UI를 보여주기 위한 참조")]
    public GameObject _clickText_On_Mark_Key;

    [Tooltip("열쇠찾기 퀘스트(5) - 열쇠찾는 과정에서 수건을 떨어뜨릴 버튼 UI를 활성화 위한 참조")]
    public GameObject _gravity_button;
    [Tooltip("열쇠찾기 퀘스트(5) - 열쇠찾는 과정에서 열쇠를 얻기위해 확대하는 버튼 UI를 활성화 위한 참조")]
    public GameObject _getKey_button;

    public static ShowMarkText instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    private void OnTriggerEnter(Collider other)
    {
        //% Indicator_ Text 활성화 / 비활성화 관련 코드
        if (_sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_1_OPEN_THE_DOOR
        || _sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_2_TURN_ON_LIGHT)
        {
            if (other.gameObject.tag.Contains("PatJi") && !_quest_Receiver._doorOpen_CheckFlag)
            {
                _markText_O.SetActive(true);
                _markText_C.SetActive(false);
            }
            else if (other.gameObject.tag.Contains("PatJi") && _quest_Receiver._doorOpen_CheckFlag)
            {
                _markText_O.SetActive(false);
                _markText_C.SetActive(true);
            }
        }

        //% 퀘스트 3이 아닐 때만 작동한다. (즉, 퀘스트 2가 진행중일 때만 표시 되도록)
        Light_Off _lightOff_Script;
        _lightOff_Script = FindObjectOfType<Light_Off>();

        if (_sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_2_TURN_ON_LIGHT
                && !_sweetHomeSweet_Quest_Manager._quest_3_Ready && !_lightOff_Script._q_02_LightBottonPushed)
        {
            _lightOnMark_Text.SetActive(true);
        }

        if (_sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_3_OPEN_CAP_AND_CLEAN)
        {
            if (!_quest_Receiver._toiletCap_LiftUp_CheckFlag)
            {
                _toiletCap_Text_P.SetActive(true);
                _toiletCap_Text_R.SetActive(false);
            }
            else if (_quest_Receiver._toiletCap_LiftUp_CheckFlag)
            {
                _toiletCap_Text_P.SetActive(false);
                _toiletCap_Text_R.SetActive(true);
            }

        }

        if (_sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_3_1_OPEN_CAP_AND_CLEAN)
        {
            //if(!_quest_Receiver.to)
            _toiletWater_Text_K.SetActive(true);
        }

        if (_sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_4_BATH_WATER)
        {
            if (!_quest_Receiver._grab_Bottle)
            {
                //! _pickUp_Bottle_Text_G.SetActive(true);

                for (int i = 0; i < _bathBottleNuberText_Group.Length; i++)
                {
                    _bathBottleNuberText_Group[i].SetActive(true);
                }
            }
            else if (_quest_Receiver._grab_Bottle)
            {
                //! _pickUp_Bottle_Text_G.SetActive(false);

                for (int i = 0; i < _bathBottleNuberText_Group.Length; i++)
                {
                    _bathBottleNuberText_Group[i].SetActive(false);
                }
            }

        }

        if (_sweetHomeSweet_Quest_Manager._currentQuest == SweetHomeSweet_Quest_Manager.Quest_Name.Q_5_FIND_KEY)
        {
            if (_sweetHomeSweet_Quest_Manager._readyToGetKeyFlag)
                return;

            _clickText_On_Mark_Towel.SetActive(true);
            _gravity_button.SetActive(true);
            Debug.Log("click Text Enable");
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("PatJi"))
        {
            _markText_O.SetActive(false);
            _markText_C.SetActive(false);

            _lightOnMark_Text.SetActive(false);

            _toiletCap_Text_P.SetActive(false);
            _toiletCap_Text_R.SetActive(false);

            _toiletWater_Text_K.SetActive(false);

            for (int i = 0; i < _bathBottleNuberText_Group.Length; i++)
            {
                _bathBottleNuberText_Group[i].SetActive(false);
            }
        }
    }


    public void Esc_Toilet()
    {
        _toiletCap_Text_P.SetActive(false);
        _toiletCap_Text_R.SetActive(false);

        _toiletWater_Text_K.SetActive(false);
    }

    public void BathBottleNumberTextDisable()
    {
        for (int i = 0; i < _bathBottleNuberText_Group.Length; i++)
        {
            _bathBottleNuberText_Group[i].SetActive(false);
        }
    }

    public void BathBottleNumberTextEnable()
    {
        for (int i = 0; i < _bathBottleNuberText_Group.Length; i++)
        {
            _bathBottleNuberText_Group[i].SetActive(true);
        }
    }

    public void ClickText_On_Mark_Towel_Q5_FindKey_Disable()
    {
        _clickText_On_Mark_Towel.SetActive(false);
        _gravity_button.SetActive(false);
    }

    public void ClickText_On_Mark_Key_Q5_FindKey_Disable()
    {
        _clickText_On_Mark_Key.SetActive(false);
    }

    public void ClickText_On_Mark_Key_Q5_FindKey_Enable()
    {
        _clickText_On_Mark_Key.SetActive(true);
        _getKey_button.SetActive(true);
    }
}
