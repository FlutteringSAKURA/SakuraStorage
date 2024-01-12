using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# Flock System Basic Test Code 
//# For Flock - Boid
public class ZombieCrowFlock : MonoBehaviour
{

    public float minSpeed = 5.0f;
    public float turnSpeed = 10.0f;
    //randomForce값에 기반하여 randomPush값을 갱신하는 빈도 결정에 사용
    public float randomFreq = 20.0f;
    public float randomForce = 20.0f;

    //* 정렬 관련 변수
    //군집의 중심으로부터 일정 범위내에 유지
    public float toOriginForce = 50.0f;
    //군집의 흩어짐 정도를 결정
    public float toOriginRange = 100.0f;

    public float gravity = 2.0f;

    //* 분산 관련 변수
    //개체간의 최소 거리 유지
    public float avoidanceRadius = 50.0f;
    public float avoidanceForce = 20.0f;

    //* 응집 관련 변수
    //군집의 리더 또는 군집의 중심 위치와의 최소 거리 유지
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;

    //* 개별 개체의 이동과 관련된 함수
    //origin은 군집 오브젝트 전체 그룹을 제어하는 부모 오브젝트
    //boid는 무리 안에 있는 다른 boid에 대해 알고 있어야 함 -> objects와 otherFlocks 속성을 사용 -> 이웃 boid들의 정보 저장
    private Transform origin;
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Transform[] objects;
    private ZombieCrowFlock[] otherFlocks;
    private Transform transformComponent;

    private void Start()
    {
        randomFreq = 1.0f / randomFreq;

        //* parent를 origin에 할당(origin이 컨트롤러 오브젝트의 역할을 한다는 의미)
        origin = transform.parent;
        //군집 Transform
        transformComponent = transform;
        //임시 Component
        Component[] tempFlocks = null;

        //그룹 내의 부모 트랜스폼으로부터 모든 좀비 군집 컴포넌트를 얻는다
        if (transform.parent)
        {
            tempFlocks = transform.parent.GetComponentsInChildren<ZombieCrowFlock>();
        }

        //그룹 내의 모든 군집 오브젝트를 할당하고 저장한다.
        objects = new Transform[tempFlocks.Length];
        otherFlocks = new ZombieCrowFlock[tempFlocks.Length];

        for (int i = 0; i < tempFlocks.Length; i++)
        {
            objects[i] = tempFlocks[i].transform;
            otherFlocks[i] = (ZombieCrowFlock)tempFlocks[i];
        }

        //parent에 null을 지정하면 ZombieFlockController오브젝트가 리더가 된다
        transform.parent = null;
        //주어진 랜덤 주기에 따라 랜덤 푸시를 계산
        StartCoroutine(UpdateRandom());

    }

    //* randomFreq변수의 시간 간격에 기반 -> randomPush값을 갱신
    //* Random.insideUnitSphere는 randomForce를 반지름으로 하는 구 내에서 임의의 x, y, z값으로 Vector3 오브젝트를 반환
    //* 임의 시간을 기다린 후 다시 randomPush값을 갱신하도록 잠시 대기
    private IEnumerator UpdateRandom()
    {
        while (true)
        {
            randomPush = Random.insideUnitSphere * randomForce;
            yield return new WaitForSeconds(randomFreq +
            Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
        }
    }

    private void Update()
    {
        //내부 변수
        float speed = velocity.magnitude;
        Vector3 avgVelocity = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        float count = 0;
        float f = 0.0f;
        float d = 0.0f;
        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        Vector3 toAvg;
        Vector3 wantedVel;

        //# 분리 규칙
        //# 현재 boid와 다른 boid 사이의 거리를 검사하고 속도를 그에 맞춰 갱신
        //# -> 현재 속도를 무리에 포함된 boid의 수로 나누어 군집의 평균 속도 계산

		//# 최종적으로 구하고자 하는 속도 wantedVel
		//# -> 이를 위한 randomPush, originPush, avgVelocity 요소 추가
		//# 현재 velocity를 Vector3.RotateTowards 메소드를 사용한 리니어 보간법으로 계산하여 wantedVel로 갱신
		//# -> Translate() 메소드를 사용하여 새로운 속도로 boid를 이동

        for (int i = 0; i < objects.Length; i++)
        {
            Transform transform = objects[i];
            if (transform != transformComponent)
            {
                Vector3 otherPosition = transform.position;

                //응집을 계산하기 위한 평균 위치
                avgPosition += otherPosition;
                count++;

                //다른 군집에서 이 군집까지의 방향 벡터
                forceV = myPosition - otherPosition;

                //방향 벡터의 크기(길이)
                d = forceV.magnitude;

                //만일 벡터의 길이가 followRadius보다 작다면 값을 늘림
                if (d < followRadius)
                {
                    f = 1.0f - (d / avoidanceRadius);
                    if (d > 0)
                    {
                        avgVelocity += (forceV / d) * f * avoidanceForce;
                    }
                    //리더와의 현재 거리 유지
                    f = d / followRadius;
                    ZombieCrowFlock otherZombieGirl = otherFlocks[i];
                    //otherZombieGirl 속도 벡터를 정규화해 이동 방향을 얻은 후, 새로운 속도를 설정
                    avgVelocity += otherZombieGirl.normalizedVelocity * f * followVelocity;
                }
            }
        }
        if (count > 0)
        {
            //군집의 평균 속도를 계산(정렬)
            avgVelocity /= count;

            //군집의 중간 값을 계산(응집)
            toAvg = (avgPosition / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }

        //리더를 향한 방향 벡터
        forceV = origin.position - myPosition;
        d = forceV.magnitude;
        f = d / toOriginRange;

        //리더에 대한 군집의 속도를 계산
        //만약 boid가 무리의 중심에 있지 않다면
        if (d > 0)
        {
            originPush = (forceV / d) * f * toOriginForce;
        }

        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }
        wantedVel = velocity;

        //최종 속도 계산
        wantedVel -= wantedVel * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avgVelocity * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;

        //무리를 회전시키기 위한 최종 속도 계산
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.00f);
        transformComponent.rotation = Quaternion.LookRotation(velocity);

        //계산한 속도에 기반해 군집 이동
        transformComponent.Translate(velocity * Time.deltaTime, Space.World);

        //속도 정상화
        normalizedVelocity = velocity.normalized;
    }
}
