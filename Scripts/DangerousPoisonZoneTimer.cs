using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Poison Zone Timer Script

public class DangerousPoisonZoneTimer : TimerCtrl
{
    public GameObject _posisonZoneObject;
   
    public bool _isOutchk = false;

    protected override void Update()
    {
        if (m_leftTime > 0f) // 남은시간이 0보다 크다면
        {
            m_leftTime -= Time.deltaTime; // -Time 정규화
            Minutes = GetLeftMinutes();
            Seconds = GetLeftSeconds();

            if (m_leftTime > 0f)
            {
                m_text.text = "함정 발동 까지 남은 시간 : " + Minutes + ":" + Seconds.ToString("00");
            }

            else
            {
                m_text.text = "살인 가스 살포!!!! ";
                StartCoroutine(ShowText()); // 함정 발동에 의한 효과 TEXT UI 작동
            }
        }
        // 함정 발동시간이 0초보다 작아 함정이 발동된 적이 있고 다시 입장하였을 때
        else if (m_leftTime < 0.0f && _posisonZoneObject.GetComponent<DangerousPoisonZone>()._isOutOfZoneChk)
        {            
            m_leftTime = 10.0f; // 함정 발동 시간 초기화
        }
    }
    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(1.2f);
        m_text.text = "당신은 죽어가고 있습니다.";
    }
}
