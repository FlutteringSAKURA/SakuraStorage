using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.31 

// NOTE: //# 3D 게임 - 벽 관련 스크립트
//#          1) 
//#          2) 
//#          3) 

//~ ------------------------------------------------------------------------
public class Wall : MonoBehaviour
{
    public GameObject _hitEffect;


    //~ ------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag.Contains("Bullet"))
        // {
        //     Instantiate(_hitEffect, other.transform.position, Quaternion.identity);
        //     Debug.Log("hit");

        //     ////Destroy(_hitEffect, 2.0f);
        //     Destroy(other.gameObject, 0.5f);
        // }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Bullet") || other.gameObject.tag.Contains("CreatureBullet"))    //& if(other.gameObject.CompareTag("Bullet"))
        {
            ContactPoint _contact = other.GetContact(0);   //& 첫번째 충돌지점의 정보
            //# NOTE:  Quaternion .. 회전없이 만드는 코드
            Quaternion _rot = Quaternion.LookRotation(-_contact.normal);

            Instantiate(_hitEffect, _contact.point, _rot);
            Debug.Log("hit");
            //// Destroy(_hitEffect, 2.0f);
            
            if (other.gameObject.tag.Contains("CreatureBullet"))
                Destroy(other.gameObject, 0.5f);
        }

    }
}
