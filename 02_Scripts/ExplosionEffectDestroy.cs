using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10

// NOTE: //# 2D 게임 폭발이펙트 자동파괴 제어 스크립트
//#          1) 일정 시간 후 자동으로 폭발이펙트 파괴해서 없애기
//#          2) 
//#          3) 
//#          4) 
//#          5)

public class ExplosionEffectDestroy : MonoBehaviour
{
    public float timeFlow = 0.0f;

    
    private void Update()
    {
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
