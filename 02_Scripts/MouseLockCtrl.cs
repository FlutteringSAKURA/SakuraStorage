using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10

// NOTE: //# 마우스 락과 관련된 제어 스크립트       .. TEST 연습용
//#          1) 
//#          2) 
//#          3) 
//#          4) 
//#          5) 

public class MouseLockCtrl : MonoBehaviour
{
    public float floatX = 0f;
    public float floatY = 0f;
    public float valueSpeed = 100f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float keyHorizontal = Input.GetAxis("Horizontal");

        floatX += mouseX * Time.deltaTime * valueSpeed + keyHorizontal;     //& 가로축
        floatY += mouseY * Time.deltaTime * valueSpeed;     //& 세로축 

        floatX %= 360;
        floatY %= 360;

        transform.eulerAngles = new Vector3(-floatY, floatX, 0f);
        transform.eulerAngles = new Vector3(0, floatX, 0f);
    }
}
