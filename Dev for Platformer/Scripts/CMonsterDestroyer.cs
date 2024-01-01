using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonsterDestroyer : CDestroyer
{
    // 파괴 이펙트
    public GameObject _destroyEffectPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public override void Destroy()
    {
        if (_destroyEffectPrefab != null)
        {
            // 오브젝트 파괴 이펙트 생성
            Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity);
        }

        base.Destroy();

    }



}
