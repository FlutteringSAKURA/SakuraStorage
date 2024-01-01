using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Update: //@ 2023.10.26 
// Update: //@ 2023.10.27 

//~ ------------------------------------------------------------------------

public class SakuraUIManager : MonoBehaviour
{
    public Text _playerName;
    public Text _playerGold;
    public Image _playerHpBar;

    // TEST: //@ 2023.10.27 
    // not Working:
    //public Image _PlayerSkillGauageBar;
    public GameObject _gameOver;
    public GameObject _gameOver1;

    public static SakuraUIManager instance;

    //~ ------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //~ ------------------------------------------------------------------------
    private void Start()
    {
        _gameOver.SetActive(false);
        _gameOver1.SetActive(false);
    }

    //~ ------------------------------------------------------------------------
    public void UpdatePlayerUI(SakuraParams _playerParams)
    {
        //& SakuraParams의 name을 UI에 표현
        _playerName.text = _playerParams._name;
        _playerGold.text = "Gold \n " + _playerParams._gold.ToString();
        _playerHpBar.rectTransform.localScale = new Vector3((float)_playerParams._currentHp / (float)_playerParams._maxHp, 1f, 1f);
    }


    // TEST: //@ 2023.10.27 
    // not Working:
    // public void SkillGaugeBar(SakuraParams _playerParams)
    // {
    //     float _skillPoint = Random.Range(0.15f, 0.25f);
    //     _PlayerSkillGauageBar.rectTransform.localScale =
    //     new Vector3((float)_playerParams._minMana + (float)_skillPoint, 1f, 1f);
    //     //// Debug.Log("skill");
    //     if (_playerParams._minMana > 1)
    //     {
    //         _playerParams._minMana = 1;
    //     }
    // }

    public void ShowGameOver()
    {
        _gameOver.SetActive(true);
        _gameOver1.SetActive(true);
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene("Sakura_3D_RTS");
    }
}
