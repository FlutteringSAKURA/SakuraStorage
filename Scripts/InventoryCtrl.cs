using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 

public class InventoryCtrl : MonoBehaviour
{
    public GameObject _inventory;
    public bool _inventoryOpenChk = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryOpenChk = !_inventoryOpenChk;

            if (_inventoryOpenChk)
            {

                _inventory.SetActive(true);
            }
            else if (!_inventoryOpenChk)
            {
                _inventoryOpenChk = false;
                _inventory.SetActive(false);
            }
        }

  
    }




}
