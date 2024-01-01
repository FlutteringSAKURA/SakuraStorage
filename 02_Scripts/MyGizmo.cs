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

    public enum Type { NORMAL, GENPOINT_CREATURE, GENPOINT_CYBERCREATURE }

    public MyGizmoType _myGizmoType = MyGizmoType.SPHERE;

    //# Update:-----------< CUSTOM >---------------------------------
    public Type _type = Type.NORMAL;
    const string _creatureGenPointFile = "nightmare_Creature";
    const string _cyberCreatureGenPointFile = "cyberCreature";
    //# ------------------------------------------------------------

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


        //# Update:-----------< CUSTOM >---------------------------------

        if (_type == Type.NORMAL)
        {
            Gizmos.color = _color;
            Gizmos.DrawCube(transform.position, new Vector3(1 * size.x, 1 * size.y, 1 * size.z));
        }
        else if (_type == Type.GENPOINT_CREATURE)
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.5f, _creatureGenPointFile, true);
            Gizmos.DrawWireCube(transform.position, new Vector3(1 * size.x, 1 * size.y, 1 * size.z));
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.5f, _cyberCreatureGenPointFile, true);
            Gizmos.DrawWireCube(transform.position, new Vector3(1 * size.x, 1 * size.y, 1 * size.z));

        }
        //# ------------------------------------------------------------
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
