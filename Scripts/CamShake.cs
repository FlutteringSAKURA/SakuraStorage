using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public Transform camTransform;

    // How long the object should shake for.
    // public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    //  public float decreaseFactor = 1.0f;

    Vector3 originalPos;
   // FireCtrl _fireCtrlScript;
    GameObject _playerObject;
    private float _fireCharge;
    // FollowCam _followCamScript;
  
    void Awake()
    {
        //_fireCtrlScript = GetComponent<FireCtrl>();
        // _followCamScript = GetComponent<FollowCam>();

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    private void Start()
    {
        //  _fireCtrlScript.imgFireChargeBar.fillAmount = _fireCharge;
        // _followCamScript._zoomDynamic = false;
        //  _fireCharge = 0.0f;       
        _playerObject = GameObject.FindGameObjectWithTag("Player");
    }
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        // _fireCharge += Time.deltaTime;   
        //for (vf = 0; vf < _fireCtrlScript.imgFireChargeBar.fillAmount; vf++)
        //{
        //    camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * 0.5f;
        //}
        // 흔들리는 설정 시간이 0보다 크고 ChargieBar의 게이지가 가득차지 않은 동안


        //if (_fireCtrlScript.GetComponentInChildren<FireCtrl>().imgFireChargeBar.fillAmount < 1.0f)
        if(_playerObject.GetComponentInChildren<FireCtrl>().imgFireChargeBar.fillAmount < 1.0f)
        {
            Debug.Log("now Action");         
            //캠을 진동시켜라.
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * 0.5f;

            //shakeDuration -= Time.deltaTime * decreaseFactor;
            // shakeDuration -= Time.deltaTime * _fireCtrlScript.imgFireChargeBar.fillAmount;
        }

        else
        {
            //shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

}
