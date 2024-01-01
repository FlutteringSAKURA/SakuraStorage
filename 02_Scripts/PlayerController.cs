//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

// Update: //@ 2023.10.31 
// Update: //@ 2023.11.02 
// Update: //@ 2023.11.03 
// Update: //@ 2023.11.04 
// Update: //@ 2023.11.07 
// Update: //@ 2023.11.08 
// Update: //@ 2023.11.13 

// NOTE: //# 3D 게임 - 플레이어 컨트롤러
//#          1) 
//#          2) 
//#          3) 

//~ ------------------------------------------------------------------------
public class PlayerController : MonoBehaviour
{
    [Header("[ BASIC INFO ]")]
    Transform _transform;
    public float _moveSpeed = 5.5f;
    public float _rotateValue = 30.0f;
    public bool _canFireFlag = false;
    public float _timeFlow = 0.0f;
    public float _coolTime = 0.25f;

    public float _playerHp = 100.0f;
    public float _currentHp;


    [Header("[ BLOOD REALATED EFFECTS INFO ]")]
    [SerializeField]
    GameObject _bloodEffect;
    public GameObject _bloodDecalFx;

    // Update: //@ 2023.11.03 
    //#        블러드 카메라 적용
    
    GameObject _bloodCam;

    Animation _playerAnim;


    // Update: //@ 2023.11.07 
    public Transform _bloodEffectPivot;


    [Header("[ TAG REALATED INFO ]")]
    private const string CreatureTag = "Creature";
    private const string CyberCreatureTag = "CyberCreature";
    private const string CreatureTeethTag = "CreatureTeeth";


    // Update: //@ 2023.11.03 
    //#        델리게이트 선언, 이벤트 연결
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;   //& 델리게이트 이름과 이벤트 이름


    [Header("[ FLAG RELATED INFO ]")]
    // Update: //@ 2023.11.03 
    //#         플레이어 사망 구현
    public bool _isAlive = true;
    // Update: //@ 2023.11.07 
    public bool _isDamaged = false;


    [Header("[ FLASHLIGHT AND RIGIDBODY RELATED EVENT INFO ]")]

    // Update: //@ 2023.11.04 
    public GameObject _flashLight = null;
    public float _rigidBodyInitTime = 0.0f;
    public float _recoveryRigidMassTime = 2.5f;

    [Header("[ CAMERA RELATED INFO ]")]

    // Update: //@ 2023.11.08 
    public GameObject _mainCam;
    public GameObject _zoomCam;

    public bool _shootPoseFlag = false;


    [Header("[ SOUNDS RELATED INFO ]")]
    public AudioClip _dieSoundClip;
    public AudioClip _damagedSoundClip;
    public AudioClip _damagedVoiceSound;

    public bool _attackPoseFlag = false;

    [Header("[ UI RELATED INFO ]")]
    public GameObject _standbyUI;
    public GameObject _attackUI;
    public GameObject _snipUI;


    public static PlayerController instance;


    [Header("[ HP BAR REALATED INFO ]")]
    // Update: //@ 2023.11.08 
    //# 체력바 수치에 따른 색깔 변화
    public Image _hpBar;
    Color _initColor = new Vector4(0.0f, 1.0f, 0, 1.0f);       //& 초기값 녹색(0, 1, 0, 1) 설정
    Color _currentColor;

    [Header("[  RELOAD REALATED INFO ]")]
    // Update: //@ 2023.11.10 
    //# 재장전 애니메이션
    FireController _fireCtrl;


