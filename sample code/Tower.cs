using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# Code for Final Integration Test Sample
//# -> Terret A.I. 구현 (센싱과 인지 결합)
public class Tower : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float fireSpeed = 2.0f;
    private float fireCounter = 0.0f;
    private bool canFire = true;

    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private GameObject projectile;
    private bool isLockedOn = false;

    public bool LockedOn
    {
        get { return isLockedOn; }
        set { isLockedOn = value; }
    }

    private void Update()
    {
        if (LockedOn && canFire)
        {
            StartCoroutine(Fire());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ZombieGirl")
        {
			//Debug.Log("Zombie Chk!!");
            animator.SetBool("ZombieInRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ZombieGirl")
        {
            animator.SetBool("ZombieInRange", false);
        }
    }

    private void FireProjectile()
    {
        GameObject bullet = Instantiate(projectile, muzzle.position, muzzle.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(muzzle.forward * 400);
    }

    private IEnumerator Fire()
    {
        canFire = false;
        FireProjectile();
        while (fireCounter < fireSpeed) //발포간격
        {
            fireCounter += Time.deltaTime;
            yield return null;
        }
        canFire = true;
        fireCounter = 0.0f;
    }
}
