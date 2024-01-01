using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.06 ~ 07

//# NOTE: monoBehaviour가 없는 C#스크립팅
//#         1) 적 캐릭터 파라미터 속성(프로퍼티)
//#         2) 생성자란 -> 클래스로부터 객체 만들고 자동으로 실행하는 역할을 하는 것을 의미
//#         3) 내장함수 보유
//#         4) 독자적인 C#클래스는 오브젝트에 붙이지 못한다.
public class Devil
{
    // TEST: //! 게임을 만들땐 이것이 쉽지만, 퍼블릭은 해킹 등의 위험이 있어서 실무에서는 이렇게 잘 안쓴다.

    /*
    public string name;
    public int hp;
    public int attackPow;
    public float moveSpeed;
    */


    // TEST: //! 보안 강화를 위해 사용하는 방법 == 프로퍼티로 변수 선언
    public string name { get; set; }    //# get과 set의 약식 프로퍼티 선언 

    int _hp;    //# 내부에서 가져다 활용

    public int hp
    {
        get { return _hp; } //# 겟터 == 읽기전용
        set
        {
            if (value > 25)     //& 외부에서 해킹으로 100의 값을 주더라도 
            {                   //& 강제적으로 50으로 만들어 준다 (정상화)
                _hp = 50;       //& 특정한 조건을 주어 보호하기 위해 만든 프로퍼티 
            }
            else
            {
                _hp = value;
            }
        } //# 셋터 == 쓰기전용 // 밖에서(MeteoCube 스크립트) 값을 넣어줌
    }

    int _attackPow; // 내부에서 가져다 활용
    public int attackPow
    {
        get { return _attackPow; }
        set { _attackPow = value; }
    }
    public float moveSpeed { get; set; } //& get과 set의 약식 프로퍼티 선언 >> 필요시 값 넣어 사용  
                                         //& 실무에서는 보통 코드가 길어져서 약식으로 우선 선언 후 나중에 필요에 의해 작성한다


    // TEST: //! 디폴트 생성자에서 데이터 초기화
    public Devil()
    {
        name = "생성자 악마";
        _hp = Random.Range(25, 51);
        attackPow = Random.Range(50, 151);
        moveSpeed = Random.Range(7, 10.5f);
    }
    

    // TEST: //# 마우스를 눌렀을 때 데미지를 입어 생명력이 깎이는
    public void HitDam()
    {
        int hitDam = Random.Range(2, 21);
        _hp = _hp - hitDam; //& hp -= hitDam;

    }

    //& C#스크립트기 때문에 이렇게 해서는 아래 함수가 작동하지 않는다. 
    
    private void Start() {
        //HitDam();
    }
    

    // TEST: //# 생명력이 0 이하로 내려가면 파괴시켜라
    
    public void DieMeteo(){
        if (_hp <= 0)
        {
            //& 게임오브젝트 이름을 찾아 0.5초 후에 파괴시켜라
            GameObject.Destroy(GameObject.Find("Meteorite"),0.5f);
            //GameObject.Destroy(GameObject.Find("Meteorite"));
            Debug.Log("메테오가 파괴되었습니다.");
        }
    }
    

}
