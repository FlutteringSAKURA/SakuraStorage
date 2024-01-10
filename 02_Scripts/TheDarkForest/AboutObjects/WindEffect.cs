using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.21 
//# NOTE: 오브젝트에 바람효과를 적용하기 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class WindEffect : MonoBehaviour
{
    public float _amplitude = 0.5f;
    public float _frequency = 1.0f;

    private Vector3 _startPos;


    private void Start()
    {
        _startPos = transform.position;

    }

    private void Update()
    {
        transform.position = _startPos + _amplitude
            * new Vector3(Mathf.Sin(_frequency * Time.deltaTime), 0, 0);
    }
}
