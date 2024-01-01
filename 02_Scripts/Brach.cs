using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brach : MonoBehaviour
{
    public float _moveSpeed = 0.7f;

    private void Start() {
        //transform.position += new Vector3(0,2,0);
    }

    private void Update() {
        transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    }
}