    [Header("[ EXPLOSIIN DIE REALATED INFO ]")]
    public bool _explosionFlag = false;
    // Update: //@ 2023.11.13 
    //# 폭살구현
    public SkinnedMeshRenderer[] _skinMesh;
    public Material _fireBurnMat;
    public Transform _firePivot;
    public GameObject _fireEffect;


    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ ------------------------------------------------------------------------
    //# NOTE: Start()함수는 코루틴으로 사용 가능.. 게임 시작하자마자 화면이 회전하는 것을 방지. (콜백을 스스로 함) 
    IEnumerator Start()
    {
        _currentHp = _playerHp;

        _transform = GetComponent<Transform>();
        _playerAnim = GetComponent<Animation>();
        //& Animation 컴포넌트의 Play Automatically를 체크해제하고 별도로 애니메이션 클립을 제어하고 싶을 때 쓰는 방법
        _playerAnim.Play("MenuIdle");

        // Update: //@ 2023.11.08 
        //_mainCam.SetActive(true);
        _zoomCam.SetActive(true);

        _shootPoseFlag = false;

        // Update: //@ 2023.11.08 
        //# 체력바 수치에 따른 색깔 변화
        _hpBar.color = _initColor;
        _currentColor = _initColor;

        // Update: //@ 2023.11.10   
        _fireCtrl = gameObject.GetComponent<FireController>();

        // TEST://* 정규화의 역할 확인 (참고)
        // float _vec1 = Vector3.Magnitude(Vector3.forward);       //$ Magnitude == vec의 합
        // float _vec2 = Vector3.Magnitude(Vector3.forward + Vector3.right);
        // float _vec3 = Vector3.Magnitude((Vector3.forward + Vector3.right).normalized);

        // Debug.Log("v1"+_vec1);
        // Debug.Log("v2"+_vec2);
        // Debug.Log("v3"+_vec3);
        _canFireFlag = false;
        _attackPoseFlag = false;

        ModeUIReset();

        // Update: //@ 2023.11.04 
        _flashLight = GameObject.Find("FlashLight");
        // _flashLight.SetActive(false);

        _rotateValue = 0.0f;
        yield return new WaitForSeconds(0.6f);
        _rotateValue = 30.0f;

        // Update: //@ 2023.11.03 
        _bloodCam = GameObject.Find("Splatter Camera");
        //_bloodCam.SetActive(false);

        _bloodEffect = Resources.Load<GameObject>("SplashBlood");

        _isAlive = true;

        // Update: //@ 2023.11.10 
        //_zoomCam.GetComponent<PostProcessVolume>().enabled = false;
        //_zoomCam.GetComponent<PostProcessLayer>().enabled = false;

        // Update: //@ 2023.11.13 
        //# 폭살 구현
        _explosionFlag = false;
        _skinMesh = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    private void ModeUIReset()
    {
        _standbyUI.SetActive(true);
        _attackUI.SetActive(false);
        _snipUI.SetActive(false);
    }

    //~ ------------------------------------------------------------------------
    private void Update()
    {

        if (!_isAlive)
            return;
        //% -1.0f ~ 1.0f (연속적) 
        //& 전진, 후진
        float _vertical = Input.GetAxis("Vertical");
        //& 좌우
        float _horizontal = Input.GetAxis("Horizontal");

        //% 전후좌우 방향 백터 계산
        //# NOTE: Vector3.forward(0,0,1) .. Vector3.right(1,0,0)
        Vector3 _moveDirection = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);

        //# NOTE: nomalized = 정규화 코드 .. 일정한 수치값이 입력되도록.. == 일정한 속도로 움직이도록
        _transform.Translate(_moveDirection.normalized * _moveSpeed * Time.deltaTime);
        //% 애니메이션 동작
        PlayerAnimation(_horizontal, _vertical);

        //% Y축을 중심으로 회전 (모두 같은 코드)
        // _transform.Rotate(0.0f, Time.deltaTime, 0.0f);
        // _transform.Rotate(Vector3.up * Time.deltaTime);
        //_transform.Rotate(new Vector3(0.0f, Time.deltaTime, 0.0f));

        //% 마우스의 이동에 따라 회전
        float _mouseXValue = Input.GetAxis("Mouse X");
        float _mousYValue = Input.GetAxis("Mouse Y");
        _transform.Rotate(Vector3.up * _rotateValue * Time.deltaTime * _mouseXValue);

        // TEMP:
        //_transform.Rotate(Vector3.left * _rotateValue * Time.deltaTime * _mousYValue);

        //% 곧 바로 방향 및 속도 변경 (불연속적)
        // float _horizontal_Raw = Input.GetAxisRaw("Horizontal");
        //% Vector3.forward // Legacy:
        // transform.position += new Vector3(0, 0, 1);
        //% 전진방향 + 속도 // Legacy:
        // _transform.position += Vector3.forward * 1.0f;
        //% 초당 30프레임 .. 1초에 30번 호출 .. 300유닛 이동
        // _transform.Translate(Vector3.forward * _horizontal * 1.0f * Time.deltaTime * _moveSpeed);
        //#  NOTE: Time.deltaTime * 10 * 프레임수; >>>> 1/30 (0.033초) * 10 * 30 = 10유닛
        //% 프레임마다 10씩 이동
        // transform.Translate(Vector3.forward * 10);
        //% 매 초에 10 유닛 이동
        // transform.Translate(Vector3.forward * 10 * Time.deltaTime);


        // if (_isDamaged)
        // {
        //     _rigidBodyInitTime += Time.deltaTime;
        //     if (_rigidBodyInitTime >= _recoveryRigidMassTime)
        //     {
        //         this.gameObject.GetComponent<Rigidbody>().mass = 1.0f;
        //         _isDamaged = false;
        //         _rigidBodyInitTime = 0.0f;

        //         Debug.Log("rigid" + this.gameObject.GetComponent<Rigidbody>().mass);
        //     }

        // }
        if (_isDamaged)
            _playerAnim.CrossFade("hit", 0.25f);

        if (Input.GetKeyDown(KeyCode.T) && !_attackPoseFlag)
        {

            _canFireFlag = !_canFireFlag;

            if (!_canFireFlag)
            {
                if (!_attackPoseFlag && _canFireFlag == true)
                {
                    _standbyUI.SetActive(false);
                    _attackUI.SetActive(false);
                    _snipUI.SetActive(true);
                }


                _playerAnim.CrossFade("Idle");

            }


            //_timeFlow = 0.0f;
            else
            {
                _playerAnim.CrossFade("IdleFire");
                _shootPoseFlag = false;

                _standbyUI.SetActive(false);
                _attackUI.SetActive(false);
                _snipUI.SetActive(true);
            }


        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _attackPoseFlag = !_attackPoseFlag;

            if (!_attackPoseFlag)
            {
                _playerAnim.CrossFade("Idle");

                _standbyUI.SetActive(false);
                _attackUI.SetActive(true);
                _snipUI.SetActive(false);
                _shootPoseFlag = true;
            }

            else
            {
                _playerAnim.CrossFade("IdleFire");
                _shootPoseFlag = false;


            }
        }
        // Update: //@ 2023.11.10 
        if (_fireCtrl._isReloadFlag && _fireCtrl._reloadAnimFlag)
        {
            _playerAnim.CrossFade("reload", 0.25f);
        }
    }

