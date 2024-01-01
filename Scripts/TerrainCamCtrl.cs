using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCamCtrl : MonoBehaviour
{

    public float horizon = 10f;
    public float vertical = 10f;
    private Transform Camtr;
    public float mSpeed = 10.0f;
    public float rSpeed = 100.0f;

    public bool verticalChk; // 수직 체크
    public bool horizontalChk; // 수평 체크
    public float rotDamp = 2.0f;
    //  GameObject avataCam;
    Quaternion defaultRotation;
    float defaultCamZoom;

    // Mouse Lock Code
    private bool mousePointlock = true;



    // Use this for initialization
    void Start()
    {
        Camtr = GetComponent<Transform>();
        //   avataCam = GameObject.Find("MainCAM");
        defaultRotation = Camtr.transform.rotation;
        defaultCamZoom = Camera.main.fieldOfView;
       
    }

    // Update is called once per frame
    void Update()
    {
        horizon = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 mDirection = (Vector3.forward * vertical) + (Vector3.right * horizon);
        Camtr.Translate(mDirection.normalized * mSpeed * Time.deltaTime);

        //float _verticalPos = Input.GetAxis("Vertical") * mSpeed * Time.deltaTime;
        //float _horizonPos = Input.GetAxis("Horizontal") * mSpeed * Time.deltaTime;

        //transform.position += new Vector3(-1 * _horizonPos, 0, -1 * _verticalPos);

        //Camtr.Rotate(Vector3.up * rSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));



        //}
        //private void LateUpdate()
        //{


        // Camera.main.transform.Rotate(Vector3.up * rSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
        // transform.RotateAround(transform.position, transform.up, Time.deltaTime* Input.GetAxis("Mouse X"));
        // avataCam.transform.Rotate((Vector3.up * rSpeed * Time.deltaTime * Input.GetAxis("Mouse X")));
        //   if (Input.GetAxis("Mouse Y") != 0.0f && !horizontalChk)
        //    {
        //    avataCam.transform.Rotate(Input.GetAxis("Mouse Y") * 10, 0, 0);
        //Camera.main.transform.Rotate(Input.GetAxis("Mouse Y") * 10, 0, 0);
        //  avataCam.transform.rotation = Quaternion.Euler(transform.eulerAngles.x,0, 0);
        //         avataCam.transform.rotation = (Quaternion.Euler(transform.eulerAngles.x, 0, 0));
        //      Camera.main.transform.rotation = Quaternion.Euler(transform.eulerAngles.x * Time.deltaTime, 0, 0);
        //  Camera.main.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y * Time.deltaTime, 0);
        //      Camera.main.transform.rotation = avataCam.transform.rotation;
        // Camera.main.transform.rotation = avataCam.GetComponent<Transform>().rotation;
        // Camera.main.transform.LookAt(transform.forward);
        //       verticalChk = true;

        //        Camtr.Rotate(Vector3.left * rSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));

        //  Camtr.transform.position = Camera.main.transform.position;


        //   Camtr.transform.localRotation = Quaternion.Slerp(Camtr.rotation, transform.localRotation, Time.deltaTime * rotDamp);
        //   Camtr.transform.rotation = Quaternion.Euler(Input.GetAxis("Mouse Y"), 0, 0);

        //    }
        //    else
        //    {
        //         verticalChk = false;
        // Camtr.transform.rotation = Quaternion.Slerp(Camtr.rotation, defaultRotation, Time.deltaTime * rotDamp);
        //    }



        if (Input.GetAxis("Mouse X") != 0.0f)
        {
            horizontalChk = true;
            Camtr.Rotate(Vector3.up * rSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
            // Camtr.rotation = Camtr.rotation;
            //Camtr.transform.rotation = Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0.0f);

        }
        else
        {
            horizontalChk = false;
            // [ Slep Code ]
            //  Camtr.transform.localRotation = Quaternion.Slerp(Camtr.transform.rotation, camPivot.rotation, Time.deltaTime * rotDamp);
        }

        if (Input.GetAxis("Mouse Y") != 0.0f) // downView
        {
            verticalChk = true;

            Camtr.Rotate(Vector3.left * rSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
            //if (Input.GetAxis("Mouse Y") <= -40.0f)
            //{
            //    Camtr.transform.rotation = Quaternion.Euler(-40, 0, 0f);
            //}
        }
        //if (Input.GetAxis("Mouse Y") >= 0.0f) // upView
        //{
        //    verticalChk = true;
        //    Camtr.Rotate(Vector3.left * rSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
        //    if(Input.GetAxis("Mouse Y") >= 40.0f)
        //    {
        //        Camtr.transform.rotation = Quaternion.Euler(40, 0, 0f);
        //    }
        //}
        else
        {
            verticalChk = false;

        }
        //   Camera.main.transform.rotation = defaultRotation;
        // Camtr.transform.rotation = defaultRotation;

        if (Input.GetMouseButton(1))
        {
            Camtr.transform.rotation = Quaternion.Slerp(Camtr.transform.rotation, defaultRotation, Time.deltaTime * rotDamp);
            //  Camtr.transform.rotation = Quaternion.Euler(Input.GetAxis("Mouse Y"), 0, 0);
            //   Camtr.transform.position = Vector3.Slerp(new Vector3(100, 0, 0), new Vector3(0, 0, 100),Y);
        }


        // Mouse Lock Mode
        if (mousePointlock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mousePointlock = !mousePointlock;
        }

        Camera.main.fieldOfView += (20.0f * Input.GetAxis("Mouse ScrollWheel"));
        if (Camera.main.fieldOfView < 10)
        {
            Camera.main.fieldOfView = 10;
        }
        else if (Camera.main.fieldOfView > 120)
        {
            Camera.main.fieldOfView = 120;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.fieldOfView = defaultCamZoom;
        }
    }
}
