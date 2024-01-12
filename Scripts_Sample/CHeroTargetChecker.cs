using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격 타겟 체커
public class CHeroTargetChecker : MonoBehaviour {

    // 공격 컴포넌트 참조
    private CHeroAttack _attack;
    // 체력 컴포넌트 참조
    private CHeroHealth _health;

    private void Awake()
    {
        // 공격 컴포넌트와 체력 컴포넌트를 참조함
        _attack = GetComponent<CHeroAttack>();
        _health = GetComponent<CHeroHealth>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 영웅이 이미 사망한 상태라면
        if (_health._isDeath)
        {
            // 체크를 무시함
            return;
        }

        // 상대 플레이어가 체크되었다면
        if (collider.tag == "Player")
        {
            // 영웅을 공격 준비 상태로 설정함
            _attack.AttackReay(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // 상태 플레이어가 체크 영역을 나갔다면
        if (collider.tag == "Player")
        {
            // 공격을 중단함
            _attack.StopAttack();
        }
    }
}
