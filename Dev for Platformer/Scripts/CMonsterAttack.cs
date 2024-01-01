using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMonsterAttack : MonoBehaviour
{
    protected Transform _attackPoint;        // 공격 위치
    protected Animator _animator;           // 애니메이터
    protected CCharacterState _monsterState; // 몬스터 상태 컴포넌트 참조

    protected virtual void Awake()
    {
        // 애니메이터 컴포넌트 참조
        _animator = GetComponent<Animator>();

        // 몬스터 상태 컴포넌트 참조
        _monsterState = GetComponent<CCharacterState>();

        // 공격 위치 참조
        _attackPoint = transform.Find("AttackPoint").transform;
    }

   protected virtual void Start()
    {
    }
    
    // 공격 준비
    public virtual void AttackReady()
    {
        // 공격 상태 변경
        _monsterState.state = CCharacterState.State.Attack;

        // 공격 애니메이션
        _animator.SetBool("Attack", true);
    }

    // 공격 
    public abstract void Attack();

    // 공격 끝
    public virtual void AttackFinish()
    {
        // 대기 상태 변경
        _monsterState.state = CCharacterState.State.Idle;
        // 대기 애니메이션
        _animator.SetBool("Attack", false);
    }

    public void AttackAnimationEvent()
    {
        Attack();
    }

    public void AttackFinishAnimationEvent()
    {
        AttackFinish();
    }
}
