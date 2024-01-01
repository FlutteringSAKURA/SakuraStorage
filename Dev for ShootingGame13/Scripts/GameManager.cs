using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 사용
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text timeText;
    public static int timeCount = 0;

    private void Start()
    {
        // string -> int 변경하려면 int.Parase 메소드를 사용함
        timeText.text = timeCount.ToString();

        InvokeRepeating("GameTimer", 0, 1);
        // int -> string 변경 하려면 ToString 메소드를 사용함
    }

    private void GameTimer()
    {
        timeCount = int.Parse(timeText.text);
        timeCount++;

        // int -> string 변경하려면  ToString 메소드를 사용함
        timeText.text = timeCount.ToString();
    }
}
