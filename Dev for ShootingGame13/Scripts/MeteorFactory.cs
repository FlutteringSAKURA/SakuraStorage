using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFactory : MonoBehaviour {

    // 운석 프리팹 참조 (로드하기위해)
    public GameObject meteorPrefab;

    // C++ 배열
    // Transform meteorPisitions[24];
    // Transform* meteorPositions = new Transform[24];

    // C# 배열
    // public Transform[] meteorPositions = new Transform[24];
    public Transform[] meteorPositions; // 이번 배열 생성은 참조 연결로...

    public float createTime; // 생성 주기

    private void Start()
    {
        // 타이머 생성

        // 1.2 초 마다 FactoryTimer 메소드를 실행해 줌
        InvokeRepeating("FactoryTimer", createTime, createTime);
    }

    // 생산 타이머
    public void FactoryTimer()
    {
        // 생성 위치에 대한 랜덤 번호를 정함
        // 0 ~ 23
        int createPosNum = Random.Range(0, meteorPositions.Length);

        // 운석 생성 위치 오브젝트의 Transform 컴포넌트의 position값을 불러옴
        Vector3 pos = meteorPositions[createPosNum].position;

        // * 운석 파일을 게임 오브젝트를 운석 생성 위치에 생성함
        Instantiate(meteorPrefab, pos, Quaternion.identity); // new GameObject;
    }
}
