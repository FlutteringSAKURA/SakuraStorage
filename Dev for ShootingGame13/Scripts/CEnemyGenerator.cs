using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적기 생성 컴포넌트 클래스
public class CEnemyGenerator : MonoBehaviour
{
    // 적기 프리팹들 참조
    public GameObject[] _enemyPrefab;

    // 생성 위치들 참조
    public Transform[] _genPos;

    // 생성 지연 최소 최대 시간 간격
    public float _genDelayMinTime;
    public float _genDelayMaxTime;

    // 지연 시간
    public float _delayTime;

    private void Start()
    {
        // 적기 생성 타이머를 생성함
        CreateGenTimer();
    }

    public void CreateEnemyShip()
    {
        // 랜덤하게 생성 위치 배열의 번호를 선택함
        int createPosIndex = Random.Range(0, _genPos.Length);

        // 랜덤하게 비행기 타입을 선택함
        int enemyType = Random.Range(0, _enemyPrefab.Length);

        // 프리팹참조, 위치, 회전각도
        Instantiate(_enemyPrefab[enemyType], _genPos[createPosIndex].position, Quaternion.identity);

        // 적기 생성 타이머를 생성함
        CreateGenTimer();
    }

    void CreateGenTimer()
    {
        // 생성을 위한 지연 시간 추출함
        _delayTime = Random.Range(_genDelayMinTime, _genDelayMaxTime);

        // 적기를 생성함
        Invoke("CreateEnemyShip", _delayTime);
    }
}