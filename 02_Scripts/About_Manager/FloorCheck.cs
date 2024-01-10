using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.22 
//# NOTE: 발소리를 내기 위해 바닥의 종류를 체크하는 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class FloorCheck : MonoBehaviour
{
    public static FloorCheck instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public bool _woodType = false;
    public bool _tileType = false;
    public bool _grassType = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.tag == "WoodenFloor")
        {
            FloorCheck[] _object_Have_FloorCheck = GameObject.FindObjectsOfType<FloorCheck>();
            for (int i = 0; i < _object_Have_FloorCheck.Length; i++)
            {
                _object_Have_FloorCheck[i].GetComponent<FloorCheck>()._woodType = true;
                _object_Have_FloorCheck[i].GetComponent<FloorCheck>()._tileType = false;
            }

        }

        if (other.gameObject.CompareTag("PatJi") && this.gameObject.tag == "TileFloor")
        {
            FloorCheck[] _object_Have_FloorCheck = GameObject.FindObjectsOfType<FloorCheck>();
            for (int i = 0; i < _object_Have_FloorCheck.Length; i++)
            {
                _object_Have_FloorCheck[i].GetComponent<FloorCheck>()._tileType = true;
                _object_Have_FloorCheck[i].GetComponent<FloorCheck>()._woodType = false;
            }
        }


    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PatJi") && this.gameObject.tag == "Ground")
        {
            FloorCheck[] _object_Have_FloorCheck = GameObject.FindObjectsOfType<FloorCheck>();
            for (int i = 0; i < _object_Have_FloorCheck.Length; i++)
            {
                _object_Have_FloorCheck[i].GetComponent<FloorCheck>()._grassType = true;
            }
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("PatJi") && this.gameObject.tag == "WoodenFloor")
    //     {
    //         GameObject[] _woodArea = GameObject.FindGameObjectsWithTag("WoodenFloor");
    //         for (int i = 0; i < _woodArea.Length; i++)
    //         {
    //             _woodArea[i].GetComponent<FloorCheck>()._woodType = false;
    //         }

    //     }

    //     if (other.gameObject.CompareTag("PatJi") && this.gameObject.tag == "TileFloor")
    //     {
    //         GameObject[] _tileArea = GameObject.FindGameObjectsWithTag("TileFloor");
    //         for (int i = 0; i < _tileArea.Length; i++)
    //         {
    //             _tileArea[i].GetComponent<FloorCheck>()._tileType = false;
    //         }
    //     }
    // }

}
