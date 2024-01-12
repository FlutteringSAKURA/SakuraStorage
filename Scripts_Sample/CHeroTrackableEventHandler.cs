using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여웅 트랙킹 이벤트 핸들러 클래스 : 기본 트래킹 이벤트 핸들러
public class CHeroTrackableEventHandler : DefaultTrackableEventHandler {

    // 영웅 사눙드 재생
    public AudioSource _trackableAudioSource;

    // 오브젝트 찾았을때 사운드
    public AudioClip _foundSound;
    // 오브젝트 잃었을때 사운드
    public AudioClip _lostSound;

    // 오브젝트를 찾았을때 이벤트
    protected override void OnTrackingFound()
    {
        // 기본 이벤트 처리
        base.OnTrackingFound();

        // 영웅 카운트 증가
        CGameManager.HeroCount++;

        // 오브젝트를 찾았을때 사운드를 재생함
        _trackableAudioSource.clip = _foundSound;
        _trackableAudioSource.Play();
    }

    // 오브젝트를 잃었을때 이벤트
    protected override void OnTrackingLost()
    {
        // 기본 이벤트 처리
        base.OnTrackingLost();

        // 영웅 카운트를 감소함
        CGameManager.HeroCount--;

        // 오브젝트를 잃었을때 사운드를 재생함
        _trackableAudioSource.clip = _lostSound;
        _trackableAudioSource.Play();
    }
}
