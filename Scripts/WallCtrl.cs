using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCtrl : MonoBehaviour
{
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // 벽에 부딪힌 collider의 tag가 BULLET이면
        if(collision.collider.tag == "BULLET")
        {
            // spark Effect를 생성
            GameObject spark = (GameObject)Instantiate(sparkEffect, collision.transform.position, Quaternion.identity);
            // 4.8초 후에 spark Effect 파괴
            Destroy(spark, 4.8f); 
            // 총알 삭제
            Destroy(collision.gameObject);
        }
    }
    // [ 월 코드 ]
    private void OnDamage(object[] _params)
    {
        GameObject spark = (GameObject)Instantiate(sparkEffect, (Vector3) _params[0], Quaternion.identity);
        Destroy(spark, 5.0f);
    }
    // ================================ 월 코드 End
}
