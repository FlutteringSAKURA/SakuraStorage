using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //? UI를 사용하기 위한 코드 >> Text // Image
//using System; //? 순수하게 C# 언어를 사용하기 위해 사용하는 코드

// Update: //@ 2023.09.12
// Update: //@ 2023.09.13
// Update: //@ 2023.09.14


//# NOTE:
//#         1) 곤충이 거미를 먹을때마다 함수를 만들어서 값을 하나씩 증가시키기
//#         2) 게임종료시 게임종료 메뉴(UI) 띄우기
//#         3) 이미지(UI) 삽입하기
//#         4) 스코어(UI) 표시하기

public class StageGameManager : MonoBehaviour
{
    int spiderCount = 0;
    public bool gameEnd = false;
    // Canvas gameCanvas;

    public Text gameEndUI;
    public Text eatCountUI;
    public Text scorePointUI;
    public Image insectImage;

    GameObject canvasPanel;

    // Update: //% 거미를 먹을 때, 스코어 획득
    public int initScore;
    //% ------------------------------------------------------------------------

    private void Start()
    {
        // gameCanvas = GameObject.Find("StageCanvas").GetComponent<Canvas>();
        // gameCanvas.enabled = false;
        // gameEndUI.enabled = false;
        gameEndUI.gameObject.SetActive(false);
        // pointUI.enabled = false;
        eatCountUI.gameObject.SetActive(false);

        scorePointUI.gameObject.SetActive(false);

        //gameEndUI = GameObject.Find("GameEndText");

        canvasPanel = GameObject.Find("CanvasPanel");
        canvasPanel.SetActive(false);

    }
    // Update: //% 거미를 먹을 때, 스코어 획득------------------------------------------
    public void GetScore(int randGetScore)      //? HitSpider 스크립트에서 사건 발생시(충돌) 주입하는 방법
    {
        this.initScore += randGetScore;
        scorePointUI.text = "흡수한 생명력: " + initScore.ToString();
    }
    //% ------------------------------------------------------------------------


    public void AddSpiderCount()
    {
        spiderCount += 1;
        Debug.Log("기괴한 곤충이 잡아먹은 거미의 숫자: " + spiderCount);

        // pointUI.enabled = true;
        eatCountUI.gameObject.SetActive(true);
        scorePointUI.gameObject.SetActive(true);

        // TEMP: //? 질문) 유니티 텍스트 상에 써 놓은 글자 + spiderCount; 방법은 없는지
        eatCountUI.text = "기괴한 곤충이 잡아먹은 거미의 숫자 : " + spiderCount;
        // scorePointUI.text = "흡수한 생명력 : " + initScore;
        /*
        if (spiderCount == 5)
        {
            Debug.Log("곤충의 식사가 끝났습니다.");
        }
        */

        if (spiderCount >= 5)   //? 스크립트의 연산식상 이 조건식이 더 안정적이다.
        {
            gameEnd = true;
            // gameCanvas.enabled = true;
            canvasPanel.SetActive(true);
            // gameEndUI.enabled = true;
            gameEndUI.gameObject.SetActive(true);
            scorePointUI.gameObject.SetActive(true);
            //// pointUI.gameObject.SetActive(true);
            Debug.Log("곤충의 식사가 모두 끝났습니다.");
        }

    }


}