    private void PlayerAnimation(float horizontal, float vertical)
    {
        if (!_isAlive)
            return;

        //% 전진 애니메이션
        if (vertical >= 0.1f)
        {
            _playerAnim.CrossFade("RunF", 0.25f);
            //_canFireFlag = true;
            _shootPoseFlag = true;

            _standbyUI.SetActive(false);
            _attackUI.SetActive(true);
            _snipUI.SetActive(false);
        }
        else if (vertical <= -0.1f)
        {
            _playerAnim.CrossFade("RunB", 0.25f);
            //_canFireFlag = true;
            _shootPoseFlag = true;

            _standbyUI.SetActive(false);
            _attackUI.SetActive(true);
            _snipUI.SetActive(false);
        }
        //% 좌우 애니메이션
        else if (horizontal >= 0.1f)
        {
            _playerAnim.CrossFade("RunR", 0.25f);
            //_canFireFlag = true;
            _shootPoseFlag = true;

            _standbyUI.SetActive(false);
            _attackUI.SetActive(true);
            _snipUI.SetActive(false);
        }
        else if (horizontal <= -0.1f)
        {
            _playerAnim.CrossFade("RunL", 0.25f);
            //_canFireFlag = true;
            _shootPoseFlag = true;

            _standbyUI.SetActive(false);
            _attackUI.SetActive(true);
            _snipUI.SetActive(false);
        }

        else if (!_canFireFlag && !_attackPoseFlag)
        {
            _playerAnim.CrossFade("MenuIdle", 0.25f);
            _shootPoseFlag = false;
            ModeUIReset();
        }

        else
        {
            _playerAnim.CrossFade("Idle", 0.25f);
            _shootPoseFlag = true;

            if (_attackPoseFlag)
            {
                _standbyUI.SetActive(false);
                _attackUI.SetActive(true);
                _snipUI.SetActive(false);
            }

        }

    }


