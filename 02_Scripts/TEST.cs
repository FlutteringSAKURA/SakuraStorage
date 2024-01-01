using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//@ 현재 기획하고 있거나 제작하고 싶은 게임을 기반으로 아래 문항을 작성하세요. 
//@ 체력바의 경우에는 원형이나 가로막대바 그리고 그래픽 또는 유니티에서 제공한 콤포넌트를 활용 가능
//@ 플레이어 케릭터의 체력 이미지 값이 줄어듦에 따라 변화하는 과정을 코딩하세요.
//@ 변수는 임의 설정, 플레이어 공격력도 임의 설정. 상대방의 공격력도, 임의의 케릭터를 스테이지에 배치하는 과정도 임의, hP차감하는 것 구현

//~ ------------------------------------------------------------------------ 시작 -------------------------- 

public class TEST : MonoBehaviour
{

    public Slider _playerHpbar;
    GameObject _playerObj;
    GameObject _gamePanel;
    GameObject _startPanel;

    public GameObject _monster;
    public int _monsterHp = 1000;
    public int _initScore = 0;
    public Text _scoreText;

    private void Start()
    {
        _playerObj = GameObject.FindWithTag("Player");
        _gamePanel.SetActive(false);

    }

    void addScroe(int getPoint)
    {
        _initScore = _initScore + getPoint;
        _scoreText.text = _initScore.ToString() + "\n POINT";
    }

}


//~ ------------------------------------------------------------------------ 끝 -------------------------- 
