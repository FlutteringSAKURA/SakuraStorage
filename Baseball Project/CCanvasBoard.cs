using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CCanvasBoard : MonoBehaviour {

	public static CCanvasBoard instance = null;
	public Text vDirText;
	public Text hDirText;
	public Text speedText;
	public Text predictDistText;
	public Text actualDistText;
	public Text predictTimeText;
	public Text actualTimeText;

	public GameObject disPanel;
	public GameObject foulPanel;
	public GameObject leftPanel;
	public GameObject rightPanel;
	public GameObject cinhitPanel;
	public GameObject cinoutPanel;


	private void Awake() 
	{
		if(instance == null)
		{
			instance = this;
		}	
		else{
			Destroy(gameObject);
			InitGame();
		}
	}
	private void InitGame()
	{
	}

	public void closeCanvas()
	{
		disPanel.SetActive(false);
		foulPanel.SetActive(false);
		leftPanel.SetActive(false);
		rightPanel.SetActive(false);
		cinhitPanel.SetActive(false);
		cinoutPanel.SetActive(false);
	}
	public void showCanvas()
	{
		disPanel.SetActive(true);
		foulPanel.SetActive(true);
		leftPanel.SetActive(true);
		rightPanel.SetActive(true);
		cinhitPanel.SetActive(true);
		cinoutPanel.SetActive(true);
	}

}
