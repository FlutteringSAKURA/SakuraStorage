using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCoroutineTest : MonoBehaviour
{
    int count = 0;

    private void Start()
    {
        StartCoroutine("TestCoroutine");
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3);

        count++;
        Debug.Log("count : " + count);

        yield return new WaitForSeconds(5);

        count++;
        Debug.Log("count : " + count);

        yield return new WaitForSeconds(2);

        count++;
        Debug.Log("count : " + count);
    }
}