using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.11
// Update: //@ 2023.09.12
// Update: //@ 2023.09.13

//# NOTE:  게임오브젝트 사용자 조작
//#         1) 움직이는 속도 
//#         2) 움직이는 방향
//#         3) 회전하는 속도
//#         4) 애니메이션 활용
//#         5) 게임 종료시 작동 불가 구현

public class MonInsect : MonoBehaviour
{
    public float moveSpeed = 10.0f;      //! 이동속도
    public float rotateSpeed = 200.0f;   //% 회전속도
    // Animation monInsectAnim;    //& 애니메이션 활용
    Animator monInsecAnimator;      //^ Animator 선언
    bool isFlying = false;
    bool isAttack = false;

    StageGameManager gameManager;
    public bool IsFlying(float hAxisValue, float vAxisValue)   //! Void가 아닌 타입은 반드시 리턴값을 주어야 한다.
    {
        if (Mathf.Abs(hAxisValue) > 0.5f || MathF.Abs(vAxisValue) > 0.5)    //& 가로v세로 절대값을 통해 움직임 표현
        {
            return true;   //^ 움직임이 있으면 true값이 들어감 >> 아래 isFlying에 들어감 >> 움직임
        }
        return false;    //^ 움직임이 없으면 아무 값이 안들어옴
    }

    // TEST: //! HitSpider스크립트에서 곤충의 게임오브젝트에 접근해 좌표를 알아내기 위한 함수
    public void InsectTrans(){
        Transform insectTrans = GetComponent<Transform>();
        Debug.Log("게임오브젝트를 이용한 곤충의 좌표는? " + insectTrans);
    }
    //! ------------------------------------------------------------------------

    private void Start()
    {
        // monInsectAnim = GetComponent<Animation>();      //& Animation 가져오기       
        monInsecAnimator = GetComponent<Animator>();     //^ Animator 가져오기(할당)
        gameManager = GameObject.Find("StageGameManager").GetComponent<StageGameManager>();
    }

    private void Update()
    {
        if(gameManager.gameEnd == true)
        {
             monInsecAnimator.SetBool("isFlying", false);
            // monInsecAnimator.enabled = false;
            return;     //! 함수 실행 중단.. 프로그램 자체는 계속 실행 >> 이하 이동 코드 실행 불가
        }

        // TEST: //? 유니티 project setting의 Input manager의 horizontal 접근법
        // Result: //# if문을 쓸 때보다 코드가 간결해졌다! (Getkey) 잘 안씀

        float axisValueH = Input.GetAxis("Horizontal");
        float axisValueV = Input.GetAxis("Vertical");
        //Debug.Log("TEST Horizontal : " + axisValueH);
        //Debug.Log("TEST: Vertical : " + axisValueV);
        //^ ------------------------------------------------------------------

        // TEST: //% Animator를 활용해 Animation 구현 ---------------
        // NOTE: //# 함수하나 만들고, 이 함수의 상태를 체크, 업데이트 애니메이션 변경부분만 처리

        /*
                if (axisValueH > 0.5f || axisValueH < -0.5f)    //# 
                {
                    isFlying = true;    //& 움직임 감지
                }
                else if (axisValueV > 0.5f || axisValueV < -0.5f)
                {
                    isFlying = true;    //& 움직임 감지
                }
                else if(Input.GetKeyDown(KeyCode.Space))
                {
                    isAttack = true;    //& 공격 자세 감지
                }
                else
                {
                    isFlying = false;   //& 움직임 없음
                }
                // Debug.Log("곤충의 상태는? : " + isFlying); //값 출력 확인
        */
        isFlying = IsFlying(axisValueH, axisValueV);    //# 애니메이터에 있는 움직임을 제어 >> 값이 주입된 함수를 변수에 넣음
        Debug.Log("함수의 동작상태 : " + isFlying);

        monInsecAnimator.SetBool("isFlying", isFlying);     //^ 불 변수의 T/F를 통해 Animator의 Bool 값을 제어
        monInsecAnimator.SetBool("isAttack", isAttack);     //& 공격기능 추가
        
        
        //% ------------------------------------------------------------------

        float moveSpeedPerSeconds = moveSpeed * Time.deltaTime;         //$ 이동값 // 시간 증가분에 따른 속도값
        float rotateSpeedPerSeconds = rotateSpeed * Time.deltaTime;     //$ 회전값 // 시간 증가분에 따른 회전값

        // TEST: //& 이동 구현
        float xMoveSpeed = rotateSpeedPerSeconds * axisValueH; // Result: -1~1 값 //! 좌우 움직임 // 
        float zMoveSpeed = moveSpeedPerSeconds * axisValueV;                   //! 앞뒤 움직임

        transform.Translate(0, 0, zMoveSpeed);     //! 전진 후진
        transform.Rotate(0, xMoveSpeed, 0);          //! 좌우 전환

        // TEST: //& 공격 기능 추가
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttack = true;
        }
        else { isAttack = false; }


        // return; //? 이하 작동 X
        /* TEMP: //! 이하 불필요
        if (Input.GetKey(KeyCode.W))
        {
            //^ 같은 표현 (but 좀 더 간결한 코드)
            //float speedPerSeconds = moveSpeed * Time.deltaTime;
            transform.Translate(0, 0, moveSpeedPerSeconds);
            // transform.Translate(0, 0, moveSpeed * Time.deltaTime);
            monInsectAnim.CrossFade("Flying");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //float speedPerSeconds = Time.deltaTime * moveSpeed * -1f;
            transform.Translate(0, 0, moveSpeedPerSeconds * -1f);
            // transform.Translate(0, 0, -moveSpeed * Time.deltaTime); 
            monInsectAnim.CrossFade("Flying");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //float speedPerSeconds = Time.deltaTime * rotateSpeed * -1f;
            transform.Rotate(0, rotateSpeedPerSeconds * -1f, 0);
            //transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
            monInsectAnim.CrossFade("Flying");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //float speedPerSeconds = Time.deltaTime * rotateSpeed;
            transform.Rotate(0, rotateSpeedPerSeconds, 0);
            //transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
            monInsectAnim.CrossFade("Flying");
        }
        // TEMP: //^ 공격테스트
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            monInsectAnim.CrossFade("Attack(3)");
        }
        */
    }

}
