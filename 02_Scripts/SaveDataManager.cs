using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveDataManager : MonoBehaviour
{

    [Header("[ UI INFO ]")]
    public TMP_Text _scoreText;
    float _score = 0;

    public TMP_Text _killCountText;
    int _killCount = 0;

    public void DisplaySocre(float getScore)
    {
        _score += getScore;
        _scoreText.text =
        string.Format($"<color=#00ff00>SCORE: </color><color=yellow>{_score:#,##0}</color>");
        // NOTE:
        // {12345:#,##0} ===> 12,345
        // {123:#,##0} ===> 123
        // {123:0,000} ===> 0,123
        // {123:0000} ===> 0123

        //% 스코어 저장
        PlayerPrefs.SetFloat("SCORE", _score);
    }

    public void AddKillCount(int kill_Number)
    {
        _killCount += kill_Number;
        _killCountText.text = string.Format($"<mark=yellow><color=red> kill count :   </mark><color=white>{_killCount:#,##0}");

        //% 킬카운트 저장
        PlayerPrefs.SetInt("KILLCOUNT", _killCount);
    }
}
