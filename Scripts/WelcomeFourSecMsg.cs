using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeFourSecMsg : MonoBehaviour
{
    WelcomeFourSecMsg Welcom4sec;
    public GameObject SpawnManager;
   // public bool showMsg = false;

    private void Start()
    {
        Welcom4sec = GetComponent<WelcomeFourSecMsg>();
    }

    private void OnTriggerEnter(Collider coll)
    {
    //    showMsg = true;
        if (coll.gameObject.tag == "Player")
        {
          //  showMsg = true;
            SpawnManager.GetComponentInChildren<SpawnPointCtrl>().enabled = true; // Call to Create Hell Monster
            DispMsg.dispMessage(" 으음..?! 무슨소리지??? \n 괴생물체들의 날카로운 울부짖음이 매우 가까이에서 들린다.");
            StartCoroutine(DeleteMsg());
        }           
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        showMsg = false;
    //    }
    //}

    private IEnumerator DeleteMsg()
    {
        yield return new WaitForSeconds(2.5f);
        DispMsg.flgDisp = false;
       Destroy(Welcom4sec, 0.5f);
    }
}
