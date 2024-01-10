using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.05 
//# NOTE: 정전 효과를 주기 위한 라이트 제어 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class Light_Off : MonoBehaviour
{
    [SerializeField]
    GameObject _lights;

    [SerializeField]
    Material _emissions;

    [SerializeField]
    GameObject _indicator_Mark;

    [SerializeField]
    GameObject _indicator_LightOn_Mark;


    public bool _lightOff_CheckFlag = false;
    public bool _q_02_LightBottonPushed = false;

    public static Light_Off instance;

    //~ -------------------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    //~ -------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PatJi" && !_lightOff_CheckFlag)
        {
            AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_02_TURNONLIGHT_ENTRANCE);
            StartCoroutine(ElectronicFailure());

        }
    }

    IEnumerator ElectronicFailure()
    {
        float _flikerTime = UnityEngine.Random.Range(0.7f, 1.0f);
        _lightOff_CheckFlag = true;

        //% 퀘스트 표시 마크 비활성화 
        _indicator_Mark.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(true);
        _emissions.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(true);
        _emissions.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(true);
        _emissions.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(_flikerTime);
        AudioSoundSystem.instance.PlayAudioSound(AudioSoundId.Q_02_TURNONLIGHT_EXPLOSION);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);

        //% 퀘스트 1 완료 .. 퀘스트 2 활성화
        SweetHomeSweet_Quest_Manager.instance._quest_1_Completed = true;

    }

    //@ 퀘스트 2. 화장실 불을 켜보는 함수 
    public void TryingToLightOn()
    {
        StartCoroutine(TotallyElectronicFailure());
    }

    public IEnumerator TotallyElectronicFailure()
    {
        float _flikerTime = UnityEngine.Random.Range(0.5f, 1.0f);
        _lightOff_CheckFlag = true;

        _q_02_LightBottonPushed = true;

        //% 퀘스트 표시 마크 비활성화 
        _indicator_LightOn_Mark.SetActive(false);


        yield return new WaitForSeconds(1.5f);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(true);
        _emissions.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(true);
        _emissions.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(true);
        _emissions.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(_flikerTime);
        _lights.SetActive(false);
        _emissions.SetColor("_EmissionColor", Color.black);

    }

    //@ HomeSweetHome Scene 클리어 후 전등 Material Emission 원상 복구 함수 
    public void ReturnMaterialEmission()
    {
        _emissions.SetColor("_EmissionColor", Color.white);
    }
}
