using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;      //# Scene관리 네임스페이스

// Update: //@ 2023.09.14
// Update: //@ 2023.09.15

//# NOTE:
//#         1) 리스트 배열을 활용한 케릭터 옷 갈아 입히기
//#         2) 버튼을 누르면 머리,상의,하의 바뀌도록 구현
//#         3) Animator사용해서 Animation 구현
//#         4) 저장기능을 활용한 함수를 사용해 저장 버튼을 눌러 최근의 캐릭터 상태를 저장하기
//#         5) Scene과 Scene을 전환하기 구현

public class SakuraChracter_DressUp : MonoBehaviour
{
    Transform hairGroup;        //^ public으로 선언해도 되지만 private이 좋다.
    Transform upperBodyGroup;
    Transform downBodyGroup;
    List<GameObject> hairs = new List<GameObject>();    //! hairs 리스트 선언
    List<GameObject> upperBodies = new List<GameObject>();      //! upperBodies 리스트 선언
    List<GameObject> downBodies = new List<GameObject>();       //! downBodies 리스트 선언
    
    int currentHairNum = 0;
    int currentUpperBodyNum = 0;
    int currentDownBodyNum = 0;
    // Update: //% 3) Animator사용해서 Animation 구현
    Animator dressUpGirlAnimator;
    int aniNum = 0;
     //% ------------------------------------------------
    private void Awake()    //$ 가장 먼저 호출 되어야 할 함수
    {

    }
    
    private void Start()
    {
        // NOTE: // ^ 만약 null값 에러가 발생하면 Start내장 함수내에서 간혹 꼬이는 경우가 있으니, Awake 함수에서 호출해도 무방
                 // ^ 함수 내에서도 어느 위치의 줄에 넣느냐에 따라 연산이 달라지는 경우 있음.
                 // ^ 안정적인 구현을 위해서는 반드시 초기에 셋팅해둘 필요가 있는 부분은 Awake에 넣어 사용한다.

        hairGroup = transform.Find("hairGroup");        //$ 위에서 private으로 선언을 해서 각 위치를 찾아 치환(넣어줘야)해야 한다.
        upperBodyGroup = transform.Find("upbodyGroup");
        downBodyGroup = transform.Find("downBodyGroup");
        //^ ------------------------------------------------
        // Update: //% 3) Animator사용해서 Animation 구현
        dressUpGirlAnimator = GetComponent<Animator>();
       //// int aniNum = 0;
        //// dressUpGirlAnimator.SetInteger("Anyname", 0);
        //% ------------------------------------------------
        
        // TEST: //& 방법 (1)
        /*
        foreach (Transform hair in hairGroup)
        {
            hair.gameObject.SetActive(false);
            hairs.Add(hair.gameObject);     //! Add명령어는 리스트의 맨 뒤로 들어감
        }
        hairs[0].SetActive(true);

        foreach (Transform upperBody in upperBodyGroup)     //! upperBodyGroup
        {
            upperBody.gameObject.SetActive(false);
            upperBodies.Add(upperBody.gameObject);   //! upperBodies(리스트)에 upperBody를 추가
        }
        upperBodies[2].SetActive(true);

        foreach (Transform downBody in downBodyGroup)
        {
            downBody.gameObject.SetActive(false);
            downBodies.Add(downBody.gameObject);
        }
        downBodies[2].SetActive(true);
        */

        //% 집어넣기
        MakeDresses(hairGroup, hairs);      //#콜백, 재귀함수호출   >> 리스트에
        MakeDresses(upperBodyGroup, upperBodies);
        MakeDresses(downBodyGroup, downBodies);

        // Update: //% 4) 저장기능을 활용한 함수 
        LoadCurrentSavedDress();        //# 저장한 데이터가 있는 경우
        //% -------------------------------------------------------

        UpdateDresses();
        
    }

