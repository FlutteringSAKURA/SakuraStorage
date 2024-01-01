using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.15

//# NOTE: 캐릭터가 여러 위치에서 생성되게 만들기
//#         1) 캐릭터가 스폰될 위치를 배열로 만들기
//#         2) 생성될 캐릭터를 프리팹으로 만들고 해당 프리팹을 저장할 리스트 만들기
//#         3) 
//#         4) 

public class GirlGenManager : MonoBehaviour
{
    public GameObject girlPrefab;
    public Transform[] girlGenPoint;    //& 위치 배열 선언
    List<GameObject> anotherGirl = new List<GameObject>();      //& 프리팹(게임오브젝트) 저장할 리스트 선언

    private void Start()
    {
        GnerateAnotherGirl();   //^ 콜백 = 만든 함수를 불러주는 것 >> 제대로 함수가 작동하는지(TEST)
    }
    void GnerateAnotherGirl()
    {
        for (int i = 0; i < girlGenPoint.Length; i++)       //^ 배열이 몇개인지 포루프로 찾는 코드
        {
            Debug.Log("찾은 배열의 수 : " + girlGenPoint.Length);
            GameObject girlChracter = Instantiate(girlPrefab) as GameObject;     //! 복제명령어(Instantiate) as == 게임오브젝트로~
                                                                                 //! 프리팹이 girlChracter 변수에 저장된다.
            girlChracter.transform.position = girlGenPoint[i].position;     //& 생성될 프리팹의 좌표값을 생성될 포인트의 배열 좌표값으로 치환
            anotherGirl.Add(girlChracter);      //& Instantiate된 프리팹을 프리팹을 저장할 리스트에 추가하기
            print("캐릭터의 갯수 : " + anotherGirl.Count);
        }
       
    }

}
