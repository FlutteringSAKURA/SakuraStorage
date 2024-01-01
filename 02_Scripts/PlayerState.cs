using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.05

// NOTE: //# 플레이어 상태 스크립트
//#          1) 괴물에게 공격당하면 체력 감소
//#          2) 괴물에게 피격시 사운드 재생 + 사망시 사운드 재생
//#          3) zoom Cam 구현 하기
//#          4) 플레이어 사망시 괴물의 콜라이더 비활성화
//#          5) 랜턴 구현

public class PlayerState : MonoBehaviour
{
    public int playerHp = 100;
    public bool isPlayerAlive;
    public AudioClip playerHittedSound;
    public AudioClip playerDieSound;
    public GameObject zoomCam;
    public bool zoomCamActive;
    public GameObject bloodyCamFX;
    //public GameObject creatureAttackPosCollider;
    //public GameObject gameManagerScript;
    //SoundManagerSrc soundManagerSrc;
    public GameObject flashLantern;
    public bool flashLanternOff;

    public static PlayerState instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //creatureAttackPosCollider = GameObject.FindWithTag("CreatureAttackPos");
        //gameManagerScript =GameObject.Find("GameManager");
        bloodyCamFX.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            zoomCamActive = !zoomCamActive;

            if (zoomCamActive)
            {
                zoomCam.SetActive(true);
                //GameManagerSrc.instance.viewFinderTargetMark.enabled = true;
                GameManagerSrc.instance.targetMark.SetActive(false);
            }
            else
            {
                zoomCam.SetActive(false);
                //GameManagerSrc.instance.viewFinderTargetMark.enabled = false;
                GameManagerSrc.instance.targetMark.SetActive(true);

            }

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLanternOff = !flashLanternOff;
            if(flashLanternOff)
            {
                flashLantern.SetActive(true);
            }
            else
            {
                flashLantern.SetActive(false);
            }
        }
    }

    void GetDamaged(int creatureAttackDam)
    {
        bloodyCamFX.SetActive(true);
        bloodyCamFX.GetComponent<RainCameraController>().Play();

        StartCoroutine(EraseBloodyMark());
        
        playerHp -= creatureAttackDam;
        AudioSource.PlayClipAtPoint(playerHittedSound, transform.position);
        Debug.Log("플레이어의 현재 체력 상태 : " + playerHp);
        //soundManagerSrc = GetComponent<SoundManagerSrc>();

        GameManagerSrc.instance.playerHpbar.value = playerHp;   //% 플레이어 체력과 UI 연결

        if (playerHp <= 0 && isPlayerAlive)
        {
            playerHp = 0;
            isPlayerAlive = false;

            AudioSource.PlayClipAtPoint(playerDieSound, transform.position);
            if (!isPlayerAlive)
            {
                //bloodyCamFX.GetComponent<RainCameraController>().Play();
                //bloodyCamFX.SetActive(true);
                GameManagerSrc.instance.PlayerDie();

                //soundManagerSrc.GameEndSoundPlay();
                SoundManagerSrc.instanceSound.GameEndSoundPlay();
            }
            //GameManagerSrc.instance.SendMessage("addScroe", -100);
            // gameManagerScript.GetComponent<GameManagerSrc>().PlayerDie();

        }

    }

    IEnumerator EraseBloodyMark()
    {
        yield return new WaitForSeconds(0.5f);
        bloodyCamFX.GetComponent<RainCameraController>().Stop();
        yield return new WaitForSeconds(1.3f);
        bloodyCamFX.SetActive(false);
    }
}
