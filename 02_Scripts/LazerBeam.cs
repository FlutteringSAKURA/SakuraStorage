//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update: //@ 2023.10.06

// NOTE: //# Lazer Beam 구현
//#          1) 

public class LazerBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line1;
    private LineRenderer line2;
    private RaycastHit hit;
    public float nextFire = 0.1f;
    private float fireTime = 0.0f;

    public bool laserPointMode = false;

    private void Start()
    {
        tr = GetComponent<Transform>();
        line1 = GetComponent<LineRenderer>();
        line2 = GetComponent<LineRenderer>();
        line1.useWorldSpace = false;
        line1.enabled = false;
        line1.startWidth = 0.1f;
        line1.endWidth = 0.05f;

        line2.useWorldSpace = false;
        line2.enabled = false;
        line2.startWidth = 0.01f;
        line2.endWidth = 0.005f;
    }

    private void Update()
    {
        Ray ray = new Ray(tr.position, tr.forward);

        fireTime += Time.deltaTime;
        if (fireTime >= nextFire)
        {
            fireTime = 0.0f;


        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            laserPointMode = !laserPointMode;

            line1.enabled = true;
            line1.SetPosition(0, tr.InverseTransformPoint(ray.origin));

            if (laserPointMode)
            {
                line2.SetPosition(0, tr.InverseTransformPoint(ray.origin));
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    line2.SetPosition(1, tr.InverseTransformPoint(hit.point));
                }
                else
                {
                    line2.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));
                }
                line2.enabled = true;
            }
            else
            {
                line2.enabled = false;
            }

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                line1.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else
            {
                line1.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));
            }

            // StartCoroutine(this.ShowLaserBeam());


        }
    }

    IEnumerator ShowLaserBeam()
    {
        line1.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        line1.enabled = false;

    }
}
