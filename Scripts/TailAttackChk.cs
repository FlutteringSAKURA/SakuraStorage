using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailAttackChk : MonoBehaviour
{
    public GameObject _sharkObject;
   

    private void OnTriggerEnter(Collider coll)
    {
        if( coll.gameObject.tag == "Player")
        {
            if (coll.gameObject.tag == "Player")
            {
                // Debug.Log("CHHHHHHHHHHHHHHHHHHHHHK TAIL");
                _sharkObject.GetComponent<SharkCtrl>()._swingTail = true;
            }
            else
            {
                _sharkObject.GetComponent<SharkCtrl>()._swingTail = false;
            }
        }
     
    }
}
