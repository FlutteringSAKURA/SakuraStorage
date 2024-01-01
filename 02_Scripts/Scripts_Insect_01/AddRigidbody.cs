using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidbody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
