using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.11

// NOTE: //# 2D 게임 - 게임 에너미 제너레이터 제어 스크립트
//#          1) 특정 시점(시간)에 에너미가 규칙적으로 생성 구현     // Completed:
//#          2) 랜덤 좌표에 생성 구현                         // Completed:
//#          3) 게임매니저에서 접근할 수 있도록 정적변수화 ((싱글톤) 사용   // Completed:
//#          4) 게임리셋 함수 구현                                 // Completed:
//#          5)

public class GeneratorCtrl : MonoBehaviour
{
    public Vector3[] _postions = new Vector3[5];        //$ Vector3를 5개 만듬.. 젠 위치 좌표 배열선언
    public GameObject _enemyPrefab;     //$ 에너미 생성 프리팹
    
    [SerializeField]
    List<GameObject> _enemiesListBox = new List<GameObject>();

    public bool _isGenStart = false;

    public float _timeFlow = 0.0f;
    public float _coolTime = 1.5f;

    public static GeneratorCtrl instance;
    private void Awake() {
        if (GeneratorCtrl.instance == null)
        {
            GeneratorCtrl.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    

    private void Start()
    {
        CreateGenPositions();
    }

    private void Update()
    {
        GenEnemies();
    }

    private void GenEnemies()
    {
        //# 에너미들이 살아있으면 자연스럽게 시간이 흐르다가 1.5f의 시간값보다 커지면 에너미 생성
        if (_isGenStart)
        {
            _timeFlow += Time.deltaTime;

            if (_timeFlow > _coolTime)
            {
                //^ 위치 무작위 생성((0~생성한 위치 배열의 길이만큼)하여 그 위치에 에너미들 출현
                int _randomPosition = Random.Range(0, _postions.Length);

                //^ 에너미를 생성한 월드좌표 배열((_postions)의 랜덤위치(_randomPosition)에 생성
                //! 방법 (1)
                //Instantiate(_enemyPrefab, _postions[_randomPosition], Quaternion.identity);
                //! 방법 (2)    _enemy라는 변수명으로 게임오브젝트타입으로 만들어 넣어서 사용
                GameObject _enemy = Instantiate(_enemyPrefab, _postions[_randomPosition], Quaternion.identity) as GameObject;
                //^ _enemyPrefab을 _enemiesListBox((리스트)에 저장
                //! 방법 (1)
                //_enemiesListBox.Add(_enemyPrefab);
                //! 방법 (2)
                _enemiesListBox.Add(_enemy);

                //Debug.Log("생성된 리스트의 갯수 : " + _enemiesListBox.Count + "갯수");

                 //^ 시간 흐름 초기화
                _timeFlow = 0.0f;      
            }

        }
    }

    //& 플레이어가 사망시 작동
    public void ClearEnemies()
    {
        // TEMP:
        for (int i = 0; i < _enemiesListBox.Count; i++)
        {
            if(_enemiesListBox[i] != null)  //! 방어 코드
            {
                Destroy(_enemiesListBox[i]);    
            }
        }
        _enemiesListBox.Clear();
        //Debug.Log("에너미 올제거, 클리어");
    }

    private void CreateGenPositions()       //& 에너미를 생성시킬 젠 위치 생성 함수
    {   
        //^ 뷰포트 기준 적 생성 Y좌표
        float _viewPosY = 1.2f;
        //^ 뷰포트 기준 적 생성 Y좌표
        float _viewPosX = 0f;
        //^ 생성할 에너미 간격
        float _gapX = 1f / 6f;
        
        //^ 배열로 선언한 5개의 좌표를 탐색(looping)
        //& 5개의 위치값 배열 길이 만큼 ++   NOTE: for : 조건 불필요 [VS] foreach : 조건 필요
        for (int i = 0; i < _postions.Length; i++)    //$ 배열의 길이 값(_postions.Length)만큼 루프를 도는 함수
        {
            //^ _viewPosX의 좌표에 _gapX 만큼 띄운 값을 넣어줌
            _viewPosX = _gapX + _gapX * i;

            //^ _viewPos 값을 생성 (= Vector3의 viewPort 값)
            Vector3 _viewPos = new Vector3(_viewPosX, _viewPosY, 0);

            //^ 뷰포트 좌표(왼쪽)를 월드 좌표(오른쪽)로 전환 ((담기))
            Vector3 _worldPos = Camera.main.ViewportToWorldPoint(_viewPos);

            _worldPos.z = 0;    //^ z값 월드좌표 변경 (안전한 코딩을 위해)
            
            _postions[i] = _worldPos;       //! 월드 좌표로 생성한 위치(오른쪽)를 배열(왼쪽)에 넣음

            //Debug.Log("월드좌표 확인 :" + _worldPos);

        }
    }
}
