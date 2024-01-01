using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 직선 이동 기능 컴포넌트 클래스
public class CDirectMovement : MonoBehaviour
{
    public Vector2 _direction; // 방향
    public float _speed; // 이동 속도 (Frame당 이동 크기)
    float _originalSpeed;

    // Awake : 오브젝트 생성 시 자동 호출되는 이벤트 메소드
    // * Start와 차이
    // - Awake 호출 시점에서는 계층뷰에 있는 모든 오브젝트들이 생성되어 있다는 보장이 없음
    // - Start 호출 시점에는  계층뷰에 있는 모든 오브젝트들이 생성되어 있다는 보장되어 있음
    // - 때문에 다른 오브젝트에 대한 참조는 반드시 Start에서 해줘야 함
    private void Awake()
    {
        // 원본 속도를 저장함
        _originalSpeed = _speed;
    }

    void Update()
    {
        // 지정한 방향과 속도로 이동함
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    // 이동 정지
    public void Stop()
    {
        _speed = 0;
    }

    // 이동 재개
    public void Resume()
    {
        _speed = _originalSpeed;
    }
}