using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Update: //@ 2023.09.22
// Update: //@ 2023.09.25
// Update: //@ 2023.09.26
// Update: //@ 2023.09.27


// NOTE:  //# 게임 마스터
//#          1) 플레이어의 피를 깎기
//#          2) 알람을 찾아서 지속적으로 호출해 애니메이터 동작
//#          3) 메테오 생성 구현
//#          4) 2종류의 에너미 생성
// NOTE: //# 플레이어 사망시 (1)총알발사금지 / (2) 몬스터 생성 금지 / (3) 메테오 생성 금지
// NOTE: //#             (4) GameOver 문구 출력 / 0.5초마다 게임오버텍스트 깜빡깜빡(while)

public class GameMaster : MonoBehaviour
{
    public int playerHp = 100;
    public int enemyHp = 100;
    public Slider hpSlider;
    public Text scoreText;
    public Text playerHpText;
    GameObject gamePanel;   //^ 게임패널 컨트롤

    public int initScore = 0;

    public GameObject meteoRitePrefabs;     //& meteo 프리팹 연결
    // IEnumerator enumerator; //& 코루틴 변수 // TEMP: 다음에 사용법 배우기
    public GameObject ladyBugPrefabs;
    public GameObject fireMothPrefabs;
    public float timeFlow = 0.0f;
    public float interVal = 3.0f;
    public int enemiesType = 0;     //$ 몬스터들의 초기값
    // Update: //% 플레이어 사망시 총알 발사 불가 코드 구현
    GameObject missileFireScript;
    public bool isPlayerAlive = true;
    public static GameMaster instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        //gamePanel.GetComponent<Image>().enabled = false;
        hpSlider.GetComponent<Slider>();
        gamePanel = GameObject.Find("GamePanel");
        // gamePanel.SetActive(false);

        // enumerator = GenerateMeteo();

        // TEST: //! Failed
        // StartCoroutine(GenerateEnemies());

        // Update: //% 플레이어 사망시 총알 발사 불가 코드 구현
        missileFireScript = GameObject.Find("FirePos");

        StartCoroutine(GenerateMeteo());

    }

    /* TEST: //! Failed
    IEnumerator GenerateEnemies()
    {
        GameObject genEnemies = Instantiate(ladyBugPrefabs) as GameObject;
        float enemiesGenPos = Random.Range(-4.5f, 5.5f);
        genEnemies.transform.position = new Vector3(enemiesGenPos, 6.0f, -1);
        yield return new WaitForSeconds(0.7f);
    }
    */

    IEnumerator GenerateMeteo()
    {
        GameObject alarm = GameObject.Find("alarm");


        alarm.GetComponent<Animator>().SetTrigger("Wanning");
        float posX = Random.Range(-4.5f, 5.5f);
        alarm.transform.position = new Vector3(posX, 6.0f, -1);

        yield return new WaitForSeconds(0.7f);
        GameObject genMeteos = Instantiate(meteoRitePrefabs) as GameObject;     //& meteo 생성

        genMeteos.transform.position = alarm.transform.position;

        Debug.Log("알람호출");
        yield return new WaitForSeconds(5.0f);

        // Update: //% 플레이어 사망시 몬스터 생성 불가 코드 구현
        if (!isPlayerAlive)
        {
            yield break;        //& meteo 생성 중단
            // yield return null;       //# 같은 코드   
        }

        StartCoroutine(GenerateMeteo());    //! 콜백;
    }

    void PlayerDamage(int playerDam)    //# (MetoeController)스크립트의 SendMessage로 주입된 값을 받음.
    {
        playerHp -= playerDam;     //! 플레이어가 데미지 입는 함수 
        if (playerHp <= 0)      //& < 0 보다 <= 0으로 표현하는 것이 연산이 더 정확하다
        {
            playerHp = 0;
            // Update: //% 플레이어 사망시 총알 발사 불가 코드 구현
            //missileFireScript.GetComponent<MissileFire>().isAlivePlayer = false;
            // MissileFire.instance.isAlivePlayer = false;
            isPlayerAlive = false;

            //gamePanel.SetActive(true);

            // hpSlider.value = playerHp;
            Debug.Log("플레이어 데미지 함수 작동시작" + playerHp);

            // Update: //@ 2023.09.27   게임오버 구현
            GameTextScript.instance.GameOver();
        }

        hpSlider.value = playerHp;
        playerHpText.text = playerHp.ToString() + " % ";    //& UI 반영
    }

    void hitEnemyDamage()   //! 몬스터가 입는 피해 
    {
        enemyHp -= 10;
    }
    void addScore(int getPoint)     //! 스코어 획득
    {
        initScore = initScore + getPoint;
        scoreText.text = initScore.ToString();
        Debug.Log("스코어점수 : " + getPoint);
    }

    void Update()
    {
        GameObject enemies;
        timeFlow += Time.deltaTime;

        // Update: //% 플레이어 사망시 몬스터 생성 불가 코드 구현
        if (timeFlow > interVal && isPlayerAlive)        //$ 기준시간 보다 증가되는 시간이면
        {
            enemiesType = Random.Range(0, 2);    //$ 2가지 종류 (몬스터의 종류)
            int escapeHole = Random.Range(0, 4);     //$ 
            for (int i = 0; i < 5; i++)
            {
                if (escapeHole != i)    //& ((0 != 0) 조건 미충족 /// (0 != 1) 조건 충족
                {
                    if (enemiesType == 1)
                    {
                        enemies = Instantiate(ladyBugPrefabs) as GameObject;    //# 무당벌레 생성
                    }
                    else
                    {
                        enemies = Instantiate(fireMothPrefabs) as GameObject;   //# 파이어모기 생성
                    }
                    enemies.transform.position = new Vector2(i * 2 - 4, 14); //? 0일때는 -4값 // 4일 때는 4값이 리턴 즉 (-4 ~ +4)
                                                                             // print("몬스터생성좌표 : " + enemies.transform.position);
                }   //% end of escapeHole
            }   //% end for
            timeFlow = 0.0f;    //% 시간값 리셋
        }
    }
}