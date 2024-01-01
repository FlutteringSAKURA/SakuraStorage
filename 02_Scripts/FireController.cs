//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Update: //@ 2023.10.31 
// Update: //@ 2023.11.02 
// Update: //@ 2023.11.08 
// Update: //@ 2023.11.09 
// Update: //@ 2023.11.13 


// NOTE: //# 3D 게임 - Fire 컨트롤러
//#          1) 총알 발사 구현 (+사운드 +탄피효과)   Completed:
//#          2) 무기 타입별 사운드 변경 적용     Completed:
//#          3) 머즐플래시 구현  Completed:

//% 총알 발사 및 재장전을 위한 클립저장 구조체 생성
[System.Serializable]
public struct PlayFireSFX
{
    public AudioClip[] _fireSoundBox;
    public AudioClip[] _reloadSoundBox;
}

[RequireComponent(typeof(AudioSource))]

//~ ------------------------------------------------------------------------
public class FireController : MonoBehaviour
{
    //% 무기 타입
    public enum WeaponType
    {
        RIPLE = 0, SHOTGUN = 1
    }
    public bool _changedGunFlag = false;

    // Update: //@ 2023.11.13 
    //# projectile 방식, Raycast 방식에 따른 발사 구현 ----------
    public enum FireStyle
    {
        PROJECTILE, RAYCAST
    }
    public FireStyle _currentfireStyle = FireStyle.PROJECTILE;
    public bool _changedFireStyleFlag = false;
    public GameObject _projectileModeUi;
    public GameObject _rayCastModeUi;
    //# ---------------------------------------------------
    public WeaponType _currentWeapon = WeaponType.SHOTGUN;
    public GameObject _bulletPrefab;
    public Transform _firePos;
    public AudioClip _fireSfx;
    AudioSource _fireAudioBox;
    GameObject _playerObj;
    //% 구조체 변수 선언
    public PlayFireSFX _playerFireSFX;
    public ParticleSystem _bulletCartridgeEffect;
    // TEMP: // Legacy:
    // ParticleSystem _muzzleFlash;
    // Update:
    MeshRenderer _muzzleFlashMesh;

    public float _timeFlow = 0.0f;
    public float _coolTime = 8.0f;

    // TEST: //# 빛반응 
    public bool _lightDetectFlag = false;


    // Update: //@ 2023.11.08 
    //ShakeCamera _shakeCamera;
    ShakeEffectController _shakeCameraEffect;
    //ShakeCamera _shakeCamera;

    // Update: //@ 2023.11.09 
    //# 탄창 10발 장전, 재장전 구현 (UI변수)
    public int _maxBullet = 10;     //& 최대 총알 수 
    public int _remainedBullet = 10;        //& 남은 총알 수 (초기값)
    public float _reloadTime = 2.0f;        //& 재장전 시간
    public bool _isReloadFlag = false;      //& 재장전 플래그
    public bool _reloadAnimFlag = false;
    public Image _BulletImage;     //& 탄창 이미지 UI
    public TMP_Text _remainedBulletText;    //& 남은 총알 개수 표시 텍스트

    PlayerController _playerCtrl;


    // Update: //@ 2023.11.13 
    //# 총 변경 기능 
    public GameObject _rifleGun;
    public GameObject _shotGun;

    //# 총알 없을 때 경고창
    public GameObject _warningBulletEmptyUi;

    //# 적케릭터 레이어 관련 변수
    int _creatureLayer;     //& 크리처 레이어값 저장 변수
    int _obstaclesLayer;        //& 장애물 레이어값 저장 변수
    int _barrelLayer;       //& 배럴 레이어값 저장 변수
    int _layerMask;         // & 레이어마스크 비트연산을 위한 변수
    bool _isAutoFire = false;
    public float _nextFire;
    public float _fireInterval = 0.5f;

