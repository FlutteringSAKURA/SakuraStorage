using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonsterMoveShot : CMonsterMoveAttack
{
    public GameObject _bulletPrefab;

    public override void Attack()
    {
        if(_bulletPrefab != null)
        {
            // 총알 발사
            GameObject bullet = Instantiate(_bulletPrefab, _attackPoint.position,
                Quaternion.identity);
            bullet.SendMessage("Init", _monsterState._isRightDir);
        }
        //Debug.Log("Shot Attack!!!");
    }

}
