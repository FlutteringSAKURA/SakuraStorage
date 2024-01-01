using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectController : MonoBehaviour
{
    int insectPower = 100;
    public bool isAlive = true;
    public float insectSpeed = 1.0f;

    private void Start() {
        
    }
    private void Update() {
        transform.Translate(0,0,insectSpeed*Time.deltaTime);
    }
}
