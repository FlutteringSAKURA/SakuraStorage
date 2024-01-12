using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSensorChecker : MonoBehaviour {

	public bool _sensorChk = false;


	private void OnTriggerEnter(Collider ohter) {
		if(ohter.gameObject.name == "GenerateBall_ONSensor")
		{
			if(!_sensorChk)
			_sensorChk = true;			
		}
		if(ohter.gameObject.name == "GenerateBall_OFFSensor")
		{
			_sensorChk = false;
		}		
	}	
}
