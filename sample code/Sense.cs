using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 센서 구현 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

//! 다른 커스텀 감각을 구현할 때 사용하는 인터페이스
//! Initialize, UpdateSense메소드는 커스텀 클래스에서 내용을 구현하며 각기 Start와 Update에서 실행된다.

//! - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
public class Sense : MonoBehaviour
{

    public bool bDebug = true;
    public Aspect.aspect aspectName = Aspect.aspect.Player;
    public float detectionRate = 1.0f;

    protected float elapsedTime = 0.0f;

    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }

    //초기화에 사용
    private void Start()
    {
        elapsedTime = 0.0f;
        Initialize();
    }
    private void Update()
    {
        UpdateSense();
    }
	
}
