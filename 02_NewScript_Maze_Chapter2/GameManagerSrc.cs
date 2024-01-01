using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 - 자동 미로생성 프로그램 (게임 매니저) 게임 정보 관리
//#          1) 스테이지 클리어 판단
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class GameManagerSrc : MonoBehaviour
{
     //& true가 되면 스테이지가 클리어 된것으로 판단.
    private static bool m_stageClearedFlag = false;     


    //% 스테이지가 클리어 되면 호출
    public static void SetStageClear()      
    {
        //# 스테이지가 클리어 됐음을 표시
        m_stageClearedFlag = true;
    }


    //% 스테이지가 클리어 됐는지 확인

    public static bool IsStageCleared()     
    {
        return m_stageClearedFlag;
    }
}
