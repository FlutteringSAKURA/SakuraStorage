using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShield : MonoBehaviour
{
    public int _hp = 5; // 방패 체력

    // 스프라이트 렌더러 (왜 참조하지?)
    public SpriteRenderer _spriteRenderer;

    // 쉴드 생성 초기화
    public void Create()
    {
        // 쉴드 설정
        ShieldReset(true);
    }

    // 쉴드 리셋 (활성 여부)
    private void ShieldReset(bool isActive)
    {
        // 게임 오브젝트 활성(true)/비활성화(false)
        // GameObject.SetActive(활성여부)

        // 체력 복원
        _hp = 5;

        // 알파 복원
        // Color(Red, Green, Blue, Alpha) : 1(255)
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

        // 활성 또는 비활성화
        gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적기 또는 적기 총알과 쉴드가 충돌하면
        if (collision.tag == "Enemy" || collision.tag == "ESLaser")
        {
            // 적기 또는 적기 총알을 파괴하라
            Destroy(collision.gameObject);

            // 방어막의 체력을 감소함
            _hp--;

            // 방어막의 알파값을 감소 시킴
            float alpha = _spriteRenderer.color.a - 0.2f;
            _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            // 방어막이 소멸되면
            if (_hp <= 0)
            {
                // 방어막을 비활성 상태로 초기화함
                ShieldReset(false);
            }
        }
    }
}