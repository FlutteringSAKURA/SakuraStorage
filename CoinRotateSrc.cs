using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 - 코인 회전 제어 스크립트

//~ ---------------------------------------------------------
public class CoinRotateSrc : MonoBehaviour
{
    public float rotSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

//~ ---------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotSpeed * Time.deltaTime, 0);
    }
}
