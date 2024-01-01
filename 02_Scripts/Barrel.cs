//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.01 
// Update: //@ 2023.11.08 

// NOTE: //# 3D 게임 - 배럴 컨트롤러
//#          1) 총으로 3번 맞추면 배럴 폭발 구현    Completed:
//#          2) 폭발 사운드 및 불붙는 효과 구현      Completed:
//#          2) 랜덤 텍스처가 입혀진 배럴 생성       Completed:
//#          3) 폭발시 주변 폭발영향 입히기 구현 (불 옮겨 붙기)      Completed:
//#          4) 주변 배럴 연쇄 폭발 효과 구현 (랜덤폭발시간)         Completed:
//#          5) 폭발시 텍스처 변경해 불탄 배럴 구현      Completed:

public class Barrel : MonoBehaviour
{
    Transform _barrelTransform;
    int _hitCount = 0;
    Rigidbody _objRigidBody;

    public float _forceValue = 35.0f;
    public GameObject _hitEffect;
    public GameObject _explosionEffect;
    public GameObject _fireBurnEffect;
    public Transform _fireBurningPos;
    public AudioClip _explosionSfx;

    //& 무작위 적용 텍스처 배열
    public Texture[] _textures;
    MeshRenderer _rendere;
    public float _radius = 10.0f;

    int _barrelLayer;

    // Update: //@ 2023.11.08 
    //ShakeCamera _shakeCamera;
    ShakeEffectController _shakeCameraEffect;

    //~ ------------------------------------------------------------------------

    private void Start()
    {
        _barrelTransform = GetComponent<Transform>();
        _objRigidBody = GetComponent<Rigidbody>();
        _rendere = GetComponentInChildren<MeshRenderer>();

        //% 텍스처 배열의 마지막번째 텍스처는 폭발시 사용할 텍스처 이므로 이것은 빼고 _index로 할당
        int _index = Random.Range(0, _textures.Length - 1);
        _rendere.material.mainTexture = _textures[_index];

        //% 배럴 레이어를 할당
        _barrelLayer = LayerMask.NameToLayer("Barrel");

        // Update: //@ 2023.11.08 
        //_shakeCamera = GameObject.Find("ActionCamera").GetComponent<ShakeCamera>();
        _shakeCameraEffect = GameObject.FindWithTag("MainCamera").GetComponent<ShakeEffectController>();
    }

    //~ ------------------------------------------------------------------------

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Bullet"))
        {
            GameObject spark = (GameObject)Instantiate(_hitEffect, other.transform.position, Quaternion.identity);
            Destroy(spark, 2.5f);
            //Destroy(other.gameObject);

            _hitCount++;
            if (_hitCount == 3)     //& (++_hitCount >= 3) 같은 코드
            {
                ExplosionBarrel();
            }
        }

