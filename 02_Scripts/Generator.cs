using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.26 

// NOTE: //# 3D 게임 - 공룡 생성 스크립트
//#          1) 
//#          2) 
//#          3) 

public class Generator : MonoBehaviour
{
    List<Transform> _dinoGenPosBox = new List<Transform>();
    public GameObject _dinosaurRaptor;      //& 생성할 프리팹
    GameObject[] _dinosaurEggs;

    //& 생성 갯수
    public int _genNumber = 1;
    public float _genCoolTime = 3.0f;
    //& 죽을 때 마다 1씩 증가
    int _deadDino = 0;

    private void Start()
    {
        GenPosSetting();

    }

    //@ 공룡이 생성될 좌표 값 셋팅을 위한 함수 
    private void GenPosSetting()
    {
        //% (transform)좌표에서 (Transform)타입의 genPos 변수 선언
        //% foreach를 활용해 젠오브젝트의 트랜스폼 좌표값을 모두 추출하여 리스트에 저장
        foreach (Transform genPos in transform)
        {
            //& 만약 Transform 타입의 genPos tag가 ("GenPos")라면
            if (genPos.tag.Contains("GenPos"))
            {
                //^ Transform타입의 List로 선언한 변수에 그 genPos를 Add(추가)하기
                _dinoGenPosBox.Add(genPos);
            }
            ////Debug.Log(_dinoGenPosBox.Count);
        }
        //! [방어코드] .. 리스트로 선언한 변수(_dinoGenPosBox)에 담긴 최대 (genPos)좌표숫자 이상으로 생성되지 않게하는 방어코드
        if (_genNumber > _dinoGenPosBox.Count)
        {
            _genNumber = _dinoGenPosBox.Count;
        }
        //& 배열에 갯수 지정해 할당
        _dinosaurEggs = new GameObject[_genNumber];
        DinosaurRaptors();
    }

    //@ 셋팅된 좌표값 위치에 공룡 생성하는 함수 
    private void DinosaurRaptors()
    {
        for (int i = 0; i < _genNumber; i++)        //& 리스트에 담긴 공룡
        {
            //& 공룡 생성 후 그 위치값을 인자로 전달
            GameObject _dinoRaptor = Instantiate(_dinosaurRaptor, _dinoGenPosBox[i].position, Quaternion.identity) as GameObject;
            _dinoRaptor.GetComponent<DinosaurFSM>().SetGenerateObj(gameObject, i, _dinoGenPosBox[i].position);

            //& 몬스터 비활성화(스탠바이)
            _dinoRaptor.SetActive(false);
            //& 배열에 게임오브젝트 공룡 저장
            _dinosaurEggs[i] = _dinoRaptor;
            //& 게임매니저에 만든 몬스터 등록
            GameManagerController.instance.AddNewDinosaurs(_dinoRaptor);
        }
    }

    //@ 콜라이더 트리거시 발생하는 효과에 관한 코드 ..
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            GenDinosaurRaptors();
            //& 재차 콜라이더가 트리거되어 반복 생상되는 것을 방지하기 위한 코드.. 콜라이더 비활성화
            GetComponent<Collider>().enabled = false;
        }
    }

    //@ 리스트에 담긴 만큼 SetActive(true)
    void GenDinosaurRaptors()
    {
        for (int i = 0; i < _dinosaurEggs.Length; i++)
        {
            _dinosaurEggs[i].GetComponent<DinosaurFSM>().ReturnTotheWorld();
            _dinosaurEggs[i].SetActive(true);
        }
    }

    //@ 몬스터 사망후 제거
    public void RemoveDinosaur(int genID)
    {
        _deadDino++;
        ////Destroy(_dinoGenPosBox[genID]);
        _dinosaurEggs[genID].SetActive(false);
        ////Debug.Log("월드에서 사라지려는 랩터 : " + genID);
        if (_deadDino == _dinosaurEggs.Length)
        {
            
            StartCoroutine(InitDinosaurs());
            //& 사망한 공룡수 초기화
            _deadDino = 0;
        }
    }

    IEnumerator InitDinosaurs()
    {
        yield return new WaitForSeconds(_genCoolTime);
        GetComponent<Collider>().enabled = true;        //& 콜라이더 재활성화
    }
}
