using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMiss : MonoBehaviour {
	[Header("This Script will be Destroy an Object which pass by MissZone")]

	public AudioSource src;
	private void OnTriggerEnter(Collider other) {
		src.Play();
		Destroy(other.gameObject);
	}
}
