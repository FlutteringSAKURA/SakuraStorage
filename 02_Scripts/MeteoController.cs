using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.22
// Update: //@ 2023.09.26

// NOTE:  //# 메테오 구현(코루틴 사용) == 시간차 구현
//#          1) 
//#          2) 
//#          3) 
//#          4) 

public class MeteoController : MonoBehaviour
{
    public GameObject gameMaster;
    //public GameObject camController;
    int delayTime = 2;
    void Start()
    {
        StartCoroutine(SelfDie());
        gameMaster = GameObject.Find("GameManager");
        
    }
    IEnumerator SelfDie()
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        gameMaster.SendMessage("PlayerDamage", 40);
        gameMaster.SendMessage("Hitted");
        
        SoundManager.instance.MeteoCrush();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
