using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.24 

// NOTE: //# 3D 게임 - 빌보드 스크립트
//#          1) 어느 방향의 시점에서든 hp바를 확인할 수 있도록 오브젝트를 카메라를 바라보게끔 제어
//#          2) 

public class BillBoard : MonoBehaviour
{
    Transform _cam;

    private void Start()
    {
        _cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);
    }
}
