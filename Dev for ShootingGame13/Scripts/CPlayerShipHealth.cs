using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티 UI 사용
using UnityEngine.UI;

// 플레이어 체력 클래스
public class CPlayerShipHealth : CShipHealth
{
    // 체력 게이지 컴포넌트 참조
    public Image _hpBarProgress;

    public override void HpDown(int damage)
    {
        // 데미지를 1/100로 환산함
        float d = ((float)damage) * 0.01f;

        // 체력 게이지에서 데미지를 감소시킴
        _hpBarProgress.fillAmount -= d;

        // base : 부모 객체 참조 키워드
        // CShipHealth의 HpDown을 호출함
        base.HpDown(damage);
    }

    // 체력을 증가 시킴
    public void HpUp(int upValue)
    {
        _hp += upValue;

        _hp = (_hp > 100) ? 100 : _hp;

        _hpBarProgress.fillAmount = _hp * 0.01f;
    }
}