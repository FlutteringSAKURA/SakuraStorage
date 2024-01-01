using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 사운드 제어 스크립트
//#          1) 포인트 구슬 먹을 때 사운드 재생
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class SoundManagerController : MonoBehaviour
{
    public static SoundManagerController instance;

    AudioSource _AudioBox;

    public AudioClip _getCoinSound;
    public AudioClip _stageClearSound;
    public AudioClip _screamCreatureSound;

//~ ---------------------------------------------------------
    private void Awake() {
        if (SoundManagerController.instance == null)
        {
            SoundManagerController.instance = this;
        }
        else
        {
          //   Destroy(this);
        }
    }
//~ ---------------------------------------------------------
    private void Start() {
        _AudioBox = GetComponent<AudioSource>();
    }

    public void GetCoinSoundPlay()
    {
        _AudioBox.PlayOneShot(_getCoinSound);
    }

    public void StageClearSoundPlay()
    {
        _AudioBox.PlayOneShot(_stageClearSound);
    }

    public void ScreamCreatureSoundPlay()
    {
        _AudioBox.PlayOneShot(_screamCreatureSound);
    }

}
