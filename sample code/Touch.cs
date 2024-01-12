using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 센서 구현 관련 - 촉각

public class Touch : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            //특성검사
            if (aspect.aspectName == aspectName)
            {
                print("Fresh meat Touch Detected");
            }
        }
    }

}
