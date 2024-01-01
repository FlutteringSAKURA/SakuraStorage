using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.10

// NOTE: //# 2D 게임 배경 스크롤링 제어 스크립트
//#          1) 게임 배경 자동 스크롤링 구현
//#          2) 
//#          3) 
//#          4) 
//#          5) 

public class BackGroundScrollder : MonoBehaviour
{
    public float bgScrollSpeed = 0.5f;
    Material bgMaterial;        //& 컴포넌트 속성값

    private void Start() {
        bgMaterial = GetComponent<Renderer>().material;     //^ 랜더러 -> 매터리얼 접근

    }
    private void Update() {
        float newOffsetY = bgMaterial.mainTextureOffset.y + bgScrollSpeed * Time.deltaTime;
        Vector2 newOffset = new Vector2(0, newOffsetY);     //& newOffset 변수에 0, newOffsetY) 값 담기 == 새로운 x, y 좌표 설정
        bgMaterial.mainTextureOffset = newOffset;
    }
    
    
}
