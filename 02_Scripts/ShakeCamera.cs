using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Transform _cameraTransform;
    public Transform _zoomCamTr;
    //& 카메라 회전시키는 변수
    public bool _shakeRotateFlag = false;
    //& 카메라 초기 회전값 및 좌표값
    Vector3 _originPos;
    Quaternion _originRot;

    Vector3 _originZoomCamPosValue;
    Quaternion _originZoomCamRotValue;
    public float _shakeAmount = 0.7f;

    private CameraController _cameraCtrl;
    GameObject _playerObject;

    private void Awake()
    {
        if (_cameraTransform == null)
            _cameraTransform = GetComponent(typeof(Transform)) as Transform;
    }

    private void Start()
    {
        // TEMP:
        // _originPos = _cameraTransform.localPosition;
        // _originRot = _cameraTransform.localRotation;

        // _originZoomCamPosValue = _zoomCamTr.localPosition;
        // _originZoomCamRotValue = _zoomCamTr.localRotation;

        // _cameraCtrl = GameObject.FindWithTag("Player").GetComponent<CameraController>();
        _playerObject = GameObject.FindGameObjectWithTag("Player");

    }
    private void OnEnable()
    {
        _originPos = _cameraTransform.localPosition;
    }

    private void Update()
    {
        if (_shakeRotateFlag)
        {
            _cameraTransform.localPosition = _originPos + Random.insideUnitSphere * _shakeAmount * 0.5f;
        }

        else
        {
            _cameraTransform.localEulerAngles = _originPos;
            _shakeRotateFlag = false;
        }
    }

    // TEMP:
    // public IEnumerator ShakeCameraPlay(float duration = 0.05f, float magnitudePos = 0.2f, float magnitudeRot = 0.4f)
    // {
    //     //& 지난 시간 누적 변수
    //     float _passedTime = 0.0f;
    //     while (_passedTime <= duration)
    //     {
    //         //& 반경이 1인 구체 내의 3차원좌표 불규칙하게 변환
    //         Vector3 _shakePos = Random.insideUnitSphere;
    //         _cameraTransform.localPosition = _shakePos * magnitudePos;

    //         _originZoomCamPosValue = _shakePos * magnitudePos;
    //         // _shakeCamTr.localPosition = _originPos + Random.insideUnitSphere * _shakeAmount * 0.5f;
    //         if (_shakeRotateFlag)
    //         {
    //             //& PerlinNoise(난수 발생 함수)
    //             Vector3 _shakeRot = new(0, 0, Mathf.PerlinNoise(magnitudeRot * Time.deltaTime, 0f));
    //             _cameraTransform.localRotation = Quaternion.Euler(_shakeRot);

    //             _zoomCamTr.localRotation = Quaternion.Euler(_shakeRot);
    //         }
    //         _passedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     //& 진동 후 카메라 초기값으로 설정
    //     _cameraTransform.localPosition = _originPos;
    //     _cameraTransform.localRotation = _originRot;

    //     _zoomCamTr.localPosition = _originZoomCamPosValue;
    //     _zoomCamTr.localRotation = _originZoomCamRotValue;

    // }

}
