using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.11

// NOTE: //# 기즈모 생성기

public class GizmoCtrl : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float radius = 0.1f;

    private void OnDrawGizmos() {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
