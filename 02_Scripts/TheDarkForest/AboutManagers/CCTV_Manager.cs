using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Update: //@ 2023.12.21 
//# NOTE: The Dark Forest의 Virtual Camera의 제어를 위한 스크립트
//#       path : CCTV_Manager

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class CCTV_Manager : MonoBehaviour
{
    [Header("[ REALATED REFERENCE ABOUT GameObject INFO ]")]
    [TextArea(2, 4)]
    public string _descripttion_About_GameObject_References;
    [SerializeField]
    [Tooltip("어둠의 숲 메인 카메라 참조")]
    private GameObject _darkForest_MainCamera;


    [Header("[ REALATED REFERENCE ABOUT Virtual Camera INFO ]")]
    [TextArea(2, 4)]
    public string _descripttion_About_VirtualCamera_References;
    [SerializeField]
    [Tooltip("어둠의 숲 가상카메라 01번 참조")]
    private GameObject _virtualCam01;
    [SerializeField]
    [Tooltip("어둠의 숲 가상카메라 02번 참조")]
    private GameObject _virtualCam02;
    [SerializeField]
    [Tooltip("어둠의 숲 가상카메라 03번 참조")]
    private GameObject _virtualCam03;
    [SerializeField]
    [Tooltip("어둠의 숲 가상카메라 04번 참조")]
    private GameObject _virtualCam04;
    [SerializeField]
    [Tooltip("어둠의 숲 가상카메라 05번 참조")]
    private GameObject _virtualCam05;


    [Space(10f)]
    [Header("[ BOOL CONDITIONS INFO RELATED READY FOR QUESTS ]")]
    [TextArea(3, 5)]
    [Tooltip("가상카메라의 활성/비활성화를 위한 조건")]
    public string _descripttion_About_Bool_Conditions01;

    public bool _CCTV_01_ON_Flag = false;
    public bool _CCTV_02_ON_Flag = false;
    public bool _CCTV_03_ON_Flag = false;
    public bool _CCTV_04_ON_Flag = false;
    public bool _CCTV_05_ON_Flag = false;


    public static CCTV_Manager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }


    //@ 가상카메라를 모두 찾아서 비활성화 
    public void All_CCTV_OFF()
    {
        CinemachineVirtualCamera[] _object_Have_CinemachineVirtualCamera
                        = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();

        for (int i = 0; i < _object_Have_CinemachineVirtualCamera.Length; i++)
        {
            _object_Have_CinemachineVirtualCamera[i].gameObject.SetActive(false);
            Debug.Log("모든 가상카메라를 찾아 비활성화 시켰습니다.");
        }

        //% 어둠의 숲 팔로잉 카메라로 전환을 위한 시네머신 작동 중지 
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = false;

        _CCTV_01_ON_Flag = false;
        _CCTV_02_ON_Flag = false;
        _CCTV_03_ON_Flag = false;
        _CCTV_04_ON_Flag = false;
        _CCTV_05_ON_Flag = false;
        Debug.Log("모든 CCTV가 꺼졌습니다.");
    }


    //@ 가상카메라 01번 작동 
    public void CCTV_01_ON()
    {
        _CCTV_01_ON_Flag = true;

        _virtualCam01.SetActive(true);
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = true;

        _virtualCam02.SetActive(false);
        _virtualCam03.SetActive(false);
        _virtualCam04.SetActive(false);
        _virtualCam05.SetActive(false);
        Debug.Log("CCTV 01번이 켜졌습니다.");
    }
    public void CCTV_01_OFF()
    {
        _CCTV_01_ON_Flag = false;

        _virtualCam01.SetActive(false);

        if (_CCTV_02_ON_Flag || _CCTV_03_ON_Flag || _CCTV_04_ON_Flag || _CCTV_05_ON_Flag)
            return;
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = false;
        Debug.Log("CCTV 01번이 꺼졌습니다.");
    }
    //@ 가상카메라 02번 작동 
    public void CCTV_02_ON()
    {
        _CCTV_02_ON_Flag = true;

        _virtualCam02.SetActive(true);
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = true;

        _virtualCam01.SetActive(false);
        _virtualCam03.SetActive(false);
        _virtualCam04.SetActive(false);
        _virtualCam05.SetActive(false);
        Debug.Log("CCTV 02번이 켜졌습니다.");
    }
    public void CCTV_02_OFF()
    {
        _CCTV_02_ON_Flag = false;

        _virtualCam02.SetActive(false);

        if (_CCTV_01_ON_Flag || _CCTV_03_ON_Flag || _CCTV_04_ON_Flag || _CCTV_05_ON_Flag)
            return;
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = false;

        Debug.Log("CCTV 02번이 꺼졌습니다.");
    }
    //@ 가상카메라 03번 작동 
    public void CCTV_03_ON()
    {
        _CCTV_03_ON_Flag = true;
        _virtualCam03.SetActive(true);
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = true;


        _virtualCam01.SetActive(false);
        _virtualCam02.SetActive(false);
        _virtualCam04.SetActive(false);
        _virtualCam05.SetActive(false);
        Debug.Log("CCTV 03번이 켜졌습니다.");
    }
    public void CCTV_03_OFF()
    {
        _CCTV_03_ON_Flag = false;

        _virtualCam03.SetActive(false);

        if (_CCTV_01_ON_Flag || _CCTV_02_ON_Flag || _CCTV_04_ON_Flag || _CCTV_05_ON_Flag)
            return;
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = false;

        Debug.Log("CCTV 03번이 꺼졌습니다.");
    }

    //@ 가상카메라 04번 작동 
    public void CCTV_04_ON()
    {
        _CCTV_04_ON_Flag = true;
        _virtualCam04.SetActive(true);
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = true;


        _virtualCam01.SetActive(false);
        _virtualCam02.SetActive(false);
        _virtualCam03.SetActive(false);
        _virtualCam05.SetActive(false);
        Debug.Log("CCTV 04번이 켜졌습니다.");
    }
    public void CCTV_04_OFF()
    {
        _CCTV_04_ON_Flag = false;

        _virtualCam04.SetActive(false);

        if (_CCTV_01_ON_Flag || _CCTV_02_ON_Flag || _CCTV_03_ON_Flag || _CCTV_05_ON_Flag)
            return;
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = false;

        Debug.Log("CCTV 04번이 꺼졌습니다.");
    }

    //@ 가상카메라 05번 작동 
    public void CCTV_05_ON()
    {
        _CCTV_05_ON_Flag = true;
        _virtualCam05.SetActive(true);
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = true;


        _virtualCam01.SetActive(false);
        _virtualCam02.SetActive(false);
        _virtualCam03.SetActive(false);
        _virtualCam04.SetActive(false);
        Debug.Log("CCTV 05번이 켜졌습니다.");
    }
    public void CCTV_05_OFF()
    {
        _CCTV_05_ON_Flag = false;

        _virtualCam05.SetActive(false);

        if (_CCTV_01_ON_Flag || _CCTV_02_ON_Flag || _CCTV_03_ON_Flag || _CCTV_04_ON_Flag)
            return;
        _darkForest_MainCamera.GetComponent<CinemachineBrain>().enabled = false;

        Debug.Log("CCTV 04번이 꺼졌습니다.");
    }
}
