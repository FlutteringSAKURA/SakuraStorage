//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update: //@ 2023.10.23 
// Update: //@ 2023.10.26 

// NOTE: //# 3D 게임 - 플레이어 파라미터 스크립트 - CharacterParams를 상속함
//#          1) 플레이어의 랜덤 공격력
//#          2) Hp 구현

//~ ------------------------------------------------------------------------
public class SakuraParams : CharacterParams
{
    // public int _attackMin = 20;
    // public int _attackMax = 30;
    public string _name { get; set; }
    public int _currentExp { get; set; }      //^ 현재 경험치
    public int _nextLevelExp { get; set; }    //^ 다음 레벨 경험치
    // public int _currentMana { get; set; }     //^ 현재 마나
    // public int _minMana { get; set; }
    // public int _maxMana { get; set; }         //^ 최대치 마나
    public int _gold { get; set; }            //^ 소지 골드

    // Update: //@ 2023.10.26 
    public Image _sakuraHpBar;
    // Update: //@ 2023.10.27 
    public Image _skillGagueBar;

    //~ ------------------------------------------------------------------------

    //@ 사쿠라 플레이어 파라미터 재정의 
    public override void InitParams()
    {
        //base.InitParams();
        _name = "[Sakura]";
        _level = 1;
        _maxHp = 100;   //! 선언의 순서 주의..치환할 떄 선언 잘못 하면 문제 생길 수 있음.

        _currentHp = _maxHp;

        _attackMaxPower = 35;
        _attackMinPower = 20;
        _defense = 1;
        _isAlive = true;

        _currentExp = 0;
        _nextLevelExp = 50 * _level;
        _currentMana = _minMana;
        _minMana = 0;
        _maxMana = 100;
        _gold = 0;

        InitHpbarSize();
        InitSkillGaugeBarSize();

    }

    private void InitHpbarSize()
    {
        _sakuraHpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    private void InitSkillGaugeBarSize()
    {
        _skillGagueBar.rectTransform.localScale = new Vector3(0f, 1f, 1f);
    }

    protected override void UpdateAfterAttackRecieved()
    {
        base.UpdateAfterAttackRecieved();

        ////_sakuraHpBar.rectTransform.localScale = new Vector3((float)_currentHp / (float)_maxHp, 1f, 1f);
        //& this == (SakuraParams)스크립트..Class를 통째로 SakuraUIManager.instance.UpdatePlayerUI함수에 넘김.
        SakuraUIManager.instance.UpdatePlayerUI(this);
    }

    //@ 플레이어가 얻은 골드
    public void AddGoldReward(int _newGold)
    {
        //& NOTE: 보통 this는 해당 스크립트에 선언된 변수를 가리킨다. this.변수
        this._gold += _newGold;
        SakuraUIManager.instance.UpdatePlayerUI(this);
    }


    //~ ------------------------------------------------------------------------
    private void Update()
    {

    }

}
