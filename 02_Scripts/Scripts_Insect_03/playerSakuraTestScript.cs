using System;
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
public class playerSakuraTestScript : CharacterTestScript
//? MonoBehaviour를 상속받은 CharacterTestScript(부모)를 상속받은 playerSakuraTestScript(자식)
{

    public int playerSakuraExp { get; set; }
    public int playerLevel { get; set; }
    public string playerName { get; set; }
    public bool playerAlive { get; set; }
    public int playerAttackPow { get; set; }
    public int playerHp { get; set; }

    // public int addScore {get; set;}
    public int addScore = 0;
    GameObject monsterInsect;

    public override void InitParams()
    {
        // base.InitParams(); //$ 부모의 것
        playerHp = 1000;    //& 부모의 자원을 가져다가 자식이 오버라이딩하는 코드
                            //& 1000의 생명력을 가지고 시작
        playerLevel = 100;
        playerName = "SAKURA";
        playerAlive = true;
        playerAttackPow = 100;
        characterAttackPow = playerAttackPow;

       // playerHp = 1000;
        characterHp = playerHp;

        monsterInsect = GameObject.FindGameObjectWithTag("Monster");

        //addScore = 0;

        print("플레이어의 레벨은 : " + playerLevel);
        print("플레이어의 이름은 : " + playerName);
        print("플레이어의 상태는 : " + playerAlive);
        print("플레이어의 공격력은 : " + playerAttackPow);
        print("플레이어의 생명력은 : " + playerHp);

    }

    private void OnTriggerEnter(Collider monsterObj)
    {
        if (monsterObj.gameObject.tag == "Monster")
        {   //# 방법(1)
            if (monsterInsect.GetComponent<MonsterScript>().insectAttack == playerHp)
                Destroy(gameObject, 0.5f);
        }
        /* //# 방법(2)
                playerHp -= playerHp;
                    print("SAKURA TEST" + playerHp);
                    if(playerHp <= 0)
                    {
                        Destroy(gameObject, 0.5f);
                    }
        */
    }

    public void AddPlayerScore(int SakuraScore)
    {
        this.addScore += SakuraScore;
        print("사쿠라가 얻은 점수는 : " + addScore);
    }

}
