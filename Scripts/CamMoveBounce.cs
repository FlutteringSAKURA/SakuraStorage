using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMoveBounce : MonoBehaviour
{
    public Animator camAnim;
    public bool _dynamicMode = false;
   // PlayerCtrl _playerCtrlScript;

    private void Start()
    {
        camAnim = GetComponent<Animator>();
     //   _playerCtrlScript = GetComponent<PlayerCtrl>();    
    }

    private void Update()
    {
      //  Action();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _dynamicMode = !_dynamicMode;
        }
        if (_dynamicMode && Input.GetKeyDown(KeyCode.W))
        {
            Action();
        }
    }

    private void Action()
    {
        camAnim.SetTrigger("MoveValueFB");
     
    }
}
