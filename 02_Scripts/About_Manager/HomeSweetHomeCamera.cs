using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.01 
//# NOTE: SweetHomeSweet Camera의 전반을 관리하기 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class HomeSweetHomeCamera : MonoBehaviour
{
    public Transform _patJiTransform;
    Transform _camTransform;

    [Range(2.0f, 20.0f)]
    public float _distance = 3.0f;

    [Range(0.0f, 10.0f)]
    public float _height = 3.0f;
    public float _dampingValue = 10.0f;
    private Vector3 _velocity = Vector3.zero;
    public float _trargetOffset = 2.0f;
    public float _rotateValue = 30.0f;

    [Header("Camera Modifying Options 01")]
    public float _modifiedHeightValue = 6.0f;
    public float _colliderRadius = 1.8f;
    public float _originalHeight;
    public float _camHeightDampingValue = 2.38f;
    public bool _coolCheck = false;
    GameObject _patjiObj;

    [Header("Camera Modifying Options 02")]
    public float _heightAboveObstacle = 12.0f;
    public float _castOff = 1.0f;

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _camTransform = GetComponent<Transform>();

        _originalHeight = _height;
        _patjiObj = GameObject.FindWithTag("PatJi");
    }

    //~ ------------------------------------------------------------------------
    private void Update()
    {

        if (Physics.CheckSphere(_camTransform.position, _colliderRadius))
        {
            //& 부드럽게 카메라 높이를 조절하기 위한 보간법 (상승)
            _height =
                Mathf.Lerp(_height, _modifiedHeightValue, Time.deltaTime * _camHeightDampingValue);
        }
        else
        {
            //& 초기 높이값으로 조절하기 위한 보간법 (하강)
            _height =
                Mathf.Lerp(_height, _originalHeight, Time.deltaTime * _camHeightDampingValue);
        }


        //% 플레이어가 장애물에 가려졌는지를 판단하는 Ray의 높낮이 설정
        Vector3 _castTarget = _patJiTransform.position + (_patJiTransform.up * _castOff);
        //% 정해진 좌표로 방향 벡터 산출
        Vector3 _casDirection = (_castTarget - _camTransform.position).normalized;
        //% 충돌정보 값
        RaycastHit _hitInfo;
        if (Physics.Raycast(_camTransform.position, _casDirection, out _hitInfo, 4f))
        {
            //& 플레이어가 hit되지 않았을 경우
            if (!_hitInfo.collider.CompareTag("PatJi"))
            {
                //^ 선형보간함수 활용해 카메라의 높이를 부드럽게 상승
                _height =
                Mathf.Lerp(_height, _heightAboveObstacle, Time.deltaTime * _camHeightDampingValue);
            }
            else
            {
                //^ 선형보간함수 활용해 카메라의 높이를 부드럽게 하강
                _height =
               Mathf.Lerp(_height, _originalHeight, Time.deltaTime * _camHeightDampingValue);
            }
        }

    }

    //~ ------------------------------------------------------------------------
    //# NOTE: 플레이어 캐릭터의 Update함수보다 약간 늦게 작동하게 하기 위해 LateUpdate()를 사용하는 것이 좋음.
    private void LateUpdate()
    {
        //% 플레이어 타겟의 위치의 뒤쪽 방향 + 떨어진 거리 + 카메라의 높이(Vector3.up)
        Vector3 _position =
        _patJiTransform.position + (-_patJiTransform.forward * _distance) + (Vector3.up * _height);

        //# NOTE:   Lerp().. 선형(직선) 보간법 : 시작과 끝의 vector값을 균등하게 나누어 보간하는 함수 .. 이동, 회전할 때 사용
        //#         Slerp()는 구면(구형) 보간법..(처음, 끝 다소 느리게 .. 휘어진 부분에서 속도를 증가시켜 전체 속도를 일정하게 해줌)
        //% (1) 구면 보간법을 활용해 부드럽게 위치 변경
        //& 카메라 좌표에서 시작 >> 
        // Legacy:
        //* _camTransform.position = Vector3.Slerp(_camTransform.position, _position, Time.deltaTime * _dampingValue);

        //% (2) smoothDamp를 이용한 보간 (출발지),(도착지), (속도), (도달할 시간)
        _camTransform.position = Vector3.SmoothDamp(_camTransform.position, _position, ref _velocity, _dampingValue);

        //% 플레이어쪽(피봇 좌표)으로 방향전환(바라보기)
        _camTransform.LookAt(_patJiTransform.position + (_patJiTransform.up * _trargetOffset));

    }

    //~ ------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_patJiTransform.position + (_patJiTransform.up * _trargetOffset), 0.1f);
        Gizmos.DrawLine(_patJiTransform.position + (_patJiTransform.up * _trargetOffset), transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _colliderRadius);

        // Update: //@ 2023.11.15 
        //# 키메라 벽 충돌시 시야 보정 구현(2)

        //% 플레이어가 장애물에 가려져 있는지를 판단할 Ray표시
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_patJiTransform.position + (_patJiTransform.up * _castOff), transform.position);

    }

}
