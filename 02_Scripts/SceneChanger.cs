using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Update: //@ 2023.10.12

// NOTE: //# 2D 게임 - 씬전환 스크립트 (TEST)
//#          1) 씬 전환 (게임 화면)
//#          2) 


// TEST: 
// TEMP: //& 사용하지는 않는 스크립트

public class SceneChanger : MonoBehaviour
{
    public void ChangeSceneGame()
    {
        SceneManager.LoadScene("불러올 씬 명칭");
    }
}
