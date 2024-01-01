using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerShipUpgrade : MonoBehaviour
{
    public CShield _shield; // 방어막 참조

    public Transform[] _miniShipPos; // 미니 비행기 생성 위치

    // 방어막 업그레이드
    public void UpgradeShieldSystem()
    {
        _shield.Create();
    }

    // 미니비행기 생성 업그레이드
    public void UpgradeMiniShipSystem(GameObject miniShipPrefab)
    {
        // 비행기 생성 위치를 체크함
        // for (int i=0; i<_miniShipPos; i++) {
        //      if (_miniShipPos[i].childCount >0) continue;
        //      ....
        //  }

        foreach (Transform genPos in _miniShipPos)
        {
            // 현재 위치에 이미 비행기가 있다면 다음 위치로 넘김

            // Transform.childCount : 현재 게임오브젝트의 자식 갯수
            if (genPos.childCount > 0) continue;

            // 미니 비행기를 생성함
            GameObject miniShip = Instantiate(miniShipPrefab,
                genPos.position, genPos.rotation);

            // 생성한 미니 비행기를 현재 위치의 자식으로 등록함

            // Transform.SetParent(부모의Transform참조)
            // - 현재 오브젝트를 지정한 부모 오브젝트의 자식으로 등록함
            miniShip.transform.SetParent(genPos);

            break;
        }
    }
}