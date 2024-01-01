using UnityEngine;
using System.Collections;

// 파티클 이펙트 파괴자
public class CParticleEffectDestroyer : CDestroyer
{

    protected ParticleSystem _particleSystem;   // 파티클 시스템

    protected virtual void Awake()
    {
        // Component.GetComponentInChildren<클래스타입>()
        // - 현재 컴포넌트가 추가된 게임오브젝트의 자식 오브젝트들 중에
        // 지정한 클래스타입을 가진 컴포넌트가 있다면 참조함

        // 파티클 시스템 컴포넌트 참조
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Use this for initialization
    protected override void Start()
    {

        // 파티클 지연 시간
        _destroyDelayTime = _particleSystem.main.duration;

        base.Start();
    }

}
