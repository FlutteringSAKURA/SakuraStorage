using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! NOTE: JUST TEST Script about GUI
[ExecuteInEditMode]
public class KrakenGUIMgr : MonoBehaviour {
	public bool debugMode = false;
	public Rect position =  new Rect(200,15,150,25);
	public string text = " TEST TEXT ";

	public GUISkin krakenSkin = null;

	private void OnGUI() {
		if(debugMode || Application.isPlaying)
		{
			GUI.skin = krakenSkin;
			GUI.Label(position, text);
		}
	}
  
}
