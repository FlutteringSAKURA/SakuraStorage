using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06 ~ 07
// Update: //@ 2023.09.1

//# NOTE: monoBehaviour가 없는 C#스크립팅
//#         1) 
//#         2) 
//#         3) 
//#         4) 

public class Insect : CharacterTestScript
{
    public override void InitParams()
    {
        //base.InitParams();
        chracterLevel = 10;
        characterHp = 100;
        charcterName = "괴물 곤충";
        characterAlive = true;
        characterAttackPow = 100;
    }

    protected override void Shout()     //$ 부모에서 상속받아 자식에서 재정의
    {
        // base.Shout(); //& 자동생성
        Debug.Log("새끼 곤충이 알수없는 소리를 내고 있습니다.");

    }
}
