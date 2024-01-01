using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.27 

public class Gold : MonoBehaviour
{
    public float _rotSpeed = 360;

    [System.NonSerialized]
    public int _goldValue = 100;

    public AudioClip _getGoldSound = null;
    

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(0f, _rotSpeed * Time.deltaTime, 0);
    }

    //@ 골드의 가치 함수
    public void SetGoldValue(int gold)
    {
        this._goldValue = gold;
    }

    private void OnTriggerEnter(Collider other)
    {
        //& 플레이어와 접촉시 플레이어 파람스에 그 값 전달
        if (other.gameObject.tag.Contains("Player"))
        {
            SoundManager.instance.GetGoldSoundPlay();
            ////_sakuraObj.GetComponent<SakuraParams>().AddGoldReward(_goldValue);
            other.gameObject.GetComponent<SakuraParams>().AddGoldReward(_goldValue);

            // Legacy:
            //Destroy(gameObject, 0.15f);
            // Update: //@ 2023.10.27 
            //# 파괴시키지 않고 재활용하기 위한 코드로 업데이트
            gameObject.SetActive(false);        //& 게임에서 비활성화
            
            ////Debug.Log("Gold get");
        }
    }
}
