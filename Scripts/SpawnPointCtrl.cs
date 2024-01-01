using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCtrl : MonoBehaviour
{
    public Transform[] _points;
    public GameObject _monsterPrefab;
    public List<GameObject> _monsterPool = new List<GameObject>();

    public float _createTime = 0.3f;
    public int _maxMonster = 200;
    public bool _isGameOver = false;

    private void Start()
    {
        // DropPoin명칭의 오브젝트를 찾아서 그 하위 오브젝트들의 Transform을 가져옴
        _points = GameObject.Find("DropPoint").GetComponentsInChildren<Transform>();
        for (int i = 0; i < _maxMonster; i++)
        {
            GameObject _monster = (GameObject)Instantiate(_monsterPrefab);
            _monster.name = "HellMonster_" + i.ToString();
            _monster.SetActive(false); // 몬스터 비활성화
            _monsterPool.Add(_monster);
        }

        if(_points.Length > 0)
        {
            StartCoroutine(this.MakeMonster());
        }
    }

    private IEnumerator MakeMonster()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(_createTime);
            if (_isGameOver) yield break;
            foreach (GameObject _monster in _monsterPool)
            {
                if (!_monster.activeSelf)
                {
                    int _idx = Random.Range(1, _points.Length);
                    _monster.transform.position = _points[_idx].position;
                    _monster.SetActive(true); // 몬스터 활성화
                    break;
                }
            }           
        }           
    }
}
