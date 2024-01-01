using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.30 

// NOTE: //# 3D 게임 - 워터 사운드 제어
//#          1) 플레이어가 물을 밟을 때 나는 소리 구현
//#          2) 
//#          3) 
//#          4) 

public class WaterWalk : MonoBehaviour
{
    GameObject _sakuraObj;
    GameObject _groundWalkSound;
    public bool _waterFlag = false;
    public static WaterWalk instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        _sakuraObj = GameObject.FindWithTag("Player");
        GameObject _groundWalkSound = GameObject.Find("GroundWalkSound");

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag.Contains("Player") && _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.MOVE_WALK)
        {
            _waterFlag = true;

            SoundManager.instance._waterSoundObj.SetActive(true);
            SoundManager.instance._groundSoundObj.SetActive(false);

            Debug.Log("sound water");
        }
        else if (GroundWalk.instance._groundFlag == false && _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.IDLE ||
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.ATTACK ||
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.ATTACKWAIT ||
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.DEAD)
        {
            SoundManager.instance._waterSoundObj.SetActive(false);
            ////_groundWalkSound.SetActive(false);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        _waterFlag = false;
        GroundWalk.instance._groundFlag = true;

        SoundManager.instance._groundSoundObj.SetActive(true);
        SoundManager.instance._waterSoundObj.SetActive(false);
    }
}
