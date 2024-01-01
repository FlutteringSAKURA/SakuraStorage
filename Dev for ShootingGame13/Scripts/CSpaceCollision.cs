using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpaceCollision : MonoBehaviour
{
    // 충돌 이벤트 함수
    // OnTriggerExit2D : Trigger 충돌된 오브젝트가 충돌 영역을 빠져나갈때 자동 호출됨
    // collision 매개변수 : 현재 오브젝트의 충돌 영역을 빠져나간 오브젝트의 Collider2D 컴포넌트 참조
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroy(게임오브젝트)
        Destroy(collision.gameObject);
    }
}