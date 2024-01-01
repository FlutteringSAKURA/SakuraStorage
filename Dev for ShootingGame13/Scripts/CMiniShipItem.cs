using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMiniShipItem : MonoBehaviour
{
    public GameObject _miniShipPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CPlayerShipUpgrade upgrade = collision.GetComponent<CPlayerShipUpgrade>();

        if (upgrade != null)
        {
            // 플레이어에게 미니 비행기를 장착함
            upgrade.UpgradeMiniShipSystem(_miniShipPrefab);
            Destroy(gameObject);
        }
    }
}