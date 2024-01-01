//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.06 
// Update: //@ 2023.11.07 

// NOTE: //# 3D 게임 - Cyber Creature Fire 컨트롤러


[RequireComponent(typeof(AudioSource))]

public class CyberCreatureFireCtrl : MonoBehaviour
{
    MeshRenderer _muzzleFlashMesh;
    //public Transform _muzzlePos;
    public Transform _firePivot;

    public GameObject _creatureBullet;
    GameObject _playerObj;

    public AudioClip _fireSound;
    public AudioClip _reloadSound;

    AudioSource _audioBox;

    public float _timeFlow = 0.0f;
    public float _coolTime = 2.0f;

    private readonly float _reloadTime = 2.0f;
    private readonly int _maxBullet = 5;
    private int _currentBullet = 5;
    public bool _isReload = false;
    //public bool _isFire = false;
    Animator _creatureAnimator;

    readonly int _hashReload = Animator.StringToHash("Reload");

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _playerObj = GameObject.FindWithTag("Player");
        _muzzleFlashMesh = _firePivot.GetComponentInChildren<MeshRenderer>();
        _muzzleFlashMesh.enabled = false;

        _audioBox = GetComponent<AudioSource>();

        _creatureAnimator = GetComponent<Animator>();
    }

    //~ ------------------------------------------------------------------------

    private void Update()
    {

    }


    //~ ------------------------------------------------------------------------

    //@ 애니메이션 이벤트로 함수 호출 
    private void GunFire()
    {
        if (!gameObject.GetComponent<CyberCreatureController>()._shootFlag)
            return;
        GameObject _cyberCreatureBullet = Instantiate(_creatureBullet, _firePivot.position, _firePivot.rotation);
        StartCoroutine(ShowMuzzleFlash());
        SoundManager.instance.PlaySfx(gameObject.transform.position, _fireSound);

        Destroy(_cyberCreatureBullet, 3.0f);

        //% 총알이 없는 경우
        _isReload = (--_currentBullet % _maxBullet == 0);

        Debug.Log("현재 남은 총알 갯수 : " + _currentBullet);
        if (_isReload)
        {
            StartCoroutine(ReloadBullet());

            Debug.Log("reload");
        }

    }

    IEnumerator ShowMuzzleFlash()
    {
        _muzzleFlashMesh.enabled = true;
        Vector2 _offSet = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        float _muzzleScale = Random.Range(2.0f, 3.5f);
        _muzzleFlashMesh.transform.localScale = Vector3.one * _muzzleScale;
        Quaternion _muzzleRot = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        _muzzleFlashMesh.transform.localRotation = _muzzleRot;

        yield return new WaitForSeconds(Random.Range(0.08f, 0.18f));
        _muzzleFlashMesh.enabled = false;
    }

    IEnumerator ReloadBullet()
    {
        //% 머즐플래시 비활성화
        _muzzleFlashMesh.enabled = false;
        //% 재장전 애니메이션 실행
        _creatureAnimator.SetBool(_hashReload, true);

        //% 재장전 사운드 재생
        yield return new WaitForSeconds(1.5f);
        SoundManager.instance.PlaySfx(gameObject.transform.position, _reloadSound);
        //_audioBox.PlayOneShot(_reloadSound, _reloadSound.length);
        //% 제어권 잠시 양보
        yield return new WaitForSeconds(2.0f);
        //% 총알갯수 초기화
        _currentBullet = _maxBullet;
        _isReload = false;


    }

}
