using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLaserUpItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerShip")
        {
            collision.GetComponent<CInputShot>().ShotCountUp();

            Destroy(gameObject);
        }
    }
}