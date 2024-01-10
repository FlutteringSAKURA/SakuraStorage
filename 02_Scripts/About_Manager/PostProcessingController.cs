using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

// Update: //@ 2023.11.23 
//# NOTE: CinemaChine PostProcessing Effect 제어를 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class PostProcessingController : MonoBehaviour
{

    public PostProcessProfile _profile;
    private AutoExposure _autoExposure;

    // Update: //@ 2023.11.30 
    private DepthOfField _depthOfFeild;

    public float _timeFlow = 0.0f;
    public float _wakeUpMaxTime = 0.8f;

    public bool _autoExposureCheckFlag = false;
    public bool _depthOfFeildCheckFlag = false;


    // Update: //@ 2023.11.30 
    public static PostProcessingController instance;
    public enum PostProcessing_VirtualCameraNumber
    {
        VIRTUAL_00, VIRTUAL_01, VIRTUAL_02, VIRTUAL_03, VIRTUAL_04, VIRTUAL_05,
        VIRTUAL_06, VIRTUAL_07, VIRTUAL_08
    }
    public PostProcessing_VirtualCameraNumber _currentVirtualCamNum =
    PostProcessing_VirtualCameraNumber.VIRTUAL_00;


    //~ -------------------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ -------------------------------------------------------------------------------

    private void Start()
    {
        _profile.TryGetSettings(out _autoExposure);

    }

    //~ -------------------------------------------------------------------------------

    private void Update()
    {

        SelectionVirtualCam();

    }

    //~ -------------------------------------------------------------------------------

    private void SelectionVirtualCam()
    {
        switch (_currentVirtualCamNum)
        {
            case PostProcessing_VirtualCameraNumber.VIRTUAL_00:

                _timeFlow += Time.deltaTime;

                {
                    _autoExposure.minLuminance.value -= _timeFlow / 10;

                    //& 가상 카메라를 01번으로 바꿔 시간 흐름이 계속되는 것을 방지
                    StartCoroutine(MoveToVirtualCam01());
                    //// Debug.Log("Exposure if");

                }

                break;

            case PostProcessing_VirtualCameraNumber.VIRTUAL_01:

                break;

            case PostProcessing_VirtualCameraNumber.VIRTUAL_02:

                DepthOfField();

                _timeFlow += Time.deltaTime;
                if (!_depthOfFeildCheckFlag)
                {
                    _depthOfFeild.focusDistance.value -= _timeFlow % 2;
                    StartCoroutine(ResetFocusDistanceValue());
                    //// Debug.Log("depth TEST");

                    if (_depthOfFeild.focusDistance.value <= 1.8)
                    {
                        _depthOfFeild.focusDistance.value = 1.8f;
                        _depthOfFeildCheckFlag = true;
                        //// Debug.Log("depth Stop");
                    }
                }

                break;

            case PostProcessing_VirtualCameraNumber.VIRTUAL_03:

                break;
            case PostProcessing_VirtualCameraNumber.VIRTUAL_04:

                break;
            case PostProcessing_VirtualCameraNumber.VIRTUAL_05:

                break;
            case PostProcessing_VirtualCameraNumber.VIRTUAL_06:

                break;


            case PostProcessing_VirtualCameraNumber.VIRTUAL_07:

                _timeFlow += Time.deltaTime;

                if (!_autoExposureCheckFlag)
                {
                    _autoExposure.minLuminance.value += _timeFlow / 2;

                    ////Debug.Log("Exposure TEST");

                    if (_autoExposure.minLuminance.value >= 8.5f)
                    {
                        _autoExposureCheckFlag = true;

                        _autoExposure.minLuminance.value = 0.0f;
                        ////  Debug.Log("Exposure TEST2");
                    }
                }

                break;


            case PostProcessing_VirtualCameraNumber.VIRTUAL_08:

                break;


        }
    }

    IEnumerator MoveToVirtualCam01()
    {

        yield return new WaitForSeconds(4.0f);
        _autoExposure.minLuminance.value = -1.8f;
        _currentVirtualCamNum = PostProcessing_VirtualCameraNumber.VIRTUAL_01;
    }

    IEnumerator ResetFocusDistanceValue()
    {
        yield return new WaitForSeconds(6.0f);
        _depthOfFeild.focusDistance.value = 6.8f;
    }

    // IEnumerator ResetExposureValue()
    // {
    //     yield return new WaitForSeconds(2.0f);
    //     _autoExposure.minLuminance.value = -1.5f;

    // }

    public void DepthOfField()
    {
        _profile.TryGetSettings(out _depthOfFeild);

    }

}
