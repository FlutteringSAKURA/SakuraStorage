using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.07 
//# NOTE: SendMessage 관리를 위한 라이트 제어 스크립트
//#       Attatched Objects : Q.3-1 Toilet_Water
//#                           Q.4 Bottles  
//#                           Q_07_Picture

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class SendMessage_Manager : MonoBehaviour
{
    [Header("[ INFO RELATED REFERENCES ]")]
    [TextArea(3, 5)]
    public string _descripttion_About_References;

    [SerializeField]
    [Tooltip("Sweet Home Sweet Quest Manager 스크립트에 있는 함수를 SendMessage로 호출하기 위해 참조")]
    private SweetHomeSweet_Quest_Manager _sweetHomeSweet_Quest_Manager;

    //@ 퀘스트3.1(화장실 물내리는 퀘스트) 완료 후 퀘스트 표시 마크를 비활성화 해주기 위해 보내는 SendMessage 함수 
    public void SendMessage_Q_3_1_ToiletWater_Done()
    {
        _sweetHomeSweet_Quest_Manager.SendMessage("Disable_ToiletWater_Quest_IndicatorMark", SendMessageOptions.DontRequireReceiver);
        ////Debug.Log("편지 보냈어요");
    }

    //@ 퀘스트 4(욕조에 입욕제 넣기 퀘스트) 입욕제를 제대로 선택했는지 확인 후, 결과에 따라 메세지 출력하기 위해 보내는 SendMessage 함수
    public void SendMessage_Q_4_BathWaterBomb_Fail()
    {
        _sweetHomeSweet_Quest_Manager.SendMessage("DisplayWaning_itIsNotBathBomb", SendMessageOptions.DontRequireReceiver);
    }

    public void SendMessage_Q_4_BathWaterBomb_Pass()
    {
        GameControlTower.instance.MouseCursorInActive();
        _sweetHomeSweet_Quest_Manager.SendMessage("ItIsBathBomb", SendMessageOptions.DontRequireReceiver);
    }

    //@ 퀘스트 7(벽걸이 그림 이벤트) 후에 다시 카메라를 원상으로 회복 시켜주기 위해 SendMessage를 호출하는 함수.. 
    public void SendMessage_Q_7_PictureAnimationPlay()
    {
        _sweetHomeSweet_Quest_Manager.SendMessage("ReturnCameraOriginalView_LookBook", SendMessageOptions.DontRequireReceiver);
    }





}
