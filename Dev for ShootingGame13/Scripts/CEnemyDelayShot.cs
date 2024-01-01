using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyDelayShot : CShot
{
    // 발포 재개 지연 시간
    public float _reStartShotDelayTime;

    // 격발 위치
    private int _shotPosIndex = 0;

    // 이동 컴포넌트 참조
    public CDirectMovement _movement;

    private void Start()
    {
        // 지연 시간뒤에 발포 메소드를 실행함
        Invoke("Shot", _shotDelayTime);
    }

    // 부모
    // public virtual void Shot() <- 오버라이드 할 메소드

    // 자식
    // public override void Shot() <- 오버라이드 된 메소드

    public override void Shot()
    {
        _movement.Stop(); // 이동 중지

        // 부모클래스명::메소드() <- C++
        // base.Shot();

        // 총알을 발포 위치에 생성함
        Instantiate(_laserPrefab, _shotPos[_shotPosIndex].position,
            _shotPos[_shotPosIndex].rotation);

        _shotPosIndex++; // 발포 위치 인덱스 증가

        // 발포 위치 인덱스가 발포 위치 배열의 길이랑 같아지면
        if (_shotPosIndex >= _shotPos.Length)
        {
            _movement.Resume(); // 이동 다시 시작
            Invoke("Shot", _reStartShotDelayTime);
            _shotPosIndex = 0; // 발포 위치를 초기화함
            return;
        }

        Invoke("Shot", _shotDelayTime);
    }
}