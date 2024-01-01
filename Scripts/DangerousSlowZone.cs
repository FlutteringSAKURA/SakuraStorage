using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousSlowZone : DangerousZoneCtrl
{
    [SerializeField]
    protected float actionTime = 11.5f;
    
    public GameObject TimerCanvas; // Slow Zone 알림 Canvas

    public bool _isOutOfZoneChk = false; // Slow Zone을 벗어났는지 여부


    protected override void Update()
    {
        actionTime -= Time.deltaTime; // Time 정규화    
        if (_enterThePlayer && actionTime <= 0.0f)
        {
            Debug.Log("Slow Action");
            DangerousAction();
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _enterThePlayer = true; // SlowZone 입장 확인
            TimerCanvas.SetActive(true); // Slow Zone Timer UI 활성화    
            actionTime = 11.5f; // 함정 발동 시간 초기화
            
            StartCoroutine(OutOfTheZoneChk()); // SlowZone에서 벗어났는지 여부
        
        }
       
    }

    private IEnumerator OutOfTheZoneChk()
    {
        yield return new WaitForSeconds(1.5f); 
        _isOutOfZoneChk = false;
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _enterThePlayer = false; // 퇴장 확인
            TimerCanvas.SetActive(false);  // Slow Zone Timer UI 비활성화   
            
            _isOutOfZoneChk = true; // SlowZone에서 벗어났는지 여부

            StartCoroutine(OriginMoveSpeed()); //원래의 이동 속도로 되돌림
        }
    }



    private IEnumerator OriginMoveSpeed()
    {
        yield return new WaitForSeconds(3.5f);
        _playerObject.GetComponent<PlayerCtrl>().moveSpeed = 7.0f;
    }

    public override void DangerousAction() // 이동속도 저하 함정 발동
    {
        _playerObject.GetComponent<PlayerCtrl>().moveSpeed = 3.0f;
    }
}
