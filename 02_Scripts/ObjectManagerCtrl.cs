using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.12

// NOTE: //# 2D 게임 - 오브젝트 풀링을 위한 제어 스크립트
//#          1) 총알 재생성 및 관리 기능 (= 오브젝트 풀 기능)
//#          2) 

public class ObjectManagerCtrl : MonoBehaviour
{
    public static ObjectManagerCtrl instance;
    public GameObject _lazerPrefab;

    [SerializeField]
    List<GameObject> _lazerBox = new List<GameObject>();    //^ 레이저 저장소 (= 배열) .. 레이저를 가져와서 여기에 담는 의미



    private void Awake()
    {
        if (ObjectManagerCtrl.instance == null)
        {
            ObjectManagerCtrl.instance = this;
        }
    }

    private void Start()
    {
        //^ 게임이 실행되면 레이저 5개 주입
        InjectionLazer(5);
    }

    //! _lazer 프리팹을 담는 함수
    void InjectionLazer(int _lazerShootableNum)
    {
        for (int i = 0; i < _lazerShootableNum; i++)
        {
            //^ Instantiate로 생성한 게임오브젝트를 변수 _lazer에 담는 코드
            GameObject _lazer = Instantiate(_lazerPrefab) as GameObject;
            //^ _lazer의 부모 위치좌표를 이 게임오브젝트의 위치좌표로 하는 코드 (= _lazer를 이 게임오브젝트의 자식으로 넣는 코드)
            _lazer.transform.parent = transform;
            //^ 초기 레이저 비활성화
            _lazer.SetActive(false);
            //^ 레이저 박스 리스트에 레이저 담기
            _lazerBox.Add(_lazer);

        }
    }

    //! _lazer를 충전하기(가져오기)..   이 함수에서 return  ..   void 타입의 함수가 아니면 return을 반드시 써야 함
    public GameObject ChargeLazer(Vector3 _playerShipPos)     //$ 외부전달 위치 .. 필요(..)
    {
        //& 필요한 레이저가 없다면..
        GameObject _requireLazer = null;
        //& 레이저((프리팹)가 담긴 박스를 뒤지기(배열 검색)
        for (int i = 0; i < _lazerBox.Count; i++)
        {
            if (_lazerBox[i].activeSelf == false)   //^ 레이저 박스(배열)에 들어있는 것들이 비활성상태
            {
                _requireLazer = _lazerBox[i];   //^ 필요한 레이저가 없다면 ..레이저 박스의 배열에 들어있는 것을 가져오는 효과
                break;
            }
        }

        //& 레이저 소진 후 재생산해서 활용
        if (_requireLazer == null)
        {
            GameObject _newChargeLazer = Instantiate(_lazerPrefab) as GameObject;
            _newChargeLazer.transform.parent = transform;
            //^ 새로 차지된 레이저도 레이저 박스에 담기
            _lazerBox.Add(_newChargeLazer);
            //^ 새로 차지된 레이저로 치환 (..)
            _requireLazer = _newChargeLazer; 
        }

        _requireLazer.SetActive(true);  //^ 활성화
        _requireLazer.transform.position = _playerShipPos;   //$ 플레이어 위치의 좌표 값으로 치환
        return _requireLazer;   //^ 이 함수밖으로 리턴해주는 것.. 

    }

    //! 게임오버가 되면 인게임 내에 남아있는 레이저 클리어
    public void ClrearLazer()
    {
        for (int i = 0; i < _lazerBox.Count; i++)
        {
            _lazerBox[i].SetActive(false);
            Debug.Log("인게임상 레이저 초기화");
        }
    }


}
