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

public class CharacterTestScript : MonoBehaviour  //? CharacterTestScript는 MonoBehaviour의 자원을 받는 다는 의미
{
    public int chracterLevel { get; set; }
    public string charcterName { get; set; }
    public bool characterAlive { get; set; }
    public int characterHp { get; set; }

    public int characterAttackPow { get; set; }

    private void Start()
    {
        InitParams();
    }
    public virtual void InitParams()    //$ 자식이 스스로 파라미터 인자 만들어서 사용해라
    {

    }

    protected virtual void Shout()
    {

    }




}
