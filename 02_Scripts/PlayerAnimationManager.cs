using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.19 

// NOTE: //# 3D 게임 - 플레이어 애니메이션 관리 
//#          1) 플레이어의 상태에 따른 애니메이션
//#          2) 
//#          3) 
//#          4) 

//~ ------------------------------------------------------------------------
public class PlayerAnimationManager : MonoBehaviour
{
    //% 상수 선언 .. 숫자를 기억할 필요없다.
    public const int ANIM_IDLE = 0;
    public const int ANIM_WALK = 1;
    public const int ANIM_RUN = 2;
    public const int ANIM_ATTACK = 3;
    public const int ANIM_ATTACKWAIT = 4;
    public const int ANIM_DEAD = 5;


    //// public static PlayerAnimationManager instance;
    Animator _sakuraAnimator;

    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        // // if (instance == null)
        // // {
        // //     instance = this;
        // // }
    }

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _sakuraAnimator = GetComponent<Animator>();
    }
    //~ ------------------------------------------------------------------------
    //@ 애니메이션 교체를 위한 정수 값 설정 함수 
    public void ChangeAnimation(int _anyNumber)
    {
        _sakuraAnimator.SetInteger("State", _anyNumber);
    }
}
