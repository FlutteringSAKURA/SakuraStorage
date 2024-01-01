using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update: //@ 2023.09.27

// NOTE: //#   1) GameOver 문구 출력 / 0.5초마다 게임오버텍스트 3회 깜빡깜빡(while)
//#            2) 
//#            3) 
//#            4) 
//#            5) 

public class GameTextScript : MonoBehaviour
{
    public GameObject gameOverText;     //^ 게임종료시 출현
    public static GameTextScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameOverText.SetActive(false);
    }

    // Update: //@ 2023.09.27   
    //# 0.5초 간격으로 GameOver Text 3회 깜빡깜빡
    public void GameOver()
    {
        StartCoroutine(gameOverTxt());
    }

    IEnumerator gameOverTxt()
    {
        int countNum = 0;  
        while (countNum < 3)
        {
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);

            gameOverText.SetActive(true);
           //// yield return new WaitForSeconds(0.5f);
            countNum++;     //! 무한루프 방지용 코드 
        }
    }
}
