using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonsterStandShot : CMonsterAttack
{
    public GameObject _bulletPrefab;        // 총알 프리팹
    // 타겟 체크(센서링) 컴포넌트 참조
    protected CMonsterTargetChecker _targetChecker;

    public enum CheckType { FRONT, CIRCLE };

    public CheckType _checkType = CheckType.FRONT;

    protected override void Awake()
    {
        base.Awake();

        // 타겟 체크 컴포넌트 참조
        _targetChecker = GetComponent<CMonsterTargetChecker>();
    }

    protected override void Start()
    {
        base.Start();

        // 센서링을 요청함
        StartTargetChecker();
    }

    private void StartTargetChecker()
    {
        // 타겟 체킹 방식에 따라 요청함
        switch (_checkType)
        {
            case CheckType.FRONT:
                _targetChecker.FrontTargetChecker(this);
                break;
            case CheckType.CIRCLE:
                _targetChecker.CircleAreaTargetChecker(this);
                break;
        }
    }

    // 공격함
    public override void Attack()
    {
        // 총알을 발포함
        GameObject bullet = Instantiate(_bulletPrefab, _attackPoint.position,
            _attackPoint.rotation) as GameObject;

        // 총알을 초기화함(방향 지정)
        bullet.SendMessage("Init", _monsterState._isRightDir);
    }

    // 공격을 마침
    public override void AttackFinish()
    {
        base.AttackFinish();
        // 공격이 끝나고 다시 센서링을 시작함
        StartTargetChecker();
    }
}
