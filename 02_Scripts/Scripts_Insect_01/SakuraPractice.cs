using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.07 ~ 08

//# NOTE: 다른 스크립트의 변수를 다른 스크립트에서 접근해 사용하는 방법
//#         1) 
//#         2) 
//#         3) 
//#         4) 
public class SakuraPractice : MonoBehaviour
{

    public int sakuraHp = 100;
    public bool isAlive = true;
    GameObject meteoCube;

    public int meteoBonusScore = 500;   //$ 변수로 점수를 가져오는 방법 사용시 

    private void Start()
    {
     meteoCube = GameObject.FindWithTag("Meteorite");
     print(meteoCube);
    }

    private void OnTriggerEnter(Collider meteo)
    {

        print("TEST");
        // NOTE: //! 선생님 구현 (주입방식)
        //& 매번 GetComponent하는 것은 낭비가 심하다.
        //# 이런 식의 코드 작성이 좀 더 효율적인 코딩이다.
        if (meteo.tag == "Meteorite")       //$ 메테오 인식
        {
              print("TESTTEST");
            int hitDamage = Random.Range(50, 151);
            sakuraHp -= hitDamage;
            //print("사쿠라가 운석에 의해 상처를 입고 남은 생명력: " + sakuraHp);

            //# (MeteoCube)스크립트에서 (AddScore)함수를 만들어 주입함 (함수로 넘기는 방식)
            meteoCube.GetComponent<MeteoCube>().AddScore(500);

        }

    }
}
