using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.31 

// NOTE: //# 3D 게임 - Mouse cusor Lock 컨트롤
//#          1) 
//#          2) 
//#          3) 

//~ ------------------------------------------------------------------------
public class MouseLock : MonoBehaviour
{

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        
    }

    //~ ------------------------------------------------------------------------
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UIManager.instance._onUIFlag)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
