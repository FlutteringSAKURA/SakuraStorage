using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 매니저
public class CGameManager : MonoBehaviour {

    public static int HeroCount = 0;
    public int _heroCount = CGameManager.HeroCount;

    // 게임 시작 패널 참조
    public GameObject _startPanel;
    // 게임 배틀 패널 참조
    public GameObject _battalePanel;
	
	// Update is called once per frame
	void Update () {

        // 영웅 카운트 인스펙터에 표시
        _heroCount = CGameManager.HeroCount;

        // 배틀을 수행할 영웅 카운트가 2명 이상이면
        if (_heroCount >= 2)
        {
            // 배틀 시작 패널 비활성화
            _startPanel.SetActive(false);
            // 배틀 패널 활성화
            _battalePanel.SetActive(true);
        }
        else
        {
            // 배틀 시작 패널 활성화
            _startPanel.SetActive(true);
            // 배틀 패널 비활성화
            _battalePanel.SetActive(false);
        }
    }

    // 영웅 배틀 시작
    public void BattleStart()
    {
        // 플레이어 오브젝트를 참조함
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 배틀을 수행할 영웅이 2명 미만이면 배틀 취소
        if (players == null || players.Length < 2) return;

        // 배틀을 수행할 영웅들이 공격이 가능한 상태라면 배틀을 수행함
        foreach (GameObject player in players)
        {
            CHeroAttack heroAttack = player.GetComponent<CHeroAttack>();
            if (heroAttack._isAttackable)
            {
                heroAttack.PlayAttack();
            }
        }
    }
}
