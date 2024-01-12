using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBallSpinning : MonoBehaviour
{
	public float _spinningSpeed = 10.0f;


	private void Update() 
	{	
		// Spin BaseBall
		transform.Rotate(Vector3.up * _spinningSpeed * Time.deltaTime);
	}
	
}
