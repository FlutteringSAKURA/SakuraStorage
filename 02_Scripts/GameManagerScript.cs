//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Update: //@ 2023.11.09 
// Update: //@ 2023.11.13 

// NOTE: //# 3D 게임 - GameManager
//#          1) 

public class GameManagerScript : MonoBehaviour
{
    [Header("[ UI INFO ]")]
    public static GameManagerScript instance;

    public GameObject _uIPanel;

    public bool _onUIFlag = false;

    //# Update: Gen Creature
    [Header("[ CREATURE GENERATE INFO ]")]
    public Transform[] _eggCreatureTransformPoints;     //& Creature 배열
    public Transform[] _eggCyberCreatureTransformPoint;     //& CyberCreature 배열
    public Transform[] _tentacleZoneTransformPoints;

    // Update:
    public List<Transform> _eggCreatureTrGroup = new List<Transform>();

    //% 크리처 저장 리스트 타입
    // public List<GameObject> _creatureEggPool = new List<GameObject>();
    public int _maxCreturesEgg = 10;


    //!

    public Transform[] _middleParent1;
    public Transform[] _middleParent2;
    public GameObject _creaturePrefab;
    public GameObject _cyberCreaturePrefab;
    public GameObject _creature2Prefab;

    //% 크리처 티쓰 저장 리스트 타입
    public List<GameObject> _creaturesEggPool = new List<GameObject>();

    //% 사이버 크리처 저장 리스트 타입
    public List<GameObject> _cyberCreatureEggPool = new List<GameObject>();
    private int _deadCreature = 0;

    public float _createTime = 0.3f;
    public int _maxCreatures = 5;
    public int _maxCyberCreatures = 4;

    public bool _isGameOver; //& 게임종료 체크

    //% 프로퍼티
    public bool IsGameOver
    {
        get { return _isGameOver; }
        set
        {
            _isGameOver = value;
            if (_isGameOver)
            {
                CancelInvoke("GenerateCreatureTeeth");
                CancelInvoke("GenCreature");
            }
        }
    }

    // TEST:
    GameObject _emptyObject1;
    GameObject _emptyObject2;
    //! -------------------------------------- 

    [Header("[ OBJECT POOLING ]")]
    [SerializeField]
    GameObject _bulletPrefab;       //& 생성할 총알
    [SerializeField]
    int _maxPool = 10;
    [SerializeField]
    List<GameObject> _bulletPool = new List<GameObject>();

    GameObject _tentacleSensor;
    GameObject _playerObj;

    [Header("[ UI INFO ]")]
    public TMP_Text _scoreText;
    float _score = 0;

    public TMP_Text _killCountText;
    int _killCount = 0;
    // Update: //@ 2023.11.13 
    public GameObject _restartBtn;


    //~ ------------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null) instance = this;
        GeneratePoling();

