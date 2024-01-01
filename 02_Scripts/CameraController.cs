using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.31 

// NOTE: //# 3D 게임 - 카메라 컨트롤러
//#          1) 
//#          2) 
//#          3) 

//~ ------------------------------------------------------------------------
public class CameraController : MonoBehaviour
{
    public Transform _playerTarget;
    Transform _camTransform;
    //% 대상으로부터 떨어질 거리
    [Range(2.0f, 20.0f)]
    public float _distance = 10.0f;
    //% 대상으로부터 떨어질 높이
    [Range(0.0f, 10.0f)]
    public float _height = 2.0f;
    public float _dampingValue = 10.0f;
    private Vector3 velocity = Vector3.zero;        //& 현재 속도값 0,0,0
    public float _trargetOffset = 2.0f;     //& Camera.LookAt에서 활용되는 Offset변수

    public float _rotateValue = 30.0f;
    

    //~ ------------------------------------------------------------------------

    private void Start()
    {
        _camTransform = GetComponent<Transform>();
    }

    //~ ------------------------------------------------------------------------
    

    //# NOTE: 플레이어 캐릭터의 Update함수보다 약간 늦게 작동하게 하기 위해 LateUpdate()를 사용하는 것이 좋음.
    private void LateUpdate()
    {
        //% 플레이어 타겟의 위치의 뒤쪽 방향 + 떨어진 거리 + 카메라의 높이(Vector3.up)
        Vector3 _position = //! _camTransform.position +
        _playerTarget.position + (-_playerTarget.forward * _distance) + (Vector3.up * _height);



        //# NOTE:   Lerp().. 선형(직선) 보간법 : 시작과 끝의 vector값을 균등하게 나누어 보간하는 함수 .. 이동, 회전할 때 사용
        //#         Slerp()는 구면(구형) 보간법..(처음, 끝 다소 느리게 .. 휘어진 부분에서 속도를 증가시켜 전체 속도를 일정하게 해줌)
        //% 0부터 10까지 (Y값)
        // Mathf.Lerp(0, 10, 0.0f);    //& 0을 반환
        // Mathf.Lerp(0, 10, 0.5f);    //& 0.5(= 5)를 반환
        // Mathf.Lerp(0, 10, 0.8f);    //& 0.8(= 8)을 반환
        // Mathf.Lerp(0, 10, 1.0f);    //& 1.0(= 10)을 반환

        //% (1) 구면 보간법을 활용해 부드럽게 위치 변경
        //& 카메라 좌표에서 시작 >> 
        // Legacy:
        //* _camTransform.position = Vector3.Slerp(_camTransform.position, _position, Time.deltaTime * _dampingValue);

        //% (2) smoothDamp를 이용한 보간 (출발지),(도착지), (속도), (도달할 시간)
        _camTransform.position = Vector3.SmoothDamp(_camTransform.position, _position, ref velocity, _dampingValue);

        //% 플레이어쪽(피봇 좌표)으로 방향전환(바라보기)
        _camTransform.LookAt(_playerTarget.position + (_playerTarget.up * _trargetOffset));
    }

    //~ ------------------------------------------------------------------------



    public void GameOver()
    {
        //! TEMP: 게임오버시 사용하는 효과로도 좋을 듯.. 카메라 날라감
        //? 나중에 테스트 해보기
        Vector3 _position = _camTransform.position +
        _playerTarget.position + (-_playerTarget.forward * _distance) + (Vector3.up * _height);
    }


}
