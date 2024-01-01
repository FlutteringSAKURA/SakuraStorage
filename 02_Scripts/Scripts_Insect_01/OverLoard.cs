using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.05

//# NOTE: 오버로딩 
//#         1) 같은 함수에 다른 데이터 타입을 가진 인자(파라미터==> 주입된 임시 변수)전달
//#         2) 이름이 같고 인자의 데이터 타입이나 갯수가 다른 함수를 만들고 
//#         3) 마치 하나의 함수를 사용하듯 편하게 활용하는 방법

public class OverLoard : MonoBehaviour
{
    // Update: //@ 2023.09.06------------
    //# 예시 : public이 명시적으로 있지 않은 경우 Local / private이다.
    int MonLv = 10;
    float myValue = 50.0f;
    string monName = "Devil";
    bool isAttending = true;
    //@ ---------------------------------

    void Start()
    {
        
    }

    // NOTE: //@ 오버로딩 = 함수 이름은 같고 주입되는 인자가 다름
    //! 정수값을 주입시켜서 곱한 뒤 답을 반환하는 함수 만들기

    int multipleTwoNumbers(int numA, int numB) //# 인자값 = 파라미터 = 매개변수
    {
        int answer = numA * numB;
        return answer;
    }
    int multipleThreeNumbers(int numA, int numB, int numC)
    {
        int answer = numA * numB * numC;
        return answer;
    }

    //! 소수값을 주입시켜서 곱한 뒤 답을 반환하는 함수 만들기
    float multipleTwoNumbers(float numA, float numB)
    {
        float answer = numA * numB;
        return answer;
    }

    private void OnMouseDown()
    {
        int answerInt = multipleTwoNumbers(10, 10);
        int answerIntThree = multipleThreeNumbers(10, 10, 25);
        float answerFloat = multipleTwoNumbers(10.0f, 10.0f);
        print("정수로 표현(x2) :" + answerInt);
        print("정수로 표현(x3) :" + answerIntThree);
        print("소수점 표현 :" + answerFloat);
        // Update: //@ 2023.09.06------------
        string myName = GetCreatureName();
        print(myName);
        int MonLv = GetCreatureLv();
        print("크리처의 레벨 :" + MonLv);
        //@----------------------------------
    }
    // Update: //@ 2023.09.06------------
    int GetCreatureLv() //! int 타입은 int타입으로 리턴 해줘야 한다. == 형이 같아야 한다.
    {
        return 100;
    }

    string GetCreatureName(){
        return "Death Creature";
    }
    //@----------------------------------

    void Update()
    {
        // Update: //@ 2023.09.06------------
        transform.Translate(0,0,1.0f * Time.deltaTime); //# 지속적인 값 투입
        //@----------------------------------
    }
}
