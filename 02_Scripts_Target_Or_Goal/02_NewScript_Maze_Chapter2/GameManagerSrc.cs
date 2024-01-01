using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Update: //@ 2023.10.13 
// Update: //@ 2023.10.17 

// NOTE: //# 3D 게임 - 자동 미로생성 프로그램 (게임 매니저) 게임 정보 관리
//#          1) 스테이지 클리어 판단    // Completed:
//#          2) 다음 스테이지 이동      // Completed:
//?          3) 제한시간 타이머 구현 .. 타이머 다되면 게임 오버..  구현 후에 씬 Stage2로 연결 새로하기 + 캐릭터 미끄러지는 현상 해결하기
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class GameManagerSrc : MonoBehaviour
{
    //& true가 되면 스테이지가 클리어 된것으로 판단.
    private static bool m_stageClearedFlag = false;
    public static GameObject _stageClearPanel;
    public static GameObject _timerPanel;
    public GameObject _playerObj;

    public float _timeFlow = 0.0f;

//~ ---------------------------------------------------------
    private void Start()
    {
        _stageClearPanel = GameObject.Find("StageClearPanel");
        //_timerPanel = GameObject.Find("TimerPanel");
        //_timerPanel.GetComponent<Text>();
        _stageClearPanel.SetActive(false);
        _playerObj = GameObject.Find("UnityChan");
        _playerObj.SetActive(false);
        
    }
//~ ---------------------------------------------------------
    private void Update()
    {
        // _timeFlow += Time.deltaTime;
        // _timerPanel.GetComponent<Text>().text = _timerPanel.ToString();
        
    }
//~ ---------------------------------------------------------

    //@ 스테이지가 클리어 되면 호출(1)
    public static void SetStageClear()
    {


        //# 스테이지가 클리어 됐음을 표시
        m_stageClearedFlag = true;
        
        //# 스테이지 클리어 UI 표시
        _stageClearPanel.SetActive(true);
        Debug.Log("클리어 : " + m_stageClearedFlag);

    }
    //@ 스테이지 클리어 되면 호출(2)
    public static void MovetoNextStage()
    {   
        //# UI 셋팅
         //# 다음 스테이지 이동
        SceneManager.LoadScene("Stage02");
        
    }


    //@ 스테이지가 클리어 됐는지 확인
    public static bool IsStageCleared()
    {
        return m_stageClearedFlag;
    }
}
