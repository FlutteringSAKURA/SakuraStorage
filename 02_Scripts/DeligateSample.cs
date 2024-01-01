using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.03 

// NOTE: //# 델리게이트 연습용 스크립트
         //# deligate가 전달 역할을 하기 때문에 Handler라는 명칭을 사용 권장
public class DeligateSample : MonoBehaviour
{

    delegate float SumHandler(float a, float b);    //& 선언
    SumHandler _sumHandler;     //& 타입변수 (함수를 변수처럼 사용)

    float Sum(float a, float b)
    {
        return a + b;
    }

    private void Start()
    {
        _sumHandler = Sum;      //& 연결(=할당)
        float _sum = _sumHandler(10.0f, 20.0f);
        Debug.Log("더하기 함수 : " + _sum);
    }

}
