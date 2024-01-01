using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10
// Update: //@ 2023.10.12

// NOTE: //# 2D 게임 플레이어 비행기 제어 스크립트
//#          1) 플레이어 움직이게 하기 + 뷰포트 좌표 내에서만 움직일 수 있도록 제한 하기
//#          2) 레이저 발사 구현
//#          3) 자동/수동모드 구현(기본 자동모드)
//#          4) 총알 오브젝트풀링 방식으로 전환
//#          5) 플레이어 부활 구현

public class PlayerShipCtrl : MonoBehaviour
{
    public float moveSpeed = 4.3f;

    public GameObject laserPrefab;
    public float timeFlow = 0.0f;
    public float coolTime = 1.0f;
    //public bool isSecondLaser = false;
    public bool isAutoFire = false;

    public float laserSpeed = 10.0f;
    public bool isPlayerAlive = true;

    public GameObject crushEffect;  //^ 충돌 이펙트

    void Start()
    {

    }


    void Update()
    {
        PlayerMoveControl();

        timeFlow += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isAutoFire = !isAutoFire;
        }

        if (isAutoFire)
        {
            if (timeFlow > coolTime)
            {
                // TEMP: //? 재활용 코드로 바꿀 예정.. 오브젝트 풀링 방식 코드 활용 예정
                // Update: //@ 2023.10.12   
                //# 총알 오브젝트풀링 방식으로 전환 -----------------// Completed:---------------------
                // Instantiate(laserPrefab, transform.position, transform.rotation);

                //& ObjectManagerCtrl에서 함수를 호출해서 여기서 위치값 주입해줌
                ObjectManagerCtrl.instance.ChargeLazer(transform.position);       
                //# --------------------------------------------------------------

                SoundManager.instance.LaserSoundsPlay();
                timeFlow = 0.0f;

                Debug.Log("자동모드");
            }

        }

        else if (Input.GetKeyDown(KeyCode.Space) && !isAutoFire)
        {
            if (timeFlow > coolTime)
            {
                // LaserFire();

                // TEMP: //? 재활용 코드로 바꿀 예정.. 오브젝트 풀링 방식 코드 활용 예정
                // Update: //@ 2023.10.12
                //# 총알 오브젝트풀링 방식으로 전환 --------------// Completed:------------------------
                // Instantiate(laserPrefab, transform.position, transform.rotation);
                ObjectManagerCtrl.instance.ChargeLazer(transform.position);
                //# --------------------------------------------------------------
                
                SoundManager.instance.LaserSoundsPlay();
                timeFlow = 0.0f;

                Debug.Log("수동모드");
            }
        }

    }

    // private void LaserFire()
    // {

    //     transform.Translate(Vector3.up * laserSpeed * Time.deltaTime);
    //     Debug.Log("레이저 발사 확인");
    // }

    private void PlayerMoveControl()
    {
        // TEMP: TEST:
        // float moveX = moveSpeed = Time.deltaTime * 1f;
        // transform.Translate(moveX, 0, 0);

        float moveX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.Translate(moveX, 0, 0);

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);     //& 플레이어의 좌표를 월드 좌표에서 뷰포트 좌표계로 전환
        viewPos.x = Mathf.Clamp01(viewPos.x);       //^ 강제적으로 0과 1사이로 조정 ((리밋 효과)
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;
        //Debug.Log("월드좌표값: "+ worldPos);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //^ 폭발 이펙트 발생
            Instantiate(crushEffect, transform.position, transform.rotation);
            Debug.Log("Crushed with Enemy");

            //^ 에너미와 충돌시 플레이어 사망처리 함수 호출
            GameManagerCtrl.instance.PlayerDie();

            //^ 플레이어 사망시 사운드 재생
            SoundManager.instance.PlayerDieSoundsPlay();

            //^ 충동한 물체 파괴 (=에너미)
            Destroy(other.gameObject);

            //! NOTE: 추후 재활용을 하기 위해서는 SetActive(false)를 사용
            //Destroy(gameObject);
            // Update: //@ 2023.10.12
            //& 플레이어 부활 구현
            //this.gameObject.SetActive(false);
            //timeFlow = 0.0f;

            RivivalPlayer();
        }
    }

    // NOTE: //# Invoke()와 StartCorutine()의 차이
    //? Invoke의 경우 함수 자체에 특정 시간차를 두지만.. StartCourtine 함수는 함수내에서 시간차를 쪼개서 둘 수 있다.

    //? Invoke의 경우 해당 함수가 있는 게임오브젝트가 비활성화 되더라도 작동하지만.. StartCourtine 함수는 해당 오브젝트가 비활성화 되는 순간 중단된다.
    private void RivivalPlayer()
    {
        //^ 플레이어 비활성화 ,  isAutoFire 
        this.gameObject.SetActive(false);
        timeFlow = 0.0f;
        //isAutoFire = false;
        Invoke("IsPlayerRivival", 3.0f);
    }
    private void IsPlayerRivival()
    {
        //^ 플레이어 활성화
        this.gameObject.SetActive(true);
        StartCoroutine(IsAutoFireCheck());
    }

    IEnumerator IsAutoFireCheck()
    {
        //^ isAutoFire 활성화
        yield return new WaitForSeconds(1.7f);
        isAutoFire = true;
        //Debug.Log("자동발사 코루틴 체크");
    }
}
