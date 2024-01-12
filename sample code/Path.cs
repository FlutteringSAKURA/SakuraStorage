using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Path Script - - - - - - - - - - - -

//! Length 속성을 가지고 있으며 요청을 받으면 웨이포인트 배열의 길이를 반환
//! GetPoint 메소드는 배열 내 특정 인덱스의 웨이포인트를 Vector3 위치로 반환

//! OnDrawGizmos 메소드는 에디터 창에서 경로를 그릴 때 사용
//! 게임뷰에서는 우측 상단의 Gizmo를 활성화 하지 않는 한 경로를 그리지 않음
public class Path : MonoBehaviour
{

    public bool bDebug = true;
    public float Radius = 2.0f;
    public Vector3[] pointA;

    public float Length
    {
        get
        {
            return pointA.Length;
        }
    }
    public Vector3 GetPoint(int index)
    {
        return pointA[index];
    }

    private void OnDrawGizmos()
    {
        if (!bDebug)
        {
            return;
        }
        for (int i = 0; i < pointA.Length; i++)
        {
            if (i + 1 < pointA.Length)
            {
				Debug.DrawLine(pointA[i], pointA[i + 1], Color.yellow);
            }
        }
    }
}
