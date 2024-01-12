using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDistanceManager : MonoBehaviour {

	public Text _distText;
	public GameObject _distPanel;
	public GameObject _foulPanel;
	public static CDistanceManager instance = null;

	private void Awake() {
		if (instance == null)
            instance = this;
 
		else if (instance != this)
			Destroy(gameObject);    
		InitGame();
	}

	private void Start() {
		_distPanel.SetActive(false);
		_foulPanel.SetActive(false);
	}

	private void InitGame()
	{

	}

	public void UpdateDistance(float dist)
	{
		_distPanel.SetActive(true);
		_foulPanel.SetActive(false);
		_distText.text = "DISTANCE : " + dist.ToString();
	}
	// public void FoulCheck(string _text)
	// {
	// 	_distText.text =  _text;
	// }
}
