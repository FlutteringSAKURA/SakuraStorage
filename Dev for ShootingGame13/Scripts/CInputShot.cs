using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInputShot : CShot
{
    // 발포 간격 시간
    public float _shotDelayRange;

    // 부모의 Start 메소드를 재정의 함
    protected override void Start()
    {
    }

    private void Update()
    {
        // Time.deltaTime : 이전 업데이트와의 간격 시간
        // 발포 지연 시간 계산
        _shotDelayTime += Time.deltaTime;

        // 스페이키를 눌렀다면 true
        // * 눌리는 순간 1회만 true

        // 레이저 발포 키를 누르고 발포 제한 간격이 넘었다면
        if (Input.GetKeyDown(KeyCode.Space) && _shotDelayTime >= _shotDelayRange)
        {
            Shot();

            // 발포 지연 시간을 초기화함
            _shotDelayTime = 0f;
        }
    }

    // 발포 카운트 증가
    public void ShotCountUp()
    {
        if (_shotCount >= _shotPos.Length) return;

        _shotCount += 2;
    }
}