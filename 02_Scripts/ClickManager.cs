using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.19 
// Update: //@ 2023.10.20 

// NOTE: //# 3D 게임 - 클릭 제어 스크립트
//#          1) 마우스클릭 좌표를 사쿠라가 움직이는 좌표로 치환
//#          2) 
//#          3) 
//#          4) 

public class ClickManager : MonoBehaviour
{
    GameObject _sakuraObj;
    public bool _groundFlag = false;

    public static ClickManager instance;

    private void Awake() {
        if(instance == null) instance = this;

    }

    //~ ------------------------------------------------------------------------
    void Start()
    {
        _sakuraObj = GameObject.FindWithTag("Player");

    }

    //~ ------------------------------------------------------------------------
    void Update()
    {
        CheckClick();
    }
    //~ ------------------------------------------------------------------------


    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("마우스 위치 값 : " + Input.mousePosition); 
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);   //& 카메라의 마우스 위치 값을 레이의 함수로 바꿈
                                                                            //& = Camera의 mousePosition의 입력값을 Ray함수의 _ray변수로 넣어줌
            RaycastHit _hitInfo;        //& 레이를 쏴서 부딪히는 값을 넣기 위한 RaycastHit 변수 선언

            if (Physics.Raycast(_ray, out _hitInfo))    //% _ray로 얻은 _hitInfo값을 활용하는 함수
            {
                //# 땅이면 
                if (_hitInfo.collider.gameObject.name == "Ground")
                {
                    // TEMP: //* _sakuraObj.transform.position = _hitInfo.point;
                    //Debug.Log("Raycast Info : " + _hitInfo.point);

                    //% _hitInfo의 좌표값을 PlayerFSM스크립트의 Moveto함수에 넘겨줌
                    //% 마우스지점의 클릭좌표를 플레이어가 전달받아 상태가 바뀌게..
                    _sakuraObj.GetComponent<PlayerFSM>().MoveTo(_hitInfo.point);

                    _groundFlag = true;

                }
                //Debug.Log("RAY VALUE" + _ray);

                //# 공룡이면
                else if (_hitInfo.collider.gameObject.tag.Contains("Dinosaur"))
                {

                    _sakuraObj.GetComponent<PlayerFSM>().AttackDinosaur(_hitInfo.collider.gameObject);

                    _groundFlag = false;


                    ////Debug.Log("공룡");


                }
            }
        }
    }
}
