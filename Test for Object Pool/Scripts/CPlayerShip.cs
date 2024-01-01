using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerShip : MonoBehaviour
{
    public GameObject _laserPrefab;

    public float _shotDelay;

    public Transform[] _shotPositions;

    private void Start()
    {
        StartCoroutine("Shot");
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");

        transform.Translate(Vector2.right * h * 5f * Time.deltaTime);
    }

    IEnumerator Shot()
    {
        while (true)
        {
            for (int i = 0; i < _shotPositions.Length; i++)
            {
                // 오브젝트 풀에서 오브젝트 (레이저)를 뺴냄
                GameObject obj = CObjectPool.current.GetObject(_laserPrefab);
                // 위치와 회전을 설정함
                obj.transform.position = _shotPositions[i].position;
                obj.transform.rotation = _shotPositions[i].rotation;
                // 오브젝트를 활성화함
                obj.SetActive(true);
            }
            yield return new WaitForSeconds(_shotDelay);
        }
    }

}
