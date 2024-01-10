using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Update: //@ 2023.11.21 
// Update: //@ 2023.11.23 
//# NOTE: Game Scenes 관리를 위한 스크립트


//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class GameSceneManager : MonoBehaviour
{
    public GameObject _mainPanel;
    public GameObject _introCreditVideo;
    public bool _introCreditFlag = false;

    private void Awake()
    {
        _introCreditVideo.SetActive(false);
        _introCreditFlag = false;

    }


    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {

            if (_introCreditFlag)
            {
                _introCreditFlag = false;

                _mainPanel.SetActive(true);

                RewindTape();

            }

        }

    }

    private void RewindTape()
    {
        VideoController.instance.RestartTape();
        _introCreditVideo.SetActive(false);
    }

    public void GameStartBtn()
    {
        SceneManager.LoadScene("02_Bramble_The_Kong_And_Pat_Game");
    }
    public void ChapterSelectionBtn()
    {

    }

    public void ProductionBtn()
    {
        _introCreditFlag = true;
        _introCreditVideo.SetActive(true);

        VideoController.instance.PlayTape();


    }

    public void ExitGameBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("게임을 종료합니다.");
#else
        
            Application.Quit();
        
#endif

    }
}
