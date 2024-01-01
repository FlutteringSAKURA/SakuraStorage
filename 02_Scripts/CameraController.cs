using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.19 

// NOTE: //# 3D 게임 - 카메라 제어 스크립트
//#          1) 카메라가 플레이어를 일정 거리를 두고 따라다님
//#          2) 
//#          3) 
//#          4) 

public class CameraController : MonoBehaviour
{
    public Transform _sakuraTransform;
    Vector3 _offSet;

    private void Start()
    {
        _offSet = _sakuraTransform.position - transform.position
;
    }

    private void LateUpdate()
    {
        
        transform.position = _sakuraTransform.position - _offSet;
    }
}
