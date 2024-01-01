using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.21
// Update: //@ 2023.09.26

// NOTE:  //# 미사일 발사 생성 스크립트
//#          1) 미사일 발사 구현
//#          2) 미사일 발사 사운드 구현
//#          3) 기준시간이 증가해 쿨타임 시간보다 커지면 미사일 생성하기(쿨타임으로 미사일 발사 간격 구현)
//#          4) 자동발사 / 수동발사 모드 구현 (Tap)키 사용
//#          5) 오디오 관련 매니징 코드 구현 ((업그레이드) 
//#          6) 플레이어 사망시 총알 발사 금지
// TEMP: //? 오디오 관련 코드 구현 나중에 (((임시 주석처리)

public class MissileFire : MonoBehaviour
{
    public GameObject missilePrefab;
    public float timeFlow = 0.0f;   //^ 시간 초기 설정
    float coolTime = 1.0f;      //^ 쿨타임
    //public AudioClip fireSound;
    //AudioSource audioBox;   //^ 오디오 컴포넌트
    public bool isAutoFire = false;
    // Update: //% 6)
    // public bool isAlivePlayer = true;
    // public static MissileFire instance;


    void Start()
    {
        //audioBox.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeFlow += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))      //! Tab을 누를 때 마다 상태값 변경((수동, 오토)
        {
            isAutoFire = !isAutoFire;
        }

        if (Input.GetButton("Fire1") && !isAutoFire && GameMaster.instance.isPlayerAlive)
        {
            if (timeFlow > coolTime)
            {
                Instantiate(missilePrefab, transform.position, transform.rotation);
                timeFlow = 0.0f;
                //AudioSource.PlayClipAtPoint(fireSound, transform.position);
                // Update: //@ 2023.09.26 - 오디오 구현 업그레이드
                //audioBox.PlayOneShot(fireSound);
                SoundManager.instance.FireSounds();      //! 싱글톤(정적변수화)시켜서 호출함
                                                        //! SoundManager스크립트에서 호충
            }
        }
        else if (timeFlow > coolTime && isAutoFire && GameMaster.instance.isPlayerAlive)
        {
            Instantiate(missilePrefab, transform.position, transform.rotation);
            timeFlow = 0.0f;
            // Update: //@ 2023.09.26 - 오디오 구현 업그레이드
            //AudioSource.PlayClipAtPoint(fireSound, transform.position);
            //audioBox.PlayOneShot(fireSound);
            SoundManager.instance.FireSounds();
        }


        // TEST: // TEMP: 
        /*
        timeFlow += Time.deltaTime;

        //^ 2초 시간이 지나면
        if(timeFlow >= 2.0f)
        {
            timeFlow = 2.0f;
        }

        //& 미사일 생성
        if (Input.GetButton("Fire1"))
        {
            
            if (timeFlow > 0)
            {
                Instantiate(missilePrefab, transform.position, transform.rotation);    
                timeFlow -= coolTime; //& 시간감소
                AudioSource.PlayClipAtPoint(fireSound, transform.position);
                //print(timeFlow);
            }
            else
            {
                timeFlow += Time.deltaTime;     //& 시간 증가
            }
        }
        */
    }
}
