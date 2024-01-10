using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.12.08 
//# NOTE: Object를 Scene View처럼 제어하기 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class MakingObjectLikeSceneView : MonoBehaviour
{

    [Header("[ RELATED REFERENCE INFO ]")]
    [TextArea(3, 5)]

    [Tooltip("입욕제 병의 종류 참조")]
    public string _description_About_Related_ReferenceInfo;

    [SerializeField]
    private GameObject _blueBottle_01;

    [SerializeField]
    private GameObject _yellowBottle_02;
    [SerializeField]
    private GameObject _purpleBottle_03;
    [SerializeField]
    private GameObject _redBottle_04;
    [SerializeField]
    private GameObject _greenBottle_05;

    [SerializeField]
    [Tooltip("입욕제 병을 확대하여 관찰하기 위한 병을 고를 때 조건을 확인하기 위한 스크립트 참조")]
    private SweetHomeSweet_Quest_Manager _q_4_BathBombWater_script;


    [Header("[ RELATED OBJECT INSPECTING INFO ]")]
    [TextArea(3, 5)]
    [Tooltip("입욕제 병을 확대하여 관찰하기 위한 변수 선언")]
    public string _description_About_Related_InspectInfo;

    public float _rotationX;
    public float _rotationY;
    public float _rotattionSpeed = 5.0f;

    public bool _isRotating = false;

    // TEST:
    Vector3 _startMousePos;
    Vector3 _currentMousePos;


    GameObject _lookAtPosition;

    [SerializeField]
    GameObject _lookAtPos_Key;


    Vector3 _lastMousePosition;

    private void Start()
    {

        _lookAtPosition = GameObject.Find("LookAtPosition");
        //_lookAtPos_Key = GameObject.Find("LookAt_Pos_FindKey");
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //& 마우스 버튼이 눌렸을 때 마지막 마우스 위치를 저장합니다.
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            //% Begin Scene - 입욕제

            if (_q_4_BathBombWater_script._bathBomb_Number01)
            {
                //& 마우스 버튼이 눌린 상태일 때
                Vector3 delta = Input.mousePosition - _lastMousePosition; // 마우스의 이동 거리를 계산합니다.
                Vector3 axis = new Vector3(delta.y, -delta.x, 0); // 회전 축을 설정합니다.
                _lookAtPosition.transform.Rotate(axis * _rotattionSpeed * Time.deltaTime, Space.World); // 오브젝트를 회전시킵니다.
                _lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 업데이트합니다.
            }

            else if (_q_4_BathBombWater_script._bathBomb_Number02)
            {
                //& 마우스 버튼이 눌린 상태일 때
                Vector3 delta = Input.mousePosition - _lastMousePosition; // 마우스의 이동 거리를 계산합니다.
                Vector3 axis = new Vector3(delta.y, -delta.x, 0); // 회전 축을 설정합니다.
                _lookAtPosition.transform.Rotate(axis * _rotattionSpeed * Time.deltaTime, Space.World); // 오브젝트를 회전시킵니다.
                _lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 업데이트합니다.
            }

            else if (_q_4_BathBombWater_script._bathBomb_Number03)
            {
                //& 마우스 버튼이 눌린 상태일 때
                Vector3 delta = Input.mousePosition - _lastMousePosition; // 마우스의 이동 거리를 계산합니다.
                Vector3 axis = new Vector3(delta.y, -delta.x, 0); // 회전 축을 설정합니다.
                _lookAtPosition.transform.Rotate(axis * _rotattionSpeed * Time.deltaTime, Space.World); // 오브젝트를 회전시킵니다.
                _lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 업데이트합니다.
            }

            else if (_q_4_BathBombWater_script._bathBomb_Number04)
            {
                //& 마우스 버튼이 눌린 상태일 때
                Vector3 delta = Input.mousePosition - _lastMousePosition; // 마우스의 이동 거리를 계산합니다.
                Vector3 axis = new Vector3(delta.y, -delta.x, 0); // 회전 축을 설정합니다.
                _lookAtPosition.transform.Rotate(axis * _rotattionSpeed * Time.deltaTime, Space.World); // 오브젝트를 회전시킵니다.
                _lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 업데이트합니다.
            }
            else if (_q_4_BathBombWater_script._bathBomb_Number05)
            {
                //& 마우스 버튼이 눌린 상태일 때
                Vector3 delta = Input.mousePosition - _lastMousePosition; // 마우스의 이동 거리를 계산합니다.
                Vector3 axis = new Vector3(delta.y, -delta.x, 0); // 회전 축을 설정합니다.
                _lookAtPosition.transform.Rotate(axis * _rotattionSpeed * Time.deltaTime, Space.World); // 오브젝트를 회전시킵니다.
                _lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 업데이트합니다.
            }

            //% Begin Scene - 열쇠
            else
            {
                //& 마우스 버튼이 눌린 상태일 때
                Vector3 delta = Input.mousePosition - _lastMousePosition; // 마우스의 이동 거리를 계산합니다.
                Vector3 axis = new Vector3(delta.y, -delta.x, 0); // 회전 축을 설정합니다.
                _lookAtPos_Key.transform.Rotate(axis * _rotattionSpeed * Time.deltaTime, Space.World); // 오브젝트를 회전시킵니다.
                _lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 업데이트합니다.
            }

        }
    }

    /*
        void OnMouseDrag()
        {
            _rotationX = Input.GetAxis("Mouse X") * _rotattionSpeed * Mathf.Deg2Rad;
            _rotationY = Input.GetAxis("Mouse Y") * _rotattionSpeed * Mathf.Deg2Rad;

            if (_q_4_BathBombWater_script._bathBomb_Number01)
            {
                _blueBottle_01.transform.RotateAroundLocal(Vector3.up, _rotationX);
                _lookAtPosition.transform.RotateAroundLocal(Vector3.right, -_rotationY);

                Debug.Log("1번째 입욕제 관찰 중");
            }
            else if (_q_4_BathBombWater_script._bathBomb_Number02)
            {
                _yellowBottle_02.transform.RotateAround(Vector3.up, -_rotationX);
                _yellowBottle_02.transform.RotateAround(Vector3.right, -_rotationY);
                Debug.Log("2번째 입욕제 관찰 중");
            }
            else if (_q_4_BathBombWater_script._bathBomb_Number03)
            {
                _purpleBottle_03.transform.RotateAround(Vector3.up, -_rotationX);
                _purpleBottle_03.transform.RotateAround(Vector3.right, -_rotationY);
            }
            else if (_q_4_BathBombWater_script._bathBomb_Number04)
            {
                _redBottle_04.transform.RotateAround(Vector3.up, _rotationX);
                _redBottle_04.transform.RotateAround(Vector3.right, _rotationY);
            }
            else
            {
                _greenBottle_05.transform.RotateAround(Vector3.up, _rotationX);
                _greenBottle_05.transform.RotateAround(Vector3.right, _rotationY);
            }
        }
    */


}
