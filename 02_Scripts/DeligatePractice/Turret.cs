using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.08 

// NOTE: //# Deligate방식이 아닌 C#문법으로 동일한 기능 구현하는 코드 스크립트(1)
//#          1) 

public class Turret : MonoBehaviour
{
    public GameObject _missilePrefab;
    public Transform _firePivot;

    //~ ------------------------------------------------------------------------
    // TEST: //# 1
    // IGameController _iGcontroller;      //& 인터페이스 타입 변수
    // public void SetGameController(IGameController _iGameCtrl)
    // {
    //     this._iGcontroller = _iGameCtrl;
    // }

    // //# NOTE: MonoBehaviour의 파생클래스는 생성자를 만들 수 없음.. 
    // //#             상속받은 클래스 입장에서는 생성자 새로 만들기 불가
    // //#             아래와 같이 코드를 작성해서 GameManagerCtrl의 new생성자를 만들 수 없음
    // /*
    // public Turret(IGameController _iGCtrl)
    // {
    //     this._iGcontroller = _iGCtrl;
    // }
    // */

    // private void Start()
    // {
    //     //# NOTE: 이렇게 그때 그때 생성하는 것은 비효율적이다. 
    //     //#         위의 SetGameController함수를 만들어 사용
    //     // _iGcontroller = new MouseGameController();      //& 인스턴스 생성
    //     // _iGcontroller = new KeyBoardGameController();       //& 인스턴스 생성
    // }

    // private void Update()
    // {
    //     // Legacy:
    //     /*
    //     if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
    //                         //$ || OVRInput.Getdown()OVRInput.Button.One
    //                         // NOTE: //# 이런식으로 계속 늘려가는 것은 비효율적 
    //     {
    //         Debug.Log("미사일 발사");
    //     }
    //     */
    //     if (_iGcontroller != null)
    //     {
    //         if (_iGcontroller.FireMissileButtonPressd())
    //         {
    //             Debug.Log("미사일 발사");
    //         }

    //     }
    //     else
    //     {
    //         Debug.Log("Warining : no Controller");
    //     }


    // }


    //~ ------------------------------------------------------------------------
    // TEST: //# 2
    // public void OnFireMissileButtonPressed()
    // {
    //     Debug.Log("미사일 발사");
    //     //_missilePrefab.SetActive(true);
    //     Instantiate(_missilePrefab, _firePivot.position, _firePivot.rotation);
    // }

    //~ ------------------------------------------------------------------------
    // TEST: //# 3
    public void OnFireMissileButtonPressed(Vector3 position)
    {
        Debug.Log("미사일 발사 : " + position);
        //_missilePrefab.SetActive(true);
        //if(Input.mousePosition == position)
        {
            Instantiate(_missilePrefab, _firePivot.position, _firePivot.rotation);
        }
        
    }
}
