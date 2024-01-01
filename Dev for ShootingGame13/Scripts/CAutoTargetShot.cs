using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoTargetShot : MonoBehaviour
{
    public GameObject _laserPrefab; // 레이저

    public float _shotDelayTime; // 발포 지연 시간

    public Transform _shotPos; // 발포 위치

    // Use this for initialization
    private void Start()
    {
        StartCoroutine("AutoShotCoroutine");
    }

    private IEnumerator AutoShotCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_shotDelayTime);

            // 적기 목록을 구함
            // GameObject[] objects = GameObject.FindGameObjectsWithTag("태그명");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // 적기가 없으면 패쓰
            if (enemies.Length <= 0) continue;

            // 타겟 가능 위치에 있는 대상 리스트
            List<GameObject> targetEnemyList = new List<GameObject>();

            // 미니 비행기보다 앞에 존재하는 대상체만 걸러냄
            foreach (GameObject enemy in enemies)
            {
                if ((transform.position.y + 2f) < enemy.transform.position.y)
                {
                    targetEnemyList.Add(enemy);
                }
            }

            if (targetEnemyList.Count <= 0) continue;

            Transform targetEnemy = null;
            float minDist = 0; // 최소 거리

            foreach (GameObject enemy in targetEnemyList)
            {
                // 두 위치간의 거리를 float값으로 구함
                // float 거리 = Vector2.Distance(위치1, 위치2);

                float dist = Vector2.Distance(transform.position,
                    enemy.transform.position);

                // 아직 지정된 타겟이 없으면
                if (targetEnemy == null)
                {
                    // 첫번째 대상을 타겟으로 설정함
                    targetEnemy = enemy.transform;
                    minDist = dist; // 최소값 설정
                    continue;
                }

                // 이전 짧은 거리보다 현재 적기가 더 가까이 있으면
                if (minDist > dist)
                {
                    // 현재 적기를 타겟으로 설정함
                    targetEnemy = enemy.transform;
                    minDist = dist;
                }
            }

            if (targetEnemy == null) continue;

            if (_laserPrefab == null) Debug.Log("laserprefab is null");

            // 레이저를 생성함
            GameObject laser = Instantiate(_laserPrefab,
                _shotPos.position, Quaternion.identity);

            if (laser == null) Debug.Log("laser is null");

            // 레이저 발포 방향을 현재 타겟 적을 향한 방향으로 설정함
            laser.GetComponent<CTargetMovement>().Init(targetEnemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "EnemyLaser")
        {
            Destroy(gameObject);
        }
    }
}