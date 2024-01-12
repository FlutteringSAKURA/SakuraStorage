using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# NOTE: This is the script for Management Character Animation
public class KrakenAnimationMgr : MonoBehaviour
{

    private Animator _animator;

    // Update: Bool chk
    public bool _kuronaIdle;
    public bool _kuronaAttack;
    public bool _kuronaWin;
    public bool _zombiegirlIdle;
    public bool _zombiegirlAttack;
    public bool _zombiegirlDie;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    //# NOTE: Kurona
    //@ Kurona Combo Atk animation (start)
    public void IdleKurona()
    {
        //_animator.SetBool("Idle", true);
        _kuronaIdle = true;
        _animator.SetBool("Win", false);
		_animator.SetBool("Hit", false);
    }
    public void PlayKuronaComboAtk()
    {
        StartCoroutine(KuronaAtkBoolChk()); // NOTE: for sound sync
        _kuronaIdle = false;
        _animator.SetBool("ComboAttack", true);
        _animator.SetBool("Hit", false);
    }

    private IEnumerator KuronaAtkBoolChk()
    {
        yield return new WaitForSeconds(0.8f);
        _kuronaAttack = true;
    }

    //@ Kurona Combo Atk animation (stop)
    public void StopKuronaComboAtk()
    {
        _kuronaAttack = false;
        _animator.SetBool("ComboAttack", false);
        _animator.SetBool("Hit", false);
    }
    //@ Kurona Hit animation
    public void HitKurona()
    {
        _animator.SetBool("Hit", true);
    }
    //@ Kurona Win animaion (cute)
    public void KuronaWin()
    {
        _kuronaAttack = false;
        _animator.SetBool("ComboAttack", false);
        _animator.SetBool("Hit", false);

        _animator.SetBool("Win", true);
    }
    //# NOTE: Zombie Gril
    //@ ZombieGirl Idle animation
    public void IdleZombieGirl()
    {
        //_animator.SetBool("Idle",true);
        _zombiegirlIdle = true;
        _animator.SetBool("ZombieDie", false);
		_animator.SetBool("ZombieHit", false);
    }

    //@ ZombieGirl Attack animation
    public void PlayZombieGrilAtk()
    {
        _zombiegirlAttack = true;
        _zombiegirlIdle = false;
        _animator.SetBool("ZombieAtk", true);
    }
    public void StopZombieGirlAtk()
    {
        _zombiegirlAttack = false;
        _zombiegirlIdle = true;
        _animator.SetBool("ZombieAtk", false);
		//_animator.SetBool("Hit", false);
    }
	
	//@ ZombieGirl hit animation
	public void ZombieGirlHit()
	{
		_zombiegirlAttack = false;		
		_animator.SetBool("ZombieHit", true);
	}

    //@ ZombieGirl die animation
    public void DieZombieGirl()
    {
        _zombiegirlIdle = false;
        _animator.SetBool("ZombieDie", true);
    }
}
