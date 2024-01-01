using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.10.20 
// Update: //@ 2023.10.23 

// NOTE: //# 3D 게임 - 공룡 애니메이션 관리
//#          1) 공룡 상태에 따른 애니메이션

public class DinosaurAnimationManager : MonoBehaviour
{
    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string ATTACK = "Jump_Attack_new";
    public const string FIGHTIDLE = "Fight Idle";
    public const string DAMAGED = "Hit Front";
    public const string DEAD = "Death";

    Animation _dinosaurAnimator;


    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _dinosaurAnimator = GetComponentInChildren<Animation>();
    }
    //~ ------------------------------------------------------------------------
    public void ChangeAnimation(string _anyString)
    {
        _dinosaurAnimator.CrossFade(_anyString);
    }

    // Update: //@ 2023.10.20 
    //@ 공룡이 공격할 때 공격대상의 애니메이션 효과 주는 함수 
    public void SendDinoAttack()
    {
        //transform.parent.gameObject.SendMessage("AttackCalculate");
        SendMessage("AttackCalculate");
    }

}
