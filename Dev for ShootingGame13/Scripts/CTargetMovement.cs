using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTargetMovement : MonoBehaviour
{
    public float _speed; // 이동 속도

    private float _originSpeed; // 원래 속도

    public Vector2 _direction; // 이동 방향

    public Transform _target = null;
    private Vector3 _originTargetPos; // 처음 설정한 타겟의 위치

    // 타겟 방향 설정
    public void Init(Transform target)
    {
        // 타겟이 살아있으면
        if (target != null)
        {
            _target = target;

            // 타겟을 향한 방향을 계산함
            _originTargetPos = _target.position;
            _direction = _originTargetPos - transform.position;
        }
        else // 타겟이 존재하지 않는다면
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // 추적할 타겟을 잃었다면
        if (_target == null)
        {
            // 처음 생성시 설정된 방향으로 이동함
            transform.Translate(_direction.normalized * _speed * Time.deltaTime);
            return;
        }

        // 타겟의 방향을 다시 구해서 이동함
        _direction = _target.position - transform.position;

        //-------------------------

        // 적군 비행기가 방향으로 회전하기 위한 각도를 구함
        float angle = Vector2.Angle(transform.up, _direction.normalized);

        //Debug.Log("angle => " + angle);

        // 회전에 대한 수직 벡터를 외적을 통해 구함
        Vector3 cross = Vector3.Cross(transform.up, _direction.normalized);
        if (cross.z < 0)
        {
            angle = 360 - angle;
        }

        // 회전 하기
        transform.Rotate(0f, 0f, angle);

        //-------------------------

        transform.Translate(_direction.normalized * _speed * Time.deltaTime);
    }
}