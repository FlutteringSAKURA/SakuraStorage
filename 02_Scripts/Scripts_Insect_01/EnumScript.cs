using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.05

//# NOTE: 열거형(enum) 연습
//#         1) 상수(const)에는 값 ==> 오탈자 문제 발생 가능성 있음
//#         2) 열거형식을 사용하면 위 문제 해결 가능
//#         3) 여러개의 상수 선언보다 하나의 열거형식이 나음

public class EnumScript : MonoBehaviour
{
    //# 열거할 변수 선언
    public int IDLE = 0;
    public int FLYING = 1;
    public int ATTACK = 2;
    public int EATING = 3;
    public int DEAD = 4;

    public int insectHP = 100; //# insect HP 선언

//! 데이터 타입이기 때문에 네이밍을 잘 지어야한다.
    public enum InsectState { 
        idle,Flying,Attack,Eating,Dead
    }

    //! 담을 변수 선언
    public InsectState monInsect = InsectState.Attack; //# 몬스터 상태를 공격상태로 선언

    // TEST: //@ 박스 충돌시 박스 매터리얼 변화---------------------------
    Material meteoRite;

    private void Start() {
        meteoRite = GetComponentInChildren<Renderer>().material;
        Debug.Log("매터리얼을 얻었습니다.");
    }
    //@ -----------------------------------------------------------

    private void OnMouseDown() {
        // TEST: //@ (1) if 구문 (열거형 테스트를 위한 주석처리)
        /*int InsectState = IDLE;
        if (InsectState == IDLE)
        {
            print("곤충이 쉬고 있습니다");
        }*/

        // TEST: //@ (2) 열거형 연습 ----------- if 구문 연동
       // monInsect = InsectState.idle; //? 기본 상태 초기값 선언 
                                        //! 결과 : if 불충족 >> else 충족 == 곤충이 회전한다. 

        if(monInsect == InsectState.Flying){
            print("곤충이 쉬고 있는 상태입니다.");
            transform.Translate(0,0,5.0f);
        }
        else{
            transform.Rotate(0,45f,0);
        }        
    }
    // TEST: //@ (1) 큐브 낙하시 색변화하거나 회전-----------------
    //@ (2) enum활용하여 특정메시지 상태 표현
    //@ (3) 곤충 잠시 후 사라지기
    private void OnTriggerEnter(Collider rock)
    { //! other == 부딪힌 게임 오브젝트
        // if(rock.tag == "Meteorite") //! 태그로 찾는 방법
        if (rock.name == "Cube_meteoRite") //! 게임오브젝트 이름으로 찾기 
        {
            rock.GetComponentInChildren<Renderer>().material.color = Color.red;
            Debug.Log("운석이 곤충의 생명력을 흡수해 붉게 변했습니다.");

            insectHP -= 100; //# == insectHP = insectHP-10;
            Debug.Log("곤충의 생명력 :" + insectHP);

            monInsect = InsectState.Dead;
            if (insectHP <= 0 && monInsect == InsectState.Dead)
            {
                Debug.Log("곤충이 죽었습니다.");
                Destroy(gameObject, 0.5f);
            }
        }
    }
    //@ ----------------------------------------------

    private void Update() {
        if (gameObject.tag == "Monster")
        {
            if (monInsect == InsectState.Attack)
            {
                print("곤충이 미쳐 날뛰는 상태");
            }
        }
        {
            if (monInsect == InsectState.idle)
            {
                print("곤충이 쉬고있는 상태");
            }
        }
        {
            if (monInsect == InsectState.Flying)
            {
                print("곤충이 이동하는 상태");
            }
            if (monInsect == InsectState.Flying) print("이동하는 중");
            if (monInsect == InsectState.Eating) print("먹이를 먹는 중");
            if (monInsect == InsectState.Dead) print("죽은 상태");

        }
    }

}
