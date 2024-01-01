using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.05

//# NOTE: 포루프 문 
//#         1) 정해진 일을 반복적으로 수행할 때 사용
//#         2) 
//#         3) 

public class ForLoop : MonoBehaviour
{
    void Start()
    {
        /*
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        print("나는 사쿠라 입니다.");
        */
    }

    private void OnMouseDown()
    {
        // NOTE: //# 선언+할당(초기값) >> 조건문 >> (증가,감소)
        //# 정수 0 할당 >> 10이 될때까지 >> 0부터 1씩 증가 반복
        // TEST: //@ (1) 프린트 10번 출력
        /*
        for (int i = 0; i < 10; i++)
        {
            print("나는 사쿠라 입니다.");
        }
        // TEST: //@ (2) 프린트 10번 회전
        for (int i = 0; i < 10; i++)
        {
            gameObject.transform.Rotate(0,45,0);
        }
        */

        // Update: //@ 2023.09.06------------
        /*       
        int myNum; //# 선언을 하고 포루프문에서 할당하는 것과 ==== 포루프문 내에서 선언과 할당을 같이 해도 같은 말

        for (myNum = 0; myNum < 10; myNum++) //# 포루프문 내에서 선언과 할당 같이 하는 것 -> int myNum = 0
        {            
            transform.Rotate(0,10.0f,0);
            print(myNum);
        }
        */

        // TEST: //# 1) 마우스를 누를 때 마다(10번 반복) 오브젝트가 Z축 방향으로 0.5f만큼 이동하는 함수를 만들어서 동작해보기
        for (int mouthClick = 1; mouthClick < 10; mouthClick++)
        {
            MoveForward();
           //transform.Translate(0, 0, 0.5f);  
            Debug.Log("마우스 클릭한 수 : " + mouthClick);
        }

    }


    //# 움직이는 함수
    void MoveForward()
    {
        transform.Translate(0, 0, 0.5f);   
    }
     //@----------------------------------

    // Update is called once per frame
    void Update()
    {
        
    }
}
