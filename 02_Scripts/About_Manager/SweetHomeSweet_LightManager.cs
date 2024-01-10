using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.30 
//# NOTE: SweetHomeSweet 씬의 Light를 제어를 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class SweetHomeSweet_LightManager : MonoBehaviour
{
    public GameObject _kongjiBedSpotLight;

    public void TurnOffTheLight()
    {
        _kongjiBedSpotLight.SetActive(false);
    }
}
