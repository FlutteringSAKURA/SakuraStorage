using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.24 

// NOTE: //# 3D 게임 - 사운드 매니저
//#          1) 비전투시 일반 음악, 전투시 전투음악 구현 해보기
//#          2)

[RequireComponent(typeof(AudioSource))]     //  오디오소스 컴포넌트 강제로 지정
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource _audioBox;
    public AudioClip _mainBgm;
    public AudioClip _battleBgm;
    public AudioClip _getGodlSound;
    public AudioSource _mainBGM, _battleBGM;
    public GameObject _groundSoundObj;
    public GameObject _waterSoundObj;

    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _audioBox = GetComponent<AudioSource>();
        MainBgmPlay();
    }

    //~ ------------------------------------------------------------------------

    public void MainBgmPlay()
    {
        _mainBGM.Play();
        _battleBGM.Stop();
    }

    public void BattleBgmPlay()
    {
        _battleBGM.Play();
        _mainBGM.Stop();
    }

    public void GetGoldSoundPlay()
    {
        _audioBox.PlayOneShot(_getGodlSound);
    }
}
