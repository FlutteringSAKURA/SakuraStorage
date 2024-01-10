using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Update: //@ 2023.12.21 
// Update: //@ 2024.01.02 

// NOTE: //# 스크립터블오브젝트 방식으로 구현하는 오디오 사운드 관련 스크립트 ..
//#          1) 인스팩터창으로 관리하는 것이 아니라 에셋형식으로 오디오 데이터 관리
//#          2)

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
[CreateAssetMenu]
public class AudioSoundStorage_DF : ScriptableObject
{
    [SerializeField]
    AudioSoundStruct_DF[] _audioSoundStruct_Group;

    Dictionary<AudioSoundId_DF, AudioClip> _dicAudioSounds = new Dictionary<AudioSoundId_DF, AudioClip>();

    void GenerateDictionary()
    {
        for (int i = 0; i < _audioSoundStruct_Group.Length; i++)
        {
            _dicAudioSounds.Add(_audioSoundStruct_Group[i].AudioIdFile,
                _audioSoundStruct_Group[i].AudioSoundFile);
        }
    }
    public AudioClip Get(AudioSoundId_DF id_DF)
    {
        Debug.Assert(_audioSoundStruct_Group.Length > 0, "No Existance Audio Sound Data In Here ");
        if (_dicAudioSounds.Count == 0)
        {
            GenerateDictionary();
        }
        return _dicAudioSounds[id_DF];
    }
}
//@ 오디오 사운드 ID 값 
public enum AudioSoundId_DF
{

    //# The Dark Forest - Walk Step Sound 
    THE_DARK_FOREST_WALK_STEP_SOUND_GRASS_FLOOR_01,
    THE_DARK_FOREST_WALK_STEP_SOUND_GRASS_FLOOR_02,
    THE_DARK_FOREST_RUN_STEP_SOUND_GRASS_FLOOR,
    THE_DARK_FOREST_RUN_STEP_SOUND_FALLEN_WOOD,

    //# Patji Behaviour - JumpVoice, Ladder Climb, Patji Breath Sound
    THE_DARK_FOREST_JUMP_VOICE, THE_DARK_FOREST_LADDER_CLIMB,
    THE_DARK_FOREST_LADDER_PATJI_BREATH


}

[Serializable]
public struct AudioSoundStruct_DF
{
    [SerializeField]
    AudioClip _soundFile;
    [SerializeField]
    AudioSoundId_DF _soundId;

    public AudioClip AudioSoundFile
    {
        get { return _soundFile; }
    }
    public AudioSoundId_DF AudioIdFile
    {
        get { return _soundId; }
    }
}
