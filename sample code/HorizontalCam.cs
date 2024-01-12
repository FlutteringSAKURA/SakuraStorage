using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//# Code for Final Integration Test Sample
//# -> Camera Script
public class HorizontalCam : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    private Vector3 targetPosition;

	//* 카메라의 목표지점을 모든 축에 대해 현재 위치와 모두 동일하게 설정
	//* 이후 목표지점의 z축을 Zombie Girl과 동일하게 재지정
	//# -> 선형 보간법을 사용해서 부드럽게 카메라를 현재 위치에서 목표 위치로 매 프레임 이동
    private void Update()
    {
        targetPosition = transform.position;
        targetPosition.z = target.transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
}
