using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float moveSpead = 5f;
	public float rotationSpeed = 360f;
    // TEST CODE
    public GameObject expEffect;
    public GameObject fireEffect;
    // =================== TEST CODE END
	
	CharacterController characterController;
	Animator animator;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {	
		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (direction.sqrMagnitude > 0.01f) {
			Vector3 forward = Vector3.Slerp(
				transform.forward,
				direction,
				rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction)
			);
			transform.LookAt(transform.position + forward);
		}
		characterController.Move(direction * moveSpead * Time.deltaTime);
		
		animator.SetFloat("Speed", characterController.velocity.magnitude);
		
		if (GameObject.FindGameObjectsWithTag("Dot").Length == 0) {
            //Application.LoadLevel("Win");
            SceneManager.LoadScene("Win");
		}
	}
	
	void OnTriggerEnter (Collider other)
    {
        // TEST CODE    
        GameObject exp = (GameObject)Instantiate(expEffect, other.transform.position, Quaternion.identity);
        Destroy(exp, 4.5f);
        GameObject fire = (GameObject)Instantiate(fireEffect, other.transform.position, Quaternion.identity);
        Destroy(fire, 4.5f);
        // ====================== TEST CODE END

        if (other.tag == "Dot") {
			Destroy(other.gameObject);
		}
		if (other.tag == "Enemy") {
            //Application.LoadLevel("Lose");
            SceneManager.LoadScene("Lose");
		}
	}
}
