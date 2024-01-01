using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    // Update: //@ 2023.11.10 
    //# 위아래 보기
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes _axes = RotationAxes.MouseXAndY;
    public float _sensitivityX = 10f;
    public float _sensitivityY = 10f;
    public float _minimumY = -30f;
    public float _maximumY = 30f;
    public float _rotationY = 0f;

    void Start()
    {

    }

    void Update()
    {
        // Update: //@ 2023.11.10 
        float _rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivityX;
        _rotationY -= Input.GetAxis("Mouse Y") * _sensitivityY;
        _rotationY = Mathf.Clamp(_rotationY, _minimumY, _maximumY);
        transform.localEulerAngles = new Vector3(_rotationY, transform.localEulerAngles.y, 0);
    }
}
