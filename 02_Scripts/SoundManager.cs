using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] //? 강제로 컴포턴트를 붙이는 명령어

// Update: //@ 2023.09.26

// NOTE:  //# 메테오 구현(코루틴 사용) == 시간차 구현
//#          1) 
//#          2) 
//#          3) 
//#          4) 

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;        //! 싱글톤 == 글로벌 참조가 가능
    public AudioClip fireSound;
    public AudioClip meteoCrush;
    public AudioClip enemyHittedSound;
    public AudioClip enemyBiteSound;
    AudioSource audioBox;
    private void Awake()
    {
        /*
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
        */
        //# 위와 같은 코드
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
    public void FireSounds()
    {
        audioBox.PlayOneShot(fireSound);
    }

    public void MeteoCrush()    //& 메테오와 충돌시 재생 사운드
    {
        audioBox.PlayOneShot(meteoCrush);
    }
    public void EnemiesHittedSounds()    //& 몬스터가 공격을 받았을 때 재생 사운드
    {
        audioBox.PlayOneShot(enemyHittedSound);
    }
    public void EnemyBiteSounds()    //& 몬스터와 캐릭터가 충돌시 재생 사운드
    {
        audioBox.PlayOneShot(enemyBiteSound);
    }
}