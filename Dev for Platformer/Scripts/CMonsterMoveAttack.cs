using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonsterMoveAttack : CMonsterAttack
{
    // 몬스터 타겟 체크 컴포넌트 참조
    protected CMonsterTargetChecker _targetChecker;

    // 몬스터 이동 컴포넌트 참조
    protected CMonsterMovement _monsterMove;

    protected override void Awake()
    {
        base.Awake();

        _targetChecker = GetComponent<CMonsterTargetChecker>();
        _monsterMove = GetComponent<CMonsterMovement>();
    }

    protected override void Start()
    {
        base.Start();
        // 몬스터 공격 타겟 체크
        _targetChecker.FrontTargetChecker(this);
        
    }

    // 공격 준비(시작)
    public override void AttackReady()
    {
        // 이동 정지
        _monsterMove.AttackStop();
        base.AttackReady();
    }

    // 공격
    public override void Attack()
    {
    }

    // 공격 종료
    public override void AttackFinish()
    {
        base.AttackFinish();
        // 몬스터 이동 재개
        _monsterMove.MoveResume();
        // 몬스터 공격 타겟 체크
        _targetChecker.FrontTargetChecker(this);

       
    }

}
