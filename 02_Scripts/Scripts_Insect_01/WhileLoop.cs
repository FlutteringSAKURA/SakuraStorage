using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06

//# NOTE: 와일루프 문 
//#         1) 어떤 조건이 참인 동안 명령문을 반복 실행
//#         2) 
//#         3) 

public class WhileLoop : MonoBehaviour
{
    void Start()
    {
        int count = 0;
        while (count < 10) //# 10보다 작으면 함수 안의 코드를 실행해라
        {
            print("카운트 : "+count);
            count++; //# 값을 1씩 증가
            
        }
    }


    void Update()
    {
        
    }
}
