using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CnControls;

public class CInputAttack : MonoBehaviour
{
    private Animator _animator;

    // 현재 공격 애니메이션 번호
    private int _attackIndex = 1;

    // 공격 피격 위치
    public Transform _attackPoint;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool IsAttack()
    {
        // bool isPlaying = Animator.GetCurrentAnimaotrStateInfo(레이어번호),
        //                      isName("애니메이션노드이름")
        // - 현재 지정한 레이어의 지정한 애니메이션노드가 재생 중인지를 리턴함

        // 전사의 공격 상태는 Attack1 ~ Attack3 애니메이션 상태를 의미함
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            return true;
        }
        return false;
    }

    private void Attack()
    {
        // X키를 누르면
        // if (Input.GetKeyDown(KeyCode.X))
        if (CnInputManager.GetButtonDown("CustomAttack"))
        {
            if (IsAttack()) return;

            _animator.SetTrigger("Attack" + _attackIndex++);

            if (_attackIndex > 3) _attackIndex = 1;
        }
    }

    private void Update()
    {
        Attack();
    }

    // 공격 애니메이션 타이밍 이벤트 메소드
    public void AttackAnimationEvent()
    {
        // Collider2D[] 검출된대상의콜라이더들 = Physics2D.OverlapCircleAll(
        //       검출위치, 검출범위, 1 << LayerMask.NameToLayer("검출대상레이어이름"));

        // 공격 포인트에서 1반지름 안에 들어오는 몬스터들의 콜라이더를 검출함
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            _attackPoint.position, 1f, 1 << LayerMask.NameToLayer("Monster"));

        Debug.Log("hit monster count : " + colliders.Length);

        if (colliders.Length <= 0) return;

        // colliders[0].GetComponent<CMonsterDamage>().Damage(1f);

        // GameObject.SendMessage("메소드명", 매개변수)
        // - 현재 오브젝트의 컴포넌트들 중 해당 메소드명을 가진 메소드를 호출함
        // colliders[0].SendMessage("Damage", 1f);

        // 충돌 검출 영역안에 있는 몬스터들에게 데미지를 넣어라
        foreach(Collider2D collision in colliders)
        {
            collision.SendMessage("Damage", 1f);
        }

        Debug.Log("hit first monster name : " + colliders[0].name);
    }

}