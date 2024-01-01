using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public Text TimeText;

    private void Start()
    {
        // 게임 매니저에서 측정한 게임 시간을 출력함
        TimeText.text = GameManager.timeCount.ToString();
    }

    public void OnREStartButtonClick()
    {
        // 게임씬으로 이동함
        GameManager.timeCount = 0;
        SceneManager.LoadScene("Game");
    }
}
