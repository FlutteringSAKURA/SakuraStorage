using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.08

//# NOTE:  C#스크립트에 접근해 사용하는 방법 (상속)
//#         1) 
//#         2) 
//#         3) 
//#         4) 

public class Creature : BossInsect
{
    // BossInsect myQueen = new Creature();

    protected override void Shout()
    {
        base.Shout(); //& 자동생성
        Debug.Log("크리처가 울부짖습니다.");

    }

    private void OnMouseDown()
    {
        // Debug.Log(myQueen.insectHp);

    }
    
}
