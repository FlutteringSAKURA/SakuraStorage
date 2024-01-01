using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(AudioSource))]

// Update: //@ 2023.10.16 
// Update: //@ 2023.10.17 

// NOTE: //# 3D 게임 - 타겟 관리자
//#          1) 타겟 생명력 구현 + 타겟을 총알로 맞출 경우 랜덤 데미지 값을 줌      // Completed:
//#          2) 이펙트 구현      // Completed:
//#          3) 폭발 사운드 구현        // Completed:
//#          4) 승리 조건 구현      // Completed:
//#          5) 

//~ ---------------------------------------------------------
public class TargetManager : MonoBehaviour
{
    public GameObject _targetExplosion;
    public GameObject _targetHitEffect;

    public AudioClip _targetExplosionSound;
    AudioSource _audioBox;
    public int _targetHp = 100;

    static int _targetTotalNumbers = 0;

//~ ---------------------------------------------------------
    private void Awake()
    {
        _targetTotalNumbers++;  //# 이 스크립트를 가지고 있는 객체당 넘버 (= 1 할당)
    }
//~ ---------------------------------------------------------
    private void Start()
    {
        _audioBox = GetComponent<AudioSource>();


    }
//~ ---------------------------------------------------------
    private void Update()
    {

    }
//~ ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag.Contains("Bullet"))
        // {

        // } 

        //# NOTE: if구문의 연산 부담을 조금이라도 덜어 줄 수 있는 코드
        GameObject _hitObject = other.gameObject;
        if (_hitObject.GetComponent<BulletManager>() == null)     //^ 총알이 아니면 이라는 뜻(=오브젝트에 총알 스크립트가 없는 경우)
        {
            return; //^ 총알아니면 무시.
        }
        //# ------------------------------------------------

        int _randomDam = Random.Range(10, 31);

        _targetHp -= _randomDam;
        Instantiate(_targetHitEffect, transform.position, transform.rotation);
        //AudioSource.PlayClipAtPoint(_targetExplosionSound, transform.position);

        Debug.Log(_targetHp);

        if (_targetHp <= 0)
        {
            //@ 승리조건 (1-1)
            _targetTotalNumbers--;
            if (_targetTotalNumbers <= 0)
            {
                GameManagerSrc.SetStageClear();
                Debug.Log("스테이지 클리어");
            }

            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            Instantiate(_targetExplosion, transform.position, transform.rotation);
            //float _audioLength = _audioBox.clip.length;     // 오디오 클립 길이
            _audioBox.PlayOneShot(_targetExplosionSound);
            Destroy(gameObject, 1.8f);
        }

        {

        }

    }
}
