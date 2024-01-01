using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    private GameObject cameraParent;

    private Vector3 defaultPosition;    // 초기 좌표 저장
    private Quaternion defaultRotation; // 초기 각도 저장
    private float defaultZoom;      // 초기 줌 저장

    public float rotDamp = 5.0f;

    private Quaternion defaultRot_CP;

    // Use this for initialization
    private void Start()
    {
        // 카메라의 부모를 얻는다
        cameraParent = GameObject.Find("CameraParent");

        // 기본 위치를 저장한다
        defaultPosition = Camera.main.transform.position;
        defaultRotation = cameraParent.transform.rotation;
        defaultZoom = Camera.main.fieldOfView;

        defaultRot_CP = cameraParent.transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        //// 카메라 이동
        //if (Input.GetMouseButton(0))
        //{
        //    Camera.main.transform.Translate(Input.GetAxisRaw("Mouse X") / 10, Input.GetAxisRaw("Mouse Y") / 10, 0);
        //}
        //else
        //{
        //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, defaultPosition, rotDamp * Time.deltaTime);
        //}

        //// 카메라 회전
        //if (Input.GetMouseButton(1))
        //{
        //    cameraParent.transform.Rotate(0, Input.GetAxisRaw("Mouse X") * 10, 0);
        //}
        //else
        //{
        //    cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.rotation, defaultRotation, Time.deltaTime * rotDamp);
        //}

        // 카메라 이동
        if (Input.GetMouseButton(1))
        {
            Camera.main.transform.Translate(0, -Input.GetAxisRaw("Mouse Y") / 10, 0);
            cameraParent.transform.Rotate(0, Input.GetAxisRaw("Mouse X") * 10, 0);
        }
        else
        {
            //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, defaultPosition, rotDamp * Time.deltaTime);
            cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.rotation, defaultRot_CP, Time.deltaTime * rotDamp);
        }

        // 줌 인, 줌 아웃
        Camera.main.fieldOfView += (20 * Input.GetAxis("Mouse ScrollWheel"));

        if (Camera.main.fieldOfView < 10)
        {
            Camera.main.fieldOfView = 10;
        }
        else if (Camera.main.fieldOfView > 120)
        {
            Camera.main.fieldOfView = 120;
        }

        // 카메라 위치 초기화
        if (Input.GetMouseButton(2))
        {
            Camera.main.transform.position = defaultPosition;
            //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, defaultPosition, rotDamp * Time.deltaTime);
            //cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.rotation, defaultRotation, Time.deltaTime * rotDamp);
            Camera.main.fieldOfView = defaultZoom;
        }
    }
}