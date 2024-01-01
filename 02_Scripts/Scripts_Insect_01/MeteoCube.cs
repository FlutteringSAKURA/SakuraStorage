using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06 ~ 07 ~ 08

//# NOTE: 만든 C#스크립트에 접근해 사용하는 방법
//#         1) 
//#         2) 
//#         3) 
//#         4) 

public class MeteoCube : MonoBehaviour
{
    //! 이미 C#스크립팅으로 만들어둔 스크립트 (Devil)스크립트
    Devil mySelf; //@ C# 스크립트 접근
    BoxList myBoxList; //@ MonoBehaviour 스크립트 접근
    GameObject saKura;
    
    // TEST: //# 사쿠라의 위치 좌표 찾아 출력하기
    Transform sakuraTrans; 

    // TEMP: //? 사쿠라의 생명령을 가져와 표시해라 (SakuraPractice스크립트) 로부터// 테스트를 위한 코드)
    //* SakuraPractice saKura;

    // TEST: //# 박스의 스코어 점수 올리기
    
    public int MeteoScore = 0;      //$ 초기 점수 값 
                                    //$ >> 스크립트에 0이라고 써도 유니티 인스펙터에 100이라고 쓰면 100이 우선권을 갖는다


    void Start()
    {
        mySelf = new Devil(); //# 클래스 생성 = 인스턴스생성 
        // myBoxList = new BoxList();
        // myBoxList.nameBox = "리스트 몬스터 이름은 운석입니다.";
        //mySelf.hp = 50;

        /*
        print("몬스터의 이름은 : " + mySelf.name);
        print("운석의 체력값은 :" + mySelf.hp);
        print("운석의 공격력은 :" + mySelf.attackPow);
        print("운석의 이동속도는 :" + mySelf.moveSpeed);
        */

        // TEST: //! 게임을 만들땐 이것이 쉽지만, 퍼블릭은 해킹 등의 위험이 있어서 실무에서는 이렇게 잘 안쓴다.        
                 //! 겟터와 셋터 이용 ---> Devil 스크립트에서 겟터와 셋터 셋팅


        // TEST: //@생성자 데이터 초기화를 위한 주석처리 == Devil 스크립트의 Devil() 함수 사용을 위해
        /*
        mySelf.name = "METEORITE";
        mySelf.attackPow = 100;
        mySelf.moveSpeed = 10.5f;
        mySelf.hp = 50;
        */
        

        // TEST: //# 사쿠라의 생명력을 가져와 표시해라 (SakuraPractice스크립트)로부터
        // TEMP: //? 임시 주석
        //* saKura = new SakuraPractice();
        //* print("사쿠라의 생명력은 : " + saKura.sakuraHp);


        // TEST: //! 게임오브젝트 찾기 (태그로 찾는 법)
        saKura = GameObject.FindWithTag("Player");


        // TEST: //# 사쿠라의 위치 좌표 찾아 출력하기 
                 //$ (방법1)
        // sakuraTrans = GameObject.FindWithTag("Player").transform;
                 //$ (방법2)
                 //# 사쿠라 게임오브젝트 찾기 + 해당 오브젝트 좌표 찾기
        saKura = GameObject.Find("SAKURA_PRACTICE");
        sakuraTrans = saKura.gameObject.transform;
        
        print("사쿠라야 어디에 있니? : " + sakuraTrans.position);
    }

