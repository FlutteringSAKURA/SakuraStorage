using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TimerCtrl : MonoBehaviour
{
    [SerializeField]
    protected int Minutes = 0;
    [SerializeField]
    protected int Seconds = 0;

    protected Text m_text;
    [SerializeField]
    protected float m_leftTime;

    protected virtual void Awake()
    {
        m_text = GetComponent<Text>();
        m_leftTime = GetInitialTime();
    }

    protected virtual void Update()
    {
        if (m_leftTime < 0f) // 남은시간이 0보다 크다면
        {
            m_leftTime -= Time.deltaTime; // -Time 정규화
            Minutes = GetLeftMinutes();
            Seconds = GetLeftSeconds();

            if (m_leftTime > 0f)
            {
                m_text.text = "함정 발동까지 남은 시간 : " + Minutes + ":" + Seconds.ToString("00");
            }
            else
            {
                m_text.text = "Time : 0:00";
            }
        }
    }
   

 

    public virtual float GetInitialTime()
    {
        return Minutes * 60f + Seconds;
    }

    public virtual int GetLeftMinutes()
    {
        return Mathf.FloorToInt(m_leftTime / 60f);
    }

    public virtual int GetLeftSeconds()
    {
        return Mathf.FloorToInt(m_leftTime % 60f);
    }
}
