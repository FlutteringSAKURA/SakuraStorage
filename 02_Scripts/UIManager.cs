using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;       //% 유니티관련 API 사용을 위한 네임스페이스

// Update: //@ 2023.11.09 

// NOTE: //# 3D 게임 - UI Manager 스크립트
//#          1) 

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public Button _startBtn;
    public Button _optBtn;
    public Button _endBtn;


    public GameObject _uIPanel;

    public bool _onUIFlag = false;


    public UnityAction _action;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        _onUIFlag = false;
        _uIPanel.SetActive(false);

        //@ 유니티 액션 활용
        _action = () => OnButtonClick(_startBtn.name);
        _startBtn.onClick.AddListener(_action);

        //@ 익명함수 호출방법
        _optBtn.onClick.AddListener(delegate { OnButtonClick(_optBtn.name); });

        //@ 람다식을 이용한 이벤트 연결방식
        _optBtn.onClick.AddListener(() => OnButtonClick(_endBtn.name));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _onUIFlag = !_onUIFlag;
            _uIPanel.SetActive(true);
        }

        else if (!_onUIFlag)
        {
            _uIPanel.SetActive(false);
        }
        else
            _uIPanel.SetActive(true);
    }


    public void OnButtonClick(string btnMessage)
    {
        Debug.Log($"Button Click :  {btnMessage}");
        //Debug.Log($"Button Click :" + btnMessage) ;
    }

}