    //public string _textVariable = "<color = yellow> BULLET <color = red> {0} </color>/ {1}";


    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        //// ActiveMouseLock();
    }
    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _fireAudioBox = GetComponent<AudioSource>();
        _playerObj = GameObject.FindWithTag("Player");

        // TEMP: // Legacy:
        // _muzzleFlash = _firePos.GetComponentInChildren<ParticleSystem>();
        // Update:
        _muzzleFlashMesh = _firePos.GetComponentInChildren<MeshRenderer>();
        _muzzleFlashMesh.enabled = false;

        // TEST: //# 빛반응 
        _lightDetectFlag = false;


        _reloadAnimFlag = false;

        // Update: //@ 2023.11.08 
        //_shakeCamera = GameObject.Find("ActionCamera").GetComponent<ShakeCamera>();
        _shakeCameraEffect = GameObject.FindWithTag("MainCamera").GetComponent<ShakeEffectController>();
        //_shakeCamera = GameObject.FindWithTag("MainCamera").GetComponent<ShakeCamera>();
        _playerCtrl = GetComponent<PlayerController>();

        // Update: //@ 2023.11.13 
        //# 모드 변경(샷건, 라이플)을 위한 플래그 
        _changedGunFlag = false;
        _changedFireStyleFlag = false;

        //# 총기 변경
        _rifleGun.SetActive(false);
        _shotGun.SetActive(true);

        _warningBulletEmptyUi.SetActive(false);

        _projectileModeUi.SetActive(true);
        _rayCastModeUi.SetActive(false);

        //% 레이어값 추출
        _creatureLayer = LayerMask.NameToLayer("Creature");
        _obstaclesLayer = LayerMask.NameToLayer("Obstacles");
        _barrelLayer = LayerMask.NameToLayer("Barrel");

        //& 장애물이 검출되거나 크리처가 검출되거나
        //& 1 << 12 | 1<< 8    
        _layerMask = 1 << _obstaclesLayer | 1 << _creatureLayer | 1 << _barrelLayer;

    }
    //~ ------------------------------------------------------------------------

    private void Update()
    {
        Debug.DrawRay(_firePos.position, _firePos.forward * 25.0f, Color.green);


        // NOTE: //@ Creature가 ray로 검출되는 경우 자동발사가 되고 그 외에는 자동발사가 되지 않는 코드 .. (참고)

        //% Ray를 쏘아 검출된 객체의 정보를 저장
        RaycastHit _hit;
        //# NOTE: 발사 원점, 방향백터, 객체정보 반환 받을 변수, 도달거리, 검출레이어
        if (Physics.Raycast(_firePos.position, _firePos.forward, out _hit, 50.0f, 1 << _creatureLayer))
        {
            // Legacy:
            // if (_hit.collider.CompareTag("Creature"))
            //     _isAutoFire = true;
            // Update:  위의 두줄 코드를 아래 한줄로 바꿔도 동일한 기능
            _isAutoFire = (_hit.collider.CompareTag("Creature") || _hit.collider.CompareTag("CyberCreature"));

        }

        else
            _isAutoFire = false;


        if (!_isReloadFlag && _isAutoFire && _playerObj.GetComponent<PlayerController>()._shootPoseFlag == true
        && _playerObj.GetComponent<PlayerController>()._isAlive && _changedFireStyleFlag)

            if (Time.time > _nextFire)
            {
                --_remainedBullet;
                GunFire();
                if (_remainedBullet == 0)
                {
                    StartCoroutine(IsReloadBullets());
                }
                _nextFire = Time.time + _fireInterval;
            }



        //? .. Raycast 방식. 구현하기..위의 것은 자동 발사.






        //? .. Projectile 방식

        if (!_isReloadFlag && Input.GetMouseButtonDown(0) && _playerObj.GetComponent<PlayerController>()._shootPoseFlag == true
            && _playerObj.GetComponent<PlayerController>()._isAlive && !_changedFireStyleFlag)
        {

            // Update: //@ 2023.11.09 
            //% 총알 수 감소
            --_remainedBullet;
            GunFire();


            if (_remainedBullet == 0)
            {
                _isReloadFlag = true;
                _warningBulletEmptyUi.SetActive(true);

            }


        }
        else if (_isReloadFlag && Input.GetKeyDown(KeyCode.R))      //& 총알이 떨어졌을 때 재장전 R
        {
            StartCoroutine(IsReloadBullets());
            _reloadAnimFlag = true;
            _warningBulletEmptyUi.SetActive(false);
            //_playerObj.GetComponent<Animation>().CrossFade("reload", 0.25f);
            ////Debug.Log("Reload");
        }

        _timeFlow += Time.deltaTime;
        RaycastHit _raycastHit;
        if (Physics.Raycast(_firePos.position, _firePos.forward, out _raycastHit, 25.0f))
        {

            if (_raycastHit.collider.tag.Contains("Creature"))
            {
                // TEST: //? 빛반응 .. ING
                _lightDetectFlag = true;

                // TEST://# 크리처 발견 음악 재생 
                // Completed:
                _timeFlow = 0.0f;
                SoundManager.instance.CreatureDetection();
                ////Debug.Log(" music");

            }
            else if (_timeFlow > _coolTime) //# 8초동안 크리처를 조준하지 않으면 비전투로 간주.. 전투음악 중단
            {
                // TEST: //? 빛반응.. ING 
                _lightDetectFlag = false;

                SoundManager.instance.CreatureUndetection();
                ////Debug.Log("music Stop");

            }

        }

        // Update: //@ 2023.11.13 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _changedFireStyleFlag = !_changedFireStyleFlag;

            if (_changedFireStyleFlag)
                _currentfireStyle = FireStyle.RAYCAST;

            else
            {
                _currentfireStyle = FireStyle.PROJECTILE;
            }
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            _changedGunFlag = !_changedGunFlag;

            if (_changedGunFlag)
            {
                _currentWeapon = WeaponType.RIPLE;
                // _rifleGun.SetActive(true);
                // _shotGun.SetActive(false);
            }

            else
            {
                _currentWeapon = WeaponType.SHOTGUN;
                // _rifleGun.SetActive(false);
                // _shotGun.SetActive(true);
            }
        }

        StartCoroutine(UpdatePlayerGunTypeMode());
        StartCoroutine(UpdatePlayerFireStyleMode());


    }


    IEnumerator UpdatePlayerGunTypeMode()
    {
        // Update: //@ 2023.11.13 
        while (PlayerController.instance._isAlive)
        {
            yield return new WaitForSeconds(0.4f);
            switch (_currentWeapon)
            {
                case WeaponType.RIPLE:
                    _rifleGun.SetActive(true);
                    _shotGun.SetActive(false);

                    break;

                case WeaponType.SHOTGUN:
                    _rifleGun.SetActive(false);
                    _shotGun.SetActive(true);

                    break;

            }
            //# NOTE: Corutine >> Update >> yield return null..이하 실행. while문 밖으로 빠져나옴
            yield return null;

        }

    }

    IEnumerator UpdatePlayerFireStyleMode()
    {
        yield return new WaitForSeconds(0.3f);

        while (PlayerController.instance._isAlive)
        {
            switch (_currentfireStyle)
            {
                case FireStyle.PROJECTILE:
                    _projectileModeUi.SetActive(true);
                    _rayCastModeUi.SetActive(false);

                    break;

                case FireStyle.RAYCAST:
                    _projectileModeUi.SetActive(false);
                    _rayCastModeUi.SetActive(true);

                    break;

            }
            yield return null;

        }
    }



    IEnumerator IsReloadBullets()
    {
        //_isReloadFlag = true;
        //% 재장전 사운드 (현재 무기의 재장전 사운드 불러오기)
        _fireAudioBox.PlayOneShot(_playerFireSFX._reloadSoundBox[(int)_currentWeapon], 1.0f);

        yield return new WaitForSeconds(_playerFireSFX._reloadSoundBox[(int)_currentWeapon].length + 0.15f);

        //% 변수 초기화
        _isReloadFlag = false;
        _reloadAnimFlag = false;
        _BulletImage.fillAmount = 1.0f;
        //% 총알 재장전 (10발 복구)
        _remainedBullet = _maxBullet;
        //% 남은 총알수 보여주는 함수 갱신
        UpdateBulletTextDisplay();

    }

    //~ ------------------------------------------------------------------------

    private void GunFire()
    {

        // Legacy:
        // StartCoroutine(_shakeCamera.ShakeCameraPlay());
        // Update: //@ 2023.11.08 
        _shakeCameraEffect._shakeFxFlag = true;
        //StartCoroutine(ShakingCamera());

        // -----
        // Legacy: //! 총알 생성
        // Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
        // Update: //@ 2023.11.10 
        GameObject _bulletObj = GameManagerScript.instance.GetBullets();
        if (_bulletObj != null)      //% 총알이 있으면
        {
            //% 총알의 위치값을 총구의 위치값으로 치환
            _bulletObj.transform.position = _firePos.position;
            _bulletObj.transform.rotation = _firePos.rotation;
            _bulletObj.SetActive(true);

        }


        // Legacy:
        //// AudioSource.PlayClipAtPoint(_fireSfx, transform.position, 1.0f); 
        // _fireAudioBox.PlayOneShot(_fireSfx);

        // Update:
        PlayFireSFX();

        _bulletCartridgeEffect.Play();

        // TEMP: // Legacy:
        // _muzzleFlash.Play();
        // Update:
        StartCoroutine(ShowMuzzleFlash());


        // Update: //@ 2023.11.09 
        //% 재장전 이미지 fillAmount 값 속성 조절
        _BulletImage.fillAmount = (float)_remainedBullet / (float)_maxBullet;
        //% 남은 총알 개수 업데이트(갱신) .. 함수처리
        UpdateBulletTextDisplay();

    }

    // IEnumerator ShakingCamera()
    // {
    //     _shakeCamera._shakeRotateFlag = true;
    //     yield return new WaitForSeconds(2.3f);
    //     _shakeCamera._shakeRotateFlag = false;
    // }

    private void UpdateBulletTextDisplay()
    {
        _remainedBulletText.text = string.Format("<color=yellow>BULLET</color> \n <color=#f00>{0}</color>/{1}", "       " + _remainedBullet, _maxBullet);
    }

    IEnumerator ShowMuzzleFlash()
    {

        _muzzleFlashMesh.enabled = true;
        //% offSet 좌표값을 랜덤 함수로 생성
        Vector2 _offSet = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        //% 값 설정
        _muzzleFlashMesh.material.mainTextureOffset = _offSet;

        // Update:
        float _muzzleScale = Random.Range(5.0f, 7.5f);
        //% 랜덤으로 뽑은 Scale값을 이용해 머즐 플래쉬의 Scale 값 변경
        _muzzleFlashMesh.transform.localScale = Vector3.one * _muzzleScale;
        //% Euler 각을 그대로 사용하면 짐벌락 발생으로 엉뚱한 각으로 회전하게 되는 오류 발생 가능
        //% Euler 값(x, y, z)을 Quternion으로 환상하여 Rotation값 변경
        Quaternion _muzzleRot = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        _muzzleFlashMesh.transform.localRotation = _muzzleRot;

        yield return new WaitForSeconds(0.15f);
        _muzzleFlashMesh.enabled = false;

    }

    private void PlayFireSFX()
    {
        // Legacy:
        // _fireAudioBox.PlayOneShot(_fireSfx);
        // Update: 
        //& 현재 들고 있는 무기
        var _sfx = _playerFireSFX._fireSoundBox[(int)_currentWeapon];
        //& 현재 들고 있는 무기 사운드 출력
        _fireAudioBox.PlayOneShot(_sfx, 1.0f);
    }

}
