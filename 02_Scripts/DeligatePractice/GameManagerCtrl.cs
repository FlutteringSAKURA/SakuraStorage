using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.08 

// NOTE: //# Deligate방식이 아닌 C#문법으로 동일한 기능 구현하는 코드 스크립트(4)
//#          1) 

public class GameManagerCtrl : MonoBehaviour
{
    [SerializeField]
    Turret _turretPrefab;
    Turret _turret;     //& 인스턴스 참조 변수

    private void Start()
    {
        //& 좌표 0, 0, 0 _turretPrefab생성
        _turret = Instantiate(_turretPrefab);

        //~ ------------------------------------------------------------------------
        // TEST: //# 1
        //& 인스턴스를 직접 만들어 주입 (Dependancy Ingection)
        //! _turret.SetGameController(new MouseGameController());     //^ 마우스       
        //_turret.SetGameController(new KeyBoardGameController());    //^ 키보드


        //! 상속받은 클래스 입장에서는 생성자 새로 만들기 불가
        // _turret = new Turret(new KeyBoardGameController());
        //~ ------------------------------------------------------------------------
        // TEST: //# 2

        // //& 마우스 관련 인스턴스 생성(1)
        // // TEMP: MouseGameController _mouseGameController = gameObject.AddComponent<MouseGameController>();

        // //& 키보드 관련 인스턴스 생성(2)
        // KeyBoardGameController _keyBoardController = gameObject.AddComponent<KeyBoardGameController>();


        // //& 델리게이트 작동
        // // TEMP: _mouseGameController.FireMissileButtonPressed += _turret.OnFireMissileButtonPressed;
        // _keyBoardController.FireMissileButtonPressed = _turret.OnFireMissileButtonPressed;

        //~ ------------------------------------------------------------------------
        // TEST: //# 3

         //& 마우스 관련 인스턴스 생성(1)
        MouseGameController _mouseGameController = gameObject.AddComponent<MouseGameController>();

        //& 키보드 관련 인스턴스 생성(2)
        //KeyBoardGameController _keyBoardController = gameObject.AddComponent<KeyBoardGameController>();

        //& 델리게이트 작동
        _mouseGameController.FireMissileButtonPressed += _turret.OnFireMissileButtonPressed;
        //_keyBoardController.FireMissileButtonPressed = _turret.OnFireMissileButtonPressed;
        

    }
}
