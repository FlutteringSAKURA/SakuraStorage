using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonsterMoveSwingAttack : CMonsterMoveAttack
{
    public override void Attack()
    {
        // 공격!!
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackPoint.position,
            2f, _monsterState._targetMask);

        foreach (Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                collider.SendMessage("Damage",
                    SendMessageOptions.DontRequireReceiver);
                return;
               
            }
        }
    }

}
