using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObjectGenerator : MonoBehaviour
{
    public Transform _createPos; // 생성 위치

    public GameObject _objectPrefab; // 생성 오브젝트 프리팹

    public float _topCreatePosy; // 위쪽 기둥 생성 위치
    public float _bottomCreatePosy; // 아래쪽 기둥생성 위치

    public float _createStartTime; // 최초 생성 지연 시간 간격
    public float _createDelayTime; // 생성 지연 시간 간격

    private void Start()
    {
        //InvokeRepeating("CreateObject", _createStartTime, _createDelayTime);

        // 코루틴 생성
        StartCoroutine("CreateObjectCoroutine");
    }

    private void Update()
    {
        if (CGameController.IsGameStop)
        {
            // 지정한 이름으로 생성한 코루틴을 파괴함
            StopCoroutine("CreateObjectCoroutine");
        }
    }

    // 오브젝트 생성 코루틴
    IEnumerator CreateObjectCoroutine()
    {
        // 코루틴 시간 지연
        yield return new WaitForSeconds(_createStartTime);
        
        while (true)
        {
            CreateObject(); // 오브젝트 생성

            yield return new WaitForSeconds(_createDelayTime);
        }
    }

    private void CreateObject()
    {
        // 랜덤하게 높이를 구함
        float randY = Random.Range(_topCreatePosy, _bottomCreatePosy);

        // 생성 위치에 랜덤하게 y값을 적용함
        Vector2 createPos = new Vector2(
            _createPos.position.x, _createPos.position.y + randY);

        // 오브젝트 생성
        Instantiate(_objectPrefab, createPos, Quaternion.identity);
    }
}
