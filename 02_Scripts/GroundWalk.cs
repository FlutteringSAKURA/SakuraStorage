using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.30 

// NOTE: //# 3D 게임 - 그라운드 사운드 제어
//#          1) 플레이어가 땅을 밟을 때 나는 소리 구현
//#          2) 
//#          3) 
//#          4) 

public class GroundWalk : MonoBehaviour
{
    public static GroundWalk instance;

    public bool _groundFlag;
    GameObject _sakuraObj;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        _sakuraObj = GameObject.FindWithTag("Player");
        ////_sakuraObj.GetComponent<PlayerFSM>();
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag.Contains("Player") &&
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.MOVE_WALK &&
        WaterWalk.instance._waterFlag == false)
        {
            SoundManager.instance._groundSoundObj.SetActive(true);
            _groundFlag = true;

            Debug.Log("sound Ground");
        }
        else if (WaterWalk.instance._waterFlag || _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.IDLE ||
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.ATTACK ||
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.ATTACKWAIT ||
        _sakuraObj.GetComponent<PlayerFSM>()._currentState == PlayerFSM.SakuraState.DEAD)
        {
            SoundManager.instance._groundSoundObj.SetActive(false);
            _groundFlag = false;
        }


    }

}
