using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Dangerous Zone Main Ctrl Script

public abstract class DangerousZoneCtrl : MonoBehaviour
{
    public bool _enterThePlayer = false;

    [SerializeField]
    protected float waitTime = 0.0f;

    protected GameObject _playerObject;

    protected virtual void Awake()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Start()
    {      
    }

    protected virtual void Update()
    {
        waitTime -= Time.deltaTime; // Time 정규화 

    }

    public abstract void DangerousAction();




    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && waitTime >= 10.0f)
        {
            _enterThePlayer = true;

        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _enterThePlayer = false;
        }
    }

}
