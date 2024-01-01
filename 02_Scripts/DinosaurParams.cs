using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update: //@ 2023.10.23 
// Update: //@ 2023.10.24 
// Update: //@ 2023.10.26 

// NOTE: //# 3D 게임 - 공룡 파라미터 스크립트 .. CharacterParams를 상속함
//#          1) 공룡 체력 게이지 구현
//#          2) 공룡 일정 체력 감소시 네임컬러 변화
//#          3) 

public class DinosaurParams : CharacterParams
{
    // public int _dinoSaurHp = 100;
    // public bool _isDinoAlive = true;

    public string _name;
    public int _exp { get; set; }
    public int _rewardGold { get; set; }

    public Image _dinoHpBar;
    // Update: //@ 2023.10.26 
    public Text _textColor;
    public int _nameTextChangeHpValue = 55;


    //~ ------------------------------------------------------------------------
    // private void Start()
    // {
    //     // _isDinoAlive = true;
    //     //& 부모(CharacterParams)에서 이미 하고 있으므로 아래 InitParams를 Start함수에서 실행할 필요 없음
    //     // InitParams(); 
    // }

    public override void InitParams()
    {
        // base.InitParams();
        /*
         _name = "Raptor";
                _level = 1;
                _maxHp = 100;

                _currentHp = _maxHp;
                _attackMaxPower = 20;
                _attackMinPower = 10;
                _defense = 1;

                _exp = 10;
                _rewardGold = Random.Range(100, 301);
         */

        _isAlive = true;
        _isAttack = false;



        // Update: //@ 2023.10.24 
        InitHpBarSize();

        //% XML 매니저에서 넘겨받은 이름을 기준으로.. this는 해당되는 파라미터 값을 찾아서 반영
        XmlManager.instance.LoadDinosaurParamsFromXML(_name, this);
    }

    //@ 사쿠라의 공격으로 인해 데미지를 입는 함수 
    protected override void UpdateAfterAttackRecieved()
    {
        base.UpdateAfterAttackRecieved();
        _dinoHpBar.rectTransform.localScale = new Vector3((float)_currentHp / (float)_maxHp, 1f, 1f);
        if (_currentHp <= _nameTextChangeHpValue)
        {
            _textColor.color = Color.red;
        }
    }

    void InitHpBarSize()
    {
        //% 본래 자신의 사이즈 (1, 1, 1).. 초기화
        _dinoHpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        _textColor.color = Color.white;
    }


    //@ 사쿠라의 공격으로 인해 데미지를 입는 함수 
    // public void Damaged(int _sakuraAttackPower)
    // {
    //     _dinoSaurHp -= _sakuraAttackPower;
    //     Debug.Log("공룡의 생명력" + _dinoSaurHp);
    //     if(_dinoSaurHp <= 0)
    //     {
    //         _isDinoAlive = false;
    //         Debug.Log("공룡이 죽었습니다.");
    //     }
    // }

}
