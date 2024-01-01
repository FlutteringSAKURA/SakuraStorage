using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Poison Zone Script

public class DangerousPoisonZone : DangerousZoneCtrl
{
    public bool _poisonChk = false;

    [SerializeField]
    protected  float actionTime = 11.5f;

    
    public GameObject TimeCanvas; // Poison Zone 알림 Canvas

    public bool _isOutOfZoneChk = false; // Poison Zone을 벗어났는지 여부
    

    protected override void Update()
    {
        actionTime -= Time.deltaTime; // Time 정규화    
        if (_enterThePlayer && actionTime <= 0.0f && _poisonChk)
        {
            DangerousAction();
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
             Debug.Log("CanvasChk");
            _poisonChk = true; // 독가스 노출 여부
            TimeCanvas.SetActive(true); // Poison Zone Timer UI 활성화     
            actionTime = 11.5f; // 함정 발동시간 초기화
            
            StartCoroutine(OutofTheZoneChk()); // Poison Zone에서 벗어났는지 여부
            StartCoroutine(OriginTag());
        }
    }

    private IEnumerator OutofTheZoneChk()
    {
        yield return new WaitForSeconds(9.5f);
        _isOutOfZoneChk = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // Debug.Log("Poison Zone Exit");
            _poisonChk = false; // 독가스 노출 여부
            TimeCanvas.SetActive(false); // Poison Zone Timer UI 비활성화

            _isOutOfZoneChk = true; // Posion Zone에서 벗어났는지 여부
            Debug.Log("GROUND");
           transform.gameObject.tag = "GROUND";

      
            
        }
    }

    private IEnumerator OriginTag()
    {
        yield return new WaitForSeconds(10.3f);
        transform.gameObject.tag = "PoisonZone";
    }

    public override void DangerousAction()
    {
        Debug.Log("ACTION!!!!!!");
       // transform.gameObject.tag = "PoisonZone";
    }
}
