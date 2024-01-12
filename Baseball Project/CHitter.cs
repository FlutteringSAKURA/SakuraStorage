using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitter : MonoBehaviour
{
    public GameObject _hitter;
    public GameObject _bat;
    public Transform _hand;
    public Transform _spawnPivotTr;
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _hitter.transform.eulerAngles = new Vector3(
                _hitter.transform.eulerAngles.x,
                _hitter.transform.eulerAngles.y - 90,
                _hitter.transform.eulerAngles.z);
        }
        if (Input.GetMouseButtonDown(1))
        {
            // GameObject tempParent = new GameObject("tempParent");
            // GameObject child = Instantiate(_bat) as GameObject;
            // child.transform.parent =  tempParent.transform;

            //_spawnPivotTr.transform.parent = _hitter.transform;
            //Instantiate(_bat, _spawnPivotTr.transform.position, Quaternion.identity);

            GameObject BAT = Instantiate(_bat, _spawnPivotTr.position, transform.rotation);
            BAT.transform.parent = _spawnPivotTr.transform;

        }
    }
}
