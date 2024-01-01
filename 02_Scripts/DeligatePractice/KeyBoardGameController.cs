using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;       //# C# 언어 자체의 델리게이트 활용을 위해

// Update: //@ 2023.10.08 

// NOTE: //# Deligate방식이 아닌 C#문법으로 동일한 기능 구현하는 코드 스크립트(4)
//#          1) interface script를 상속
//#          2) 키보드 사용

public class KeyBoardGameController : MonoBehaviour, IGameController
{
    //~ ------------------------------------------------------------------------
    // TEST: //# 1
    /*
    public bool FireMissileButtonPressd()
    {
        return Input.GetKeyDown(KeyCode.Space);     //& 키보드 스패이스 값 리턴
    }
    */
    //~ ------------------------------------------------------------------------
    // TEST: //# 2

    //# NOTE: Action은 C#프레임워크에서 제공하는 델리게이트..함수 참조
    public Action FireMissileButtonPressed;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (FireMissileButtonPressed != null)
            {
                FireMissileButtonPressed();
            }
        }
    }


}

