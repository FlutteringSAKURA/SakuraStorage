using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

// 배틀 패널 클래스
public class CBattlePanel : MonoBehaviour {

    // 배틀 가능 플레이어 참조 배열
    public GameObject[] _players;

    // 배틀 버튼 참조
    public Button _battleButton;

    // 게임 매니저 참조
    public CGameManager _gameManager;

    private void Start()
    {
        // 게임 매니저를 참조함
        _gameManager = GameObject.Find("GameManager").GetComponent<CGameManager>();
    }

    // 패널이 활성화 됨
    private void OnEnable()
    {
        // 플레이어들을 참조함
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    // 패널이 비활성화 됨
    private void OnDisable()
    {
        _players = null;
    }

    private void Update()
    {
        // 플레이어를 참조하지 못했다면
        if (_players == null) return;

        // 참조된 플레이어들을 순회함
        foreach (GameObject player in _players)
        {
            // 참조된 플레이어가 공격이 불가능한 상태라면 공격 버튼을 비활성화 함
            CHeroAttack heroAttack = player.GetComponent<CHeroAttack>();
            if (!heroAttack._isAttackable)
            {
                _battleButton.interactable = false;
                return;
            }
        }

        // 참조된 두 플레이어가 공격 가능 상태라면 공격버튼을 활성화 함
        _battleButton.interactable = true;
    }

    // 배틀 버튼을 클릭함
    public void OnBattleButtonClick()
    {
        // 배틀을 시작함
        _gameManager.BattleStart();
    }

}
