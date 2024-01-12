using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTimeCtrl : MonoBehaviour {

	public CBaseBall _baseBallScript;
	//private Rigidbody _baseBallRbody;

	private void Start() {
		//_baseBallRbody = GetComponent<Rigidbody>();
		// transform.position = transform.parent.position;
	}
	private void Update() {
		if(_baseBallScript._hitBatChk == true)
		{	//transform.parent.GetComponent<Rigidbody>();
			// _baseBallRbody.isKinematic = false;

			if(Time.timeScale == 1.0f)
			{
				Time.timeScale = 10.0f;
			//	StartCoroutine(RecoveryTimeScale());
			}
			else
			{
				Time.timeScale = 1.0f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
		}	
		else {
			Time.timeScale = 1.0f;
			// _baseBallRbody.isKinematic = true;
		}
	}

    // private IEnumerator RecoveryTimeScale()
    // {
	// 	yield return new WaitForSeconds(0.1f);
	// 	Time.timeScale = 1.0f;
    // }
}
