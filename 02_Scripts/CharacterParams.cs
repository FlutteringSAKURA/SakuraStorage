using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;       //@ 이벤트 처리

// Update: //@ 2023.10.23 

// NOTE: //# 3D 게임 - 부모 파라미터 스크립트 .. 상속..
//#          1) 

//~ ------------------------------------------------------------------------

public class CharacterParams : MonoBehaviour
{

    //% 프라퍼티 선언  
    public int _level { get; set; }   //& inspector 노출 방지, 정식프로퍼티활용
    public int _maxHp { get; set; }
    public int _currentHp { get; set; }
    public int _attackMinPower { get; set; }
    public int _attackMaxPower { get; set; }
    public int _defense { get; set; }
    public int _currentMana { get; set; }
    public int _minMana { get; set; }
    public int _maxMana { get; set; }

    //// public int _attackMin;
    //// public int _attackMax;
    public bool _isAlive = true;
    public bool _isAttack = false;

    

    [SerializeField]
    public UnityEvent deadEvent = new UnityEvent();
    // TEMP:
   //! public UnityEvent battleEvent = new UnityEvent();

    //~ ------------------------------------------------------------------------

    void Start()
    {
        //$ 초기값 .. 변수 초기화
        InitParams();
        
    }

    //~ ------------------------------------------------------------------------

    //@ 가상 함수.. 자식이 쓸 때는 overrie해서 사용 
    public virtual void InitParams()
    {

    }

    // Update:
    public int GetRandomAttack()
    {
        int _randomAttack = Random.Range(_attackMinPower, _attackMaxPower + 1);
        return _randomAttack;
    }

    //@ 공격이 실행될 때 상대방(counter)의 Hp를 감소시키는 함수 
    public void SetCounterAttack(int _counterAttackPower)
    {
        _currentHp -= _counterAttackPower;
        UpdateAfterAttackRecieved();
        // TEMP:
      //!  battleEvent.Invoke();
    }

    // TEST: // TEMP:
    //? 스킬 게이지 구현 ing...
    // public float ChargeSkillGauge()
    // {
    //     float _randomSkillPoint = Random.Range(_minMana, _maxMana + 1);
    //     return _randomSkillPoint;
    //    // Debug.Log("skill Point Up");

    // }

    //@ 캐릭터가 적으로부터 공격받은 후 자동으로 실행되는 함수 .. 
    // NOTE://@ protected virtual (=반드시 부모와 자식간(상속 사이)에만 공유, 다른 곳에서는 접근 불가)
    protected virtual void UpdateAfterAttackRecieved()
    {
        Debug.Log(name + "의 남은 생명력 = " + _currentHp);
        //% 사망하는 경우
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            Debug.Log(name + "가 사망했습니다. 남은 생명력 = " + _currentHp);

            _isAlive = false;       //^ 죽음상태로 전환

            //% 등록해둔 이벤트 함수들이 자동으로 일괄 실행되는 코드
            //% 부모에 있으니 자식들도 모두 공유
            ////Debug.Log("SakuraDeadEvent");
            deadEvent.Invoke();
        }
    }

    


    //~ ------------------------------------------------------------------------
    void Update()
    {

    }
}
