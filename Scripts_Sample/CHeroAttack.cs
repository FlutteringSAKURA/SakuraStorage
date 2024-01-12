using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 영웅 공격 클래스
public class CHeroAttack : MonoBehaviour
{
    public bool _isAttackable = false;

    // 공격 타겟
    public GameObject _attackTarget;

    // 영웅 애니메이터
    private Animator _animator;

    // 영웅 공격 데미지
    public int _attackDamage;

    // 체력 참조
    public CHeroHealth _health;

    private void Awake()
    {
        // 영웅 애니메이터 참조
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 이미 사망한 상태라면
        if (_health._isDeath)
        {
            // 공격 불능 처리
            _isAttackable = false;
        }
    }

    // 지정한 타겟을 향한 공격을 준비함
    public void AttackReay(GameObject attackTarget)
    {
        // 공격 가능 설정
        _isAttackable = true;

        // 공격 타겟을 설정함
        _attackTarget = attackTarget;
    }

    // 지정한 공격 타겟을 공격함
    public void PlayAttack()
    {
        // 공격 타겟이 존재 한다면
        if (_attackTarget == null || _health._isDeath) return;

        // 공격 애니메이션을 재생함
        _animator.SetBool("Attack", true);
    }
    
    // 공격을 중지함
    public void StopAttack()
    {
        _isAttackable = false;
        // 공격 타겟을 해제함
        _attackTarget = null;
        // 재생중인 공격 애니메이션을 중지함
        _animator.SetBool("Attack", false);
    }

    // 공격 애니메이션 이벤트 (피격 수행)
    public void AttackEvent()
    {
        // 공격 타겟이 존재 한다면
        if (_attackTarget == null || _health._isDeath || !_isAttackable) return;

            // 공격을 받을 타겟의 체력 컴포넌트를 참조함
        CHeroHealth targetHealth = _attackTarget.GetComponent<CHeroHealth>();

        // 공격 타겟 대상에 데미지를 부여함
        targetHealth.Hit(_attackDamage);

        // 공격 타겟이 이미 사망 상태라면
        if (targetHealth._isDeath)
        {
            // 승리 애니메이션을 재생함
            _animator.SetTrigger("Victory");
            _attackTarget = null;
        }

    }
}