    //@ 데미지 입는 함수 
    // Legacy:
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (_currentHp >= 0.0f && other.gameObject.tag.Contains("AttackPos") || other.gameObject.tag.Contains("CreatureBullet"))
    //     {
    //         Vector3 _position = other.GetContact(0).point;
    //         Quaternion _rotation = Quaternion.LookRotation(-other.GetContact(0).normal);
    //         CreateBloodEffect(_position, _rotation);

    //         _currentHp -= 10.0f;
    //         //& _currentHp를 _initHp로 나누는 것을 디버그로 표현
    //         Debug.Log($"플레이어의 남은 생명력 = {_currentHp / _playerHp} ");

    //         // Update: //@ 2023.11.03 
    //         //& 플레이어가 물리영향에 너무 밀리는 것을 방지
    //         this.gameObject.GetComponent<Rigidbody>().mass = 20.0f;
    //         Debug.Log("rigid" + this.gameObject.GetComponent<Rigidbody>().mass);
    //         _isDamaged = true;


    //         // Update: //@ 2023.11.03 
    //         _bloodCam.SetActive(true);
    //         _bloodCam.GetComponent<RainCameraController>().Play();


    //         if (_currentHp <= 0)
    //         {
    //             //% 플레이어 사망 함수 콜백
    //             PlayerDie();
    //             _isAlive = false;

    //         }
    //     }
    //     // TEMP:
    //     if (other.gameObject.tag.Contains("CreatureBullet"))
    //     {
    //         other.gameObject.GetComponent<Rigidbody>().mass = 0.0f;
    //         Destroy(other.gameObject);
    //         Debug.Log("CreatureBullet파괴");
    //     }
    //     //! -----

    // }

    // private void CreateBloodEffect(Vector3 position, Quaternion rotation)
    // {
    //     GameObject _blood = Instantiate(_bloodEffect, position, rotation, gameObject.transform) as GameObject;
    //     Destroy(_blood, 1.5f);

    //     // Update: //# 바닥 혈흔 효과 구현
    //     Vector3 _decalPos = gameObject.transform.position + (Vector3.up * 0.02f);
    //     Quaternion _decalRot = Quaternion.Euler(90.0f, 0, Random.Range(0, 360.0f));
    //     GameObject _bloodDecal = (GameObject)Instantiate(_bloodDecalFx, _decalPos, _decalRot);

    //     float _bloodDecalscale = Random.Range(1.5f, 3.5f);
    //     _bloodDecal.transform.localScale = Vector3.one * _bloodDecalscale;
    //     Destroy(_bloodDecal, 5.0f);
    // }


