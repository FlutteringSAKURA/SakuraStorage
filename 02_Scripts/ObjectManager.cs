//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.27 

// NOTE: //# 3D 게임 - 오브젝트 관리 스크립트
//#          1) 골드 생성(오브젝트 풀 방식)

//~ ------------------------------------------------------------------------
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    public GameObject _goldPrefab;
    public int _initialCoins = 30;
    float _randomValue;

    List<GameObject> _goldBox = new List<GameObject>();

    //~ ------------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ ------------------------------------------------------------------------

    private void Start()
    {
        MakingGold();
        _randomValue = Random.Range(0.65f, 0.95f);
    }

    //~ ------------------------------------------------------------------------
    //@ 골드 생성 함수 .. 저장(_tempGold) -> 비활성화(SetActive(false)) -> 담기(_goldBox.Add(_tempGold)) 
    private void MakingGold()
    {
        for (int i = 0; i < _initialCoins; i++)
        {
            //& 골드프리팹을 게임오브젝트 타입으로 생성하여 _tempGold 변수에 넣어줌
            GameObject _tempGold = Instantiate(_goldPrefab) as GameObject;
            //& 오브젝트 매니저의 자식으로 넣어줌
            _tempGold.transform.parent = transform;
            _tempGold.SetActive(false);
            _goldBox.Add(_tempGold);
        }
    }

    //@ 원하는 위치에 골드 생성 ... CallDeadEvent()가 동작되는 그 시점에 골드 생성하는 함수 
    public void DropToGoldPosition(Vector3 pos, int goldValue)
    {
        //& 재활용할 골드 오브젝트가 있는지 여부
        GameObject _reUsedGold = null;
        //& 쓸 것이 있는 경우..
        for (int i = 0; i < _goldBox.Count; i++)
        {
            //^ 코인이 비활성화 되어 있다면
            if (_goldBox[i].activeSelf == false)
            {
                //# _reUsedGold 변수에 넣기
                _reUsedGold = _goldBox[i];
                break;
            }
        }
        //& 쓸 것이 없는 경우..(비활성화된 골드가 하나도 없는 경우) .. 30개 골드가 게임월드상 없어서 새로 만들어 사용
        if (_reUsedGold == null)
        {
            GameObject _reMakeGold = Instantiate(_goldPrefab) as GameObject;
            // //_reMakeGold.transform.parent = transform;
            // //_reMakeGold.SetActive(false);
            _goldBox.Add(_reMakeGold);
            _reUsedGold = _reMakeGold;
        }
        //& 골드를 활성화한 후, 골드의 가치 ..(gold)스크립트의 함수를 가져와 사용
        _reUsedGold.SetActive(true);
        _reUsedGold.GetComponent<Gold>().SetGoldValue(goldValue);
        //& 골드의 위치 좌표 값을 Vector3로 생성(공룡의 x, 재사용가능한 골드의 y좌표값, 공룡의 z)로 생성
        _reUsedGold.transform.position = new Vector3(pos.x + _randomValue, _reUsedGold.transform.position.y, pos.z + _randomValue);
        ////Debug.Log("골드 생성");
    }
}
