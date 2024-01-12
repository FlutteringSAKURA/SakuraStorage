using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddItem : Inventory
{	
	public InputField _addItemInputField;
	public Text _msgText;
	
	private Inventory inv;
	private ItemDatabase database;
	
	private void Start() {
		inv = GameObject.Find("Inventory").GetComponent<Inventory>();
	}
	public void OnAddItemButtonClick()
	{
		if(string.IsNullOrEmpty(_addItemInputField.text.Trim()))
		{
			Debug.Log(_msgText.text = "아이템 정보를 입력하세요.");
			return;
		}
		OnAddItemButtonClick();
	}


}
