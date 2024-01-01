using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    public Animator anim;
    public bool _openChk = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

 
    private void OnTriggerEnter(Collider collision)
    {      
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "MONSTER")
        {
            _openChk = true;

            anim.SetTrigger("openTheDoor");

            anim.SetTrigger("gateLeftDoorOpen");
            anim.SetTrigger("gateRightDoorOpen");
            anim.SetTrigger("gateDoorAction");
            // anim.Play("openTheDoor");
        }
        
        //if (collision.gameObject.tag == "Player")
        //{
        //    _openChk = true;

        //    //anim.SetTrigger("gateLeftDoorOpen");
        // // anim.Play("gateLeftDoorOpen");

        //}
        //if (collision.gameObject.tag == "Player")
        //{
        //    _openChk = true;

        //    //anim.SetTrigger("gateRightDoorOpen");
        // // anim.Play("gateRightDoorOpen");
        //}
    }
    
    
    private void OnTriggerExi(Collider collision)
    {
        _openChk = false;        
    }
}
