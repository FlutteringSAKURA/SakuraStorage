using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamCtrl : MonoBehaviour
{
    public Transform followCamTr;
    public float _dist = 2.5f;
    public float _height = 1.0f;
    public float _dampTrace = 20.0f;

    private Transform followTr;

    private RaycastHit _hit;
    private float _camDist;

    private void Start()
    {
        followTr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Ray _ray = new Ray(followCamTr.position, -followCamTr.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 10.0f, Color.blue);
        Physics.Raycast(_ray, out _hit, Mathf.Infinity);
        _camDist = Vector3.Distance(followCamTr.forward, _hit.point);

        if(_camDist <= _dist)
        {
            followTr.position = Vector3.Lerp(followTr.position, followCamTr.position
                - (followCamTr.forward * (_camDist * 0.9f)), Time.deltaTime * _dampTrace);            
        }
        else if (_camDist > _dist)
        {
            followTr.position = Vector3.Lerp(followTr.position, followCamTr.position
                - (followCamTr.forward * _dist), Time.deltaTime * _dampTrace);
        }
        followTr.LookAt(followCamTr.position);
    }



}
