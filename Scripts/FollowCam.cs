using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform targetTr;
    // [ TEST CODE ] -> 줌 인 모드를 위한 Transform 인스턴스 선언
    public Transform zoomTr;

    public float dist = 4.5f;           // 거리
    public float height = 1.0f;         // 높이
    public float dampTrace = 20.0f;     // 메인카메라의 반응속도?
    private Transform tr;

    // [ 플러스 코드 ]
    private RaycastHit hit;
    private float camDist;
    // ========================== 플러스 코드 End

    // [TEST CODE] =========
    // 줌 다이나믹 셋팅 인스턴스 선언
    public bool _zoomDynamic = false;
    //public bool _camfix = false;
    //======================

    // [ TEST CODE ] for CAMERA ROTATION
    // private GameObject secondCamera;
    // Test Code End

    // test code for camview when dynamicmode
    // PlayerCtrl _playerCtrlScript;
    // ====== test code end
    // test code for View Fix when Jump  
    // public Transform _jumpCamTr;
    //public  Camera _jumpCam;
    //public   GameObject _jumpCam;

    // TEST CODE for View Fix When Jump Action
    public Camera _jumpCamera;
    //private RaycastHit _hit;
    public bool _onGround = true;
    GameObject playerObject;
    // ============================== test code end

    private void Start()
    {
        tr = GetComponent<Transform>();

        // [ TEST CODE ] for CAMERA ROTATION
        //  secondCamera = GameObject.Find("SecondCamera");
        // Test Code End

        // test code for camview when dynamicmode
        //   _playerCtrlScript = GetComponent<PlayerCtrl>();
        // ========== test code end
        // test code for View Fix when Jump  
        //  _jumpCam = GetComponent<Camera>();
        //   _jumpCam = GetComponent<GameObject>();
        // playerObject = GetComponent<GameObject>();

        // TEST CODE for View Fix When Jump Action
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // ===================================== test code end
        //  _hit = GetComponent<RaycastHit>();

    }

    private void LateUpdate()
    {
        // [ 플러스 코드 ] -> 추가후 디버그 확인하고 다음 코드는 주석처리
        //tr.position = Vector3.Lerp(tr.position, targetTr.position
        //    - (targetTr.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);

        // [ 플러스 코드 ]
        Ray ray = new Ray(targetTr.position, -targetTr.forward); // 플레이어의 뒤로 Ray 생성
        // 디버그 뒤로 Ray 생성되는지 확인 후 주석처리
        Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.green);
        Physics.Raycast(ray, out hit, Mathf.Infinity);  // Infinity : 거리 -> 무한 감지 >> 감지되면 hit에 정보를 넣어줌
        camDist = Vector3.Distance(targetTr.position, hit.point); // 플레이어 위치에서 hit포인트 사이의 거리

        // TEST CODE for View Fix when Jump Action
        Ray _jumpCamRay = new Ray(tr.position, Vector3.down * 2.5f); // 아래 2.5미터 만큼 레이를 쏴라
        Debug.DrawRay(tr.position, Vector3.down * 2.5f, Color.red);
        // ============================================================= Test Code End

        if (camDist <= dist && _zoomDynamic == false) // 캠의 거리가 설정한 거리와 같아지거나 짧아지면 
        {
            tr.position = Vector3.Lerp(tr.position, targetTr.position
             - (targetTr.forward * (camDist * 0.9f)) + (Vector3.up * height), Time.deltaTime * dampTrace);
        }
        else if (camDist > dist)
        {
            tr.position = Vector3.Lerp(tr.position, targetTr.position
            - (targetTr.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);
        }

        // tr.LookAt(targetTr.position); // [일시 주석처리]
        // ======================================== 플러스 코드 END

        // [ TEST CODE ] -> 다이나믹 모드 
        //케릭터가 지면에 닿아있는 상태에서 왼쪽 컨트롤 키를누르면
        if (Input.GetKeyDown(KeyCode.LeftControl) && _onGround)
        {
            // 다이나믹모드 변경가능
            _zoomDynamic = !_zoomDynamic;
        }
        // test code for camview when dynamicmode
        //if (_zoomDynamic && Input.GetKeyDown(KeyCode.Space))
        //{
        //    tr.position = Camera.main.transform.position;
        //    //StartCoroutine(OriginCamview());
        //}
        // ============= test code end

        // TEST CODE for View Fix when Jump Action
        // 점프를 하거나 캐릭터가 추락 상태거나 지면위에 있는 상태가 아니라면 
        if (Input.GetKeyDown(KeyCode.Space) || playerObject.GetComponent<PlayerCtrl>()._isFalling || !playerObject.GetComponent<PlayerCtrl>()._groundChk)
        {
            // 지면 위의 상태가 아니다
            _onGround = false;
        }
        // Jump Cam으로부터의 Ray를 2.8미터 아래로 쏘아 닿는다면
        else if (Physics.Raycast(_jumpCamRay, out hit, 2.2f))
        {
            
            // 지면 위의 상태가 맞다
            _onGround = true;
            StartCoroutine(OriginCamView());
            //if (_hit.collider.tag == null)
            //{
            //    _ontheGround = false;
            //}
        }
        // ================================== Test Code End

        if (_zoomDynamic)
        {
            // 케릭터 시점으로 카메라 줌인
            ZoomCtrl();
            _zoomDynamic = true;

            // TEST CODE for View Fix when Jump
            // 지면 위가 아니라면
            if (!_onGround) 
            {
                // 점프 카메라를 켜라
                _jumpCamera.enabled = true;
             //   _jumpCamera.transform.position = tr.position;
            }
            //else if (_ontheGround)
            //{ _jumpCamera.enabled = false; }
            // ==================================== Test Code End
        }
        else if (!_zoomDynamic)
        {
            tr.position = Vector3.Lerp(tr.position, targetTr.position - (targetTr.forward * dist)
             + (Vector3.up * height), Time.deltaTime * dampTrace);
            tr.LookAt(targetTr.position);
            _zoomDynamic = false;
        }

        //========================= TEST CODE END

        // [ TEST CODE ] for CAMERA ZOOM IN, OUT
        //if (Input.GetKey(KeyCode.F))
        //{
        //    secondCamera.transform.Rotate(0, Input.GetAxisRaw("Mouse X") * 10 , 0);
        //}

        Camera.main.fieldOfView += (20 * Input.GetAxis("Mouse ScrollWheel"));
        if (Camera.main.fieldOfView < 20)
        {
            Camera.main.fieldOfView = 20;
        }
        else if (Camera.main.fieldOfView > 100)
        {
            Camera.main.fieldOfView = 100;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Camera.main.fieldOfView = 60;
        }



        // [ TEST CODE ] for CAMERA ZOOM IN & OUT
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //      tr.position = Vector3.Lerp(tr.position, targetTr.position
        //     - (targetTr.forward * (camDist * 0.9f)) + (Vector3.up * height), Time.deltaTime * dampTrace);
        //}

        // ================================= Test Code End

    }

    // TEST CODE for View Fix when Jump Action
    private IEnumerator OriginCamView()
    {
        // 0.3초 후 점프카메라를 끈다.
        yield return new WaitForSeconds(0.3f);
        
         _jumpCamera.enabled = false; 
    }
    // ================== test code end

    // [ TEST CODE ] 
    void ZoomCtrl()
    {

        // 카메라를 케릭터의 시선으로 위치
        //tr.position = Vector3.Lerp(tr.position, zoomTr.position, Time.deltaTime * dampTrace);
        //tr.LookAt(zoomTr.position);
   
       tr.position = zoomTr.position;

        //   if (Input.GetMouseButton(0))
        //    {
        // TEST CODE for Shot Effect
        //  Camera.main.transform.Translate(0, 0.5f, 0);
        //   tr.position = Vector3.Lerp(tr.position, zoomTr.position, Time.deltaTime * dampTrace);
        // ========== test code end
        //  }
    }
    // =========================================== TEST CODE END


}