    void UpdateDresses()
    {
        //% 꺼내오기
        ShowDress(hairs, currentHairNum);
        ShowDress(upperBodies, currentUpperBodyNum);
        ShowDress(downBodies, currentDownBodyNum);
    }

    // Update: //% 3) Animator사용해서 Animation 구현
    public void DressGirlAnimationsPlay()
    {
        aniNum++;
        if (aniNum > 3)     //^ 방어 코드
        {
            aniNum = 0;
        }
        dressUpGirlAnimator.SetInteger("aniNum", aniNum);   //# Parameter를 int값을 활용하여 에니메이션 제어하기
    }
    //% ------------------------------------------------

    // Update: //% 4) 저장기능을 활용한 함수 
    public void SaveCurrentDresses()
    {
        PlayerPrefs.SetInt("hairs", currentHairNum);     //# string값은 임시변수로 임의지정해도 된다.
        PlayerPrefs.SetInt("upperBodies", currentUpperBodyNum);     //& 오른쪽의 값이 왼쪽으로 덮어쓰기 되는 것으로 이해
        PlayerPrefs.SetInt("downBodies", currentDownBodyNum);
        // Update: //% 5) 다른 scene 불러오기
        SceneManager.LoadScene("dressupSakuraTEST02");
        // Debug.Log("머리 스타일 넘버 :" + currentHairNum);
    }
    public void LoadCurrentSavedDress()
    {
        currentHairNum = PlayerPrefs.GetInt("hairs");               //^ 0
        currentUpperBodyNum = PlayerPrefs.GetInt("upperBodies");    //^ 0
        currentDownBodyNum = PlayerPrefs.GetInt("downBodies");      //^ 0
    }

    // TEST: //& 방법(2) == 방법1을 함수로 한번에 처리하는 방식
    void MakeDresses(Transform group, List<GameObject> dressList)   //^ 부모(group)와 자식(dressList)간의 관계 
                                                                    //^ group ../ dressList로 부터 값을 받아들임
                                                                    //? GameObject형은 배열 등 사용x for/ foreach 사용 X --> transform을 사용해서 구현한다.
    {
        foreach (Transform dress in group)      //! group이라는 Transform(부모)에 dress(자식) Transform
        {
            dress.gameObject.SetActive(false);
            dressList.Add(dress.gameObject);
        }
        //** dressList[0].SetActive(true);       // 아래 ShowDress함수를 만들경우 이 코드는 필요 없음
        //// Debug.Log(upperBodies.Count);
    }

    void ShowDress(List<GameObject> group, int dreesNum)
    {
        for (int i = 0; i < group.Count; i++)
        {
            group[i].SetActive(false);
        }
        group[dreesNum].SetActive(true);    //^ 지정된 의상만 나타나게
    }

    public void ChangeHair()        //% 머리 변경 함수
    {
        currentHairNum++;       //^ +1

        if (currentHairNum > hairs.Count - 1) //! 전체 헤어 숫자보다 1적을 때, 보다 헤어 넘버수가 커지면 0부터 다시 시작하게 하는 방어코드
        {
            currentHairNum = 0;
        }
        ShowDress(hairs, currentHairNum);
        //print(currentHairNum);
        print("리스트의 갯수 :" + hairs.Count);
    }
    public void ChangeUpperClothes()        //% 상의 변경 함수
    {
        currentUpperBodyNum++;
        if (currentUpperBodyNum > upperBodies.Count - 1)
        {
            currentUpperBodyNum = 0;
        }
        ShowDress(upperBodies, currentUpperBodyNum);
    }

    public void ChangeDownClothes()     //% 하의 변경 함수
    {
        currentDownBodyNum++;
        if (currentDownBodyNum > downBodies.Count - 1)
        {
            currentDownBodyNum = 0;
        }
        ShowDress(downBodies, currentDownBodyNum);
    }

    private void Update()
    {
        // // if (Input.GetKeyDown(KeyCode.Space))
        // // {
        // //     ChangeHair();
        // //     print("TEST");
        // // }
    }
}
