//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ShakeEffectController : MonoBehaviour
{
    public bool _shakeFxFlag = false;
    public float _duration = 0.4f;
    public float _magnitudePos = 0.2f;
    public float _magnitudeRot = 0.4f;

    public AnimationCurve _curve;

    PlayerController _playerCtrl;
    private void Start()
    {
        _playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_shakeFxFlag)
        {
            _shakeFxFlag = false;
            StartCoroutine(Shaking());
        }

        if(_playerCtrl._canFireFlag && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shaking());
            Debug.Log("Shake");
        }
    }

    public IEnumerator Shaking()
    {

        Vector3 _OriginPosition = transform.position;
        Quaternion _OriginRotation = transform.rotation;
        float _elapsedTime = 0.0f;
        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float _strength = _curve.Evaluate(_elapsedTime / _duration);
            // Vector3 _shakeForceValue = Random.insideUnitSphere;

            transform.position = _OriginPosition + Random.insideUnitSphere * _strength;

            Vector3 _shakeRot = new(0, 0, Mathf.PerlinNoise(_magnitudeRot * _magnitudePos, 0f));

            transform.rotation = Quaternion.Euler(_shakeRot);
            //transform.position = _startPosition + _shakeForceValue;
            yield return null;
        }
        transform.position = _OriginPosition;
        transform.rotation = _OriginRotation;

    }
}
