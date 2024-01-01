using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGenCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "ActionFlower") return;
        {
            Destroy(collision.gameObject);
        }
    }

}
