using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (_myGizmoType == MyGizmoType.SPHERE)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, radius);
        }
        else if (_myGizmoType == MyGizmoType.CUBE)
        {
            Gizmos.color = _color;
            Gizmos.DrawCube(transform.position, new Vector3(1 * size.x, 1 * size.y, 1 * size.z));
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
