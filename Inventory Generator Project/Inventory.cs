using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    //! Add Item Test code - - - - - - - 
    public InventoryManager inventoryManager;
    //! - - - - - - - - - - - - - - - - - 

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        database = GetComponent<ItemDatabase>();
        //! Add Item Test Code - - - - - - - - - - - - - - - - - 
        // inventoryManager = GetComponentInChildren<InventoryManager>();
        //! - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        slotAmount = 16;
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;


        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        // AddItem(2);

        //Debug.Log(items[0].Title);
        Debug.Log(items[1].Title);
    }

    // //! Add Item Test Code - - - - - - - - - - - - - - - - - 
    //     private void Update() {
    //         if (inventoryManager._addItemInputField.text.Trim() == "0")
    //         {
    //             AddItem(0);


    //         }
    //         if (inventoryManager._addItemInputField.text.Trim() == "1")
    //         {
    //             AddItem(1);

    //         }

    //     }   
    //! - - - - - - - - - - - - - - - - - - - - - - - - - - -     
    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
        {
           // Debug.Log("SendMessage Recive chk");

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {                   
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    Debug.Log("Bug Here!!!!!!");
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().amount = 1;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    //! Change Code -> Add Item Test Code - - - - - - - - - - -
                    itemObj.transform.position = Vector2.zero;
                    itemObj.transform.position = slots[i].transform.position;                    
                    //! - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

    //! Remove Item Test Code - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    // public void RemoveItem(int id)
    // {
    //     Item itemToRemove = database.FetchItemByID(id);
    //     if (itemToRemove.Stackable && CheckIfItemIsInInventory(itemToRemove))
    //     {
    //         for (int j = 0; j < items.Count; j++)
    //         {
    //             if (items[j].ID == id)
    //             {
    //                 ItemData data = slots[j].transform.GetChild(0).GetComponent<ItemData>();
    //                 data.amount--;
    //                 data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
    //                 if (data.amount == 0)
    //                 {
    //                     Destroy(slots[j].transform.GetChild(0).gameObject);
    //                     items[j] = new Item();
    //                     break;
    //                 }
    //                 if (data.amount == 1)
    //                 {
    //                     slots[j].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "";
    //                     break;
    //                 }
    //                 break;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < items.Count; i++)
    //         {
    //             if (items[i].ID != -1 && items[i].ID == id)
    //             {
    //                 Destroy(slots[i].transform.GetChild(0).gameObject);
    //                 items[i] = new Item();
    //                 break;
    //             }
    //         }
    //     }
    // }

    // //! This is quick and dirty! 
    public void RemoveItem(int id)
    {
        Item itemToRemove = database.FetchItemByID(id);
        if (CheckIfItemIsInInventory(itemToRemove))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (items[i].Stackable && data.amount > 1)
                    {
                        // We should have a more efficient way of doing this for sure

                        data.amount -= 1;
                        // Same goes for this one. Perhaps have a setter on ItemData.amount update the text.
                        // But a better option is an event system that updates the UI on item change
                        // Works for now, though!
                        data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    }
                    else
                    {
                        // As with the rest of this inventory, this reference could be cleaned up.
                        slots[i].GetComponent<Slot>().id = i;
                        items[i] = new Item();
                        Destroy(data.gameObject);

                        Debug.LogWarning("Done!! Couldn't find item to remove anymore: " + id);
                    }
                    break;
                }
            }
        }
    }
    //! - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    bool CheckIfItemIsInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].ID == item.ID)
                return true;
        return false;
    }

}
