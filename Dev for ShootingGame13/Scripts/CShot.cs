using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShot : MonoBehaviour
{
    // 레이저 프리팹 참조
    public GameObject _laserPrefab;

    // C# 배열
    // 타입[] 배열변수명 = new 타입[갯수];

    // 발포 위치 배열 참조
    public Transform[] _shotPos;

    /*
    public Transform[] _shotPos = new Transform[1];

    private void Start()
    {
        _shotPos[0] = GameObject.Find("ShotPos").transform;
    }
    */

    // 발포 지연 시간
    public float _shotDelayTime;

    protected int _shotCount = 1;

    protected virtual void Start()
    {
        // 발포 카운트를 발포 위치 갯수와 일치함
        _shotCount = _shotPos.Length;
    }

    // C# 오버라이드
    // * 부모의 메소드에 virtual 키워드를 붙이고 오버라이드할 자식 메소드에는 override 키워드를 붙임
    public virtual void Shot()
    {
        // C# 배열 크기 : 배열변수.Length
        for (int i = 0; i < _shotCount; i++)
        {
            // Instantiate(프리팹참조, 복제(생성)위치, 생성 회전각)
            // 프리팹 파일을 게임 오브젝트로 복제함(생성함)
            // * Quaterinion.identity : 월드의 회전각을 그대로
            // * ShotPos.Quaterinion : 발포 위치의 회전각을 기준으로
            Instantiate(_laserPrefab, _shotPos[i].position, _shotPos[i].rotation);
        }
    }
}