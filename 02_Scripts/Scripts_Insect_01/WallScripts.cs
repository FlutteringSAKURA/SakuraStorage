using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //! C# 전용문법 사용을 위한 네임스페이스 선언

// Update: //@ 2023.09.05
// NOTE: //# 들여쓰기 정렬하기 ====== command + K + D (전체코드 정렬) // command + k + f (선택 영역 정렬))
public class WallScripts : MonoBehaviour
{
    // NOTE: //# 다중조건
             //# 관계연산자 <,>,--,===,!=,<=,>=
             //# 논리연산자 &&,!,||
             //# 증가, 감소 연산자 ++,--
             //# 산술연산자 +,-,*,/,%
             //# 비트연산자 <<,>>,&,^,~
             //# 할당연산자 -=
    public bool isTrue = false;
    public int insectHp = 120;

    void Start(){
        //! C#을 쓰려면 C#의 네임스페이스를 선언해줘야 해당 자원들을 사용할 수 있다
        //! using System을 선언하지 않으면 아래 명령어 사용 불가
        // TEST: //# ----------- C# 고유의 시간 표현 연습
        DateTime now = DateTime.Now;
        Debug.Log(now.ToString("yyyy-mm-dd HH:mm:dd"));
        Debug.Log(String.Format("{0:yyyy-mm-dd} HH:mm:dd", now));
        
    }

    private void OnMouseDown() {

        insectHp -= 10; //# == insectHp = insectHp -10;
        print("불변수의 현재 상태 :" + isTrue);
        isTrue = true; //# 재할당

        if (isTrue != false && insectHp < 100 && 10>9)
        {
            transform.Rotate(0,45,0);          
        }
               else
        {
            transform.Rotate(30.0f,0,0); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
