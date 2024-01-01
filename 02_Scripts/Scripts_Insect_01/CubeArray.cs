using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06

//# NOTE: 배열
//#         1) 하나의 변수 이름으로 여러개의 데이터를 담는 묶음형 변수
//#         2) 배열에서 원하는 데이터를 찾기 위해 루프를 활용하는 것이 좋음 
//#         3) for루프는 순서대로 배열의 요소를 찾을 때 사용
//#         4) foreach루프는 순서와 상관없이 배열 내부의 요소 중 특정조건에 맞는 요소를 찾을 때 사용

public class CubeArray : MonoBehaviour
{
    public GameObject[] meteorite;
    int meteoriteCount = 0;

    // TEST: //# 1) 마우스를 눌렀을 때 배열 리스트 중 임의 선정한 배열의 큐브 색바꾸기


    // Material materialCol;
    private void Start()
    {
         // materialCol = GetComponent<Renderer>().material;
        // MakeAllChangeCol();
    }



    private void OnMouseDown()
    {
       
        // TEST: //# 3) 한번에 모든 큐브색 바꾸기
        MakeAllChangeCol();

        //materialCol.color = Color.red;    
        // TEST: //# 1) 모든 색 초록으로 
        meteorite[meteoriteCount].GetComponent<Renderer>().material.color = Color.green;

        // TEST: //# 2)         
        //meteorite[meteoriteCount].GetComponent<Renderer>().material.color = Color.red;

        if (meteoriteCount < 4)
        {
            meteoriteCount++; 
            Debug.Log("메테오의 숫자는 : "+ meteoriteCount);
        }
    
        
    }

    // TEST: //# 3) 한번에 모든 큐브색 바꾸기
    void MakeAllChangeCol()
    {
        for (int meteoriteClick = 0; meteoriteClick < meteorite.Length; meteoriteClick++)
        {
            meteorite[meteoriteClick].GetComponent<Renderer>().material.color = Color.red;
            //meteorite[meteorite.Length].GetComponent<Renderer>().material.color = Color.red; 
        }
    }

    private void Update() {
        
    }
}
