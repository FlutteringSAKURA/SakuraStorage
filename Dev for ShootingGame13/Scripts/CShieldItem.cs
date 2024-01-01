using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShieldItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerShip")
        {
            // 플레이어쉽의 업그레이드 기능이 있으면
            CPlayerShipUpgrade upgrade = collision.GetComponent<CPlayerShipUpgrade>();

            if (upgrade != null)
            {
                // 쉴드 시스템을 생성함
                upgrade.UpgradeShieldSystem();
            }

            Destroy(gameObject);
        }
    }
}