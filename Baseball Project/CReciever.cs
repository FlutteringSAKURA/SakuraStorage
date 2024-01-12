using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.UI;

public class CReciever : MonoBehaviour
{

    public Transform _baseBall;
    public Transform _bat;

    // public GameObject _baseBallObj;
    // private BaseBall _baseBallScrtipt;

    // private float _dist;    
    // public Text _distTex;
    // public Reciever _recieverScript;    


    private void Start()
    {       
        // _dist = 0f;
        // CalcDist();      
       // _baseBallScrtipt = _baseBallObj.GetComponent<BaseBall>(); 
    }


    private void OnCollisionEnter(Collision coll)
    {
        // if (BaseBall.instance._hitBatChk = true)
        // {
        // };

        //* Send to CBaseBall Script 
       // if(_baseBallScrtipt.GetComponent<BaseBall>()._hitBatChk == true)
       // {           
           // Debug.Log("hit");      
            coll.gameObject.SendMessage("CalcDist", SendMessageOptions.DontRequireReceiver);  
       // }

        //* For Random hit Simulate (Input Key down -> SPACE)
        if(coll.gameObject.name == "Calc_BaseBall(Clone)")
        {
            Debug.Log("Ground Hit");
            coll.gameObject.SendMessage("CalcDist", SendMessageOptions.DontRequireReceiver);
        }

        //* TEST
        // if (dist >= 30)
        // {
        //     Debug.Log("Result : " + dist + " 홈런 ");
        // }
        // else if (dist >= 10)
        // {
        //     Debug.Log("Result : " + dist + " 안타 ");

        // }
        // else
        // {
        //     Debug.Log("result : " + dist + " 아웃 ");
        // }


        // _dist = dist;		
        //_recieverScript.enabled = false;

    }

    // private void ShowCalcDist(float dist)
    // {

    //     _distTex.text = "Dist : " + dist.ToString();
    // }
    // private void SetDistText(){

    // 	_distTex.text = "Dist : " + _distTex.ToString();
    // }


    //* TEST
    // string myLog;
    // Queue myLogQueue = new Queue();
    // void OnEnable()
    // {
    //     Application.logMessageReceived += HandleLog;
    // }

    // void OnDisable()
    // {
    //     Application.logMessageReceived -= HandleLog;
    // }

    // void HandleLog(string logString, string stackTrace, LogType type)
    // {
    //     myLog = logString;
    //     string newString = "\n [" + type + "] : " + myLog;
    //     myLogQueue.Enqueue(newString);
    //     if (type == LogType.Exception)
    //     {
    //         newString = "\n" + stackTrace;
    //         myLogQueue.Enqueue(newString);
    //     }
    //     myLog = string.Empty;
    //     foreach (string mylog in myLogQueue)
    //     {
    //         myLog += mylog;
    //     }
    // }

    // void OnGUI()
    // {
    //     GUILayout.Label(myLog);
    // }
}
