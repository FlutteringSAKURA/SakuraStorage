using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.07 
// Update: //@ 2023.11.08 

// NOTE: //# 3D 게임 - Action Cam 스크립트
//#          1) 
public class NewFollowCamera : MonoBehaviour
{
    public Transform _target;       //& 추적 대상
    public float _moveDamping = 15.0f;      //& 이동속도 계수
    public float _rotateDamping = 10.0f;        //& 회전속도 계수
    public float _distance = 5.0f;      //& 추적 대상과의 거리
    public float _height = 4.9f;        //& 추적 대상과의 높이
    public float _targetOffset = 2.0f;      //& 추적 대상의 오프셋 좌표
    Transform _transform;       //& 카메라 트랜스폼 위치값

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }
    private Vector3 velocity = Vector3.zero;        //& 현재 속도값 0,0,0
    private void LateUpdate()
    {
        //& 카메라와의 거리 및 높이 산출
        var _cameraPos =
        _target.position - (_target.forward * _distance) + (_target.up * _height);
        //& 이동할 때 속도 값 적용
        _transform.position =
        Vector3.Slerp(_transform.position, _cameraPos, Time.deltaTime * _moveDamping);


        // _transform.position =
        // Vector3.SmoothDamp(_transform.position, _cameraPos, ref velocity, _moveDamping);
        //& 회전할 때 속도 값 적용
        _transform.rotation =
        Quaternion.Slerp(_transform.rotation, _target.rotation, Time.deltaTime * _rotateDamping);
        //& 카메라를 추적대상으로 Z축 회전
        _transform.LookAt(_target.position + (_target.up * _targetOffset));



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //& 추적 및 시야조정을 위한 표시
        Gizmos.DrawWireSphere(_target.position + (_target.up * _targetOffset), 0.1f);
        //& 메인카메라와 추적 지점간을 라인 표시
        Gizmos.DrawLine(_target.position + (_target.up * _targetOffset), transform.position);
    }

}
