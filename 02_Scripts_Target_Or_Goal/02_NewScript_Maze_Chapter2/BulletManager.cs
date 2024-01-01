using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.16 

// NOTE: //# 3D 게임 - 총알 관리자 스크립트
//#          1) 총알 자체 회전                  // Completed:
//#          2) 총알이 충돌 발생시 총알 이펙트 발생 및 디스트로이   // Completed:
//#          3) 타이머 기능 // Completed:
//#          4) 총알 사운드 구현 // Completed:
//#          5) 

//~ ---------------------------------------------------------
public class BulletManager : MonoBehaviour
{
    public float _rotSpeedZY = 1600f;
    public float _bulletSpeed = 10.0f;
    public GameObject _bulletEffect;     //^ 총알 충돌 이펙트
    public AudioClip _bulletCrushSound;     //^ 총알 충돌 사운드
    public AudioSource _audioBox;

    private void Start()
    {
        _audioBox.GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, _rotSpeedZY * Time.deltaTime);
        transform.Rotate(0f, _rotSpeedZY * Time.deltaTime, 0f);
        //*  NOTE: Vector3.forward == new Vector3(0,0,1)
        //*        Vertor값을 정규화해주는 함수 == Vector3.Normalize
        Vector3 _vecAddPos = (Vector3.forward * _bulletSpeed);
        transform.position += ((transform.rotation * _vecAddPos) * Time.deltaTime);
    }
    //~ ---------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (_bulletEffect != null)
        {
            Instantiate(_bulletEffect, transform.position, transform.rotation);
            //^ 사운드 효과
            BulletCushSoundPlay();
            //! 충돌후 벽 뒤로 뚫고 나가 다른 오브젝트들과 충돌하는 연쇄효과 방지코드
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        
        Destroy(gameObject,1.0f);
    }

    void BulletCushSoundPlay()
    {
        _audioBox.PlayOneShot(_bulletCrushSound);
        Debug.Log("충돌사운드");
    }
}
