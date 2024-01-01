using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public int damage = 20;
    public float speed = 1000.0f;

    // [ 유탄 코드 ]
    public GameObject expEffect;
    private Transform tr;
    public AudioClip expSfx;
    // ============================ 유탄 코드 End

    private void Start()
    {
        // [ 유탄 코드 ] 
        tr = GetComponent<Transform>();
        // ===================================== 유탄 코드 End
        // 테스트 한 뒤 주석처리 후 아래 코드 작성
        //GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        this.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward 
            + Vector3.up * 0.1f) * speed);
        // ============================= 주석 처리전 테스트 코드 End

        // [ 유탄 코드 ]
        StartCoroutine(this.BulletTimer());
    }

    private IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(3.0f);
        ExpBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ExpBullet();
    }

    private void ExpBullet()
    {
        GameMgr.instance.PlaySfx(tr.position, expSfx); // 폭발 사운드 출력
        GameObject exp = (GameObject)Instantiate(expEffect, tr.position, Quaternion.identity);
        Destroy(exp, 5.0f);

        // 폭발력이 반경 10미터안에 미침
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);
        foreach (Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                rbody.mass = 1.34f;
                rbody.AddExplosionForce(50.0f, tr.position, 10.0f, 500.0f); // 폭발힘, 위치, 반경, ?
                coll.gameObject.SendMessage("OnExpDamage", SendMessageOptions.DontRequireReceiver);                
            }
            Destroy(gameObject, 8.0f);
        }
    }
    // ================== 유탄 코드 END
}
