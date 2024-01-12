using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    internal FlockController controller;

    //! Additional Test Code
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    //! - - - - - - - - - - - - - - - - - - - - 

    //# FlockController의 생성은 바로 이루어짐
    //# steer() 메소드를 사용 -> boid의 속도를 계산 -> rigidbody 속도에 적용
    //# rigidbody 컴포넌트의 현재 속도를 검사해 컨트롤러의 최고와 최저 속도내에 들어오는지 확인
    //# 만일 범위를 벗어났다면 범위내로 들어오게 함
    private void Update()
    {
        if (controller)
        {
            Vector3 relativePos = steer() * Time.deltaTime;

            if (relativePos != Vector3.zero)
            {
                rigidbody.velocity = relativePos;
            }
            //boid의 최소와 최대 속도를 강제
            float speed = rigidbody.velocity.magnitude;
            if (speed > controller.maxVelocity)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * controller.minVelocity;
            }
        }
    }

    //# 분리, 응집, 정렬을 포함해 군집 알고리즘에서 리더를 따라가는 규칙을 구현
    //# 각 요소에 임의의 가중치 부여하여 처리
    private Vector3 steer()
    {
        Vector3 center = controller.flockCenter - transform.localPosition;  //응집
        Vector3 velocity = controller.flockVelocity - rigidbody.velocity;   //정렬
        Vector3 follow = controller.target.localPosition;   //리더 추종

        Vector3 separation = Vector3.zero;

        foreach (Flock flock in controller.flockList)
        {
            if (flock != this)
            {
                Vector3 relativePos = transform.localPosition - flock.transform.localPosition;

                separation += relativePos / (relativePos.sqrMagnitude);
            }
        }
        //무작위화
        Vector3 randomize = new Vector3((Random.value * 2) - 1,
        (Random.value * 2) - 1, (Random.value * 2) - 1);

        randomize.Normalize();

        return (controller.centerWeight * center + 
		controller.velocityWeight * velocity +
        controller.separationWeight * separation +
        controller.followWeight * follow +
        controller.randomizeWeight * randomize);
    }
}
