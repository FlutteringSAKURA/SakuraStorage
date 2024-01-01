using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // [TEST CODE]

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    // 발포 사운드 클립 참조 인스턴스 선언
    public AudioClip fireSfx;
    // 머즐 플래쉬 참조 인스턴스 선언
    public MeshRenderer muzzleFlash;

    // [ 머즐 플래쉬 이펙트 구현 극대화 코드 2 ]
    // 발포시 불꽃 효과를 주기 위한 게임오브젝트 파티클 리소스 참조 선언
    public GameObject sparkEffect;
    // ======================================= END

    /* [TEST CODE ] 
    // 발포 지연 시간 1 (발포 간격을 위한 지연시간 값)
    //public float _shotDelay;
    public float _shotDelay = 0.1f;  //추후 변경값을 주더라도 우선 초기화값을 설정해 주는 것이 안정적이다.
    // 발포 지연 시간 (이전 업데이트와의 지연 시간 간격 계산을 위한 값)
    private float _shotDelayTime;
    */
    public float nextFire = 0.1f;
    private float fireTime = 0.0f;

    // [ 롱탄 코드 ]
    public float rotSpeed = 100.0f;
    public Transform inItFirePos;
    // =============================== 롱런 코드 End

    // 오디오 소스 초기화 -> [ 공용함수 코드 ] >> 처리 할 경우 주석 처리 
    // private AudioSource source = null;

    // [TEST CODE] for Fire Charge Bar
    public bool _isFire = false;
    public float coolDownTime = 0.0f;
    private float fireCharge = 0.0f;
    public Image imgFireChargeBar;

    //=================================== TEST CODE END

    // TEST CODE for Shot Cam Shake Effect
    Vector3 defaultPosition; // 초기 좌표 저장
    private Transform ShotCam;
    private CamShake _camShakeScript;
    public Camera _ShotShakeCam;
    
    GameObject _mainCamera;
    GameObject _shotShakeViewCam;
    // ================================== test code end

    // TEST CODE for Inventory 
   // public GameObject _inventoryChk;
    public InventoryCtrl _inventoryCtrlScript;
    // =================================== Test Code End

    private void Start()
    { 
        // [ 공용함스 코드 ] 처리 할 경우 주석 처리
        // 오디오 소스 참조 -  캐시처리 
        // source = GetComponent<AudioSource>();

        // 머즐 플래시 초기화 
        muzzleFlash.enabled = false;

        // [ 롱탄 코드 ]
        inItFirePos.rotation = firePos.rotation;
        // ================== 롱탄 코드 End

        // [TEST CODE] for Fire Charge Bar
        imgFireChargeBar.color = Color.yellow;
        // ====================== test code end

        // TEST CODE for Shot Cam Shake Effect
      //  defaultPosition = Camera.main.transform.position;
       
       // ShotCam = GetComponent<Transform>();
        _camShakeScript = GetComponent<CamShake>();
        _camShakeScript.enabled = false;

       // _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
       // _shotShakeViewCam = GameObject.FindGameObjectWithTag("SHAKECAM");
      //  _ShotShakeCam = GetComponent<Camera>();
        // ==================================== test code end
    }

    private void Update()
    {
        /* [ TEST CODE ]
        // Time.deltaTime : 이전 업데이트와의 간격 시간
        // 발포 지연 시간 계산
        _shotDelayTime += Time.deltaTime;
        // 마우스의 왼쪽 버튼이 눌러짐을 감지하고 발포지연시간 간격이 발포 지연 시간을 넘었다면
        if (Input.GetMouseButton(0) && _shotDelayTime >= _shotDelay)
        */
        
        // [ 레이 코드 ] -> 수정 및 추가
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);
        // ======================================= 레이 코드 End

        if (Input.GetMouseButton(0) && !_inventoryCtrlScript._inventoryOpenChk) // 마우스 오른쪽 버튼을 누르고 인벤토리 창이 열려있지 않다면
        {
            // TEST CODE for Shot Cam Shake Effect
            _camShakeScript.enabled = true;
            // ================================ test code end
            
            _isFire = true;
            coolDownTime = 0f;

            // TEST CODE for Shot Cam Shake Effect
            //Camera.main.transform.Translate(0, 10, 10);
 
        
            // ========== test code end

            fireTime += Time.deltaTime; // 타임 정규화 
            if (fireTime >= nextFire && imgFireChargeBar.fillAmount < 1.0f) // 발사 타임이 NEXT FIRE의 시간과 같아지거나 커지면
            {
                // [ TEST CODE ] for Fire Charge Bar

                fireCharge += Time.deltaTime; // 차징바값 증가
                imgFireChargeBar.fillAmount = fireCharge; // 증가한 차징바값을 현재 차징바에 반영시킴                             
                Color chargeBarColor = Color.yellow; // 초기값 설정

                if (fireCharge >= 0.1f && _isFire)
                {
                    chargeBarColor.r = 1.0f;
                    chargeBarColor.g = 1.0f - fireCharge;
                }
                
                
                else
                {
                    //  chargeBarColor.r = 1.0f;
                    //  chargeBarColor.g += fireCharge*Time.deltaTime;
                    // TEST CODE for Shot Effect
                    
                }
                imgFireChargeBar.color = chargeBarColor; // 차징바 이미지 컬러와 반영되는 차징값에 따른 바 컬러를 일치시켜줌

                // =================== TEST CODE END


                // 총알을 발사해라
                Fire(0);
                // 발포 지연 시간을 초기화함
                fireTime = 0.0f;

                // [ 레이 코드 ] -> 수정 및 추가
                RaycastHit hit;
                // 발사원점, 발사방향, 충돌한 오브젝트에 관한 정보를 RayCast hit에 넣어줌 , 100미터 까지
                if (Physics.Raycast(firePos.position, firePos.forward, out hit, 100.0f))
                {
                    if (hit.collider.tag == "MONSTER")
                    {
                        object[] _params = new object[2]; // 두개의 오브젝트 배열을 생성
                        _params[0] = hit.point; // Ray가 hit된 포인트(위치지점 값)를 넣어줌
                        _params[1] = 20; // 데미지 값을 넣어줌
                        // MonsterCtrl 스크립트의 OnDamage 센드메세지 해줌
                        hit.collider.gameObject.SendMessage("OnDamage", _params,
                            SendMessageOptions.DontRequireReceiver);
                    }
                    /*
                    // [ 월 코드 ] 작성 시 주석처리
                    // [ 배럴 코드 ]
                    if (hit.collider.tag == "BARREL")
                    {
                        object[] _params = new object[2];  // 배열 생성
                        _params[0] = hit.point; // Ray가 hit된 포인트(위치지점 값) 넣어줌
                        _params[1] = firePos.position; // 발사 원점의 위치값 넣어줌
                        hit.collider.gameObject.SendMessage("OnDamage", _params,
                            SendMessageOptions.DontRequireReceiver);
                    }
                    // ======================================== 배럴 코드 End
                    */

                    // [ 월 코드 ]
                    else
                    {
                        object[] _params = new object[2]; // 배열 생성
                        _params[0] = hit.point;  // Ray가 hit된 포인트(위치지점 값)를 넣어줌
                        _params[1] = firePos.position; // 발사 원점의 위치값 넣어줌
                        hit.collider.gameObject.SendMessage("OnDamage", _params,
                            SendMessageOptions.DontRequireReceiver);
                    }
                    // ================ 월 코드 End
                }
                // ==================================== 레이 코드 End
            }

            //if (!_isFire)
            //{ _coolDown = true; }

            // TEST CODE for Shot Shake Effect
            //if (_mainCamera.GetComponentInChildren<FollowCam>()._zoomDynamic && Input.GetMouseButton(0))
            //{
            //    _shotShakeViewCam.GetComponentInChildren<CamShake>().enabled = true;
            //    _ShotShakeCam.enabled = true;
            //}
            // ================ test code end
        }

        // [ TEST CODE ] for Fire Charge Bar    
        else
        {
            // TEST CODE for Shot Cam Shake Effect
            _camShakeScript.enabled = false;
            // ================================= test code end

            Color chargeBarColor = Color.yellow;

            _isFire = false; // 사격중이 아님
         
            //fireCharge = imgFireChargeBar.fillAmount;
            //float fireChargeBar = imgFireChargeBar.fillAmount - ((float)fireCharge++ * Time.deltaTime); // 차징바값 감소          
            //imgFireChargeBar.fillAmount = fireChargeBar; // 감소한 차징값을 현재 차징바와 일치시켜줌                      

            imgFireChargeBar.fillAmount -= fireCharge * Time.deltaTime;
           
          
            coolDownTime += Time.deltaTime;
            if (coolDownTime >= 0f && _isFire == false)
            {
                
                chargeBarColor.r = 1.0f;
                chargeBarColor.g += coolDownTime;
                
            }
            fireCharge = imgFireChargeBar.fillAmount;
        }
        
       //============================== TEST CODE END

       // [ 레이 코드 ] 수정 및 추가 -> 오른쪽 마우스 버튼을 누르면 
       // if (Input.GetMouseButtonDown(1)) // [ 롱탄 코드 작성시 주석처리 후 작성]            
       if (Input.GetMouseButton(1) && !_inventoryCtrlScript._inventoryOpenChk)
        {
            
            // 총알(유탄) 발사 -> [ 롱탄 코드 ] 작성시 주석처리
            // Fire(1);

            // [ 롱탄 코드 ]
            firePos.Rotate(-Vector3.right * rotSpeed * Time.deltaTime);
            // ========================================================== 롱탄 코드 End

            // // [TEST CODE] for Fire Charge Bar
            float fireChargeBar = rotSpeed * Time.deltaTime;
            imgFireChargeBar.fillAmount = fireChargeBar;
            // =================== test code end
        }
        // [ 롱탄 코드 ]
        if (Input.GetMouseButtonUp(1)&& !_inventoryCtrlScript._inventoryOpenChk)
        {
            Fire(1);
            firePos.rotation = inItFirePos.rotation;
        }
        // ====================== 롱탄 코드 End
    }

    // TEST CODE for Charge Bar ==== Debug code 
    // =========== TEST CODE END


    private void Fire(int chk)
    {
       // 만약 0보다 크다면 = 1이라면(즉 마우스 오른쪽 버튼)
       if (chk > 0)
        {
            // 총알(유탄) 생성
            CreateBullet();
        }
        // ================================= 레이 코드 End

        // 총알 사운드 100의 볼륨으로 구현 -> [ 공용함수 코드 ] 추가 할 경우 주석 처리
        // source.PlayOneShot(fireSfx, 1.0f);
        GameMgr.instance.PlaySfx(firePos.position, fireSfx);
        // ===================================================== 공용함수 End

        // [머즐 플래쉬 이펙트 구현 극대화 코드 2 ]
        // 불꽃을 firePos의 Transform 위치에 생성
        GameObject spark = (GameObject)Instantiate(sparkEffect, firePos.transform.position, Quaternion.identity);
        Destroy(spark, 0.6f);
        //=================== END

        // 머즐플래시 코루틴 수행
        StartCoroutine(this.ShowMuzzleFlash());
    }
 

    private IEnumerator ShowMuzzleFlash()
    {
        // [ 머즐 플래쉬 이펙트 구현 극대화 코드1 ]
        // 머즐 플래쉬의 Scale 랜덤값 추출
        float scale = Random.Range(1.0f, 2.0f);
        // 랜덤값으로 뽑은 Scale 값을 이용하여 머즐 플래쉬의 Scale 값 변경
        muzzleFlash.transform.localScale = Vector3.one * scale;

        // Euler 각을 그대로 사용하면 짐벌락 발생으로 엉뚱한 각으로 회전하게 되는 오류 발생
        // Euler 값(x, y, z)을 Quternion으로 환산하여 Rotation값 변경
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        // ================================================================== END

        // 머즐 플래쉬를 구현
        muzzleFlash.enabled = true;
        // 0.01초에서 0.1초 사이의 시간으로 랜덤하게 구현됨
        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        muzzleFlash.enabled = false;
    }

    void CreateBullet()
    {
        // 총알을 총알의 생성위치에 생성.
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

 
}
