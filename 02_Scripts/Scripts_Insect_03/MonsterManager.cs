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
public class MonsterManager : MonoBehaviour
{
    // NOTE: //# 1) 곤충, 크리처 객체를 생성
    //# 2) 씬에 있는 곤충과 크리처를 찾아서 해당 객체를 넣어줌

    void Start()
    {
        Insect myInsect = new Insect();     //! 각각의 객체 생성
        Creature myCreature = new Creature();   //! 각각의 객체 생성 // 모노비헤비어를 쓰는 경우 객체 생성 불가 (이렇게 못 씀)
        GameObject insect = GameObject.Find("Insect_03");
        GameObject creature = GameObject.Find("Creature_03");
        // print("찾은 괴물의 이름은? " + insect);

        //! 서로다른 인자를 전달하는 것
        // TEMP: insect.GetComponent<MonsterScript>().SetParameter(myInsect);    //$ 주입(오버로딩처럼) 
                                                                                 //? MonoBehaviour를 부모로 쓰는 경우 객체 생성하여 주입 불가
        creature.GetComponent<MonsterScript>().SetParameter(myCreature);    //$ 주입(오버로딩처럼)


    }

    void Update()
    {

    }
}
