using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownUI : MonoBehaviour
{


    private void Update()
    {
        transform.Translate(0, -1.0f * Time.deltaTime, 0);
    }
}
