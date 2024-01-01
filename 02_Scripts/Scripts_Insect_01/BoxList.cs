using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06

//# NOTE: 리스트
//#         1) 일반화컬렉션(같은 성격을 가진 데이터를 여러개 담는 자료구조)중 하나
//#         2) 배열과 달리 생성시 데이터의 갯수를 미리 지정하지 않음
//#         3) 자동으로 용량이 증가하거나 감소함
//#         4) 

public class BoxList : MonoBehaviour
{
    // TEST:
    public GameObject[] monsters; // TEST: //# 리스트 추가하기 // = null 값이 없는 상태
    //! monsters라는 게임오브젝트 배열을 선언했다. (유니티 인스펙터) // 할당은 X
    // TEST:
    public Transform[] playerTransfrom; //#
    
    public string nameBox;
     
    List<GameObject> meteorites = new List<GameObject>(); 
    //! 게임오브젝트 리스트를 선언(meteorites)해서 리스트로 할당했다. 

    private void Start() {
        // TEST: //# 유니티 내장 랜덤값 사용
        // int randomValue = Random.Range(1,101); //! 1~100 사이의 랜덤값을 사용하려면 101까지 입력해줘야한다.
        // Debug.Log("주사위를 돌려 나온 값은 : " + randomValue);

        foreach (GameObject meteo in monsters) //! in 뒤에는 일반화 변수를 넣는다. // meteo는 임시변수
        {
            //! 변수가 먼저 온다
            if (meteo.tag == "Meteorite") // TEST: //# 임시변수에 담긴 태그를 찾아라 (사용자 정의)
            {   // NOTE: //# 순서에 상관없이 컬렉션(배열)의 요소를 찾는 것
                meteorites.Add(meteo); 
                //! monsters안에서 Meteorite태그 붙은 게임오브젝트를 찾아 meteo임시변수리스트에 넣은 것을 리스트(meteorites)에 추가한다.
                Debug.Log("몬스터 리스트에 들어간 갯수는 : " + meteorites.Count);
                // NOTE: print는 유니티에서 제공하는 함수 vs Debug.Log는 C#에서 제공하는 함수
            }
        }
    }

    private void OnMouseDown() {        
        
        MakeAllChangeCol(); //# 모든 큐브 색을 바꾸는 함수

        //! Meteorite태그가 붙은 게임오브젝트를 임시변수인 meteo에 담긴 그 게임오브젝트의 색을 바꾸기
        for (int i = 0 ; i < meteorites.Count ; i++)  
        {
            meteorites[i].GetComponent<Renderer>().material.color = Color.magenta;
        }
        // TEST: //# 리스트 중에서 하나를 빼는 과정 구현하기
        if (meteorites.Count > 0) //! 방어코드 >>> 없을 경우 아래 코드 실행중에 더이상 값을 처리하지 못하는 순가 에러난다.
            meteorites.RemoveAt(meteorites.Count - 1); //! vs Remove == 전부 없애는 명령어
            print("리스트에서 빠지는 숫자: " + meteorites.Count);
        
        
        
    }

    void MakeAllChangeCol()
    {
        for (int meteoriteClick = 0; meteoriteClick < monsters.Length; meteoriteClick++)
        {
            monsters[meteoriteClick].GetComponent<Renderer>().material.color = Color.green;
            //meteorite[meteorite.Length].GetComponent<Renderer>().material.color = Color.red; 
        }
    }
}
