using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.13 

// NOTE: //# 스크립터블오브젝트 방식으로 구현하는 오디오 사운드 관련 스크립트를 참조해서 오디오를 재생하는 스크립트 ..
//#          1) 
//#          2)

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class AudioSoundSystem : MonoBehaviour
{
    public static AudioSoundSystem instance;
    [SerializeField]
    AudioSoundStorage _audioStorage;

    [SerializeField]
    AudioSoundStorage_DF _audioStorage_DF;

    AudioSource _audioBox;

    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;

        _audioBox = GetComponent<AudioSource>();
    }

    //~ ------------------------------------------------------------------------

    public void PlayAudioSound(AudioSoundId id)
    {
        _audioBox.PlayOneShot(_audioStorage.Get(id));
    }

    public void PlayAudioSound_DF(AudioSoundId_DF id_DF)
    {
        _audioBox.PlayOneShot(_audioStorage_DF.Get(id_DF));
    }

    public void PlayAudioSound_Stop()
    {
        _audioBox.Stop();
    }
    public void PlayAudioSound_Play()
    {
        _audioBox.Play();
    }
    public void AudioLoop()
    {
        //_audioBox.pitch = 1.5f;
        _audioBox.loop = true;
    }
    public void Recovery()
    {
        // _audioBox.pitch = 1.0f;
        _audioBox.loop = false;
    }
}
