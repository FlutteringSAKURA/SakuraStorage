using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Update: //@ 2023.12.03 
// Update: //@ 2023.12.13 
// Update: //@ 2023.12.15 

//# NOTE: Game의 Load를 관리하기 위한 스크립트


//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class LoadGameManager : MonoBehaviour
{
    [Header("Game Control Tower Reference")]
    [SerializeField]
    private GameControlTower _gameControlTower;


    [Header("LOAD WATING LIST")]
    [SerializeField]
    private GameObject _homeSweetHome;

    [SerializeField]
    private GameObject _patJi;
    [SerializeField]
    private GameObject _dialogue_Canvas;

    [SerializeField]
    private GameObject _timeLine;

    [SerializeField]
    private GameObject _loadGame_Canvas;

    [SerializeField]
    private GameObject _sweetHomeSweetGoOut_Timeline;

    private void Start()
    {

        if (!_gameControlTower._homeSweetHomeClear)
            StartCoroutine(LoadGameScene());

        if (_gameControlTower._homeSweetHomeClear)
            StartCoroutine(LoadGameScene_DarkForest());
    }

    //@ HomeSweetHome
    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(3.5f);
        _homeSweetHome.SetActive(true);
        _patJi.SetActive(true);
        _dialogue_Canvas.SetActive(true);
        _timeLine.SetActive(true);

        _loadGame_Canvas.SetActive(false);
    }

    public void GoOut_HomeSweetHome()
    {
        SceneManager.LoadScene("03_Bramble_The_Kong_And_Pat_Game_DarkForest");
        Light_Off.instance.ReturnMaterialEmission();
    }

    //@ Dark Forest
    IEnumerator LoadGameScene_DarkForest()
    {
        //% 게임씬 로드가 될 동안 캐릭터가 회전하거나 움직이는 것을 막기 위한 방어 코드
        EventSensorController_DF.instance._eventActiveCheckFlag = true;

        yield return new WaitForSeconds(4.5f);
        _loadGame_Canvas.SetActive(false);

        _gameControlTower._timelineActiveFlag = false;
        _patJi.SetActive(true);
        _patJi.GetComponent<Rigidbody>().isKinematic = false;

        Debug.Log("어두운 숲의 기운이 느껴집니다.");


    }
}
