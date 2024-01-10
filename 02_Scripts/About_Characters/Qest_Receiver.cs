using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.05 
// Update: //@ 2023.12.07 
// Update: //@ 2023.12.11 
//# NOTE: 팥쥐가 진행하는 퀘스트를 수신하기 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class Qest_Receiver : MonoBehaviour
{
    [Header("[ REALATED REFERENCE INFO ]")]
    [SerializeField]
    [Tooltip("퀘스트 화장실 문")]
    private GameObject _q_1_eventDoor;

    [SerializeField]
    [Tooltip("퀘스트 변기 커버")]
    private GameObject _q_3_toiletCap;

    [SerializeField]
    [Tooltip("퀘스트 변기물 내리기")]
    private GameObject _q_3_1_toiletWater;

    [SerializeField]
    [Tooltip("퀘스트 변기물 내리는 행동 딜레이 처리를 위한 참조")]
    private SweetHomeSweet_Quest_Manager _q_3_1_toiletWater_Script;

    [SerializeField]
    [Tooltip("퀘스트 욕조에 입욕제 타기 입욕제 번호별 Look At하기를 처리를 위한 참조")]
    private SweetHomeSweet_Quest_Manager _q_4_BathBombWater_Script;

    [SerializeField]
    [Tooltip("퀘스트4(입욕제 욕조에 넣기) 입욕제 넣기위한 입욕제 오브젝트 그룹 참조")]
    private GameObject[] _q_04_BathBomb_Bottle_Group;

    [Space(10f)]

    [SerializeField]
    [Tooltip("퀘스트4(입욕제 욕조에 넣기) 입욕제 원래 제자리로 가져다 놓기 위한 좌표 그룹 참조")]
    private Transform[] _q_04_BathBomb_Bottle_Transform_Group;

    [SerializeField]
    [Tooltip("퀘스트4(입욕제 욕조에 넣기) 입욕제 선택시 보기 위한 입욕제의 부모 좌표위치")]
    private Transform _lookAtTransform;

    [SerializeField]
    [Tooltip("퀘스트5(열쇠 찾기) 수건이 떨어지면 열쇠가 빛나게 하기 위한 메터리얼 참조")]
    Material _emissions;

    [SerializeField]
    [Tooltip("퀘스트5(열쇠 찾기) 열쇠를 클릭하면 확대보기 효과를 주기 위한 참조")]
    GameObject _key;

    [SerializeField]
    [Tooltip("퀘스트5(열쇠 찾기) 열쇠를 클릭하면 확대보기 효과를 주기 위한 위치 좌표 참조")]
    Transform _lookAtPos_Key;



    [SerializeField]
    [Tooltip("지시사항의 텍스트에 따른 키가 눌리면 해당 텍스트 UI 사라지게 하기 위해 Quest Sensor Group 참조..")]
    private ShowMarkText _q_01_openBathRoomDoor_showMarkText;

    [SerializeField]
    [Tooltip("지시사항의 텍스트에 따른 키가 눌리면 해당 텍스트 UI 사라지게 하기 위해 Quest Sensor Group 참조..")]
    private ShowMarkText _q_03_liftUpToiletCap_showMarkText;

    [SerializeField]
    [Tooltip("지시사항의 텍스트에 따른 키가 눌리면 해당 텍스트 UI 사라지게 하기 위해 Quest Sensor Group 참조..")]
    private ShowMarkText _q_03_1_toiletWater_showMarkText;


    [Header("[ REALATED BOOL CONDITIONS INFO ]")]
    [Tooltip("이미 열리거나 닫힌 문이 다시 열리거나 닫히지 않도록 하는 조건")]
    public bool _doorOpen_CheckFlag = false;

    [Tooltip("이미 눌린 라이트 버튼이 다시 눌리지 않도록 하는 조건")]
    public bool _toiletCap_LiftUp_CheckFlag = false;

    [Tooltip("이미 넣은 입욕제를 다시 넣지 않게 하는 조건")]
    public bool _grab_Bottle = false;

    [Tooltip("퀘스트5(열쇠찾기)에서 열쇠를 확대보기 하면서 얻을 경우, 매터리얼 에미션이 계속 코루틴 도는 것을 방지하기 위한 조건")]
    public bool _closeUpKeyFlag = false;



    //~ -------------------------------------------------------------------------------
    private void Start()
    {

    }

    //~ -------------------------------------------------------------------------------
    private void Update()
    {

    }

    //~ ----- Quest 01 --- Light On ----------------------------------------------------------------
    public void Open_BathDoor_HomeSweetHome()
    {
        if (_doorOpen_CheckFlag)
            return;

        _q_1_eventDoor.GetComponent<Animator>().SetTrigger("Open");
        _doorOpen_CheckFlag = true;
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_01_OPENTHEDOOR);

        _q_01_openBathRoomDoor_showMarkText._markText_O.SetActive(false);
        _q_01_openBathRoomDoor_showMarkText._markText_C.SetActive(true);

        ////Debug.Log("OPEN DOOR");
    }

    public void Close_BathDoor_HomeSweetHome()
    {
        if (!_doorOpen_CheckFlag)
            return;

        _q_1_eventDoor.GetComponent<Animator>().SetTrigger("Close");
        _doorOpen_CheckFlag = false;
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_01_CLOSETHEDOOR);

        _q_01_openBathRoomDoor_showMarkText._markText_O.SetActive(true);
        _q_01_openBathRoomDoor_showMarkText._markText_C.SetActive(false);

        ////Debug.Log("CLOSE DOOR");
    }

    //~ ----- Quest 03 ---- Toilet Cap -------------------------------------------------------------
    public void LiftUp_Toilet_Cap()
    {
        if (_toiletCap_LiftUp_CheckFlag)
            return;

        _q_3_toiletCap.GetComponent<Animator>().SetTrigger("LiftUp");
        _toiletCap_LiftUp_CheckFlag = true;

        _q_03_liftUpToiletCap_showMarkText._toiletCap_Text_P.SetActive(false);
        _q_03_liftUpToiletCap_showMarkText._toiletCap_Text_R.SetActive(true);

    }

    public void LiftDown_Toilet_Cap()
    {
        if (!_toiletCap_LiftUp_CheckFlag)
            return;

        _q_3_toiletCap.GetComponent<Animator>().SetTrigger("LiftDown");
        _toiletCap_LiftUp_CheckFlag = false;

        _q_03_liftUpToiletCap_showMarkText._toiletCap_Text_P.SetActive(true);
        _q_03_liftUpToiletCap_showMarkText._toiletCap_Text_R.SetActive(false);

    }

    //~ ----- Quest 03.1 -----Toilet Clean-----------------------------------------------------------
    public void Toilet_Water_Refill()
    {
        _q_3_1_toiletWater.GetComponent<Animator>().SetTrigger("Refill_ToiletWater");

        _q_3_1_toiletWater_Script._timeFlow = 0.0f;

        ////Debug.Log("화장실 변기의 물이 내려갑니다.");
    }

    //~ ----- Quest 04 -----Input Bath Bomb To Bath Water -----------------------------------------------------------
    public void LookAtBathBomb()
    {
        if (_q_4_BathBombWater_Script._bathBomb_Number01)
        {
            ////Debug.Log("팥쥐가 1번 입욕제를 집어 들었습니다.");

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 5].transform.parent
                = _lookAtTransform;

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 5].transform.position
                = new Vector3(_lookAtTransform.position.x, _lookAtTransform.position.y - 0.15f, _lookAtTransform.transform.position.z);

            _lookAtTransform.rotation = Quaternion.Euler(40f, 0, -28f);

        }

        else if (_q_4_BathBombWater_Script._bathBomb_Number02)
        {
            ////Debug.Log("팥쥐가 2번 입욕제를 집어 들었습니다.");

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 4].transform.parent
                = _lookAtTransform;

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 4].transform.position
                = new Vector3(_lookAtTransform.position.x, _lookAtTransform.position.y - 0.15f, _lookAtTransform.transform.position.z);

            _lookAtTransform.rotation = Quaternion.Euler(40f, 0, -28f);
        }

        else if (_q_4_BathBombWater_Script._bathBomb_Number03)
        {
            ////Debug.Log("팥쥐가 3번 입욕제를 집어 들었습니다.");

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].transform.parent
                = _lookAtTransform;

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].transform.position
                = new Vector3(_lookAtTransform.position.x, _lookAtTransform.position.y - 0.15f, _lookAtTransform.transform.position.z);

            _lookAtTransform.rotation = Quaternion.Euler(40f, 0, -28f);
        }

        else if (_q_4_BathBombWater_Script._bathBomb_Number04)
        {
            ////Debug.Log("팥쥐가 4번 입욕제를 집어 들었습니다.");

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 2].transform.parent
                = _lookAtTransform;

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 2].transform.position
                = new Vector3(_lookAtTransform.position.x, _lookAtTransform.position.y - 0.15f, _lookAtTransform.transform.position.z);

            _lookAtTransform.rotation = Quaternion.Euler(40f, 0, -28f);


        }
        else if (_q_4_BathBombWater_Script._bathBomb_Number05)
        {
            ////Debug.Log("팥쥐가 5번 입욕제를 집어 들었습니다.");
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 1].transform.parent
                = _lookAtTransform;

            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 1].transform.position
                = new Vector3(_lookAtTransform.position.x, _lookAtTransform.position.y - 0.15f, _lookAtTransform.transform.position.z);

            _lookAtTransform.rotation = Quaternion.Euler(40f, 0, -28f);

        }

    }

    public void ReturnBathBombObjectOriginalTransform()
    {
        _lookAtTransform.rotation = Quaternion.Euler(40f, 0, -28f);

        if (_q_4_BathBombWater_Script._bathBomb_Number01)
        {
            ////Debug.Log("팥쥐가 1번 입욕제를 제자리에 돌려 놓았습니다.");
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 5].transform.parent
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 5].transform;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 5].transform.position
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 5].transform.position;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 5].transform.rotation
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 5].transform.rotation;
        }
        else if (_q_4_BathBombWater_Script._bathBomb_Number02)
        {
            ////Debug.Log("팥쥐가 2번 입욕제를 제자리에 돌려 놓았습니다.");
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 4].transform.parent
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 4].transform;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 4].transform.position
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 4].transform.position;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 4].transform.rotation
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 4].transform.rotation;
        }

        else if (_q_4_BathBombWater_Script._bathBomb_Number03)
        {
            ////Debug.Log("팥쥐가 3번 입욕제를 제자리에 돌려 놓았습니다.");
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].transform.parent
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 3].transform;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].transform.position
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 3].transform.position;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].transform.rotation
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 3].transform.rotation;

        }
        else if (_q_4_BathBombWater_Script._bathBomb_Number04)
        {
            ////Debug.Log("팥쥐가 4번 입욕제를 제자리에 돌려 놓았습니다.");
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 2].transform.parent
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 2].transform;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 2].transform.position
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 2].transform.position;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 2].transform.rotation
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 2].transform.rotation;

        }
        else if (_q_4_BathBombWater_Script._bathBomb_Number05)
        {
            ////Debug.Log("팥쥐가 5번 입욕제를 제자리에 돌려 놓았습니다.");
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 1].transform.parent
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 1].transform;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 1].transform.position
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 1].transform.position;
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 1].transform.rotation
                = _q_04_BathBomb_Bottle_Transform_Group[_q_04_BathBomb_Bottle_Transform_Group.Length - 1].transform.rotation;

        }
    }



    //@ 욕조 물 색깔 변화 시키는 함수 
    //@ SweetHomeSweet_Quest_Manager의 Completed_Quest_04_BathWater 코루틴 함수에서 호출함.
    public void BathWaterColorChanged()
    {
        GameObject _bathWater;
        _bathWater = GameObject.Find("Bath_Water");

        _bathWater.GetComponent<Animator>().SetBool("BathBomb", true);
    }

    //@ 퀘스트 4(욕조에 입욕제 넣기 퀘스트) 입욕제 넣는 애니메이션 연출시 잠시 병을 비활성화 하는 함수 .. 
    //@ SweetHomeSweet_Quest_Manager의 Completed_Quest_04_BathWater 코루틴 함수에서 호출함.
    public void TempDisablePurpleBottle()
    {

        if (_q_4_BathBombWater_Script._bathWaterPassZoominCameraFlag)
        {
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].SetActive(false);
        }
        else
        {
            _q_04_BathBomb_Bottle_Group[_q_04_BathBomb_Bottle_Group.Length - 3].SetActive(true);
        }
    }

    //@ 퀘스트 5(열쇠찾기) 수건 떨어질 때, 열쇠가 빛나는 효과를 주는 함수
    public void EmissionEffectKey()
    {
        StartCoroutine(FlickerKey());
    }

    IEnumerator FlickerKey()
    {
        if (!_closeUpKeyFlag)
        {
            _emissions.SetColor("_EmissionColor", Color.yellow);
            if (_closeUpKeyFlag)
            {
                _emissions.SetColor("_EmissionColor", Color.black);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);

            _emissions.SetColor("_EmissionColor", Color.black);
            yield return new WaitForSeconds(0.5f);
            if (_closeUpKeyFlag)
                yield break;

            _emissions.SetColor("_EmissionColor", Color.yellow);
            if (_closeUpKeyFlag)
            {
                _emissions.SetColor("_EmissionColor", Color.black);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);

            _emissions.SetColor("_EmissionColor", Color.black);

            yield return new WaitForSeconds(0.5f);
            _emissions.SetColor("_EmissionColor", Color.yellow);
            if (_closeUpKeyFlag)
            {
                _emissions.SetColor("_EmissionColor", Color.black);
                yield break;
            }
            yield return new WaitForSeconds(0.5f);

            _emissions.SetColor("_EmissionColor", Color.black);

        }

    }

    //@ 열쇠를 클릭하면 확대보기 실행되는 함수 
    public void LookAtKeyAndGetKey()
    {
        _closeUpKeyFlag = true;

        _emissions.SetColor("_EmissionColor", Color.black);

        _key.transform.parent = _lookAtPos_Key.transform;
        _key.transform.position = _lookAtPos_Key.transform.position;
        _key.transform.rotation = _lookAtPos_Key.transform.rotation;
    }

    //@ 열쇠 비활성화 
    public void DisableKey()
    {
        _key.SetActive(false);
    }

    //@ Quest 7 (벽에 걸린 그림 애니메이션(비뚤어지기)) 호출받아 작동하는 함수 
    public void PictureAnimationPlay()
    {
        GameObject _q_07_Picture;
        _q_07_Picture = GameObject.Find("Q_07_Picture");
        _q_07_Picture.GetComponent<Animator>().SetTrigger("PictureAction");

        //% 퀘스트 6 (드레스룸의 책보기) 완료

        SweetHomeSweet_Quest_Manager[] _object_Have_SweetHomeSwett_Quest_Manager =
        GameObject.FindObjectsOfType<SweetHomeSweet_Quest_Manager>();
        for (int i = 0; i < _object_Have_SweetHomeSwett_Quest_Manager.Length; i++)
        {
            _object_Have_SweetHomeSwett_Quest_Manager[i]._quest_6_Completed = true;
            _object_Have_SweetHomeSwett_Quest_Manager[i]._lookBookDevilVoiceFinished = true;
        }

    }





}
