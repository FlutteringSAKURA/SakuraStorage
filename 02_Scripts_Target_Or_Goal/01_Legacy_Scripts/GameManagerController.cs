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
public class GameManagerController : MonoBehaviour
{
    //List<GameObject> _pointCoinBox = new List<GameObject>();
    //GameObject _pointCoin;

    public GameObject _stageClear;
    public bool _isStageClear = false;

    public static GameManagerController instance;

    private void Awake()
    {
        if (GameManagerController.instance == null)
        {
            GameManagerController.instance = this;
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
//@ 스테이지 클리어
    public void StageClear()    
    {
        _stageClear.SetActive(true);
        SoundManagerController.instance.StageClearSoundPlay();

        Debug.Log("StageClear");
    }

//@ 게임오버
    public void GameOver()      
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver");
    }

//@ 게임 리스타트
    public void RestrartGame()      
    {
        SceneManager.LoadScene("UnityChan_Maze_Stage1");
    }
//@ 다음 스테이지
    public void NextStage()     
    {
        SceneManager.LoadScene("UnityChan_Maze_Stage2");
    }




}
