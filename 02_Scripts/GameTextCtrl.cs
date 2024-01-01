using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.12

// NOTE: //# 2D 게임 - 게임 텍스트 매니저 스크립트 .. 
//#          1) 
//#          2) 
//#          3) 
//#          4) 
//#          5)

public class GameTextCtrl : MonoBehaviour
{
    public static GameTextCtrl instance;
    public GameObject _gameOverText;        //^ 게임오버 패널..
    public GameObject _gameReadyText;       //^ 게임레디 텍스트..

    private void Awake()
    {
        if (GameTextCtrl.instance == null) GameTextCtrl.instance = this;
    }

    private void Start()
    {
        //^ UI 초기 설정
        _gameOverText.SetActive(false);
        _gameReadyText.SetActive(false);

        //& 게임 시작 후 3초간 게임 레디 텍스트 깜빡이기 구현..
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        //! while문 사용법
        while (count < 3)
        {
            _gameReadyText.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _gameReadyText.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            count++;
        }

        //! do while 문 사용법      .. 위와 동일 효과
        do
        {
            count++;
            _gameReadyText.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _gameReadyText.SetActive(false);
            yield return new WaitForSeconds(0.3f);

        } while (count < 3);
    }

    public void ShowGameOver()      //& 플레이어 사망시 게임오버 패널 활성화..(+ 게임오버 텍스트 )
    {
        if(GameManagerCtrl.instance._isGameStart)
        {
            _gameOverText.SetActive(true);
        }
        
    }

    // Update: //@ 2023.10.12

    //# NOTE: 게임 초기화 함수
    //#                     1) 플레이어 위치 초기화
    //#                     2) 인게임상 남아있는 총알 클리어
    //#                     3) 인게임상 남아있는 에너미들 클리어 
    //#                     4) 스코어 초기화
    //#                     5) 게임리스타트 버튼 작동 구현

    public void GameResetAndRestart()
    {
        // TEMP:
        Debug.Log("Game Restart Check!! Wait..Player");
        _gameOverText.SetActive(false);
        
        StartCoroutine(ShowReady());
    }
}
