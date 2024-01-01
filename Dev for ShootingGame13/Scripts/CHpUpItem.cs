using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHpUpItem : MonoBehaviour
{
    public int _hpUpValue; // 증가할 체력 수치

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌된 오브젝트가 플레이어쉽이면
        if (collision.name == "PlayerShip")
        {
            // 플레이어쉽의 체력 컴포넌트를 참조함
            CPlayerShipHealth playerHealth = collision.GetComponent<CPlayerShipHealth>();

            // 플레이어의 체력을 증가함
            if (playerHealth != null)
                playerHealth.HpUp(_hpUpValue);

            Destroy(gameObject); // 아이템을 파괴함
        }
    }
}