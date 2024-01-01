using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyItemDrop : MonoBehaviour
{
    // 아이템 프리팹
    public GameObject[] _itemPrefab;

    public void ItemDrop()
    {
        // 랜덤하게 아이템 번호를 뽑음
        int itemIndex = Random.Range(0, _itemPrefab.Length);

        Instantiate(_itemPrefab[itemIndex],
            transform.position, Quaternion.identity);
    }
}