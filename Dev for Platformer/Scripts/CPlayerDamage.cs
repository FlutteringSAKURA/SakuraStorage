using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerDamage : MonoBehaviour
{
    private CCharacterState _state;
    private Animator _animator;

    private void Awake()
    {
        _state = GetComponent<CCharacterState>();
        _animator = GetComponent<Animator>();
    }

    public void Damage()
    {
        // 이미 데미지를 입은 상태면 패스~
        Debug.Log("Damage!!!");
        if (_state.state == CCharacterState.State.Damage ||
            _animator.GetCurrentAnimatorStateInfo(1).IsName("Damage")) return;

        // 피격 애니메이션을 실행함
        _animator.Play("Damage", 1);
    }

}
