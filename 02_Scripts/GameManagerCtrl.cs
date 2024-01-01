//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Update: //@ 2023.10.11
// Update: //@ 2023.10.12

// NOTE: //# 2D 게임 - 게임 매니저 스크립트 .. 정적변수화(싱글톤) 사용 .. 
//#          1) 플레이어 상태 관리
//#          2) 점수 관리
//#          3) 게임 시작시 UI / 플레이어 사망시 UI
//#          4) 
//#          5)

public class GameManagerCtrl : MonoBehaviour
{
    public static GameManagerCtrl instance;
    public bool _isGameStart = false;
    public bool _isPlayerAlive = true;
    GameObject _playerObj;
    public GameObject _rivivalPlayerPrefab;
    public GameObject _gameStartMainPanel;
    public GameObject _inGamePanel;
    public Text _scoreText;
    public GameObject _gameOverPanel;
    public int _initScore = 0;
    
    private void Awake()
    {
        if (GameManagerCtrl.instance == null)
        {
            GameManagerCtrl.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //& 게임 시작전 UI 셋팅
        _isGameStart = false;
        Time.timeScale = 0.0f;                  //% timeScale 조정하여 인게임 세계 정지 구현 
        _gameStartMainPanel.SetActive(true);    //$ 게임 메인화면 활성화
        _inGamePanel.SetActive(false);          //$ 인게임 화면 비활성화
        _gameOverPanel.SetActive(false);        //$ 게임오버 화면 비활성화

        //& 게임 시작후 플레이어를 찾는 코드
        _playerObj = GameObject.Find("PlayerShip");
        
    }
    private void Update()
    {

    }

    //! 게임 시작전 메인화면에서 스타트 버튼을 누르면 게임이 시작되게 구현하는 함수
    public void StartButtonActivation()     
    {
        _isGameStart = true;
        if(_isGameStart)
        {   
            //% timeScale 정상화
            Time.timeScale = 1.0f;

            //& 해당 함수를 3초후 동작시켜주는 함수 (.. 코루틴과 다르다. Invoke는 코루틴과 달리 여러 함수를 넣어서 쓸 수 없다.)
            //^ 3초 후 자동발사모드 시작. ((isAutoFire 활성화))
            Invoke("StartGame", 3.0f);
            
            //& 게임 시작후 UI 셋팅
            _gameStartMainPanel.SetActive(false);       //$ 게임 메인화면 비활성화
            _inGamePanel.SetActive(true);               //$ 인게임 화면 활성화
        }
        
    }
    //!------------------------------------------------------------

    private void StartGame()
    {
        //^ 플레이어의 자동발사 활성화를 위한 코드
        _playerObj.GetComponent<PlayerShipCtrl>().isAutoFire = true;
        //^ 젠 시작을 위한 코드
        GeneratorCtrl.instance._isGenStart = true;
    }

    //# 게임 스코어 함수
    public void AddScore(int enemyDamagedAndScore)
    {
        //& 에너미 피격시.. 에너미는 데미지를 입고 플레이어는 스코어를 획득
        _initScore += enemyDamagedAndScore;
        _scoreText.text = _initScore.ToString() + " POINT";

    }

    //# NOTE: 플레이어 사망처리 함수    
    //# 1) 게임오버 메시지 출력 .. UI   // Completed:
    //# 2) 젠 중지                  // Completed:
    //# 3) 플레이어 총알발사 중지       // Completed: .. 플레이어의 비행기가 파괴되어 사라지는 것으로 구현
    //# 4) 플레이어 이동 중지          // Completed: .. 플레이어의 비행기가 파괴되어 사라지는 것으로 구현
    public void PlayerDie()
    {
        //^ 플레이어 사망시 GameOver UI 활성화
        //_gameOverPanel.SetActive(true);   //$ 아래 코드와 동일 .. GameTextCtrl에서 처리하기 위해 주석처리.
        GameTextCtrl.instance.ShowGameOver();
        //^ 플레이어 사망처리
        _isPlayerAlive = false;
        _playerObj.GetComponent<MoveToInitalPoint>()._getReady = false;

        //^ 플레이어 총알 자동 발사 중지
        _playerObj.GetComponent<PlayerShipCtrl>().isPlayerAlive = false;
        _playerObj.GetComponent<PlayerShipCtrl>().isAutoFire = false;
        //^ 에너미 젠 중지
        GeneratorCtrl.instance._isGenStart = false;

        Debug.Log("플레이어의 비행기가 파괴 되었습니다.");

    }

    // Update: //@ 2023.10.12

    //# NOTE: 게임 초기화 함수
    //#                     1) 플레이어 위치 초기화
    //#                     2) 인게임상 남아있는 총알 클리어
    //#                     3) 인게임상 남아있는 에너미들 클리어 
    //#                     4) 스코어 초기화
    //#                     5) 게임리스타트 버튼 작동 구현


    public void GameResetAndRestart()
    {
        // TEMP: ObjectManagerCtrl 연결
        Debug.Log("Game Restart Check");

        //^ 플레이어 부활
        _isPlayerAlive = true;
        //^ 플레이어 부활 위치 선언하여 생성
        Vector3 _revivalPlayerPos = new Vector3(0, 2.35f, 0);
        //^ 플레이어 부활 구현 ((PlayerShipCtrl) 스크립트에서 RivivalPlayer 함수가 대신 처리
        //Instantiate(_rivivalPlayerPrefab, _revivalPlayerPos, Quaternion.identity);
        //_playerObj.SetActive(true);
        _playerObj.transform.position = _revivalPlayerPos;
        //^ 인게임 레이저 초기화
        ObjectManagerCtrl.instance.ClrearLazer();
        //^ 에너미 초기화 
        GeneratorCtrl.instance.ClearEnemies();
        //^ 에너미 젠 스타트 
        GeneratorCtrl.instance._isGenStart = true;
        //^ 스코어 초기화
        _initScore = 0;
        _scoreText.text = string.Empty + "000 POINT";
        //^ UI 셋팅
        GameTextCtrl.instance._gameOverText.SetActive(false);
        //^ ready text 코루틴 실행
        GameTextCtrl.instance.GameResetAndRestart();
        
        //StartCoroutine(AutoFireShootActive());

    }

    IEnumerator AutoFireShootActive()
    {
        _playerObj.GetComponent<PlayerShipCtrl>().isAutoFire = true;
        yield return new WaitForSeconds(4.0f);
        Debug.Log("restart Game. test");
    }
}
