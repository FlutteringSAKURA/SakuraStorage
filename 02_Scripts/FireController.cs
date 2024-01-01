using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.27

// NOTE: //# 총알 관련 스크립트
//#          1) 애니매이터 구현(총알 발사시)
//#          2) 사운드 재생(playClipatPoint())사용
//#          3) 총알 생성
//#          4)

public class FireController : MonoBehaviour
{
    public GameObject bullet;
    public float power = 50.0f;
    public AudioClip fireSound;
    GameObject fireShotAnim;    //& 애니메이터 컨트롤러 포함되어 있는 게임오브젝트
   //! AudioSource audioBox;   
    

    private void Start() {
        fireShotAnim = GameObject.Find("bazooka");
        //fireShotAnim.GetComponent<Animator>(); 
        
       //! audioBox = GameObject.Find("FirePos").GetComponent<AudioSource>(); //!

    }

    private void Update() {
        if (PlayerState.instance.isPlayerAlive && Input.GetButtonDown("Fire1") && GameManagerSrc.instance.isGamePause==false && GameManagerSrc.instance.isGameStarted)
        {
          fireShotAnim.GetComponent<Animator>().SetTrigger("Fire");        //$ 애니매이션
          AudioSource.PlayClipAtPoint(fireSound, transform.position);       //$ 사운드 구현
          //! audioBox.PlayOneShot(fireSound);        //$ 사운드 구현 

            //GameObject bazookaBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            //^ 위와 동일 코드
            GameObject bazookaBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;   //^ 재활용하려 할 때 사용하는 방식의 코드
            Rigidbody bulletRigidBody = bazookaBullet.GetComponent<Rigidbody>();            
            //bulletRigidBody.angularVelocity = transform.forward * 60;       //! angularVelocity 초당 라디안 값을 측정해서 힘을 실어서 보내는 것
            bulletRigidBody.velocity = transform.forward * power;      //! 물리력에 힘을 더해 앞으로 밀어내는 코드 >> ((발사 방향과 속도))
            // TEMP: bulletRigidBody.AddForce(transform.forward * 60);  //& AddForce 함수 
        }
    }
}
