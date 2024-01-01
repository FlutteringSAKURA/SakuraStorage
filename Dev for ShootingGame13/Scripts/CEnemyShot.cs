using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyShot : CShot
{
    // Start() : 게임 오브젝트가 생성된 후 렌더링 되기 바로 전에 딱 1회 자동 호출되는 메소드
    // * 주로 초기화 작업 처리에 사용됨
    private void Start()
    {
        // 총알 발포 타이머를 생성함
        // InvokeRepeating("타이머메소드", 시작지연시간, 반복지연시간);
        //InvokeRepeating("Shot", _shotDelayTime, _shotDelayTime);

        // Invoke("타이머메소드", 시작지연시간);
        // 1회용 총알 발포 타이머를 생성함
        Invoke("TimeShot", _shotDelayTime);
    }

    // 타임 샷
    public void TimeShot()
    {
        Shot();

        // 1회용 총알 발포 타이머를 생성함
        // * 재귀 호출 : 메소드가 같은 메소드를 다시 호출하는 것
        Invoke("TimeShot", _shotDelayTime);
    }
}