using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.21

// NOTE:  //# 미사일 컨트롤러
//#          1) 
//#          2) 
//#          3) 
//#          4) 
//#          5)

public class MissileController : MonoBehaviour
{
    public float missileSpeed = 10.0f;

    private void Update()
    {
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }
}
