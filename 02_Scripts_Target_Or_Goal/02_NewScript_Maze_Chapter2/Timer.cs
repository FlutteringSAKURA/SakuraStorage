using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.16 

// NOTE: //# 3D 게임 - 타이머 스크립트
//#          1) 일정시간 후 파괴
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class Timer : MonoBehaviour
{
    // // public float _timeFlow = 0.0f;
    // // public float _coolTime = 2.0f;
    public float _destroyTime = 2.0f;

    private void Update()
    {
        ////_timeFlow += Time.deltaTime;
        _destroyTime -= Time.deltaTime;
        if (_destroyTime <= 0)
        {
            Destroy(this.gameObject);
        }
        // // if (_timeFlow > _coolTime)
        // // {
        // //     Destroy(this.gameObject);
        // //     _timeFlow = 0.0f;
        // // }
    }


}
