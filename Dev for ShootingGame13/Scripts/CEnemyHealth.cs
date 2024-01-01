using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적기 체력 컴포넌트 클래스
public class CEnemyHealth : CShipHealth
{
    public override void DoDestroy()
    {
        // 현재 적기가 아이템 드랍 기능을 가지고 있으면
        CEnemyItemDrop itemDrop = GetComponent<CEnemyItemDrop>();
        if (itemDrop != null)
        {
            // 아이템을 떨궈라
            itemDrop.ItemDrop();
        }

        base.DoDestroy();
    }
}