        //% 게임 초기 기본 데이터 가져오는 함수 콜백
        LoadGameData();
    }

    //@ 게임 초기 기본 데이터 가져오는 함수 
    private void LoadGameData()
    {
        _score = PlayerPrefs.GetFloat("SCORE", 0);
        //% 초기점수
        DisplaySocre(0);


        _killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        // Legacy:"KILL COUNT : " + _killCount.ToString("0000");
        // Update:
        _killCountText.text =
        string.Format($"<mark=yellow><color=red> kill count :   </mark><color=white>{_killCount:#,##0}");

    }

    //~ ------------------------------------------------------------------------

    public void GeneratePoling()
    {
        //& 총알을 생성할 때 그 총알을 자식으로 넣어두고 사용할 부모를 생성
        GameObject _emptyObject = new GameObject("_objectPools");
        for (int i = 0; i < _maxPool; i++)
        {
            var _bullet = Instantiate(_bulletPrefab, _emptyObject.transform);
            _bullet.name = "Bullet_" + i.ToString("00");
            _bullet.SetActive(false);
            _bulletPool.Add(_bullet);

        }
    }

    public GameObject GetBullets()
    {
        for (int i = 0; i < _bulletPool.Count; i++)
        {
            if (_bulletPool[i].activeSelf == false)     //& 비활성상태라면
            {
                return _bulletPool[i];      //& 반환 시킴 
            }
        }
        return null;

    }
    //~ ------------------------------------------------------------------------

    private void Start()
    {
        _tentacleSensor = GameObject.Find("TentacleSensor");
        _playerObj = GameObject.FindWithTag("Player");

        _tentacleZoneTransformPoints = GameObject.Find("TentacleZonePointGroup").GetComponentsInChildren<Transform>();
        //! -------------------------------------------------------------------------------------------------------------------

        // TEST: //# (1)
        // SUCCESS:
        // Legacy: (1) 배열 활용 방식 .. TEST(1)에 사용한 코드 
        _eggCreatureTransformPoints = GameObject.Find("CreaturesGroup").GetComponentsInChildren<Transform>();
        //         (2 )Transform _eggCreatureTransformPoints = GameObject.Find("CreaturesGroup")?.transform;

        // NOTE: //# Transform 리스트 활용 방식
        // TEMP: // TEST: (3)
        //Transform _genCreaturesPoint = GameObject.Find("CreaturesGroup")?.transform;

        //_genCreaturesPoint?.GetComponentsInChildren<Transform>(_eggCreatureTransformGroup);       //$ 리스트로 선언한 경우
        // Transform[] _pointArray = _genCreaturePointGroup?.GetComponentsInChildren<Transform>(true);     //$ 배열로 선언한 경우
        // NOTE: //# 위에서 각각 한줄로 표현할 수 있는 (2) 내용을 풀어 쓰면 다음과 같이 코드를 작성할 수 있다.------------------------------------
        //#         .. ? 물음표 연산자는 ..

        /*
        GameObject _eggCreatureTransformPointsObj = GameObject.Find("CreaturesGroup");
        if (_eggCreatureTransformPointsObj != null)
        {
            Transform _eggCreatureTransformPointsGroup = _eggCreatureTransformPointsObj.GetComponent<Transform>();
            _eggCreatureTransformPoints = _eggCreatureTransformPointsGroup.GetComponentsInChildren<Transform>();
        }

        */

        // TEMP: // TEST: (3)
        // foreach (Transform genPoint in _genCreaturesPoint)
        // {
        //     _eggCreatureTrGroup.Add(genPoint);      //! Transform리스트에 genPoin를 넣어줌.. 부모까지 배열에 포함되는 것을 방지할 수 있음
        // }

        // //& 호출함수, 대기시간, 호출 인터벌
        // InvokeRepeating("GenerateCreatureTeeth", 2.0f, _createTime);

        //# ---------------------------------------------------------------------------------------------------------------

        _eggCyberCreatureTransformPoint = GameObject.Find("Cyber_CreaturesGroup").GetComponentsInChildren<Transform>();


        if (_eggCreatureTransformPoints.Length > 0)
        {
            //& 호출함수, 대기시간, 호출 인터벌
            InvokeRepeating("GenerateCreatureTeeth", 2.0f, _createTime);
        }

        if (_eggCyberCreatureTransformPoint.Length > 0)
        {
            // Legacy: 
            StartCoroutine(this.GenerateCreature());
            // _score = PlayerPrefs.GetFloat("SCORE", 0);        //& 스코어 점수 출력
            // DisplaySocre(0);

        }

        // if (_eggCyberCreatureTransformPoint.Length > 0)
        // {
        //     StartCoroutine(this.GenerateCreature());
        // }
        //! -----------------------------------------------------------------------------------------------------------------------

        // TEST: //# (2) 
        // SUCCESS:
        // _eggCreatureTransformPoints = GameObject.Find("CreaturesGroup").GetComponentsInChildren<Transform>();
        // for (int i = 0; i < _maxCreatures; i++)
        // {
        //     GameObject _creature = (GameObject)Instantiate(_creaturePrefab);
        //     _creature.name = "Creature_" + i.ToString();
        //     _creature.SetActive(false);
        //     //  _creature.transform.parent = _eggCreatureTransformPoints[i].transform;
        //     _creaturesEggPool.Add(_creature);
        // }

        // if (_eggCreatureTransformPoints.Length > 0)
        // {
        //     StartCoroutine(this.GenerateCreature());
        // }



        // _eggCyberCreatureTransformPoint = GameObject.Find("Cyber_CreaturesGroup").GetComponentsInChildren<Transform>();
        // for (int i = 0; i < _maxCyberCreatures; i++)
        // {
        //     GameObject _cyberCreature = (GameObject)Instantiate(_cyberCreaturePrefab);
        //     _cyberCreature.name = "Cyber_Creature_" + i.ToString();
        //     _cyberCreature.SetActive(false);
        //     // _cyberCreature.transform.parent = _eggCyberCreatureTransformPoint[i].transform;
        //     _cyberCreatureEggPool.Add(_cyberCreature);
        // }

        // if (_eggCyberCreatureTransformPoint.Length > 0)
        // {
        //     StartCoroutine(this.GenerateCreature());
        // }

        //& 크리처를 생성할 때 그 크리처을 자식으로 넣어두고 사용할 부모를 생성
        _emptyObject1 = new GameObject("Creatures_Teeth");
        _emptyObject2 = new GameObject("Creatures_Cyber");



        GenerateCretureEggs();

    }

    //~ ------------------------------------------------------------------------

    // TEMP: //# ----------------------------
    // void GenerateCreatureTeeth()
    // {
    //     int _index = Random.Range(0, _eggCreatureTrGroup.Count);
    //     Instantiate(_creaturePrefab, _eggCreatureTrGroup[_index].position, _eggCreatureTrGroup[_index].rotation);
    // }
    //# ---------------------------------------------

    // TEST: //# Invoke 방식
    void GenerateCreatureTeeth()
    {
        int _creature = (int)GameObject.FindGameObjectsWithTag("Creature").Length;
        if (_creature < _maxCreatures)
        {
            int _index = Random.Range(1, _eggCreatureTransformPoints.Length);
            GameObject creatureTeeth =
                Instantiate(_creaturePrefab, _eggCreatureTransformPoints[_index].position, _eggCreatureTransformPoints[_index].rotation);

            creatureTeeth.transform.parent = _emptyObject1.transform;
        }
    }

    IEnumerator GenerateCreature()
    {
        while (!_isGameOver)
        {

            // TEST:(1)
            // SUCCESS:
            int _creature = (int)GameObject.FindGameObjectsWithTag("Creature").Length;
            if (_creature < _maxCreatures)
            {
                yield return new WaitForSeconds(_createTime);

                //& 랜덤 위치 ---------------------------------
                // Legacy: 
                int _index = Random.Range(1, _eggCreatureTransformPoints.Length);
                // Legacy:
                GameObject creatureTeeth =
                Instantiate(_creaturePrefab, _eggCreatureTransformPoints[_index].position, _eggCreatureTransformPoints[_index].rotation);
                //&-------------------------------------------

                // TEST: 부모오브젝트가 1더 들어가는 것을 방지 //?
                // _creaturesEggPool.Add(creatureTeeth);
                // for (int i = 0; i < _creaturesEggPool.Count; i++)
                // {
                //     _creaturesEggPool.RemoveAt(0);
                // }
                //!

                // Legacy: 
                creatureTeeth.transform.parent = _emptyObject1.transform;

            }
            else
            {
                yield return null;
            }

            int _cyberCreature = (int)GameObject.FindGameObjectsWithTag("CyberCreature").Length;
            if (_cyberCreature < _maxCyberCreatures)
            {
                yield return new WaitForSeconds(_createTime);
                int _index = Random.Range(1, _eggCyberCreatureTransformPoint.Length);

                GameObject creatureCyber =
                Instantiate(_cyberCreaturePrefab, _eggCyberCreatureTransformPoint[_index].position, _eggCyberCreatureTransformPoint[_index].rotation);
                creatureCyber.transform.parent = _emptyObject2.transform;
            }
            else
            {
                yield return null;
            }

            //! -----------------------------------------------------------------------------------------------------------------------

            // TEST:(2)
            // SUCCESS:
            // yield return new WaitForSeconds(_createTime);
            // if (_isGameOver) yield break;

            // foreach (GameObject creature in _creaturesEggPool)
            // {
            //     if (!creature.activeSelf)
            //     {
            //         int _index = Random.Range(1, _eggCreatureTransformPoints.Length);
            //         creature.transform.position = _eggCreatureTransformPoints[_index].position;
            //         creature.SetActive(true);
            //         break;
            //     }
            // }

            // foreach (GameObject cyberCreature in _creaturesEggPool)
            // {
            //     if (!cyberCreature.activeSelf)
            //     {
            //         int _index = Random.Range(1, _eggCyberCreatureTransformPoint.Length);
            //         cyberCreature.transform.position = _eggCyberCreatureTransformPoint[_index].position;
            //         cyberCreature.SetActive(true);
            //         break;
            //     }
            // }

        }
    }
    //~ ------------------------------------------------------------------------

    //@ 오브젝트 풀에 크리처 생성 함수 
    void GenerateCretureEggs()
    {
        GameObject _creatureObj = new GameObject("Creature_Egg_Box");
        for (int i = 0; i < _maxCreturesEgg; i++)
        {
            GameObject _creatures = Instantiate<GameObject>(_creature2Prefab, _creatureObj.transform);
            _creatures.name = "Creature_" + i.ToString("Teeth_00");
            _creatures.SetActive(false);
            _creaturesEggPool.Add(_creatures);
        }

    }


    //@ CreatureTeeth(TentacleZone).. 생성함수 
    public void GenCreatureOnTentacleZone()
    {
        if ((!_isGameOver))
        {
            foreach (GameObject creatureTeeth in _creaturesEggPool)
            {
                if (!creatureTeeth.activeSelf)
                {
                    int _index = Random.Range(1, _tentacleZoneTransformPoints.Length);
                    creatureTeeth.transform.position = _tentacleZoneTransformPoints[_index].position;
                    creatureTeeth.SetActive(true);
                    break;
                }
            }
        }

    }

    //@ 불규칙한 위치 생성하고 크리처 생성 함수 
    public void TentacleZoneCreatureGenerate()
    {
        int _index = Random.Range(0, _creaturesEggPool.Count);
        GameObject _tentacleZoneCreature = GetCreaturesOnTentacleZone();
        //& 추출한 크리처의 위차와 회전값 설정
        _tentacleZoneCreature?.transform.SetPositionAndRotation(_tentacleZoneTransformPoints[_index].position,
                                                                _tentacleZoneTransformPoints[_index].rotation);
        // TEMP: //# NOTE: 위의 코드와 똑같은 표현 .. ? 물음표 연산자가 (.. != null) 과 같은말 
        // if (_tentacleZoneCreature != null)
        // {
        //     _tentacleZoneCreature.transform.SetPositionAndRotation(_tentacleZoneTransformPoints[_index].position,
        //                                                         _tentacleZoneTransformPoints[_index].rotation);
        // }


        _tentacleZoneCreature?.SetActive(true);
    }

    //@ 오브젝트 풀에서 크리처 얻는 함수 
    public GameObject GetCreaturesOnTentacleZone()
    {
        foreach (GameObject creatureTeeth in _creaturesEggPool)
        {
            if (!creatureTeeth.activeSelf)
            {
                return creatureTeeth;
            }
        }
        return null;
    }


    // 오브젝트 풀에서 크리처 얻는 함수 
    // public GameObject GetCreatures()
    // {
    //     for (int i = 0; i < _creaturesEggPool.Count; i++)
    //     {
    //         if (_creaturesEggPool[i].activeSelf == false)
    //         {
    //             return _creaturesEggPool[i];
    //         }
    //     }
    //     return null;
    // }

    public void DisplaySocre(float getScore)
    {
        _score += getScore;
        _scoreText.text =
        string.Format($"<color=#00ff00>SCORE: </color><color=yellow>{_score:#,##0}</color>");
        // NOTE:
        // {12345:#,##0} ===> 12,345
        // {123:#,##0} ===> 123
        // {123:0,000} ===> 0,123
        // {123:0000} ===> 0123

        //% 스코어 저장
        PlayerPrefs.SetFloat("SCORE", _score);
    }

    public void AddKillCount(int kill_Number)
    {
        _killCount += kill_Number;
        _killCountText.text =
        string.Format($"<mark=yellow><color=red> kill count :   </mark><color=white>{_killCount:#,##0}");

        //% 킬카운트 저장
        PlayerPrefs.SetInt("KILL_COUNT", _killCount);
    }


    // Update: //@ 2023.11.13 
    //# 리스타트 버튼 
    public void RestartGame()
    {
        SceneManager.LoadScene("Sakura3D_DommsDay_HellGate");
    }
    //@ PlayerPrefs 데이터 삭제 
    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Sakura3D_DommsDay_HellGate");
    }

}
