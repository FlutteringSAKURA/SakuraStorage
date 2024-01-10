using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.03 
//# NOTE: Indicator Marker의 제어를 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class IndicatorMakerController : MonoBehaviour
{
    Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }
    private void Update()
    {
        transform.LookAt(transform.position + _camera.rotation * Vector3.forward,
                        _camera.rotation * Vector3.up);
    }
}
