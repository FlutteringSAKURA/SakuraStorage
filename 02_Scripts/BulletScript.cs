using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.27
// Update: //@ 2023.10.02
// Update: //@ 2023.10.04

// NOTE: //# 총알 관련 스크립트
//#          1) 무언가와 총알이 충돌시 스스로 파괴
//#          2) 총알 이펙트 구현 + 총알 사운드 구현
//#          3) 총알이 괴물을 타격하면 애니메이션 재생 + 점수 획득 + 데미지 처리 (sendmessage) + (tag 얀식)
//#          4) 
//? 3번 구현해보기 >> 구현완료
public class BulletScript : MonoBehaviour
{
    public GameObject bulletEffect;
    public AudioClip explosionSound;
    // GameObject insectAnim;      //^ 총알이 괴물을 타격할 때, 애니매이션 재생 구현
    GameObject insectObject;
    GameObject creatureObject;
    GameObject gameManager;

    private void Start() {
        //insectAnim = GameObject.Find("Insect");
        insectObject = GameObject.FindWithTag("Creature_Insect");
        creatureObject = GameObject.FindWithTag("Creature");
        gameManager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.name);

        AudioSource.PlayClipAtPoint(explosionSound, transform.position);    //$ 충돌지점에서 사운드 재생

        switch (collision.tag)  //? 구현해보기 >> 구현완료  // 충돌한 것의 태그를 확인 후 case "태그":
        {
            case "Creature_Insect":
            Debug.Log(collision.name);
            //insectAnim.GetComponent<Animator>().SetTrigger("Hit");
            collision.SendMessage("Damaged", 200);   //# CreatureController 스크립트의 Damaged 함수를 호출하며 200을 주입
            break;

            case "Creature":
            Debug.Log(collision.name);
            collision.SendMessage("Damaged", 200);
            break;

            //# 이하 반복
            default:
                break;
            
        }

        Instantiate(bulletEffect, transform.position + transform.up * 0.35f, Quaternion.identity);   //! Quaternion == 회전값이 원래 가지고 있는 짐벌락을 해결하기 위해 사용하는 관련 코드
        // GameObject bulletFX = Instantiate(bulletEffect, transform.position + transform.up * 0.35f, Quaternion.identity) as GameObject;      //! 위와 같은 효과의 코드 .. 재활용할 때 주로 이런식으로 사용.. bulletFx 변수 선언과 동시에 할당
        Destroy(this.gameObject);
    }

}
