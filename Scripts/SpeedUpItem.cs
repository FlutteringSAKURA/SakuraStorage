using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpItem : MonoBehaviour
{
    private GameObject _playerObject;
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;
    private GameObject _rootingBar;
    // private Canvas _rootingBar;
    public Image imgRootingBar;
    // [ TEST CODE ]  for Item Time Viewer
   // public Text _itemTimer;
   // private int _initialItemTimer = 5;
    public bool _getItem = false;
    public Canvas _itemTimer;
    // ========================== test code end


    public float rTime = 0.0f;
    private void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
        //      _rootingBar = GetComponent<Canvas>();
        _rootingBar = GameObject.FindGameObjectWithTag("ROOTINGBAR");
    }
    private void Update()
    {
      
        if (Input.GetKey(KeyCode.R))
        {
            // 만약 플레이어가 움직인다면
            if (_playerObject.GetComponentInChildren<PlayerCtrl>()._isMove)
            {
                rTime = 0.0f; // 루팅타임 초기화
            }
            rTime += Time.deltaTime; // 루팅타임 정규화
            imgRootingBar.fillAmount = rTime; // 루팅바의 fillAmount값과 루팅타임으로 치환

        }
        //else if (_getItem)
        //{
        //    _itemTimer.GetComponentInChildren<Text>().enabled = true;
        //    DispItemTimer(5);
        //}
        else if (Input.GetKeyUp(KeyCode.R))
        {
            rTime = 0.0f; // 루팅타임 초기화
            imgRootingBar.fillAmount = 0.0f; // 루팅바 값 초기화
        }
    }



    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "HAND")
        {
            rTime = 0.0f; // 루팅타임 초기화
            _rootingBar.GetComponentInChildren<Canvas>().enabled = true; // 루팅바 생성

            if (imgRootingBar.fillAmount == 1.0f)
            {

                // [ TEST CODE ]  for Item Time Viewer

                //  rTime += Time.deltaTime; // 루팅바 값 증가
                // imgRootingBar.fillAmount = rTime;
                // imgRootingBar.fillAmount = rTime;
                //  rBar +=  Time.deltaTime;
                _getItem = true; // 아이템 획득
                _itemTimer.gameObject.SetActive(true); // Timer Canvas Object를 활성화
              //_itemTimer.GetComponentInChildren<Canvas>().enabled = true; // Timer Canvas 활성화
                // ============================================== test code end
               
                _meshRenderer.enabled = false;
                _boxCollider.enabled = false;
                _rootingBar.GetComponentInChildren<Canvas>().enabled = false;
                _playerObject.GetComponentInChildren<PlayerCtrl>().moveSpeed = 25.0f; // 플레이어의 속도를 증가
                StartCoroutine(OriginSpeed());

            }
        }
    }

    // TEST CODE for Item Timer
    //public void DispItemTimer(int Timer)
    //{
    //    _initialItemTimer -= Timer;
    //  //  _itemTimer.text = "TIMER < color = Yellow > " + _initialItemTimer.ToString() + " </ color > ";
  

    //}

    private void OnTriggerExit(Collider collision)
    {

        if (collision.tag == "HAND")
        {
            Debug.Log("EXIT HAND");
            // rTime = 0.0f;

            _rootingBar.GetComponentInChildren<Canvas>().enabled = false;

        }
    }

    private IEnumerator OriginSpeed()
    {
        _rootingBar.GetComponentInChildren<Canvas>().enabled = false;
        yield return new WaitForSeconds(5.0f);
        _playerObject.GetComponentInChildren<PlayerCtrl>().moveSpeed = 7.0f;
        Destroy(gameObject, 0.1f);
    }
}
