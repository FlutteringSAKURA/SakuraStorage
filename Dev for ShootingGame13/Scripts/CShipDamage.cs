using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShipDamage : MonoBehaviour
{
    // 레이져 피격 효과 프리팹
    public GameObject _laserBurstPrefab;

    // 애니메이터 참조
    public Animator _animator;

    // 체력 컴포넌트 참조
    public CShipHealth _health;

    // 충돌 이벤트 함수
    // OnTriggerEnter2D : Trigger 충돌된 오브젝트가 충돌 영역으로 들어올때 자동 호출됨
    // collision 매개변수 : 현재 오브젝트의 충돌 영역으로 들어온 오브젝트의 Collider2D 컴포넌트 참조
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item" || collision.name == "Shield") return;

        //Debug.Log("OnTriggerEnter2D call");

        // Instantiate(프리팹참조, 복제(생성)위치, 생성 회전각)
        // 프리팹 파일을 게임 오브젝트로 복제함(생성함)
        // * Quaterinion.identity : 월드의 회전각을 그대로

        if (_laserBurstPrefab != null)
        {
            GameObject laserBurst = Instantiate(_laserBurstPrefab,
                collision.transform.position, Quaternion.identity);

            // 0.3초 뒤에 피격 이펙트 오브젝트를 파괴해라
            Destroy(laserBurst, 0.3f);
        }

        // 우주 영역과 충돌하면 무스해라
        if (collision.name == "Space" ||
            collision.name == "PlayerShip") return;

        // 체력을 감소시킴 (임시 : 20)
        _health.HpDown(20);

        // 충돌 오브젝트를 파괴함
        Destroy(collision.gameObject);

        // 비행기 피격 애니메이션 재생을 애니메이터에게 요청함
        _animator.Play("ShipHit");
    }
}