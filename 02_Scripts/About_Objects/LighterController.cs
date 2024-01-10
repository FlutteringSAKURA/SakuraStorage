using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.29 
//# NOTE: Lighter controll Script


//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class LighterController : MonoBehaviour
{
    public GameObject _grabPos;
    public GameObject _fireFlame;
    public Transform _dropPos;

    //~ -------------------------------------------------------------------------------
    private void Start()
    {
        _grabPos = GameObject.FindGameObjectWithTag("GrabPos_Lighter");
    }

    //~ -------------------------------------------------------------------------------

    public void AttachedHand()
    {
        _grabPos = GameObject.FindGameObjectWithTag("GrabPos_Lighter");
        gameObject.transform.parent = _grabPos.transform;
        transform.position = _grabPos.transform.position;
        transform.rotation = _grabPos.transform.rotation;

        StartCoroutine(OnFireFlame());
    }

    IEnumerator OnFireFlame()
    {
        yield return new WaitForSeconds(1.8f);
        _fireFlame.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        _fireFlame.SetActive(false);
    }

    public void DropLighter()
    {
        gameObject.transform.SetParent(_dropPos);
        gameObject.transform.position = _dropPos.position;
        gameObject.transform.rotation = _dropPos.rotation;

    }
}
