using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.06 

// NOTE: //# 3D 게임 - Tentacles Zone Manager
//#             1) 

public class TentaclesZone : MonoBehaviour
{
    public GameObject _tentaclesBox;
    public bool _tentacleStartFlag = false;

    private void Start()
    {
        _tentaclesBox = GameObject.Find("TentaclesBox");
        _tentaclesBox.SetActive(false);
        _tentacleStartFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("TentacleZone Creature");
        if (other.gameObject.tag.Contains("Player"))
        {
            if (!_tentacleStartFlag)
            {
                _tentaclesBox.SetActive(true);
                _tentacleStartFlag = true;
            }

            //StartCoroutine(GenCreature());
            //GameManagerScript.instance.TentacleZoneCreatureGenerate();
            if (GameManagerScript.instance._tentacleZoneTransformPoints.Length > 0)
            {
                // StartCoroutine(GenCreature());
                InvokeRepeating("GenCreature", 1.5f, 3.0f);

            }

        }

    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.tag.Contains("Player"))
    //     {

    //         if (GameManagerScript.instance._tentacleZoneTransformPoints.Length > 0)
    //         {
    //             StartCoroutine(GenCreature());
    //         }

    //     }
    // }

    private void GenCreature()
    {
        //yield return new WaitForSeconds(1.5f);
        GameManagerScript.instance.SendMessage("TentacleZoneCreatureGenerate", SendMessageOptions.DontRequireReceiver);

    }
}
