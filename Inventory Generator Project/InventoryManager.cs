using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System;

//! Add Item Test Code
public class InventoryManager : MonoBehaviour
{

    public InputField _addItemInputField;
    public Button _addItemButton;
    public Text _msgText;
    public Inventory inventory;    
     public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    int slotAmount;
    ItemDatabase database;
    // private GameObject inventory;

    // private void Start()
    // {
    //     inventory = GameObject.Find("Inventory");

    // }

    public void AddItemButtonClick()
    {       

        if (string.IsNullOrEmpty(_addItemInputField.text.Trim()))
        {
            _msgText.text = "<b>A general system error occurred: Cannot get Item info</b>";
            return;
        }
        else if (_addItemInputField.text == "0")
        {
            Debug.Log("Add Item(0)");

            //  inventory.SendMessage("AddItem(1)", SendMessageOptions.DontRequireReceiver);
            _msgText.text = "<b>Info: Success Add *** Death Sword!!!</b>";
            inventory.AddItem(0);
        }
        else if (_addItemInputField.text == "1")
        {
            Debug.Log("Add Item(1)");

            //  inventory.SendMessage("AddItem(1)", SendMessageOptions.DontRequireReceiver);
            _msgText.text = "<b>Info: Success Add *** Bloody Axe!!!</b>";
            inventory.AddItem(1);
        }
        //! Additional Code
        else if (_addItemInputField.text == "2")
        {
            Debug.Log("Add Item(2)");

            // inventory.SendMessage("AddItem(2)", SendMessageOptions.DontRequireReceiver);
            _msgText.text = "<b>Info: Success Add *** Heavy Spike Hammer!!!</b>";
            inventory.AddItem(2);
        }
        else if(_addItemInputField.text == "3")
        {       
            _msgText.text = "<b>Info: Success Add *** Baphomet's Horn!!!</b>";
            inventory.AddItem(3);
        }
        else if(_addItemInputField.text == "4")
        {       
            _msgText.text = "<b>Info: Success Add *** Neptune's Dagger!!!</b>";
            inventory.AddItem(4);
        }
        else if(_addItemInputField.text == "5")
        {       
            _msgText.text = "<b>Info: Success Add *** Satan's Left teeth!!!</b>";
            inventory.AddItem(5);
        }
        else if(_addItemInputField.text == "6")
        {       
            _msgText.text = "<b>Info: Success Add *** Lord of Destruction Bael's Tentacle!!!</b>";
            inventory.AddItem(6);
        }
        else if(_addItemInputField.text == "7")
        {       
            _msgText.text = "<b>Info: Success Add *** Hercules's immotal helmat!!!</b>";
            inventory.AddItem(7);
        }
        else if(_addItemInputField.text == "8")
        {       
            _msgText.text = "<b>Info: Success Add *** The Red Riding Hood!!!</b>";
            inventory.AddItem(8);
        }
        else if(_addItemInputField.text == "9")
        {       
            _msgText.text = "<b>Info: Success Add *** Jelly Fishe!!!</b>";
            inventory.AddItem(9);
        }
        else if(_addItemInputField.text == "10")
        {       
            _msgText.text = "<b>Info: Success Add *** Strawberry Potion!!!</b>";
            inventory.AddItem(10);
        }
        else if(_addItemInputField.text == "11")
        {       
            _msgText.text = "<b>Info: Success Add *** Sticky eyes of Dead body!!!</b>";
            inventory.AddItem(11);
        }
        else if(_addItemInputField.text == "12")
        {       
            _msgText.text = "<b>Info: Success Add *** A Corrupt Frog!!!</b>";
            inventory.AddItem(12);
        }
        else if(_addItemInputField.text == "13")
        {       
            _msgText.text = "<b>Info: Success Add *** The Devil Contract!!!</b>";
            inventory.AddItem(13);
        }
        else if(_addItemInputField.text == "14")
        {       
            _msgText.text = "<b>Info: Success Add *** Slime's Poison!!!</b>";
            inventory.AddItem(14);
        }
        else if(_addItemInputField.text == "15")
        {       
            _msgText.text = "<b>Info: Success Add *** Arachne's 7th Claw!!!</b>";
            inventory.AddItem(15);
        }
        else
        {
            _msgText.text = "<b>Info: Please check again. This item code is not matched.</b>";
        }
        //! - - - - - - - - - - - - - -
    }



    // public void AddItem(int id)
    // {
    // 	Item itemToAdd = database.FetchItemByID(id);
    // 	if(itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
    // 	{
    // 		for (int i = 0; i < items.Count; i++)
    // 		{
    // 			if(items[i].ID == id)
    // 			{
    // 				ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
    // 				data.amount++;
    // 				data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
    // 				break;
    // 			}
    // 		}
    // 	}
    // }

    // private bool CheckIfItemIsInInventory(Item item)
    // {
    //     for (int i = 0; i < items.Count; i++)
    // 	{
    // 		if(items[i].ID == item.ID)			
    // 			return true;			
    // 		return false;
    // 	}
    // }
}
