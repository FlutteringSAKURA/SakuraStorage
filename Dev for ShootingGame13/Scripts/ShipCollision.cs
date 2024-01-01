using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 씬 관리
using UnityEngine.SceneManagement;

public class ShipCollision : MonoBehaviour
{
    // isTrigger가 체크된 게임 오브젝트와 충돌됨을 
    // 유니티가 알려주기 위해 자동 호출해줌
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // End 씬으로 이동함
      
        
            SceneManager.LoadScene("End");
    
      
        // 충돌 했으니 비행기를 파괴함
        Destroy(gameObject);

        
        
    }
}
