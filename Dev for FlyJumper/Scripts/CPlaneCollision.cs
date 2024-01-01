using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlaneCollision : MonoBehaviour
{
    // OncollisionEnter2D() : isTrigger 체크가 안된 두개의 콜라이더를
    // 가진 오브젝트끼리 충돌하면 자동 호출되는 이벤트 메소드

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Tree")
        {
            // 게임 상태를 종료 상태로 변경함
            CGameController.IsGameStop = true;

            GameObject.Find("GameController").
                GetComponent<CGameController>().GameEnd(3f); // 게임 종료
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Flower")
        {
            // 점수 올리기
            GameObject.Find("GameController").
                GetComponent<CGameController>().
                FlowerCountUp();

            // 꽃 파괴
            Destroy(collision.gameObject);
        }
    }

}
