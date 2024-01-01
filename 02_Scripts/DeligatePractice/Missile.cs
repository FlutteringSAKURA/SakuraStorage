using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody _rigidBody;
    private void Start()
    {
        _rigidBody = GetComponentInChildren<Rigidbody>();
        ShootMissle();
    }

    void ShootMissle()
    {
        _rigidBody.AddForce(transform.forward * 500f);
        Destroy(gameObject, 5.0f);
    }

}
