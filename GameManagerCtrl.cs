using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 제어 스크립트
//#          1) 스테이지 클리어시 스테이지 클리어 UI 구현
//#          2) 씬매니저로 다음 스테이지 또는 게임 오버 화면 구현
//#          3) 
//#          4) 
//#          5)  

//~ ---------------------------------------------------------
public class GameManagerCtrl : MonoBehaviour
{
    //List<GameObject> _pointCoinBox = new List<GameObject>();
    //GameObject _pointCoin;

    public GameObject _stageClear;
    public bool _isStageClear = false;

    public static GameManagerCtrl instance;

    private void Awake()
    {
        if (GameManagerCtrl.instance == null)
        {
            GameManagerCtrl.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

//~ ---------------------------------------------------------
    private void Start()
    {
        //^ UI 초기 셋팅
        _stageClear.SetActive(false);
    }

//~ ---------------------------------------------------------
    public void StageClear()    //^ 스테이지 클리어
    {
        _stageClear.SetActive(true);
        SoundManagerCtrl.instance.StageClearSoundPlay();

        Debug.Log("StageClear");
    }


    public void GameOver()      //^ 게임오버
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver");
    }
    public void RestrartGame()      //^ 게임 리스타트
    {
        SceneManager.LoadScene("UnityChan_Maze_Stage1");
    }

    public void NextStage()     //^ 다음 스테이지
    {
        SceneManager.LoadScene("UnityChan_Maze_Stage2");
    }




}
