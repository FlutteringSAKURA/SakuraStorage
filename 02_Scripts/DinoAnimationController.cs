using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//~ ------------------------------------------------------------------------
public class DinoAnimationController : CharacterParams
{

    public GameObject _dinoFSM;

    // // private void Start() {

    // // }

    //~ ------------------------------------------------------------------------

    public void SendDinoAttack()
    {
        // // _dinoFSM.GetComponent<DinosaurFSM>().SendMessage("AttackCalculate")

        // //transform.parent.gameObject.GetComponent<DinosaurFSM>().AttackCalculate();
        transform.parent.gameObject.SendMessage("AttackCalculate");
        // //transform.parent.GetComponent<DinosaurFSM>().AttackCalculate();


    }
}
