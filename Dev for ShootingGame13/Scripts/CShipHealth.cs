using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 비행기 체력 컴포넌트 클래스
public class CShipHealth : MonoBehaviour
{
    // 비행기 체력
    public int _hp;

    // 체력 감소 처리 (피격된 데미지 만큼)
    public virtual void HpDown(int damage)
    {
        // 피격된 데미지만큼을 체력에서 감소 시킴
        _hp -= damage;

        // 체력이 0이 되면
        if (_hp <= 0)
        {
            DoDestroy();
        }
    }

    public virtual void DoDestroy()
    {
        // 오브젝트를 파괴함
        Destroy(gameObject);
    }
}