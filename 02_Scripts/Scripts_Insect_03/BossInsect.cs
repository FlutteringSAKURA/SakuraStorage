//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.08
// Update: //@ 2023.09.11

//# NOTE:  C#스크립트에 접근해 사용하는 방법 (상속)
//#         1) 
//#         2) 
//#         3) 
//#         4) 

public class BossInsect     
{
    public int insectQueenHp { get; set; }
    public string insectQueenName { get; set; }
    public float insectQueenMoveSpeed { get; set; }
    public int insectQueenAttackPow { get; set; }
    public int insectHp { get; set; }

    public int insectQueenScore {get; set;}

    public BossInsect()     //& 디폴트 생성자
    {
        insectQueenName = "썪어가는 곤충여왕";
        insectQueenHp = 1000;
        insectQueenMoveSpeed = 15.0f;
        insectQueenAttackPow = Random.Range(100,151);
    }

    public void Hit(){
        int hitDamage = Random.Range(50,101);
        insectQueenHp = insectQueenHp-hitDamage;
        Debug.Log("곤충의 생명력 : " + insectQueenHp);
        Shout();        //$ 아무것도 없는 샤우트 호출

    }

    protected virtual void Shout()  //$ protected = 부모와 자식간 (다른 스크립트에서는 접근 불가) 
                                    //$ 자식들만 접근하여 사용 >> 상속하려고 만든 함수
                                    //& 재정의해서 쓰기 위함
    {
       // Debug.Log("상속하려고 만든 함수");

    }



    /*
    public void KillOneself()
    {
        Debug.Log("hp" + insectQueenHp);
        if (insectQueenHp <= 0)
        {
            insectQueenHp = 0;
            //GameObject hellCreature = GameObject.Find("Creature2_03");                  
        }
    }
    */
    


}

