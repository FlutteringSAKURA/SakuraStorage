using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# NOTE: This is the script for Management Audio and Sound
public class KrakenSoundMgr : DefaultTrackableEventHandler
{
    //@ BASE SOUND
    public AudioClip _foundCardSound;
    public AudioClip _lostCardSound;
    //@ ANIMATION SOUND
    public AudioClip _kuronaAtkSound;
    public AudioClip _zombiegirlAtkSound;

    public AudioSource _audioSource;

    // Update: Animation & Sound sync
    public KrakenAnimationMgr kuronaAnim;
    public KrakenAnimationMgr zombiegirlAnim;

    private void Update()
    {
        if (kuronaAnim._kuronaAttack == true)
        {
            _audioSource.clip = _kuronaAtkSound;
            _audioSource.Play();


            Debug.Log("kurona sound chk");
        }

        else if (zombiegirlAnim._zombiegirlAttack == true)
        {
            Debug.Log("ZombieSound CHK");
            _audioSource.clip = _zombiegirlAtkSound;
            _audioSource.Play();
        }

    }


    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        _audioSource.clip = _foundCardSound;
        _audioSource.Play();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        _audioSource.clip = _lostCardSound;
        _audioSource.Play();
    }



}
