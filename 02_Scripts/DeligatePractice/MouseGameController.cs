using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;       //# C# 언어 자체의 델리게이트 활용을 위해

// Update: //@ 2023.10.08 

// NOTE: //# Deligate방식이 아닌 C#문법으로 동일한 기능 구현하는 코드 스크립트(3)
//#          1) interface script를 상속
//#          2) 마우스 사용

public class MouseGameController : MonoBehaviour, IGameController     //% 파생클래스(상속받은 스크립트)는 MonoBehaviour를 사용하는 스크립트에 접근 하기 위해서는 MonoBehaviour를 선언해야 함.
{
    //~ ------------------------------------------------------------------------
    // TEST: //# 1
    /*
    public bool FireMissileButtonPressed()
    {
        return Input.GetMouseButtonDown(0);     //& 왼쪽 마우스 버튼 클릭값 리턴
    }
    */
    //~ ------------------------------------------------------------------------
    // TEST: //# 2

    //# NOTE: Action은 C#프레임워크에서 제공하는 델리게이트..함수 참조
    // public Action FireMissileButtonPressed;      //& 인자 타입을 설정하지 않은 경우

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (FireMissileButtonPressed != null)
    //         {
    //             FireMissileButtonPressed();     //& 이벤트 송신
    //         }

    //     }
    // }

    //~ ------------------------------------------------------------------------
    // TEST: //# 3

    public Action<Vector3> FireMissileButtonPressed;    //& 인자 타입을 설정하는 경우

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (FireMissileButtonPressed != null)
            {
                FireMissileButtonPressed(GetCurrentCheckPoint(Input.mousePosition));     //& 이벤트 송신
            }

        }
    }
    Vector3 GetCurrentCheckPoint(Vector3 mousePosition)
    {
        //& 스크린좌표를 뷰포트 포인트 좌표로 전환
        // Vector3 _point = Camera.main.ScreenToViewportPoint(mousePosition);
        //& 스크린좌표를 월드 포인트 좌표로 전환
        Vector3 _point = Camera.main.ScreenToWorldPoint(mousePosition);
        //& 2D의 경우 
        _point.z = 0f;

        return _point;
    }
}
