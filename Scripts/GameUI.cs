using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI사용

public class GameUI : MonoBehaviour
{
    public Text txtScore;
    private int totScore = 0;

    private void Start()
    {
        // [ 알파 코드 1 ]
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        // =============================================== 알파 코드 END
        DispScore(0); 
        
    }

    public void DispScore(int score)
    {
        totScore += score;

        //txtScore.text = "SCORE <color=Red>" + totScore.ToString() + "</color>";
        //txtScore.text = "SCORE <color=Red>" + totScore.ToString("d2") + "</color>";
        //txtScore.text = "SCORE <color=Red>" + string.Format("{0:n0}", totScore) + "</color>";
        //txtScore.text = "SCORE <color=Red>" + string.Format("{0}", totScore.ToString("n0")) + "</color>";
        //txtScore.text = "SCORE <color=Red>" + string.Format("{0:#,##0", totScore) + "</color>";
        txtScore.text = "SCORE <color=Red>" + string.Format("{0}", totScore.ToString("#,##0")) + "</color>";

        

        // [ 알파 코드 2 ]
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
        // =========================================== END
    }

    // [ 베타 코드 ] -> 초기값 설정
    public void OnClickResetBtn()
    { 
        totScore = 0;
        PlayerPrefs.GetInt("TOT_SCORE", 0);
        DispScore(0);
    }
}
