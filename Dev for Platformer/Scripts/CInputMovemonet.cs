using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CnControls 사용
using CnControls;

public class CInputMovemonet : CMovement
{
    // 이동속도 
    public float _speed;
    public bool _isJump = false; // 1단 점프 유무
    public bool _isDoubleJump = false; // 2단 점프 유무

    // 점프 크기(힘)
    public float _jumpPower;

    // 지면 위 여부
    public bool _isGround = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        InputMove(); // 키입력을 통한 이동 처리
        InputJump(); // 키입력을 통한 점프 처리
    }
    private void InputMove()
    {
        //float h = Input.GetAxis("Horizontal");
        float h = CnInputManager.GetAxis("Horizontal");

        // 오른쪽을 보고 있고 왼쪽키를 눌렀다면
        // 또는 왼쪽을 보고 있고 오른쪽키를 눌렀다면
        if ((_characterState._isRightDir && h < 0) ||
            (!_characterState._isRightDir && h > 0))
        {
            Flip(); // 반전하라
        }

        // 캐릭터에 속도를 부여함
        _rigidbody2d.velocity =
            new Vector2(h * _speed, _rigidbody2d.velocity.y);

        // [상태 유지 애니메이션 : 값에 따라 애니메이션 상태를 유지함]
        // Animator.SetInt("애니메이션파라미터명", 정수값);
        // Animator.SetFloat("애니메이션파라미터명", 실수값);
        // Animator.SetBool("애니메이션파라미터명", 불값);

        // [상태 발동 애니메이션 :값에 따라 애니메이션 상태를 실행함]
        // Animator.SetTrigger("트리거이름");

        _animator.SetFloat("Horizontal", h);

        // 상승 및 하강의 속도를 애니메이터에 넘겨줌
        _animator.SetFloat("Vertical", _rigidbody2d.velocity.y);
    }

    void Jump()
    {
        // 점프 애니메이션 발동(Trigger)
        _animator.SetTrigger("Jump");

        // 점프 초기화
        _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, 0f);

        // 점프 
        _rigidbody2d.AddForce(Vector2.up * _jumpPower);
    }
    void InputJump()
    {
        // 왼쪽 컨트롤키를 누르면
        // if (Input.GetKeyDown(KeyCode.Z))
        if (CnInputManager.GetButtonDown("CustomJump"))
        {
            // 점프를 안한상태면
            if (!_isJump) // _isJump == false
            {
                Jump(); // 점프 수행
                _isJump = true; // 점프를 한 상태로 변경
            }
            // 이미 점프 한 상태에서 2단 점프를 하지 않은 상태면
            else if (_isJump && !_isDoubleJump)
            {
                Jump();// 점프 수행
                _isDoubleJump = true; // 2단 점프를 한 상태로 변경
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 캐릭터가 지면에 충돌 했다면
        if(collision.gameObject.tag == "Ground")
        {
            GroundSetting(true);
            _isJump = _isDoubleJump = false;
        }
    }

    // 지면 여부 설정 
    void GroundSetting(bool isGround)
    {
        _isGround = isGround;

        // 지면과 닿거나 떨어질때 IsGround 값을 애니메이터터에 넘겨 줌
        _animator.SetBool("IsGround", _isGround);
    }

    // OnCollisionStay2D() : 충돌 중임을 알려주는 이벤트 메소드
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            GroundSetting(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 캐릭터가 지면에서 떨어졌다면
        if (collision.gameObject.tag == "Ground")
        {
            GroundSetting(false);
        }
    }
}
