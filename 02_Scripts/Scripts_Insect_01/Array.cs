using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06~07

//# NOTE: 배열
//#         1) 하나의 변수 이름으로 여러개의 데이터를 담는 묶음형 변수
//#         2) 배열에서 원하는 데이터를 찾기 위해 루프를 활용하는 것이 좋음 
//#         3) for루프는 순서대로 배열의 요소를 찾을 때 사용
//#         4) foreach루프는 순서와 상관없이 배열 내부의 요소 중 특정조건에 맞는 요소를 찾을 때 사용

public class Array : MonoBehaviour
{
    // public int[] numbers = new int[5]; //# 5개 배열 선언
    public int[] myArrays; //# 배열갯수 정함없는 배열변수 선언(사용자정의)
    int[,] myArrayPse = {{1,2,3},{4,5,6}};
    void Start()
    {
        for (int i = 0; i < myArrayPse.GetLength(0); i++) //! 초기값 / 조건문 // 효과
        {
            for (int j = 0; j < myArrayPse.GetLength(1); j++)
            {
                Debug.Log("아이템 저장소 : " + myArrayPse[i,j]);
            }
        }
        //# 데이터 타임[] 배열이름 = new 데이터타입[담을 데이터]
        //int[] numbers = new int[5];

        //print("넘버스의 정체:" + numbers);
        
        // TEST: //! 값을 수동으로 넣기 
       /*
        numbers[0]=100; 
        numbers[1]=200;
        numbers[2]=300;
        numbers[3]=400;
        numbers[4]=500;
                
        print("값을 수동을 넣어 뽑은 숫자의 정체: "+numbers[1]);
        */

        //! Result: 2번째 배열을 불러온다 >> 200이 찍혀야 한다.

        // TEST: 
        /*
        for (int i = 0; i < numbers.Length; i++)
        {
            print("for루프를 통해 뽑은 배열의 숫자: "+numbers[4]);
            //! Result: 2번째 배열을 불러온다 >> 500이 찍혀야 한다.
        }
        */

    }

private void OnMouseDown() {
    /* for (int i = 0; i < numbers.Length; i++)
    {
        print(numbers[i]);
    }
    */

    //# 사용자 정의한 배열의 수 체크 
    print("배열의 길이가 몇이야?: " + myArrays.Length);

    // TEST: //! 배열 중에서 1개를 뺀 배열의 수
    print("배열의 길이에서 1개를 빼면?:" + myArrays[myArrays.Length -1]);
    // Result: //! 4번째 배열의 수가 찍혀야 한다 

}
    // Update is called once per frame
    void Update()
    {

    }
}
