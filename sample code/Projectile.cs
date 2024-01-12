using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# Code for Final Integration Test Sample
//# -> Explosion 구현
public class Projectile : MonoBehaviour
{

    [SerializeField]
    private GameObject explosionPrefab;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ZombieGril" || other.tag == "Environment")
        {
            if (explosionPrefab == null)
            {
                return;
            }
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
        }
    }
}
