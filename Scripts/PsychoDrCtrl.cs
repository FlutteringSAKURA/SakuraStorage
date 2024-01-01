using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychoDrCtrl : FireCtrl
{
    public Transform _firePivot;
    // public GameObject bullet;
    [SerializeField]
    private float _fireTime = 0.0f;
    //  public float _nextFire = 0.1f;

    public bool _nowFire = false;


    private void Start()
    {
        //  _rayCast = GetComponent<RaycastHit>();
        // ray = GetComponent<Ray>();
        muzzleFlash.enabled = false; 
    }

    private void Update()
    {
        Debug.DrawRay(_firePivot.position, _firePivot.forward * 20.0f, Color.red);
        // if(Physics.Raycast(_firePivot.position, _firePivot.position.))

        _fireTime += Time.deltaTime; // 타임 정규화

        //RaycastHit hit;
        //if (Physics.SphereCast(_firePivot.position, 10.0f, _firePivot.forward, out hit, Mathf.Infinity))
        //{
        //    if (hit.collider.tag == "Player" && _fireTime >= nextFire)
        //    {

                
        //        CreateBullet();
        //        _fireTime = 0.0f;
        //        //    GameObject spark = (GameObject)Instantiate(sparkEffect, _firePivot.position, Quaternion.identity);

        //        //  StartCoroutine(CreateBullet());
        //        StartCoroutine(ShowMuzzleFlash());
        //    }
        
        //}
  
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && _fireTime >= nextFire)
        {
            CreateBullet();
            _fireTime = 0.0f;
            //    GameObject spark = (GameObject)Instantiate(sparkEffect, _firePivot.position, Quaternion.identity);

            //  StartCoroutine(CreateBullet());
            StartCoroutine(ShowMuzzleFlash());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && _fireTime >= nextFire)
        {
            CreateBullet();
            _fireTime = 0.0f;
            //    GameObject spark = (GameObject)Instantiate(sparkEffect, _firePivot.position, Quaternion.identity);

            //  StartCoroutine(CreateBullet());
            StartCoroutine(ShowMuzzleFlash());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _nowFire = false;
        }
    }

    private IEnumerator ShowMuzzleFlash()
    {
        Debug.Log("Muzzle Flash Active");
       // muzzleFlash.enabled = true;
      //  muzzleFlash.GetComponent<MeshRenderer>().enabled = true;
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        muzzleFlash.enabled = false;

    }

    private void  CreateBullet()
    {
        _nowFire = true;   
       // yield return new WaitForSeconds(1.5f);
        Instantiate(bullet, _firePivot.position, _firePivot.rotation);
       // muzzleFlash.enabled = true;
     //   CreateBullet();
        GameObject spark = (GameObject)Instantiate(sparkEffect, _firePivot.position, Quaternion.identity);
        Destroy(spark, 0.6f);
    }
   
}
