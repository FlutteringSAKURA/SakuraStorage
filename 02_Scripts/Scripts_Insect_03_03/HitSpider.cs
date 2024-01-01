using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.12
// Update: //@ 2023.09.13
// Update: //@ 2023.09.14

//# NOTE:  
//#         1) 거미 입장에서 내 영역에 들어온 것이 곤충이면 거미 없애기
//#         2) 소리효과 구현하기
//#         3) 곤충과 거미가 접촉시 거미 직접 파괴 아니라 일단 보이지 않게 하고 사운드 플레이 후 파괴
//#         4) 곤충과 거미가 접촉시 거미의 콜라이더 없애기
//#         5) 거미를 잡아먹을 때마다 점수를 증가시키기
public class HitSpider : MonoBehaviour
{
    //GameObject stageGameManager;      //^ 아래와 동일 코드
    StageGameManager stageGameManager;  //& 스크립트 사용을 위한 변수 선언
                                        //& Public으로 선언하는 경우는 유니티에서 수동으로 해당 스크립트를 끼워넣어주면 된다.

    // TEST://$ 거미입장에서 곤충의 위치좌표에 접근
    Transform insectTrans;  //^ 좌표 타입
    GameObject findInsect;  //^ 게임오브젝트 타입
    //$ ---------------------------------------

    private void Start()
    {
        //stageGameManager = GameObject.Find("StageGameManager");
        stageGameManager = GameObject.Find("StageGameManager").GetComponent<StageGameManager>();

        insectTrans = GameObject.Find("BossInsect_03_02").transform;    //$ 곤충의 좌표

        //^ 동일코드
        //findInsect.GetComponent<MonInsect>().InsectTrans();
        //GameObject.Find("BossInsect_03_02").GetComponent<MonInsect>().InsectTrans();  //^ 게임오브젝트 좌표

        Debug.Log("곤충의 좌표값을 이용한 곤충의 현재 위치는? : " + insectTrans);
        //Debug.Log("게임오브젝트를 이용한 곤충의 좌표는? : " + findInsect);
        //Debug.Log("게임오브젝트를 이용한 곤충의 좌표는? : " + insectTrans);
        //! ---------------------------------------------------------------
    }

    private void OnTriggerEnter(Collider someObj)
    {

        // Debug.Log("부딪힌 놈이 누구냐? : " + someObj.gameObject.name);
        // TEST: //% 거미 입장에서 내 영역에 들어온 것이 곤충이면 거미 없애기
        /*
        if (someObj.gameObject.tag == "Monster")             //^ 아래와 동일한 기능을 구현하는 코드
        {
            Destroy(this.gameObject, 0.5f);
        }
        */

        if (someObj.gameObject.tag.Contains("Monster"))     //# 자원을 더 아낄 수 있는 코드 == tag에 Monster가 포함되어 있느냐 라는 의미
        {
            // TEST: //! HitSpider스크립트에서 곤충의 게임오브젝트에 접근해 좌표를 알아내기 위해 MonInsect스크립트의 함수 호출
            findInsect = GameObject.Find("BossInsect_03_02");   //$ 곤충의 게임오브젝트 접근 좌표를 위해
            findInsect.GetComponent<MonInsect>().InsectTrans();
            //! ------------------------------------------------------------------------

            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            stageGameManager.AddSpiderCount();  //$ StageGameManager의 함수 호출 

            // Update: //% 거미를 먹을 때, 스코어 획득------------------------------------------
            int randGetScore = Random.Range(100, 201);
            stageGameManager.GetScore(randGetScore);
            //% ------------------------------------------------------------------------

            // TEST: //! 다음 코드도 동일한 기능으로 가능 (그러나 아래 방식은 별로 좋지 않다 == 변수로 선언해서 쓰는게 나중에 찾기 좋다)
            // GameObject.Find("StageGameManager").GetComponent<StageGameManager>().AddSpiderCount();

            AudioSource audio = GetComponent<AudioSource>();    //& 컴포넌트에서 오디오 소스 가져오기
            audio.Play();   //& 오디오 재생
            float audioLength = audio.clip.length;      //& 오디오 사운드 클립의 길이 값을 변수에 넣기
            //Destroy(this.gameObject, 0.5f);

            Destroy(this.gameObject, audioLength);      //& 사운드 플레이 이후 오브젝트 파괴
        }
    }
}
