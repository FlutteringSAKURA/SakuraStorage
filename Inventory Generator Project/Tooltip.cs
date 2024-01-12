using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Item item;
    private string data;
    private GameObject tooltip;

    private void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    private void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }
    public void Activate(Item item)
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }
    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        data = "<color=#0473f0><b>" + item.Title + "</b></color>\n\n" + item.Description + "\n\n<color=#ff002a><b>Power: </b></color>" + item.Power 
        + "\n\n<color=#0cff00><b>Recovery Hp: </b></color>" + item.Recovoery_Hp;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
}
