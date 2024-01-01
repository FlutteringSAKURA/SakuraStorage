using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.05

// NOTE: //# 괴물 데미지 스크립트
//#          1) 괴물의 공격에 플레이어 데미지 입기 구현

public class CreatureAttackManager : MonoBehaviour
{
    GameObject playerObj;
    GameObject soundManager;
    GameObject mainCam;
    public GameObject gameManager;

    private void Start() {
        playerObj = GameObject.FindWithTag("Player");
        // mainCam = GameObject.FindWithTag("MainCamera");
        // gameManager = GameObject.Find("GameManager");
        // soundManager = GameObject.Find("SoundManager");
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag.Contains("Player"))
        {
            playerObj.SendMessage("GetDamaged", 10);
            ////mainCam.SendMessage("Hitted");
            Debug.Log("플레이어가 괴물에게 공격당하고 있습니다.");

        }
    }
}
