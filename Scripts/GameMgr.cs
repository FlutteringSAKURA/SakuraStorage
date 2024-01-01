using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    // 트랜스폼 배열 및 몬스터 프리팹 참조 변수 선언
    public Transform[] points;
    public GameObject monsterPrefab;

    // [ 풀 코드 1 ] -> 리스트 생성
    public List<GameObject> monsterPool = new List<GameObject>();
    // ====================== 풀 코드 End

    // 리스폰 타임 설정 -> 2초마다 몬스터 생성
    public float createTime = 2.0f;
    // 리스폰 수량 설정
    public int maxMonster = 10;
    // 게임오버 여부 초기값 설정
    public bool isGameOver = false;

    // [ 공용함수 코드 1 ]
    public float sfxVolume = 1.0f; // 볼륨 초기 설정 변수 선언
    public bool isSfxMute = false; // 음소거 초기 설정 변수 선언
    // =========================================================== 공용 함수 코드 End

    // [ 싱글톤 코드 ] -> 싱글톤 패턴을 위한 인스턴스 변수 선언
    public static GameMgr instance = null;
    // >> Awake 함수에서 사용 (Start함수에서는 게임 실행시 어느 오브젝트가 먼저 수행되느냐에 따라 
    // 영향을 각기 다르게 받게 될 수 있음?

    // [ 마우스 락 코드 ]
    private bool mouseLock = true;
    // =============================== 마우스락 코드 END

    private void Awake()
    {
        instance = this;
    }
    // ================================ 싱글톤 코드 End

    private void Start()
    {
        // 캐시처리
        // SpawnPoint명칭의 오브젝트를 찾아서 그 하위 오브젝트들의 트랜스폼을 가져옴
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        // [ 풀 코드 2 ] 
        for (int i = 0; i < maxMonster; i++)
        {
            GameObject monster = (GameObject)Instantiate(monsterPrefab);
            monster.name = "Monster_" + i.ToString(); // 오브젝트 풀의 몬스터 명칭 출력
            monster.SetActive(false); // 비활성화 시킴
            monsterPool.Add(monster); // 몬스터 풀에 몬스터를 애드
        }
        // ==================================================== 풀 코드 End

        // 배열의 개수(크기)가 0보다 크다면
        if (points.Length > 0)
        {
            // 몬스터 생성 코루틴을 실행
            StartCoroutine(this.CreateMonster());
        }
    }

    private IEnumerator CreateMonster()
    {
        // 게임오버가 아닌 상태동안
        while (!isGameOver)
        {
            /*
            // 몬스터 카운트 설정 -> [ 풀 알파 코드 추가시 주석 처리 ]
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;
            // 만약 최대몬스터 생산 수량보다 생성된 몬스터의 숫자가 적다면
            if (monsterCount < maxMonster)
            {
                // 2초 마다 몬스터 생성
                yield return new WaitForSeconds(createTime);
                // 랜덤 생산을 위한 인덱스 변수의 생성
                int idx = Random.Range(1, points.Length);
                // >> Min값을 0이 아닌 1로 설정한 이유 -> 빈게임오브젝트 즉 SpawanPoint도 배열에 포함되므로
                // >> 정수의 배열은 -1 이다. 따라서 1~ 15포인트까지인것과 같다
                // 해당 위치에 몬스터 생성
                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else yield return null;
            */

            // [ 풀 알파 코드 ]
            yield return new WaitForSeconds(createTime); // 생산 시간동안 대기
            if (isGameOver) yield break; // 게임오버라면 BREAK!
            foreach (GameObject monster in monsterPool)
            {
                // 몬스터가 활성화 상태가 아니면
                if (!monster.activeSelf)
                {
                    int idx = Random.Range(1, points.Length); // 포인트들의 위치 랜덤으로 잡아줌
                    monster.transform.position = points[idx].position; // 몬스터 위치를 포인트 배열 위치에서 생성되게 해줌 (위치를 같게)
                    monster.SetActive(true); // 몬스터 활성화 실행
                    break; // foreeach문을 나감 -> while문 안에서 돈다
                }
            }
            // =========================== 풀 알파 코드 End
        }
    }

    // [ 뮤트 알파 코드 ]
    public void SoundMute()
    {
        isSfxMute = !isSfxMute ; 
        // true = false 쌍방관계코드??
        // 초기설정 false >> 이벤트 발생시 true -> false가 아님 >> [뮤트 실행 = ture] >> 이벤트 다시 발생시 -> true가 더이상 아님 >> 뮤트 해제
    }
    // ========================= 뮤트 알파 코드 End

    // [ 공용함수 코드 2 ]
    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        // 뮤트 처리
        if (isSfxMute) return;
        GameObject soundObj = new GameObject("Sfx"); // Sfx라는 명칭으로 동적 생성
        soundObj.transform.position = pos;
        AudioSource _audioSource = soundObj.AddComponent<AudioSource>();
        // >> soundObj 오브젝트를 생성하여  AudioSource를 참조하고 다시 새로 만든
        // >> _audioSource 에 넣어줌
        _audioSource.clip = sfx; // AudioSourceClip을 sfx명으로 받아줌
        _audioSource.minDistance = 10.0f;
        _audioSource.maxDistance = 50.0f;
        _audioSource.volume = sfxVolume;
        _audioSource.Play(); // 실행
        Destroy(soundObj, sfx.length); // sfx(사운드)의 길이만큼 출력 된 후 파괴
    }

    // [ 마우스 락 코드 ] 
    private void Update()
    {
        // 마우스락 모드라면
        if (mouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // 그 외 
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // esc 키를 누르면 마우스 락 모드 전환
        if (Input.GetKeyDown("escape")) 
        {
            mouseLock = !mouseLock;
        }
    }
    // =============================================== 마우스 락 코드 END
}
