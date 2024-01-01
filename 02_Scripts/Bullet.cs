//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.31 
// Update: //@ 2023.11.01 
// Update: //@ 2023.11.10 

// NOTE: //# 3D 게임 - 총알 컨트롤러
//#          1) 
//#          2) 
//#          3) 

//~ ------------------------------------------------------------------------
public class Bullet : MonoBehaviour
{
    public float _damage = 15.0f;
    public float _force = 1500.0f;
    public float _bulletSpeed = 20.0f;

    // Update: //@ 2023.11.10 
    //# 총알 초기화 변수
    Rigidbody _rigidBody;
    Transform _bulletTransform;
    TrailRenderer _bulletTrail;




    //~ ------------------------------------------------------------------------

    private void Awake()
    {
        // Legacy:
        _rigidBody = GetComponent<Rigidbody>();
        _bulletTransform = GetComponent<Transform>();
        _bulletTrail = GetComponent<TrailRenderer>();
        _damage = Random.Range(15, 25);
    }

    private void OnEnable()
    {
        // Update: //@ 2023.11.01 
        //# NOTE: 총알이 이동하는 것은 동일하나 RigidBody사용 위해 Legacy 방식 이용
        //transform.Translate(Vector3.forward * Time.deltaTime * _bulletSpeed);
        _rigidBody.AddForce(transform.forward * _force);     //& 로컬좌표(transform) 기준으로 RigidBody에 힘이 가해짐
        // _rigidBody.AddRelativeForce(Vector3.forward * _force);  //& 월드좌표(vec3) 기준으로 RigidBody에 힘이 가해짐
    }
    private void OnDisable()        //% Ondisable은 비활성화가 되면 작동하는 함수
    {
        _bulletTrail.Clear();
        _bulletTransform.position = Vector3.zero;
        _bulletTransform.rotation = Quaternion.identity;
        _rigidBody.Sleep();
    }

    private void Start()
    {

    }



    //~ ------------------------------------------------------------------------
    private void Update()
    {
        // Update: //@ 2023.11.10 
        StartCoroutine(InactiveBullet());
    }
    IEnumerator InactiveBullet()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }

    // Update: //@ 2023.11.10 
    //# 어떤 충돌 조건에서도 총알이 비활성화 되도록 
    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

}
