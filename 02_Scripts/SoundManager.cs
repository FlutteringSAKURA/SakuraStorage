using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10

// NOTE: //# 2D 게임 사운드 제어 스크립트
//#          1) 레이저 발사 소리
//#          2) 폭발이펙트 소리
//#          3) 플레이어 비행기 파괴 소리
//#          4) 
//#          5)

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip laserSound;
    public AudioClip explosionSound;
    public AudioClip playerDieSound;

    AudioSource audioBox;

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
        audioBox = GetComponent<AudioSource>();
    }

    public void LaserSoundsPlay()
    {
        audioBox.PlayOneShot(laserSound);
    }

    public void ExplosionSoundsPlay()
    {
        audioBox.PlayOneShot(explosionSound);
    }

    public void PlayerDieSoundsPlay()
    {
        audioBox.PlayOneShot(playerDieSound);
    }
}
