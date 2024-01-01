using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ TEST for Msg Code ] Script

public class WelcomeHellMsg : MonoBehaviour
{
    public bool showMsg1 = false;
    public bool showMsg2 = false;

//    DispMsg CallMsg;
    WelcomeHellMsg WelcomeHell;
   // PlayerCtrl PlayerCondition;

    private void Start()
    {
       WelcomeHell = GetComponent<WelcomeHellMsg>();
      //  CallMsg = GetComponent<DispMsg>();
        //PlayerCondition = GetComponent<PlayerCtrl>();
    }


    private void OnCollisionEnter(Collision coll)
    {
       
        Debug.Log("Msg : Welcome to the HELL!!!");
        if (coll.gameObject.tag == "Player" && !showMsg2)
        {            
            showMsg1 = true;
            showMsg2 = false;
            DispMsg.dispMessage("무언가 이곳은 발에 닿는 감촉이 물컹물컹하고 \n 끈적끈적한 것이 불길함이 느껴지는군...");
            StartCoroutine(DeleteMsg());
        }            
    }
 
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("DESTROY CALL");
        showMsg1 = false;
        showMsg2 = false;
        Destroy(WelcomeHell, 1.0f);
        //if (collision.gameObject.tag == "Player")
        //{
           
           
        //}
    }

    // TEST
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !showMsg1 & !showMsg2)
        {
            DispMsg.dispMessage("으 아 아 아 아 아 아 아 악 ~ ~ ~");
            showMsg2 = true;

        }
    }
    // ==== test code end

    private IEnumerator DeleteMsg()
    {
        
        yield return new WaitForSeconds(2.5f);
        DispMsg.flgDisp = false;

        // DispMsg.dispMessage("");

    }
}