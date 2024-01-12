using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBat : MonoBehaviour
{
    private Rigidbody _batrbody;
    private float velocityMax = 200f;   

    private void Awake()
    {
        _batrbody = gameObject.GetComponent<Rigidbody>();
       
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);         
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "BaseBall")
        {
            _batrbody.mass = 200f;
            _batrbody.useGravity = true;

            float forceMultiplier = GetBatForce(coll.gameObject.GetComponent<Rigidbody>());
            Vector3 direction = (transform.position - coll.contacts[0].point).normalized;
            _batrbody.AddForce(direction * forceMultiplier, ForceMode.Impulse);

            StartCoroutine(RecoveryBatMass());
        }
    }
    

    private float GetBatForce(Rigidbody batRbody)
    {
        return batRbody.velocity.magnitude / velocityMax * 50f;
    }

    private IEnumerator RecoveryBatMass()
    {
        yield return new WaitForSeconds(2.5f);
        _batrbody.mass = 0.125f;
        Destroy(gameObject, 2.5f);
    }
}
