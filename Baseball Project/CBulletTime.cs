using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBulletTime : MonoBehaviour {
		
	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "BaseBall"){
			Time.timeScale = 0.25f;
			StartCoroutine(GoToNormalTime());
			Physics.IgnoreCollision(other.gameObject.GetComponent<SphereCollider>(), gameObject.GetComponent<BoxCollider>());
		}
	}

    private IEnumerator GoToNormalTime()
    {
		yield return new WaitForSeconds(2f);
		Time.timeScale = 1f;
    }
}
