using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitterMotionControl : MonoBehaviour
{

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();		
		
		Debug.Log("Anim Ctrl Activate");
    }
 
    void Update()
    {
		if(Input.GetMouseButtonDown(0))
		{	
			Debug.Log("Animation Change");
			_anim.SetBool("Swing", true);
		}
		else
		{
			_anim.SetBool("Swing",false);
		}
    }

}
