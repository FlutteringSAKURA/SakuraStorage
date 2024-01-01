using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	//public GameObject target; // [ TEST CODE ]시 주석처리
	UnityEngine.AI.NavMeshAgent agent;
	Animator animator;

    // TEST CODE for Enemy in Maze
    private GameObject playerTarget;
    // ================================== test code end

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
        // [ TEST code for Enemy in Maze
        playerTarget = GameObject.FindGameObjectWithTag ("Player");
        // ============================================================ test code end
	}
	
	// Update is called once per frame
	void Update () {
		agent.destination = playerTarget.transform.position;
		animator.SetFloat("Speed", agent.velocity.magnitude);

        // TEST CODE for Enemy in Maze
        if (Game.IsStageCleared())
        {
            agent.isStopped = true;
        }
        // ============================= test code end
	}


    // TEST CODE for Enemy in Maze
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Lose");
        }
    }
   
}
