using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

// Update: //@ 2023.10.02
// Update: //@ 2023.10.05

// NOTE: //# 사운드 매니징 스크립트 
//#          1) 게임 종료시 사운드 구현
//#          2) 배경음악 사운드 구현
//#          3) 
//#          4) 

public class SoundManagerSrc : MonoBehaviour
{
    public static SoundManagerSrc instanceSound;
    // public AudioClip fireSound;
    public AudioClip gameEndSound;
    public AudioClip bgmSound;

    AudioSource audioBox;

    private void Awake()
    {
        if (instanceSound == null)
        {
            instanceSound = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        audioBox = GetComponent<AudioSource>();
        BgmSoundPlay();
        
    }


    public void GameEndSoundPlay()
    {
        audioBox.PlayOneShot(gameEndSound);
        Debug.Log("게임오버음악재생");
    }

    public void BgmSoundPlay()
    {
        audioBox.loop = true;
        audioBox.PlayOneShot(bgmSound);
    }
}
