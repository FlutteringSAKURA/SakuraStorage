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

public class MonsterScript : MonoBehaviour
{

    // BossInsect oneSelf;    //! 부모의 클래스 선언
    BossInsect srcMonster;   //# 같게 해주는 경우도 있다

    GameObject sakuraCharacter;

    public int insectAttack = 1000;

    private void Start()
    {
        // TEST: srcMonster = new Insect();
        // srcMonster = new Creature();
        // Result: //& new를 두개 동시에 쓰면, 첫번째 new 동작 안함 >> 둘 다 한번에 쓸 수 없다.
        // TEMP: srcMonster = new BossInsect();
        //GameObject sakuraCharacter =  GameObject.Find("Ai_Practice");
        sakuraCharacter = GameObject.Find("SAKURA");
        //print("test:" + sakuraCharacter);
    }

    public void SetParameter(BossInsect srcMonster)
    {   //! 상속 고리 역할 >> 밖에서 주입, 인자를 통해서 insect인지 creature인지 선택적 = srcMonster(=임시변수)
        // this.oneSelf = srcMonster;      //$ 치환하기 >> this를 넣어 구별해줌
        this.srcMonster = srcMonster;      //# 같게 해주는 경우도 있다 >> this로 구분,

        Debug.Log("SetParameter Activation TEST : " + srcMonster);
    }



    private void OnMouseDown()
    {

        //srcMonster.Hit();


        if (srcMonster != null) //? 방어코드 (이렇게 코드를 짜는 습관을 들여야 한다) == 값이 들어와야 작동 .. 값이 들어오지 않으면 else로 빠져라 
        {                       //? MonsterManager 스크립트에서 insect 파라미터터 인자를 전달하지 않을경우 insect는 else 코등 작동
            srcMonster.Hit();
            /*
            Debug.Log("곤충여왕의 이름 표시 : " + srcMonster.insectQueenName);
            Debug.Log("곤충여왕의 생명력 표시 : " + srcMonster.insectQueenHp);
            Debug.Log("곤충여왕의 이동속도 표시 : " + srcMonster.insectQueenMoveSpeed);
            Debug.Log("곤충여왕의 공격력 표시 : " + srcMonster.insectQueenAttackPow);
            */

            //& TEST: 부모의 피를 깎아 0이 되면 게임오브젝트 파괴
            if (srcMonster.insectQueenHp <= 0)
            {
                srcMonster.insectQueenHp = 0;
                Debug.Log("곤충여왕의 생명력 표시 : " + srcMonster.insectQueenHp);
                
                // TEST: //# 여기서 보너스 점수를 플레이어에게 점수 전달 (점수는 임의대로 전달)
                sakuraCharacter.GetComponent<playerSakuraTestScript>().AddPlayerScore(5000);
                //#----------------------------------------------------------
               Destroy(this.gameObject, 0.5f);
                
            }
        }
        else
        {
            Debug.Log("조건을 만족하지 않습니다.");
        }

    }

}
