using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

// Update: //@ 2023.11.04 

// NOTE: //# 3D 게임 - Sound관련 관리 스크립트
//#             1)
//#             2)
//#             3)

//~ ------------------------------------------------------------------------
public class SoundManager : MonoBehaviour
{
    public AudioClip _creatureDetectionBGM;
    AudioSource _audioBox;

    // TEST: //# 크리처 발견시 음악 재생 변경, 크리처 바라보지 않는 경우 비전투 간주. 음악 재생 변경
    // Completed:
    GameObject _detectBGM;
    GameObject _noneBattleBGM;
    //GameObject _cyberCreatureHitSound;
    //public AudioClip _cyberCreatureHittedSound;


    public static SoundManager instance;

    //~ ------------------------------------------------------------------------
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ ------------------------------------------------------------------------
    void Start()
    {
        _audioBox = GetComponent<AudioSource>();

        _detectBGM = GameObject.Find("BattleBGM");
        _noneBattleBGM = GameObject.Find("NoneBattleBGM");
        _detectBGM.SetActive(false);
    }

    //~ ------------------------------------------------------------------------
    void Update()
    {

    }

    //~ ------------------------------------------------------------------------

    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        GameObject _soundObj = new GameObject("Sfx");
        _soundObj.transform.position = pos;
        AudioSource _audioSource = _soundObj.AddComponent<AudioSource>();
        _audioSource.clip = sfx;
        _audioSource.Play();
        Destroy(_soundObj, sfx.length);
    }

    // TEST: //# 크리처 발견시 음악 재생 변경, 크리처 바라보지 않는 경우 비전투 간주. 음악 재생 변경
    // Completed:
    public void CreatureDetection()
    {
        _detectBGM.SetActive(true);
        _noneBattleBGM.SetActive(false);
    }
    public void CreatureUndetection()
    {
        _detectBGM.SetActive(false);
        _noneBattleBGM.SetActive(true);
    }

// TEST:
    // public void CyberCreatureHitSound()
    // {
    //     _cyberCreatureHitSound.SetActive(true);
        
    // }

    // public void StopCreatureHitSound()
    // {
    //     _cyberCreatureHitSound.SetActive(false);
    // }
}
