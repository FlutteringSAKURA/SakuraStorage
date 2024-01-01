using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.22

// NOTE:  //# 에너미 컨트롤러
//#          1) 에너미가 플레이어 인식 + 플레이어와 부딪혀도 플레이어가 사라지지 않게 구현
//#          2) 에너미의 피가 깎이는 것을 구현
//#          3) HP값이 0이하로 떨어져서 죽으면 폭탄 이펙트 재현
//#          4) 플레이어 보상점수 획득
//#          5) SELF - 몬스터가 충격받았을 때 사운드 효과 구현

public class Enemies : MonoBehaviour
{
    public int hP = 100;
    public float dropSpeed = 2.0f;
    Rigidbody2D rigidBody_2D;
    public GameObject bombEffect;
    Animator enemyAnimator;
    public GameObject gameMaster;       //^ 게임관리자스크립트 또는 클래스 접근
    int damage = 20;

    // Update: //! 싱글톤 사용하여 오디오 사운드 관리 업그레이드 (SoundManager)스크립트
    // public AudioClip enemyHittedSound;


    void Start()
    {
        rigidBody_2D = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        rigidBody_2D.velocity = new Vector2(0, -dropSpeed);     //$ 에너미 자동 움직임
        gameMaster = GameObject.Find("GameManager");
        // bombEffect =gam
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Missile")
        {
            hP -= 50;
            // AudioSource.PlayClipAtPoint(enemyHittedSound, transform.position);

            // Update: //! 싱글톤 사용하여 오디오 사운드 관리 업그레이드 ((SoundManager)스크립트
            SoundManager.instance.EnemiesHittedSounds();

            Destroy(other.gameObject);

            if (hP < 0)
            {
                Instantiate(bombEffect, transform.position, transform.rotation);
                Destroy(this.gameObject);  //^ 에너미 사라짐
                                           //&
                gameMaster.SendMessage("addScore", 50);  //& 점수획득 표현하기 
            }
        }
        enemyAnimator.SetTrigger("HitDam");     //^ 에너미 애니메이션 변경

        print(other.gameObject.name);



        if (other.name == "Witch")
        {   //! 플레이어에게 데미지를 입히는 함수 작정해서 전달 ((, 주입)해서도 가능
            gameMaster.SendMessage("PlayerDamage", 20);        //# SendMessage
            //  Debug.Log("현재 적의 hP는?" + hP);

            // Update: //! 싱글톤 사용하여 오디오 사운드 관리 업그레이드 (SoundManager)스크립트
            SoundManager.instance.EnemyBiteSounds();

        }
    }

    void Update()
    {

    }
}