    //@ 데미지 입는 함수 
    private void OnTriggerEnter(Collider other)
    {
        if (_currentHp >= 0.0f && other.gameObject.tag.Contains("AttackPos") || other.gameObject.tag.Contains("CreatureBullet"))
        {
            _isDamaged = true;
            CreateBloodEffect();

            _currentHp -= 10.0f;
            //  Update: //@ 2023.11.08 
            SoundManager.instance.PlaySfx(gameObject.transform.position, _damagedSoundClip);
            SoundManager.instance.PlaySfx(gameObject.transform.position, _damagedVoiceSound);

            // Update: //@ 2023.11.08 
            //# 체력바 수치에 따른 색깔 변화
            DisplayHpBar();

            //& _currentHp를 _initHp로 나누는 것을 디버그로 표현
            Debug.Log($"플레이어의 남은 생명력 = {_currentHp / _playerHp} ");

            //  Update: //@ 2023.11.03 
            // Legacy:
            // //& 플레이어가 물리영향에 너무 밀리는 것을 방지
            // this.gameObject.GetComponent<Rigidbody>().mass = 20.0f;
            // Debug.Log("rigid" + this.gameObject.GetComponent<Rigidbody>().mass);



            // Update: //@ 2023.11.03 
            _bloodCam.SetActive(true);
            _bloodCam.GetComponent<RainCameraController>().Play();


            if (_currentHp <= 0)
            {
                //% 플레이어 사망 함수 콜백
                PlayerDie();
                //? 델리게이트로 변경 

                _isAlive = false;

            }
        }
        // TEMP: no use
        // if (other.gameObject.tag.Contains("CreatureBullet"))
        // {
        //     other.gameObject.GetComponent<Rigidbody>().mass = 0.0f;
        //     Destroy(other.gameObject);
        //     Debug.Log("CreatureBullet파괴");
        // }
        //! -----
    }
    private void OnTriggerExit(Collider other)
    {
        if (_currentHp >= 0.0f && other.gameObject.tag.Contains("AttackPos") || other.gameObject.tag.Contains("CreatureBullet"))
            StartCoroutine(ResetDamagedFlag());
    }

    IEnumerator ResetDamagedFlag()
    {
        yield return new WaitForSeconds(0.5f);
        _isDamaged = false;
    }

    private void DisplayHpBar()
    {
        float _hp = _currentHp / _playerHp;
        if (_hp > 0.5f)
        {
            //& 녹색에서 >> 노랑으로 (r값 증가) ... 녹색 rgba(0, 1, 0, 1) >> 노랑 rgba(1, 1, 0, 1)
            _currentColor.r = (1 - _hp) * 2.0f;
        }
        else
        {
            //& 노랑에서 >> 빨강으로 (g값 감소)...노랑 rgba(1, 1, 0, 1) >> 빨강으로(1, 0, 0, 1)
            _currentColor.g = _hp * 2.0f;
        }
        _hpBar.color = _currentColor;
        _hpBar.fillAmount = _hp;       //0.0f~1.0f;
    }

    private void CreateBloodEffect()
    {
        GameObject _blood = Instantiate(_bloodEffect, _bloodEffectPivot.position, _bloodEffectPivot.rotation, gameObject.transform) as GameObject;
        Destroy(_blood, 1.5f);

        // Update: //# 바닥 혈흔 효과 구현
        Vector3 _decalPos = gameObject.transform.position + (Vector3.up * 0.02f);
        Quaternion _decalRot = Quaternion.Euler(90.0f, 0, Random.Range(0, 360.0f));
        GameObject _bloodDecal = (GameObject)Instantiate(_bloodDecalFx, _decalPos, _decalRot);

        float _bloodDecalscale = Random.Range(1.5f, 3.5f);
        _bloodDecal.transform.localScale = Vector3.one * _bloodDecalscale;
        Destroy(_bloodDecal, 5.0f);
    }


    //@ 폭발 데미지 + 사망 함수 
    private void OnExpDamage()
    {
        _explosionFlag = true;
        _isAlive = false;

        //% Nav Mesh 관련 에러 막기 위한 코드
        StopAllCoroutines();

        //% Creature 폭살시 시체에 걸려 플레이어 피가 줄어드는 현상 방지를 위한 코드
        // NOTE: //# 다만 Creature의 AttackPos가 손에 있으므로 그 손에 있는 SphereCollider만 비활성화. 
        //#  모든 콜라이더를 비활성화하면 배럴폭발시 물리효과를 받지 못하는 문제가 있기 때문에
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            //& 크리처의 collider 비활성화
            coll.enabled = false;
        }

