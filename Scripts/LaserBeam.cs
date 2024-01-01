using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // [ TEST CODE ]
public class LaserBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line1; // [ 포인터 코드 작성시 line1으로 변경
    // [ 포인터 코드 ] -> 새로 생성할 LaserPointer오브젝트를 참조시켜 사용할 수 있도록 참조 변수 선언 
    public LineRenderer line2;
    // ============================== 포인터 코드 End

    private RaycastHit hit;
    // 디버그 코드 확인 후 추가 작성 해줄것
    public float nexFire = 0.1f;
    private float fireTime = 0.0f;
    // =================================== End

    // [ TEST CODE ] FOR [ 포인터 코드 ]
    public bool laserPointMode = false;
    public Image imgFireChargeBar;  // << TEST CODE for ChargeBar

    // ===================================== TEST CODE End

    // TEST CODE for Inventory 
    public InventoryCtrl _inventoryCtrlScript;
    // =================================== Test Code End

    private void Start()
    {
        tr = GetComponent<Transform>();
        line1 = GetComponent<LineRenderer>();
        line1.useWorldSpace = false;
        line1.enabled = false;
        line1.startWidth = 0.1f;
        line1.endWidth = 0.05f;

        // [ 포인터 코드 ] 
        line2.useWorldSpace = false;
        line2.enabled = false;
        line2.startWidth = 0.01f;
        line2.endWidth = 0.005f;
        // ============================== 포인터 코드 End

    }

    private void Update()
    {
        Ray ray = new Ray(tr.position, tr.forward); // 발사 원점에서부터~ 앞쪽 방향으로의 값(선)
        // [ 디버그 코드 ] 확인하고 주석처리후 다음 코드 이어서 작성
       // Debug.DrawLine(ray.origin, ray.direction * 100, Color.blue);
        // ==========================================================디버그 End

        // 왼쪽 마우스 버튼을 누르면
        if (Input.GetMouseButton(0) && imgFireChargeBar.fillAmount < 1.0f && !_inventoryCtrlScript._inventoryOpenChk) 
        {
            fireTime += Time.deltaTime;
            if (fireTime >= nexFire)
            {
                fireTime = 0.0f; // 발사시간 초기화
                // 0 -> 라인의 첫번째 좌표(시작점)인 tr포지션 좌표의 월드좌표를 로컬 좌표로 변환
                // 레이 테스트 코드 -> 파란색 레이가 정상 구현 되는지 Scene View에서 확인 후 아래 코드만 남기고 주석처리
                line1.SetPosition(0, tr.InverseTransformPoint(ray.origin));
                //line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));

                // 플레이하는 경우 레이가 오브젝트를 뚫고 나가는 문제 발생 -> 해결을 위해 다음 코드를 추가해줌
               
                // 레이 테스트 코드
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    line1.SetPosition(1, tr.InverseTransformPoint(hit.point));
                }
                else
                {
                    line1.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));
                }
                
               StartCoroutine(this.ShowLaserBeam());
            }
        }
        // [ 포인터 코드 ]
        if (Input.GetKeyDown(KeyCode.C) && !_inventoryCtrlScript._inventoryOpenChk)
        {
            laserPointMode = !laserPointMode; // 레이저 포인트 온오프 
        }
        if (laserPointMode == true)
        {
            line2.SetPosition(0, tr.InverseTransformPoint(ray.origin));
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                line2.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else
            {
                line2.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100.0f)));
            }
            line2.enabled = true;
        }
        else line2.enabled = false;
    }
    // =================================================== 포인터 코드 End
    
    private IEnumerator ShowLaserBeam()
    {
        line1.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        line1.enabled = false;
    }
   
}
