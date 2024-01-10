using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Update: //@ 2023.12.13 

// NOTE: //# 스크립터블오브젝트 방식으로 구현하는 오디오 사운드 관련 스크립트 ..
//#          1) 인스팩터창으로 관리하는 것이 아니라 에셋형식으로 오디오 데이터 관리
//#          2)

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
[CreateAssetMenu]
public class AudioSoundStorage : ScriptableObject
{
    [SerializeField]
    AudioSoundStruct[] _audioSoundStruct_Group;

    Dictionary<AudioSoundId, AudioClip> _dicAudioSounds = new Dictionary<AudioSoundId, AudioClip>();

    void GenerateDictionary()
    {
        for (int i = 0; i < _audioSoundStruct_Group.Length; i++)
        {
            _dicAudioSounds.Add(_audioSoundStruct_Group[i].AudioIdFile,
                _audioSoundStruct_Group[i].AudioSoundFile);
        }
    }
    public AudioClip Get(AudioSoundId id)
    {
        Debug.Assert(_audioSoundStruct_Group.Length > 0, "No Existance Audio Sound Data In Here ");
        if (_dicAudioSounds.Count == 0)
        {
            GenerateDictionary();
        }
        return _dicAudioSounds[id];
    }
}
//@ 오디오 사운드 ID 값 
public enum AudioSoundId
{
    //# Door
    Q_01_OPENTHEDOOR, Q_01_CLOSETHEDOOR, Q_01_UNLOCKDOOR,

    //# BathRoom Light
    Q_02_TURNONLIGHT, Q_02_TURNONLIGHT_ENTRANCE, Q_02_TURNONLIGHT_EXPLOSION,

    //# Toilet
    Q_03_TOILET, Q_03_TOILET_CAP_OPEN, Q_03_TOILET_CAP_CLOSE,

    //# BathRoom
    Q_04_BATHWATERBOMB, Q_04_BATHWATERBOMB_BOTTLE_SOUND,

    //#
    Q_05_FINDKEY, Q_05_FINDKEY_TOWEL_DROP_SOUND,

    //# Devil Voice - LookBook
    Q_06_07_LOOKBOOK_AND_PICTURE_ON_THE_WALL, Q_06_07_LOOKBOOK_AND_PICTURE_ON_THE_WALL_01,

    Q_08_SWEETHOMESWEET_GO_OUT,

    //# HomeSweetHome - Walk Step Sound - Wooden Floor , Tile Floor
    SWEETHOMESWEET_WALK_STEP_SOUND_WOODEN_FLOOR, SWEETHOMESWEET_WALK_STEP_SOUND_TILE_FLOOR,
    SWEETHOMESWEET_WALK_STEP_SOUND_WOODEN_FLOOR_02
}

[Serializable]
public struct AudioSoundStruct
{
    [SerializeField]
    AudioClip _soundFile;
    [SerializeField]
    AudioSoundId _soundId;

    public AudioClip AudioSoundFile
    {
        get { return _soundFile; }
    }
    public AudioSoundId AudioIdFile
    {
        get { return _soundId; }
    }
}

