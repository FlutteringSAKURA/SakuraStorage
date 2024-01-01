using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10

// NOTE: //# 2D 게임 플레이어의 레이저발사 제어 스크립트
//#          1) 적에게 맞을 경우 사운드 효과 + 이펙트 효과 구현
//#          2) DestryoZone에 충돌시 레이저 파괴 구현
//#          3) 데미지 주입 + GameManagerCtrl sendmessage호출 .. EnemyCtrl 스크립트로 역할 넘김. (여기서 안함)
//#          4) 
//#          5)

public class LaserCtril : MonoBehaviour
{
    public float laserForceValue = 10.0f;
    public GameObject explosionEffect;
    //public GameObject _gameManager;

    private void Start()
    {
        // _gameManager = GameObject.Find("GameManager");
    }

    private void OnBecameInvisible()    //& 뷰포트 외곽선 밖에서 함수호출
    {
        // NOTE: Destroy(gameObject);   //? 디스트로이.. 이 코드를 쓰면 재활용을 못함.. 게임오브젝트 자체가 사라지기 때문
                                        //! ObjectManagerCtrl 스크립트에서 재활용할 게임오브젝트 자체가 없어지기 때문
        gameObject.SetActive(false);    //^ 비활성화
    }

    private void Update()
    {
        transform.Translate(Vector3.up * laserForceValue * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            //_gameManager.SendMessage("AddScore", 100);
            Instantiate(explosionEffect, other.transform.position, other.transform.rotation);
            SoundManager.instance.ExplosionSoundsPlay();

            // NOTE: Destroy(this.gameObject); //? 디스트로이.. 이 코드를 쓰면 재활용을 못함.. 게임오브젝트 자체가 사라지기 때문
            //Destroy(explosionEffect, 2.0f);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "DestroyZone")
        {
            // NOTE: Destroy(this.gameObject); //? 디스트로이.. 이 코드를 쓰면 재활용을 못함.. 게임오브젝트 자체가 사라지기 때문
        }
    }


}
