using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.26 
// Update: //@ 2023.10.27 

// NOTE: //# 3D 게임 - 게임 매니저 스크립트
//#          1) 게임에 등장하는 공룡들 관리

public class GameManagerController : MonoBehaviour
{
    public static GameManagerController instance;
    List<GameObject> _dinosaurEggs = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    //@ 공룡 생성 함수 
    public void AddNewDinosaurs(GameObject dinosaur)
    {
        //& 외부에서 들어온 공룡과 같은지 다른지
        bool _sameExistFlag = false;        // 새로 받은 공룡을 리스트에 추가하기 위한 플래그
        for (int i = 0; i < _dinosaurEggs.Count; i++)
        {
            //^ dinosaur(외부에서 전달한 값)이 이미 생성되어 있는다면
            if (_dinosaurEggs[i] == dinosaur)
            {
                //^ 넘어온 공룡이 기존의 리스트에 존재
                _sameExistFlag = true;
                Debug.LogError("동일한 공룡이 이미 알에서 부화하여 생성되어 있습니다.");
                break;
            }
        }
        if (!_sameExistFlag)
        {
            _dinosaurEggs.Add(dinosaur);
            Debug.Log("게임매니저 리스트 공룡의 갯수 : " + _dinosaurEggs.Count);
        }

    }

    //@ 공룡 리스트에서 제거하는 함수 
    public void RemoveDinosaur(GameObject dinosaur)
    {
        foreach (GameObject dino in _dinosaurEggs)
        {
            //& 리스트(_dinosaurEggs)안에 있는 것(dino)들이 추가했던 것들(disnoaur)이 맞다면 그 것을 제거하는 조건 코드
            if (dino == dinosaur)
            {
                _dinosaurEggs.Remove(dino);     // NOTE://* RemoveAt과 Remove는 다르다
                break;
            }

        }
    }


    public void ChangeTargetMarkToDinosaur(GameObject dino)
    {
        //& 전부 마크 비활성화
        DeSelectionOfMarkAllDinosaurs();
        //& 선택한 오브젝트는 마크 보이기.
        dino.GetComponent<DinosaurFSM>().ShowSelectionMark();
    }
    //@ 리스트 안 모두 찾아 선택마크 숨기는 코드
    public void DeSelectionOfMarkAllDinosaurs()
    {
        for (int i = 0; i < _dinosaurEggs.Count; i++)
        {
            _dinosaurEggs[i].GetComponent<DinosaurFSM>().HideSelectionMark();
        }
    }
}