    // TEST: //@ 마우스를 누르면 이름을 출력
    private void OnMouseDown()
    {
        /*
        Debug.Log("악마의 이름은 : " + mySelf.name);
        Debug.Log("악마의 공격력은 : " + mySelf.attackPow);
        Debug.Log("악마의 움직임 속도는 : " + mySelf.moveSpeed);
        Debug.Log("악마의 생명력은 : " + mySelf.hp);
        */

        // TEST: //# 변수 접근으로 스코어 올리기
        print("사쿠라 생명력 가져오기 테스트 : " +saKura.GetComponent<SakuraPractice>().sakuraHp);
        print("보너스 점수 가져오기 테스트 : " +saKura.GetComponent<SakuraPractice>().meteoBonusScore);
        TotallAddScore();

        // TEST: //$ 사쿠라의 생명력을 출력해보기 (컴포넌트를 통해 변수에 접근하는 방법)
        print ("사쿠라의 생명력은 : "+saKura.GetComponent<SakuraPractice>().sakuraHp + "입니다.");

        // TEMP://* mySelf.HitDam();
        /*
        print("몬스터의 이름은 : " + mySelf.name);
        print("운석의 체력값은 :" + mySelf.hp);
        print("운석의 공격력은 :" + mySelf.attackPow);
        print("운석의 이동속도는 :" + mySelf.moveSpeed);
        */
        // print("리스트에 있는 몬스터 종류는 : "+myBoxList.nameBox);


        // TEST: //# 생명력이 0 이하로 내려가면 파괴시켜라
                 //& 방법1 함수호출
        mySelf.DieMeteo();

                //& 방법2 직접 작성
        /*
                if (mySelf.hp <= 0)
                {
                    Destroy(gameObject, 0.5f);
                    print("메테오가 파괴되었습니다.");
                }
        */


        // TEST: //! 게임오브젝트 찾기 (태그로 찾는 법) + 사쿠라 스크립트에 접근하여 생명력 변수 찾기
        print("사쿠라의 생명력 : " + saKura.GetComponent<SakuraPractice>().sakuraHp);

        // TEST: //# 사쿠라의 생존 상태를 물어봄
                 //# 살아있다면 프린트 구문을 활용하여 살아있다는 메세지를 출력 (접근법)
        if (saKura.GetComponent<SakuraPractice>().isAlive == true)
        {
            print("사쿠라는 지금 건강하게 살아 숨쉬고 있습니다.");
        }
        
    }

    // TEST: //@ 사쿠라의 생명력을 이용해서 운석과 부딪히면 사쿠라 생명력이 깎이며 출력 (변수사용)
             //& GetComponent를 많이 쓰는 것은 효율이 떨어진다.
    private void OnTriggerEnter(Collider other)
    {
    
/*
        print("운석의 공격력은 :" + mySelf.attackPow);
        saKura.GetComponent<SakuraPractice>().sakuraHp -= mySelf.attackPow;
        if (saKura.GetComponent<SakuraPractice>().sakuraHp > 0)
        {
           print("사쿠라가 운석에 의해 상처를 입고 남은 생명력 : " + saKura.GetComponent<SakuraPractice>().sakuraHp);
            saKura.GetComponent<SakuraPractice>().isAlive = false;      //$ 죽은 상태로 불변수 변경
            // TEST: //# 박스의 스코어 점수 올리기
            MeteoScore = saKura.GetComponent<SakuraPractice>().sakuraHp;
            print("메테오가 사쿠라의 생명력을"+ MeteoScore +"만큼 흡수했습니다.");

        }
        else
        {
            print("사쿠라가 운석에 의해 상처를 입고 남은 생명력 : " + saKura.GetComponent<SakuraPractice>().sakuraHp);
            print("사쿠라가 치명적인 상처를 입고 사망하였습니다.");
            Destroy(saKura,0.5f);
        }
 */        
    }


    // NOTE: //! 선생님 구현 : 사쿠라와 운석이 충돌했을 때, 사쿠라로부터 운석으로 점수 전달 // SakuraPractice스크립트와 연동
    public void AddScore(int sakuraDamScore)     //$ 강제주입 방식 (함수로 던져주는 방식)
    {   
        MeteoScore += sakuraDamScore;
        print("나는 운석인데 운석이 흡수한 생명력의 수치는?? : " + MeteoScore + "입니다.");
    }

    void TotallAddScore() //$ 변수로 넘어온 점수의 합계
    {
        MeteoScore += saKura.GetComponent<SakuraPractice>().meteoBonusScore;
        print("변수로 넘어온 스코어의 합계는?? : " + MeteoScore + "입니다.");
    }

    void Update()
    {
        
    }
}
