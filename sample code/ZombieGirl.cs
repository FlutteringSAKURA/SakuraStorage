using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//# Code for Final Integration Test Sample
//# -> Zombie Girl A.I. 구현 (NavMesh 기능 사용)
public class ZombieGirl : MonoBehaviour
{

    [SerializeField]
    private Transform goal;
    private NavMeshAgent agent;
    [SerializeField]
    private float speedBoostDuration = 3.0f;
    [SerializeField]
    private ParticleSystem boostParticleSystem;
    [SerializeField]
    private float shieldDuration = 3.0f;
    [SerializeField]
    private GameObject shield;

    private float regularSpeed = 0.2f;
    private float boostedSpeed = 3.0f;
    private bool canBoost = true;
    private bool canShield = true;

    private bool hasShield = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(goal.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (canBoost)
            {
                StartCoroutine(Boost());
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (canShield)
            {
                StartCoroutine(Shield());
            }
        }
    }

    //# shield 게임 오브젝트를 활성화 or 비활성화
    //# shieldDuration만큼의 시간이 흐룬 후 이를 다시 비활성화 -> 다시 shield를 사용할 수 있게 함
    private IEnumerator Shield()
    {
        canShield = false;
        shield.SetActive(true);
        float shieldCounter = 0.0f;
        while (shieldCounter < shieldDuration)
        {
            shieldCounter += Time.deltaTime;
            yield return null;
        }
        canShield = true;
        shield.SetActive(false);
    }

    //# 파티클 시스템의 Play를 호출 And NavMeshAgent 속도를 2배로 일정 시간 동안 유지
    private IEnumerator Boost()
    {
        canBoost = false;
        agent.speed = boostedSpeed;
        boostParticleSystem.Play();
        float boostedCounter = 0.0f;
        while (boostedCounter < speedBoostDuration)
        {
            boostedCounter += Time.deltaTime;
            yield return null;
        }
        canBoost = true;
        boostParticleSystem.Pause();
        agent.speed = regularSpeed;
    }
}
