using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // [ 알파 코드 1 ]

// 인스펙터에 노출을 위해 public으로 class를 만듬
[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
    // [ TEST CODE ] for Player Hit & Die
    public AnimationClip playerHit;
    public AnimationClip playerDie;
    // =================== end
    // [ TEST CODE ] for Player Falling, Landing, Jump Action
    public AnimationClip playerFalling;
    public AnimationClip playerFallingToLanding;
    // ================================================ Test Code End
    // [ TEST CODE ] for Pick Up Item Action
    public AnimationClip pickUpItem;

}

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private Transform tr;

    public float moveSpeed = 7.0f;
    public float rotSpeed = 100.0f;
    public bool _isDie = false;
    public bool _isHit = false;
    // 지면에 닿아있는지에 관한 여부 설정
    // public bool _isGround = false;
    // TEST CODE for 추락시 사망 코드 구현
    public bool _groundChk = true;
    public bool _isFalling = false;
    public float waitTime = 0.0f;
    // =============================== Test Code End

    // 점프 구현 테스트 코드 ======================
    public bool _isJump = false;
    //public bool _isDoubleJump = false;
    public float _jumpPower;
    protected Rigidbody _rigidbody;
    // ============================================ END
    public bool _isMove = false;


    // 인스팩터에 참조를 위한 노출 인스탄트 선언
    public Anim anim;

    // 애니메이션의 참조를 위한 노출 인스탄트 선언
    public Animation _animation;

    // [ 1차 추가 코드 ] -> 플레이어의 체력값 설정 
    public int hp = 100;
    // ====================== END

    // [ 3차 추가 코드 ]
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    //=============================================== 3차 추가 코드 END
    // [ TEST CODE ] disable Monster's SphereCollider 
    // public static event PlayerDieHandler OffCollider;
    // ======= test code end

    // [ 알파 코드 2 ] -> 플레이어 체력을 받기위한 변수 및 체력 게이지 이미지 참조를 위한 선언
    private int initHp;
    public Image imagHpBar;
    // ======================== 알파코드 END

    // [ 감마 코드 1 추가 ] -> 혈흔 효과 구현을 위한 참조 변수 선언
    public GameObject bloodEffect;
    // ================================== 감마 코드 End

    // [ 플러스 코드 ] -> 싱글톤 플러스 코드 추가시 주석 처리 
    // -> GameMgr스크립트를 싱글톤으로 처리하는 경우 필요가 없어지는 코드임
    // private GameMgr gameMgr;
    // ========================== 플러스 코드 End

    // TEST CODE for Eyes Up & Down
    //public Transform playerEyes;
    //public Transform eyes;
    //public float rotateSpeed = 100.0f;
    // ========== end

    // [ TEST CODE for Lantern ]
    public bool _isLightView = false;
    public Light lantern;
    // ============================== TEST CODE END
    private GameObject spawnManager_4sec;

    // TEST CODE for Bloody Action Cam Effect
    //  [SerializeField]
    // RainCameraController _bloodActionCam;
    [SerializeField]
    GameObject _bloodActionCam;
    // ===================================== test code end
    // TEST CODE for PosionZone Damage
    public bool _poisonChk = false;
    [SerializeField]
    private GameObject _poisonZonObject;

    private float TimeLeft = 2.0f;
    private float nextTime = 0.0f;



    private void Start()
    {
        // [ TEST CODE for Lantern ]
        // 수정 된 코드 -> 컴포넌트의 이름으로 찾은후 그 하위 자식중 Light를 호출하는 형태
        lantern = GameObject.Find("Lantern").GetComponentInChildren<Light>();
        // [ 아래 코드는 수정전 초기 코드로 자식으로 Light가 있는 컴포넌트는 모두 적용대상이 되어 순서상 Light가 포함되어있는 상위의 컴포넌트 순서대로 적용되는 문제점이 있음        
        // lantern = GetComponentInChildren<Light>(); 
        // ========================================= Test Code End

        // [ 알파 코드 3 ]
        initHp = hp;
        // =============== 알파 코드 END

        tr = GetComponent<Transform>();

        // [ 플러스 코드 ] -> 싱글톤 플러스 코드 추가시 주석 처리 (필요없음)
        // gameMgr = GameObject.Find("GameManager").GetComponent<GameMgr>(); // 캐시 처리
        // ================================================================ 플러스 코드 End

        _animation = GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();

        // 점프 구현 테스트 코드 ============================
        _rigidbody = GetComponent<Rigidbody>();
        //============================================== END

        // [ TEST CODE ] -> 게임 시작시 체력게이지 색깔 지정
        imagHpBar.color = Color.green;
        // =================================== Test Code END


        // [ TEST CODE ] for EyesUP & DOWN
        // playerEyes.rotation = eyes.rotation;
        // ========================================= END     
        spawnManager_4sec = GameObject.FindGameObjectWithTag("4secSpawnManager");

        // TEST CODE
        _bloodActionCam.SetActive(false);
    }

    // 매 프레임마다 호출
    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //Debug.Log(" H = " + h.ToString());
        // 동일 코드 : Debug.Log("H = " + g);
        //Debug.Log(" V = " + v.ToString());

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // 아래의 코드 역시 동일 코드이나 Translate를 많이 해주면 비용측면에서 
        // 낭비가 된므로 좋은 코드는 아니다.
        //tr.Translate(Vector3.right * moveSpeed * h * Time.deltaTime, Space.Self);

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        // 마우스의 x좌표축의 이동을 통하여 Player의 시선 방향의 전환 구현
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
        // ================================================================================
        // 테스트 코드 >> 마우스의 y좌표축의 이동을 통하여 Player의 시선 방향의 전환 구현
        // 그러나 케릭터가 회전해버리는 오류가 있다 (해결 요망)
        // tr.Rotate(Vector3.left* rotSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));


        // 점프구현 테스트 코드
        InputJump();
        //======================================== END 

        // [ TEST CODE ] for Eyes UP & Down
        // EyesUpDown();
        // =================================== END


        if (v >= 0.1f && !_isFalling && !_isJump)
        {
            // 다음 애니메이션으로 0.3초 후 연결
            // CrossFade -> 수행중인 애니메이션을 자연스럽게 다음 애니메이션으로 전환 연결 시켜주는 메소드
            
            _isHit = false;
            _isMove = true;
            _animation.CrossFade(anim.runForward.name, 0.3f);
            if (v >= 0.1f && waitTime >= 0.45f && !_groundChk)
            {
                _animation.clip = anim.playerFalling;
                _animation.Play();

            }
            //else if(v >= 0.1f && !_groundChk)
            //{
            //    _animation.clip = anim.playerFalling;

            //}
        

        }
        else if (v <= -0.1f && !_isFalling && !_isJump)
        {
            _isHit = false;
            _isMove = true;
            _animation.CrossFade(anim.runBackward.name, 0.45f);
            if (v <= 0.1f && waitTime >= 0.3f && !_groundChk)
            {
                _animation.clip = anim.playerFalling;
                _animation.Play();

            }

        }
        else if (h >= 0.1f && !_isFalling && !_isJump)
        {
            _isHit = false;
            _isMove = true;
            _animation.CrossFade(anim.runRight.name, 0.3f);
            if (h >= 0.1f && waitTime >= 0.45f && !_groundChk)
            {
                _animation.clip = anim.playerFalling;
                _animation.Play();

            }
        }
        else if (h <= -0.1f && !_isFalling && !_isJump)
        {
            _isHit = false;
            _isMove = true;
            _animation.CrossFade(anim.runLeft.name, 0.3f);
            if (h <= 0.1f && waitTime >= 0.45f && !_groundChk)
            {
                _animation.clip = anim.playerFalling;
                _animation.Play();

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _isMove = true;
            // TEST CODE for Player Jump Action
            _animation.clip = anim.playerFalling;
            _animation.Play();
            _animation.wrapMode = WrapMode.Once;
            // ======================================= Test Code End
        }
        else if (!_groundChk && _isHit) // 공중에 떠 있는 상태에서 맞았다면 피격 동작을 잠깐하고 다시 점프(추락)동작으로 변경해줌
        {
            // Debug.Log("hit & fall");
            // _animation.clip = anim.playerHit;
            //_animation.clip = anim.playerFalling;
            //   _animation.Play("FallingIdle");
            _animation.CrossFade("FallingIdle", 0.5f);
        }
        // [ TEST CODE ] for Player Hit & Die 
        else if (_isDie)
        {
            _isMove = false;
            //anim.playerDie.wrapMode = WrapMode.Once;
            //_animation.CrossFade(anim.playerDie.name, 0.3f);

            _animation.Play("playerDie");
            StartCoroutine(DiePlayer());

        }
        else if (_isHit && waitTime >= 0.1f)
        {
            _isDie = false;
            _animation.CrossFade(anim.playerHit.name, 0.3f);
        }
        // ============================================== TEST CODE END

        // [ TEST CODE ] for Falling Action
        else if (_groundChk == false)
        {
            FallingAction();
        }
        // ========================================== Test Code End

        else if (Input.GetKey(KeyCode.R))
        {
            ItemRootingAction();

        }
        // ================================= test code end
        else
        {
            _isHit = false;
            _isDie = false;
            _isMove = false;
            _animation.CrossFade(anim.idle.name, 0.3f);
        }

        // [ TEST CODE for Lantern ]

        if (Input.GetKeyDown(KeyCode.V))
        {
            _isLightView = !_isLightView;
            // 랜턴을 켜라
            Lantern();
        }

        // [ TEST CODE ] for Falling Die
        if (_groundChk == false)
        {
            waitTime += Time.deltaTime;
            Falling();
        }
        // =========================== Test code for 추락 사망 코드 End 

        // [TEST CODE] for Posion Zone Damage;
        if (Time.time > nextTime)
        {
            nextTime = Time.time + TimeLeft; // 2초마다 아래의 포이즌 중독 명령 함수 호출
            PoisonIntoxication(); 
        }
        // ================== Test Code End
    }

    private void PoisonIntoxication()
    {
        // 포이즌 체크 + Poison Zone에서 벗어나지 않았다면
        if (_poisonChk && _poisonZonObject.GetComponent<DangerousPoisonZone>()._isOutOfZoneChk == false)
        {

            PlayerHit(); // Hit Animation 실행

            // TEST CODE for Bloody Action Cam Effect
            _bloodActionCam.SetActive(true);
            _bloodActionCam.GetComponent<RainCameraController>().Play(); // 블러드 캠 효과 작동

            StartCoroutine(EraseBloodyMark());
            // ========================== Test code End

            GameObject blood1 = (GameObject)Instantiate(bloodEffect, tr.transform.position + Vector3.up * 1.0f, Quaternion.identity); // blood 생성
           
            Destroy(blood1, 2.0f); // 2초후 blood 파괴

            hp -= 5;
            float hpBar = (float)hp / (float)initHp;
            Color hpBarColor = Color.black; // 초기값
            if (hpBar <= 0.5f)
            {
                hpBarColor.r = 1.0f; // RED
                hpBarColor.g = (hpBar) * 2; // (1.0f * hpBar) * 2; [<- 원래의 코드지만 1.0f를 곱해주는 것은 의미가 없으므로 생략)
            }
            else
            {
                hpBarColor.g = 1.0f;
                hpBarColor.r = (1.0f - hpBar) * 2;

            }
            imagHpBar.fillAmount = hpBar;
            imagHpBar.color = hpBarColor;

            if (hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    private IEnumerator DiePlayer()
    {
        // _animation.Play("playerDie");
        // _animation.wrapMode = WrapMode.Once;

        yield return new WaitForSeconds(4.0f);
        _animation.Stop("playerDie");
    }

    private void Lantern()
    {
        if (_isLightView)
        {
            lantern.enabled = true;
            // [아래 코드는 수정전 ]
            //gameObject.GetComponentInChildren<Light>().enabled = true;
        }
        else
        {
            lantern.enabled = false;
            // [ 아래 코드는 수정전 ]
            // gameObject.GetComponentInChildren<Light>().enabled = false;
        }
    }
    // ============================= Test Code for Lantern END


    // TEST CODE for Falling Die

    private void Falling()
    {

        if (waitTime >= 6.0f && _groundChk == false) //  6초이상 groundChk가 false라면 
        {
            StartCoroutine(FallDie()); // SlowDie Code START!!
        }
    }

    private IEnumerator FallDie()
    {
        yield return new WaitForSeconds(0.5f);

        PlayerHit();

        hp -= 1;
        float hpBar = (float)hp / (float)initHp;
        Color hpBarColor = Color.black; // 초기값
        if (hpBar <= 0.5f)
        {
            hpBarColor.r = 1.0f; // RED
            hpBarColor.g = (hpBar) * 2; // (1.0f * hpBar) * 2; [<- 원래의 코드지만 1.0f를 곱해주는 것은 의미가 없으므로 생략)
        }
        else
        {
            hpBarColor.g = 1.0f;
            hpBarColor.r = (1.0f - hpBar) * 2;
        }

        imagHpBar.fillAmount = hpBar;
        imagHpBar.color = hpBarColor;

        if (hp <= 0)
        {
            PlayerDie();
        }
    }

    // ========================================== Test Code for 추락 사망 코드 End

    // [ 1차 추가 코드 ]
    private void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.tag == "PUNCH" || coll.gameObject.tag == "BOSSPUNCH" || coll.gameObject.tag == "SpearBoobyTrap"
            || coll.gameObject.tag == "SharkTail")
        {
            // [ 감마 코드 2 추가 ]
            GameObject blood1 = (GameObject)Instantiate(bloodEffect, coll.transform.position, Quaternion.identity);

            Destroy(blood1, 2.0f);
            // ============================= 감마 코드 END
            _isHit = true;
            hp -= 5;

            // TEST CODE for Mutant BOSSPUNCH
            if (coll.gameObject.tag == "BOSSPUNCH")
            {
                hp -= 15;
            }

            else if (coll.gameObject.tag == "SharkTail") // for Alien Shark's Tail Swing Attack
            {
                Debug.Log("TailAttack OKAY");
                hp -= 20;
            }

            else if (coll.gameObject.tag == "SpearBoobyTrap") // Spear Booby Trap for Bloody Effect 
            {
                // 캐릭터 위치로 설정 후, 1미터 Vector 값을 올려준 위치에서 blood 생성
                Instantiate(blood1, tr.transform.position + Vector3.up * 1.0f, Quaternion.identity);
                hp -= 30;
            }

            PlayerHit();

            // TEST CODE for Bloody Action Cam Effect
            _bloodActionCam.SetActive(true);
            _bloodActionCam.GetComponent<RainCameraController>().Play(); // 블러드 캠 효과 작동

            StartCoroutine(EraseBloodyMark());

            // ========================== Test code End

            // [ 베타 코드 1 ] -> 정수값 끼리 나눠지면 정수이기 때문에 항상 그 결과 값은 0과 1이다
            // >> 실수값으로 변형을 해줌
            float hpBar = (float)hp / (float)initHp;
            Color hpBarColor = Color.black; // 초기값

            // 베타 코드 1 테스트후 아래 코드는 주석 처리
            /*
               if (hpBar <= 0.2f) hpBarColor = Color.red;
               else if (hpBar <= 0.5f) hpBarColor = Color.yellow;
               else hpBarColor = Color.green;
             */

            // [ 베타 코드 1- 1 ]
            // if (hpBar <= 0.6f) // [ 체력 게이지 60퍼센트 지점 노랗게 변하는 코드 예시 ]
            if (hpBar <= 0.5f)
            {
                hpBarColor.r = 1.0f; // RED
                hpBarColor.g = (hpBar) * 2; // (1.0f * hpBar) * 2; [<- 원래의 코드지만 1.0f를 곱해주는 것은 의미가 없으므로 생략)

                // [ 체력 게이지 60 퍼센트 지점 노랗게 변하는 코드 예시 ]
                // hpBarColor.g = 5f / 3f * (float) (hpBar - 1); 
            }
            else
            {
                hpBarColor.g = 1.0f;
                hpBarColor.r = (1.0f - hpBar) * 2;
                // [ 체력 게이지 60퍼센트 지점에서 노랗게 변하는 코드 예시 TEST
                // hpBarColor.g = 1.0f;
                // hpBarColor.r = (1.0f - hpBar) *2.5f;
                //================================================= TEST END

                // hpBar의 값이 데미지를 받을 때마다 그 값이 줄면서 r 값이 점점 1로 간다. (RED로 색깔변화)
                // 그런데 곱해주지 않는다면 절반만 처리되는 셈이므로 X2를 해줘야 자연스러워진다.
            }

            imagHpBar.fillAmount = hpBar;  // 체력이 가득찬 상태
            imagHpBar.color = hpBarColor;
            // ================================ 베타 코드 END

            // [ 알파 코드 4 ] -> 베타 코드 추가시 주석 처리
            // imagHpBar.fillAmount = (float)hp / (float)initHp;
            // ================================================= 알파 코드 END

            Debug.Log("Player Hp = " + hp.ToString());

            if (hp <= 0)
            {
                PlayerDie();
            }
        }

        // TEST CODE for Poison Zone Damage        
        else if (coll.gameObject.tag == "PoisonZone")
        {

            Debug.Log("Poison Zone : Action Poison Damage StartCoroutine");
            StartCoroutine(ActionPoisonDamage());
        }
        // =================== test code end

    }
    // TEST CODE for Poison Zone
    private IEnumerator ActionPoisonDamage()
    {
        yield return new WaitForSeconds(11.5f);
        _poisonChk = true;
    }
    // ============ test Code end

    // [ TEST CODE ] Bloody Action Cam Effect
    private IEnumerator EraseBloodyMark()
    {
        yield return new WaitForSeconds(0.5f);
        _bloodActionCam.GetComponent<RainCameraController>().Stop(); // 블러디 캠 효과 작동 중지
        yield return new WaitForSeconds(1.3f);
        _bloodActionCam.SetActive(false); // 블러디 캠 오프 (꺼줘야 마우스 휠 버튼을 통해 줌 인 & 아웃 가능)
    }
    // =============== Test Code End

    void PlayerHit()
    {
        // TEST CODE for player Hit
        _isHit = true;
        _animation["playerHit"].speed = 3.5f;
        _animation.clip = anim.playerHit;
        _animation.Play();

        // ======================= Test Code End
    }

    private void PlayerDie()
    {
        _isDie = true;
        Debug.Log("Player Die");

        // [ TEST CODE ]  
        //_animation.clip = anim.playerDie;
        // _animation.Play();

        // [ Test Code ] 캐릭터 사망시 공격감지를 위해 달아둔 몬스터의 Collider에 캐릭터가 부딪혀 반응하지 않도록 캐릭터의 SphereCollider 비활성화

        // gameObject.GetComponent<CapsuleCollider>().enabled = false;
        // [ 위의 코드를 수정하여 아래 코드로 변경 ]
        // -> 플레이어의 Layer를 BODY로 바꿔줌으로써 캐릭터 사망시, 공격감지를 위해 몬스터에 달아준 Collider에 죽은 플레이어의 Collider가 부딪혀 반응하지 않도록 해줌
        gameObject.layer = LayerMask.NameToLayer("BODY");
        //GameObject.FindGameObjectWithTag("PoisonZone").GetComponent<BoxCollider>().isTrigger = false;
        //GameObject.FindGameObjectWithTag("PoisonZone").transform.gameObject.tag = "GROUND";
        gameObject.tag = "Untagged";
        _poisonChk = false;

        //_animation["playerDie"].clip.wrapMode = WrapMode.Once;
        //_animation.CrossFade(anim.playerDie.name, 0.3f);

        //_animation.clip = anim.playerDie;
        //_animation.Play();

        // _animation.Play("playerDie");

        // ========================= Test Code End

        /*
        // [ 2차 추가 코드 ]
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        foreach (GameObject Mon in monsters)
        {
            Mon.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
        // ============================== 2차 추가 코드 END 
        */

        // [ 3차 추가 코드 ]
        OnPlayerDie(); // 해당 이벤트 발생 0 -> MonsterCtrl 스크립트로 이동 >> 몬스터의 승리 포즈 구현

        // [ 플러스 코드 ] -> 플러스 싱글톤 코드 추가시 주석 처리 (필요없음)
        // gameMgr.isGameOver = true;
        // ============================= 플러스 코드 End

        // [ 플러스 싱글톤 코드 ]
        GameMgr.instance.isGameOver = true;
        // =================================== 싱글톤 코드 End
        spawnManager_4sec.GetComponentInChildren<SpawnPointCtrl>()._isGameOver = true;


        // [ TEST CODE ] disable Monster's SphereCollider
        // OffCollider(); // 해당 이벤트 발생시 -> MonsterCtrl 스크립트로 이동 >> 몬스터 손의 스피어 콜라이더 비활성화
        // ================= test code end
    }

    // ============== end

    // ============================== 1차 추가 코드 END

    // 점프구현 테스트 코드 ============================================
    private void Jump()
    {

        // 점프 초기화
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.normalized.x, 0f);
        // 점프
        _rigidbody.AddForce(Vector2.up.normalized * _jumpPower);

    }

    // 점프구현 테스트 코드  ===============================================
    private void InputJump()
    {
        // 스페이스바를 눌렀을때 추락중이 아니고(waitTime >= 0.5 부터 추락 판정) 점프 상태가 아니라면
        if (Input.GetKeyDown(KeyCode.Space) && waitTime <= 0.5 && !_isJump)
        {
            // 점프실행
            Jump();


            // 점프를 한 상태로 변경
            _isJump = true;


            // 지면에서 떨어진 상태로 변경
            _groundChk = false;
        }
    }

    // TEST CODE for Falling Action
    private void FallingAction()
    {
        if (waitTime >= 0.5f) // 1.5초 이상 점프상태가 유지되면 추락으로 판정 
        {
            _isFalling = true;
            _animation.clip = anim.playerFalling;
            _animation.Play();
        }

    }
    // ================================= Test Code End

    //TEST CODE for Landing Action
    private void FallingToLandingAction()
    {
        _isFalling = false;
        _animation.clip = anim.playerFallingToLanding;
        _animation.Play();

    }
    // ====================== Test Code End

    // TEST CODE for Pick Up Item Action
    private void ItemRootingAction()
    {
        _animation.clip = anim.pickUpItem;
        _animation.Play();
        _animation.wrapMode = WrapMode.Once;
    }
    // ========================== test code end

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "GROUND")
        {
            _animation.Stop("playerHit");

            //TEST CODE for Landing Action            
            if (_isFalling)
            {
                FallingToLandingAction();
            }
            // ====================== Test Code End


            //GroundSetting(true);
            // _isGround = false;
            _groundChk = true;
            _isJump = false;
            waitTime = 0.0f;

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "GROUND")
        {
            // _isGround = false;
            _groundChk = true;
            _isFalling = false;
            waitTime = 0.0f;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (collision.gameObject.tag == "GROUND" && Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;
        }
        else
        {
          
            //    _isGround = false;
            _groundChk = false;
            _isFalling = false;
            // _isGround = true;
        }

        // return;
    }


    //================================================== TEST CODE END 

    // [ TEST CODE ] for EYES UP & DOWN 
    //private void EyesUpDown()
    //{
    //    playerEyes.Rotate(-Vector3.right * rotSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
    //    eyes.rotation = playerEyes.rotation;
    //}
    // ================== TEST CODE END
}