        if (other.gameObject.tag.Contains("Ground") || other.gameObject.tag.Contains("Wall"))
        {
            //& 땅에 있을때, 폭발 후 다시 땅에 닿을 때 mass값 초기화
            _objRigidBody.mass = 20.0f;
        }
    }

    //@ 배럴 폭발 함수 
    private void ExplosionBarrel()
    {
        //% 폭발시 텍스처 배열의 마지막 텍스처로 렌더링
        GetComponentInChildren<MeshRenderer>().material.mainTexture = _textures[_textures.Length - 1];

        GameObject explosion = (GameObject)Instantiate(_explosionEffect, _barrelTransform.position, Quaternion.identity);
        GameObject fire = (GameObject)Instantiate(_fireBurnEffect, _fireBurningPos.position, Quaternion.identity);
        SoundManager.instance.PlaySfx(_barrelTransform.position, _explosionSfx);
        fire.transform.parent = _barrelTransform;
        Destroy(explosion, 4.5f);
        Destroy(fire, 8.0f);
        // Legacy:
        //*  _objRigidBody.mass = 1.5f;
        //// _objRigidBody.AddExplosionForce(_forceValue, _barrelTransform.position * _forceValue, 8.0f, 500f);
        // Legacy: 
        //* _objRigidBody.AddForce(Vector3.up * _forceValue);

        //% 폭발영향 전달 함수 콜백
        InDirectDamage(_barrelTransform.position);

        // Update: //@ 2023.11.08 
        //StartCoroutine(_shakeCamera.ShakeCameraPlay());
        StartCoroutine(_shakeCameraEffect.Shaking());
        //_shakeCameraEffect._shakeFxFlag = true;

        Destroy(gameObject, 12.0f);

    }

    // TEMP: TEST:  //# 배열을 10개 만듬
    // Collider[] _collsBox = new Collider[10];

    //@ 폭발영향을 받아 전달하는 함수 
    private void InDirectDamage(Vector3 position)
    {
        //& OverlapSphere(원점, 지름, 검출대상 레이어 번호).. [1<<3 2의 1승.. 2의 3승] // ~(1<<8) NOTE: 8번 레이어를 제외한 모든 레이어
        // Legacy:
        Collider[] _collsBox = Physics.OverlapSphere(_barrelTransform.position, _radius, 1 << _barrelLayer | 1 << 3 | 1 << 8);
        // TEMP: TEST: //#  원점, 지름, 담아둔 배열, 검출대상 레이어 번호
        // Physics.OverlapSphereNonAlloc(position, _radius, _collsBox, 1 << _barrelLayer | 1 << 3);

        foreach (Collider coll in _collsBox)
        {
            //% 폭발 범위안에 있는 RigidBody 검출
            _objRigidBody = coll.GetComponent<Rigidbody>();
            //% 폭발시 Constraints 제한 해제 
            _objRigidBody.constraints = RigidbodyConstraints.None;

            if (_objRigidBody != null)
            {
                _objRigidBody.mass = 1.5f;
                //// Constraints 제한 해제
                // //_objRigidBody.constraints = RigidbodyConstraints.None;
                _objRigidBody.AddExplosionForce(_forceValue, _barrelTransform.position, _radius, _forceValue);

                // TEST: 
                // SUCCESS:
                coll.gameObject.SendMessage("OnExpDamage", SendMessageOptions.DontRequireReceiver);
            }

        }
    }

    // TEMP:
    /*
    private void OnDamage(object[] _params)
    {
        Vector3 _hitPos = (Vector3)_params[0];
        Vector3 _firePos = (Vector3)_params[1];
        Vector3 _incomeVector = _hitPos - _firePos;     //& 방향과 길이(힘)

        GameObject spark = (GameObject)Instantiate(_hitEffect, _hitPos, Quaternion.identity);
        Destroy(_hitEffect, 4.5f);
        _objRigidBody.AddForceAtPosition(_incomeVector.normalized * _forceValue, _hitPos);

        if (++_hitCount >= 3)
        {
            ExplosionBarrel();
        }
    }
    */
    
    //@ 주변 폭발 효과 함수.. 
    private void OnExpDamage()
    {
        //float _randomRange = Random.Range(-1.0f, 1.0f);
        //% 불을 생성
        GameObject fire = (GameObject)Instantiate(_fireBurnEffect, _fireBurningPos.position, Quaternion.identity);
        //% 주변 데미지를 받는 배럴에 옮겨 붙게 함.. 배럴의 부모에 좌표값 넣어줌
        fire.transform.parent = _barrelTransform;
        Destroy(fire, 6.9f);

        //% 연쇄 폭발 코루틴 (2.0 ~ 5.4초)후 폭발
        StartCoroutine(DelayExplosion());
    }

    IEnumerator DelayExplosion()
    {
        float _randomSecond = Random.Range(2.0f, 5.4f);
        yield return new WaitForSeconds(_randomSecond);
        ExplosionBarrel();
    }
}
