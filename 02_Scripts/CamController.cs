//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.05

// NOTE:  //# 카메라 효과
//#          1) 충격시 카메라 흔들림 효과
//#          2) 
//#          3) 
//#          4)
// TEST: //! 피격시 캠 흔들림 효과 구현 
//? 실패

public class CamController : MonoBehaviour
{
    public GameObject cam;
    GameObject playerObj;
    public Vector3 camPos;
    // Start is called before the first frame update
    void Start()
    {
        camPos = cam.transform.position;
        playerObj = GameObject.FindWithTag("Player");
        camPos = playerObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hitted()
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
            yield return new WaitForEndOfFrame();
        }
        cam.transform.position = camPos;
    }
}
