using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.23 

// NOTE: //# 큐브, 스피어 형태의 Gizmo 사용을 위한 스크립트

public class MyGizmo : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float radius = 0.1f;
    ////public float size = 0.1f;
    public Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);

    ////public bool cube = false;


    public enum MyGizmoType
    {
        SPHERE, CUBE
    }

    public MyGizmoType _myGizmoType = MyGizmoType.SPHERE;

    private void Start()
    {
        ////   cube = false;
    }
    private void OnDrawGizmos()
    {
        Matrix4x4 _rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = _rotationMatrix;

        if (_myGizmoType == MyGizmoType.SPHERE)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(Vector3.zero, radius);
        }
        else if (_myGizmoType == MyGizmoType.CUBE)
        {
            Gizmos.color = _color;
            Gizmos.DrawCube(Vector3.zero, new Vector3(1 * size.x, 1 * size.y, 1 * size.z));

        }

    }

    private void Update()
    {

        UpdateState();
    }

    public void UpdateState()
    {
        switch (_myGizmoType)
        {

            case MyGizmoType.SPHERE:
                ////cube = false;
                break;

            case MyGizmoType.CUBE:
                //// cube = true;
                break;
        }
    }


}
