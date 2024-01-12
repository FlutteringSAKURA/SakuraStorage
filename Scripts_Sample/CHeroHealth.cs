using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

// 영웅 체력 클래스
public class CHeroHealth : MonoBehaviour {

    // 영웅 사망 생태
    public bool _isDeath = false;
    // 영웅 체력 수치
    public int _hp;
    // 영웅 체력바
    public Image _hpBar;
    // 영웅 애니메이터
    private Animator _animator;

    private void Awake()
    {
        // 애니메이터 참조
        _animator = GetComponent<Animator>();
    }

    // 영웅이 지정한 데미지 만큼 타격을 입음
    public void Hit(int damage)
    {
        // 체력이 감소됨
        _hp = HpDown(damage);

        // 체력이 0이하가 되면
        if (_hp <= 0)
        {
            // 사망 애니메이션을 재생함
            _animator.SetTrigger("Death");
            // 사망 상태를 설정함
            _isDeath = true;
        }
    }

    // 영웅의 체력 수치를 감소함
    private int HpDown(int damage)
    {
        // 체력 게이지를 감소함
        float d = damage * 0.01f;
        _hpBar.fillAmount -= d;

        // 체력 게이지값을 체력 수치로 반환함
        return (int)(_hpBar.fillAmount * 100f);
    }
}