        //# 불 옮겨 붙는 효과 코드
        //% _firePivot 위치좌표의 Quaternion회전값으로 _fireFeefect 프리팹을 생성해 _burningFire 함수에 넣음
        GameObject _burningFire = (GameObject)Instantiate(_fireEffect, _firePivot.position, Quaternion.identity);
        //% _firePivot을 부모로 삼아 생성된 _burningFire가 하위로 들어감 (= 불 옮겨 붙는 효과)
        _burningFire.transform.parent = _firePivot;

        Destroy(_burningFire, 8.0f);

        //# 물리속성 영향 받게 하기 위함 .. barrel스크립트에서 이미 해주고 있음.
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;


        //# Creature가 몸이 타버린 텍스처 효과 구현
        foreach (SkinnedMeshRenderer skin in _skinMesh)
        {
            skin.material = _fireBurnMat;
        }

        // Update: //@ 2023.11.13 
        _currentHp -= 100f;

        PlayerDie();        //& 플레이어 사망함수 호출
        DisplayHpBar();     //& 플레이어 체력바 갱신

    }


    //@ 플레이어 사망 함수 
    private void PlayerDie()
    {
        // Legacy: 
        /*
        Debug.Log("플레이어가 사망했습니다.");
        //% 모든 (Creature) 테그 게임오브젝트를 찾아 _creatures 변수에 담는 코드
        GameObject[] _creaturesBox = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject creatures in _creaturesBox)
        {
            creatures.SendMessage("PlayerIsDead", SendMessageOptions.DontRequireReceiver);
        }
        */

        // Update: //@ 2023.11.09 
        GameManagerScript.instance._isGameOver = true;
        // TEMP:
        GameManagerScript.instance.IsGameOver = true;

        // Update: //@ 2023.11.03 
        // Update: //@ 2023.11.07 
        //#         플레이어 사망 구현
        if (!_isAlive)
        {
            ////Debug.Log("tes");
            // Update: //@ 2023.11.08 
            SoundManager.instance.PlaySfx(this.gameObject.transform.position, _dieSoundClip);
            _playerAnim.CrossFade("Death", 0.25f);
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;


            // Update: //@ 2023.11.03 
            // Update: //@ 2023.11.07 
            //#        델리게이트 이벤트식으로 코드 변경
            //% 플레이어 사망. 이벤트 발생
            GameObject[] _cyberCreatures;
            _cyberCreatures = GameObject.FindGameObjectsWithTag(CyberCreatureTag);
            //% CyberCreature 태그의 오브젝트를 전부 찾아 각각의 CyberCreatureController스크립트에 SendMessage호출
            for (int i = 0; i < _cyberCreatures.Length; i++)
            {
                _cyberCreatures[i].GetComponent<CyberCreatureController>().SendMessage("OnPlayerDie");
                ////Debug.Log("SendMessage");
            }

            GameObject[] _creatures;
            _creatures = GameObject.FindGameObjectsWithTag(CreatureTag);
            for (int i = 0; i < _creatures.Length; i++)
            {
                _creatures[i].SendMessage("PlayerIsDead");
            }

            // Update: //@ 2023.11.13 
            //# 크리처 티스 추가에 의한 코드 추가 
            GameObject[] _creatureTeeth;
            _creatureTeeth = GameObject.FindGameObjectsWithTag(CreatureTeethTag);
            for (int i = 0; i < _creatureTeeth.Length; i++)
            {
                _creatureTeeth[i].SendMessage("PlayerIsDead");
            }

            //& 4초 후 애니메이션 중지
            StartCoroutine(DiePlayOnce());

            // Update: //@ 2023.11.10 
            //& 마우스 움직임 정지
            gameObject.GetComponent<LookController>().enabled = false;
        }

    }

    IEnumerator DiePlayOnce()
    {
        yield return new WaitForSeconds(3.9f);
        _playerAnim.Stop("Death");

    }

}
