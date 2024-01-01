using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Update: //@ 2023.10.13

// NOTE: //# 3D 게임 크리처 제어 스크립트
//#          1) 네브매쉬 이동 구현 (플레이어 추적)
//#          2) 
//#          3) 
//#          4) 
//#          5) 

//~ ---------------------------------------------------------
public class CreatureCtrl : MonoBehaviour
{
    public bool _isCreatureAlive = true;
    Animator _creatureAnimator;
    Transform _playerPos;
    public NavMeshAgent _creatureNavMeshAgent;
    public float _timeFlow = 0.0f;
    public float _tracingCoolTime = 2.8f;

    AudioSource _screamSound;

    GameObject _soundManager;

//~ ---------------------------------------------------------
    private void Start()
    {
        _creatureAnimator = GetComponent<Animator>();
        _playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //_soundManager = GameObject.Find("SoundManager");
        //_soundManager.GetComponent<SoundManagerCtrl>().ScreamCreatureSoundPlay();

        //SoundManagerCtrl.instance.ScreamCreatureSoundPlay();
        //Invoke("TracingPlayer", 2.1f);
        _screamSound = GetComponent<AudioSource>();
        StartCoroutine(CreatureScreamActive());

    }

    IEnumerator CreatureScreamActive()
    {
        yield return new WaitForSeconds(1.85f);
        _screamSound.Play();
    }

    // private void TracingPlayer()
    // {

    // }

//~ ---------------------------------------------------------
    private void Update()
    {
        _timeFlow += Time.deltaTime;
        _creatureAnimator.SetFloat("Speed", _creatureNavMeshAgent.velocity.magnitude);


        if (_timeFlow > _tracingCoolTime)
        {

            if (_isCreatureAlive && GameManagerCtrl.instance._isStageClear == false)
            {
                _creatureNavMeshAgent.SetDestination(_playerPos.position);
            }
        }

        else if(GameManagerCtrl.instance._isStageClear)
        {
            gameObject.SetActive(false);
        }
    }
}
