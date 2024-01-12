using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# Runtime에 boid를 생성 -> 군집의 평균 속도와 중심의 위치 갱신
public class FlockController : MonoBehaviour
{

    public float minVelocity = 1;   //최저속도
    public float maxVelocity = 8;   //최고 군집 속력
    public int flockSize = 20;      //그룹 내에 있는 군집의 수
	
	//boid가 중앙에서 어느 정도 까지 떨어 질 수 있는지
    //지정(weight가 클수록 중앙에 근접)
    public float centerWeight = 1;
    public float velocityWeight = 1;    //정렬 동작

    //군집 내에서 개별 boid간의 거리
    public float separationWeight = 1;

    //개별 boid와 리더 간의 거리(weight가 클수록 가깝게 따라감)
    public float followWeight = 1;

    //추가적인 임의성 제공
    public float randomizeWeight = 1;

    public Flock prefab;
    public Transform target;

    //그룹 내 군집의 중앙 위치
    internal Vector3 flockCenter;
    internal Vector3 flockVelocity; //평균 속도

    public ArrayList flockList = new ArrayList();

    //# 주어진 군집 크기에 기반 -> boid 오브젝트를 생성
    //# 컨트롤러 클래스와 부모 프랜스폼 오브젝트를 설정
    //# 생성한 boid 오브젝트를 flockList 배열에 추가
    private void Start()
    {		

        for (int i = 0; i < flockSize; i++)
        {
            Flock flock = Instantiate(prefab, transform.position, transform.rotation) as Flock;
            flock.transform.parent = transform;
            flock.controller = this;
            flockList.Add(flock);
        }
    }

    //# 군집의 평균 중앙 위치와 속도를 계속 갱신
    //# boid 오브젝트는 이 값들을 참조해 응집과 정렬 관련 속성을 조절하는데 사용
    private void Update()
    {
        //전체 군집 그룹의 중앙 위치와 속도를 계산
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        foreach (Flock flock in flockList)
        {
            center += flock.transform.localPosition;
            velocity += flock.GetComponent<Rigidbody>().velocity;
        }
        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
