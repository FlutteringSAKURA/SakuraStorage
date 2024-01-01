//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.25

// NOTE:  //# 카메라 효과
//#          1) 충격시 카메라 흔들림 효과
//#          2) 
//#          3) 
//#          4)

public class CamController : MonoBehaviour
{
    public GameObject cam;
    public Vector3 camPos = Vector3.zero;   //^ zero == 초기의 카메라 위치값 설정 (0,0,0)
    // Start is called before the first frame update
    void Start()
    {
        // Hitted();
        camPos = cam.transform.position;
    }

    private void Hitted()
    {
        StartCoroutine(CamShake(0.5f, 0.2f));
    }

    IEnumerator CamShake(float tillTime, float shakeSense)
    {
        float timeFlow = 0.0f;
        float posX = 0.0f, posY = 0.0f;
        while (timeFlow < tillTime)
        {
            timeFlow += Time.deltaTime;
            posX = Random.Range(-1 * shakeSense, shakeSense);
            posY = Random.Range(-1 * shakeSense, shakeSense);
            cam.transform.position = new Vector3(posX, posY, -10);
            yield return new WaitForEndOfFrame();   //# 렌더링이 한번 끝나면
        }
        cam.transform.position = camPos;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
