using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoobyTrapCtrl : MonoBehaviour
{
    private Transform groundTr;
    public bool _isGroundChk = false; // 초기값 
    private float xValue = 0;
    private float yValue = 0;

    private void Start()
    {
        groundTr = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Gound Contact");
        _isGroundChk = true;

        if (collision.gameObject.tag == "Player")
        {            
            //groundTr.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(ActionBoobyTrap());
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        
    }

    private IEnumerator ActionBoobyTrap()
    {
        WaitForSeconds waitTime = new WaitForSeconds(3.0f);
        // yield return new WaitForSeconds(3.0f);
        
        while (_isGroundChk)
        {
            yield return waitTime;
            xValue = 45f;
            yValue = 45f;
            groundTr.transform.localScale = new Vector3(xValue, yValue, 1);
            
            if (xValue == 45 && yValue == 45)
            {
                yield return waitTime;
                xValue = 35f;
                yValue = 35f;
                
            }
            
            if (xValue == 35 && yValue == 35)
            {
                yield return waitTime;
                xValue = 20f;
                yValue = 20f;
                groundTr.transform.localScale = new Vector3(xValue, yValue, 1);


            }
            else
            {
                yield return waitTime;
                xValue = 10f;
                yValue = 10f;
                groundTr.transform.localScale = new Vector3(xValue, yValue, 1);
                break; 
            }
            Destroy(gameObject, 1.5f);
               
            
        }
    
    }
}
