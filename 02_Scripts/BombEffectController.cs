using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.22

// NOTE:  //# 폭발이펙트 구현
//#          1) 일정시간 지난 후 Destroy
//#          2) 
//#          3) 
//#          4) 

public class BombEffectController : MonoBehaviour
{
    public float timeFlow = 0.0f;

    void Start()
    {

    }

    void Update()
    {
        //^ 일정한 시간이 흐른 뒤 파괴.
        if (timeFlow > 1.0f)
        {
            Destroy(this.gameObject);
        }
        else
        {
            timeFlow += Time.deltaTime;   
        }
    }
}
