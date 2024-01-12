using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class CFoulChecker : MonoBehaviour
{
    public Transform _batTr;
    private CBaseBall _baseBallScript;

    private void Start()
    {
        // _baseBallScript = GetComponent<BaseBall>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BaseBall")
        {
            float dist = Vector3.Distance(transform.position, _batTr.position);
            Debug.Log("RESULT : " + dist);
            CDistanceManager.instance.UpdateDistance(dist);
            CFoulManager.instance.UpdateFoulChk(_text: "파울입니다.");

           // StartCoroutine(InitText());
        }
    }

    private IEnumerator InitText()
    {
        yield return new WaitForSeconds(2.0f);
        CFoulManager.instance.UpdateFoulChk(_text: "");
    }
}
