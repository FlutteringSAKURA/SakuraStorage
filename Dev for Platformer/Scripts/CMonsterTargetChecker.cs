using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonsterTargetChecker : MonoBehaviour
{
    protected CCharacterState _monsterState;    // 몬스터 상태 컴포넌트 참조
    public CCharacterState.State _checkState;   // 체크할 상태 설정
    public float _checkRange;                   // 체크 범위
    protected Transform _attackPoint;           // 체크 시작 위치(공격 위치)

    protected Transform _playerPos;             // 플레이어 위치

    private void Awake()
    {
        _monsterState = GetComponent<CCharacterState>();
        _attackPoint = transform.Find("AttackPoint").transform;
    }

    // 몬스터 센서링 가동
    public void FrontTargetChecker(CMonsterAttack monsterAttack)
    {
        StartCoroutine("FrontTargetCheckCoroutine", monsterAttack);
    }

    // 몬스터 센서링 수행 코루틴
    private IEnumerator FrontTargetCheckCoroutine(CMonsterAttack monsterAttack)
    {
        // 몬스터가 대기 상태라면 감시를 진행함
        while (_monsterState.state == _checkState)
        {
            // 수평 방향으로 발포 감지 레이저를 발사함
            Vector2 endPos = new Vector2
                (_attackPoint.position.x - 
                ((_monsterState._isRightDir) ? -_checkRange : _checkRange),
                _attackPoint.position.y);

            // 충돌 체크 디버그 레이저 발사
            Debug.DrawLine(_attackPoint.position, endPos, Color.green);

            //----

            // 라인 체크
            // Physics2D.Linecast(라인시작위치, 라인끝위치, 검출레이어)
            // RaycastHit2D hitInfo = Physics2D.Linecast(
            //    _attackPoint.position, endPos, _monsterState._targetMask);

            RaycastHit2D hitInfo = Physics2D.Linecast(_attackPoint.position,
                endPos, 1 << LayerMask.NameToLayer("Player"));

            // 충돌체가 존재한다면
            if (hitInfo.collider != null)
            {
                // 공격 시작
                monsterAttack.AttackReady();
            }
            yield return null;  // Update 주기와 동일
        }
    }
    public void CircleAreaTargetChecker(CMonsterAttack monsterAttack)
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine("CircleAreaTargetCheckCoroutine", monsterAttack);
    }

    public IEnumerator CircleAreaTargetCheckCoroutine(CMonsterAttack monsterAttack)
    {
        // 몬스터가 대기 상태라면 감시를 진행함
        while (_monsterState.state == _checkState)
        {
            float distance = Vector2.Distance(_attackPoint.position,
                _playerPos.position);

            //Debug.Log("player pos => " + _playerPos.position);

            if (distance < _checkRange)
            {
                // 공격 시작
                monsterAttack.AttackReady();
            }
            yield return null;

        }
    }
}