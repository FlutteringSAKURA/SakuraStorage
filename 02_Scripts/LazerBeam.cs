//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

// Update: //@ 2023.10.06

// NOTE: //# Lazer Beam 구현
//#          1) 

public class LazerBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line1;
    private LineRenderer line2;
    private RaycastHit hit;
    //FireController _fireController;
    public Transform _firePos;
    // public float nextFire = 0.1f;
    // private float fireTime = 0.0f;

    public bool laserPointMode = false;

    private void Start()
    {
        // _fireController = GetComponent<FireController>();

        tr = GetComponent<Transform>();
        line1 = GetComponent<LineRenderer>();
        line2 = GetComponent<LineRenderer>();
        line1.useWorldSpace = false;
        line1.enabled = false;
        line1.startWidth = 0.12f;
        line1.endWidth = 0.05f;

        line2.useWorldSpace = false;
        line2.enabled = false;
        line2.startWidth = 0.015f;
        line2.endWidth = 0.008f;
    }

    private void Update()
    {

        //Ray ray = new Ray(tr.position, tr.forward);
        Ray _ray = new Ray(_firePos.position, _firePos.transform.forward);

        // fireTime += Time.deltaTime;
        // if (fireTime >= nextFire)
        // {
        //     fireTime = 0.0f;


        // }

        if (PlayerController.instance._canFireFlag)
        {
            laserPointMode = true;

            line1.enabled = true;
            line1.SetPosition(0, _firePos.InverseTransformPoint(_ray.origin));

            if (laserPointMode)
            {
                // Update: //@ 2023.11.08 
                // PlayerController.instance._zoomCam.SetActive(true);
                PlayerController.instance._zoomCam.GetComponent<Camera>().enabled = true;
                // PlayerController.instance._mainCam.SetActive(false);
                PlayerController.instance._mainCam.GetComponent<Camera>().enabled = false;

                // Update: //@ 2023.11.10 
                PlayerController.instance._zoomCam.GetComponent<PostProcessVolume>().enabled = true;
                PlayerController.instance._zoomCam.GetComponent<PostProcessLayer>().enabled = true;



                PlayerController.instance._shootPoseFlag = true;

                line2.SetPosition(0, tr.InverseTransformPoint(_ray.origin));
                if (Physics.Raycast(_ray, out hit, 100.0f))
                {
                    line2.SetPosition(1, tr.InverseTransformPoint(hit.point));
                }
                else
                {
                    line2.SetPosition(1, tr.InverseTransformPoint(_ray.GetPoint(100.0f)));
                }
                line2.enabled = true;
            }
            else
            {
                line2.enabled = false;
            }

            if (Physics.Raycast(_ray, out hit, 100.0f))
            {
                line1.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else
            {
                line1.SetPosition(1, tr.InverseTransformPoint(_ray.GetPoint(100.0f)));
            }

            //StartCoroutine(this.ShowLaserBeam());

        }
        else
        {
            line1.enabled = false;
            // Update: //@ 2023.11.08 
            //PlayerController.instance._zoomCam.SetActive(false);
            PlayerController.instance._zoomCam.GetComponent<Camera>().enabled = false;
            //PlayerController.instance._mainCam.SetActive(true);
            PlayerController.instance._mainCam.GetComponent<Camera>().enabled = true;

            // Update: //@ 2023.11.10 
            PlayerController.instance._zoomCam.GetComponent<PostProcessVolume>().enabled = false;
            PlayerController.instance._zoomCam.GetComponent<PostProcessLayer>().enabled = false;

        }
    }

    // IEnumerator ShowLaserBeam()
    // {
    //     line1.enabled = true;
    //     yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
    //     line1.enabled = false;

    //}
}
