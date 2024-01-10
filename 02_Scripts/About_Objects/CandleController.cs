using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.29 
// Update: //@ 2023.12.01 
//# NOTE: Candle controll Script

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class CandleController : MonoBehaviour
{
    public GameObject _fireFlame;
    public GameObject _grabPos;

    public GameObject _leftHand_Grab_Pos;
    public MeshRenderer _candleBody;
    public MeshRenderer _candleMetal;
    public MeshRenderer _candleCylinder;



    public Light _light;

    private void Start()
    {
        _grabPos = GameObject.FindGameObjectWithTag("GrabPos_Candle");

        // Update: //@ 2023.12.01 
        _leftHand_Grab_Pos = GameObject.FindGameObjectWithTag("Left_GrabPos_Candle");

    }

    private void ChangeSpriteRenderer(Sprite _greenSprite)
    {
        SpriteRenderer _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _greenSprite;
    }

    public void OnFireFlame()
    {
        _fireFlame.SetActive(true);
    }

    //@ 시네마틱 타임라인 연출 중에 오른손으로 집기 위한 함수  
    public void AttachedRightHand()
    {
        _grabPos = GameObject.FindGameObjectWithTag("GrabPos_Candle");
        gameObject.transform.parent = _grabPos.transform;
        transform.position = _grabPos.transform.position;
        transform.rotation = _grabPos.transform.rotation;

    }

    //@ 시네마틱이 끝나고 플레이어가 팥쥐를 조작할 때 왼손으로 캔들을 들고 있기 위한 함수 
    public void AttachedLeftHand()
    {
        _leftHand_Grab_Pos = GameObject.FindGameObjectWithTag("Left_GrabPos_Candle");
        gameObject.transform.parent = _leftHand_Grab_Pos.transform;
        transform.position = _leftHand_Grab_Pos.transform.position;
        transform.rotation = _leftHand_Grab_Pos.transform.rotation;
    }
    public void MoveToOriginalPatJiBody()
    {
        GameObject _patJi;
        _patJi = GameObject.Find("PatJi");
        gameObject.transform.parent = _patJi.transform;
        AttachedLeftHand();
    }


    //@ 양초 임시 저장 부모 .. 
    // NOTE: //# PatJiActress의 손에 양초가 있는 채로 SetActive(false)가 되 버리면 양초를 Original Patji에게 가져올 수 없는 문제 때문에 만든 함수 
    public void RemovePatjiActressHand()
    {
        GameObject _gameContorlTower;
        _gameContorlTower = GameObject.Find("GameControlTower");
        gameObject.transform.parent = _gameContorlTower.transform;
    }

    public void LightRangeUp()
    {
        _light.range = 12.5f;
    }
    public void ModifyingLightInensity()
    {
        _light.intensity = 5.0f;
    }
    public void ModifyingLightInensity02()
    {
        _light.intensity = 3.0f;
    }

    //@ SweetHomeSweet Go Out Scene 양초 내려 놓는 위치 함수 .. 
    // NOTE: //# PatJiActress의 손에 양초가 있는 채로 SetActive(false)가 되 버리면 양초를 Original Patji에게 가져올 수 없는 문제 때문에 만든 함수 
    public void CandleDropPosition()
    {
        GameObject _candleDropPos;
        _candleDropPos = GameObject.Find("CandleDropPos");
        gameObject.transform.parent = _candleDropPos.transform;
        gameObject.transform.position = _candleDropPos.transform.position;
        gameObject.transform.rotation = _candleDropPos.transform.rotation;
    }

    //@ 양초를 들고 있을 때 빛에 의한 쉐도우 발생하지 않도록 하기
    public void NoShadowActive()
    {
        _candleBody.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _candleMetal.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _candleCylinder.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
    public void ShadowActive()
    {
        _candleBody.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _candleMetal.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _candleCylinder.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    public void DisableFlame()
    {
        StartCoroutine(DisableFlameActve());

    }

    IEnumerator DisableFlameActve()
    {
        yield return new WaitForSeconds(1.5f);
        _light.enabled = false;
        yield return new WaitForSeconds(0.4f);
        _light.enabled = true;
        yield return new WaitForSeconds(1.2f);
        _light.enabled = false;
        yield return new WaitForSeconds(0.3f);
        _light.enabled = true;
        yield return new WaitForSeconds(0.9f);
        _light.enabled = false;
        yield return new WaitForSeconds(0.2f);
        _light.enabled = true;
        yield return new WaitForSeconds(1.2f);
        _fireFlame.SetActive(false);
    }
}
