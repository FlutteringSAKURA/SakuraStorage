using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActionCtrl : MonoBehaviour
{
    public Animation _trapAnim;
    // public AnimationClip _trabAnimClip;
    // private PlayerCtrl _aboutPlayer;
    //GameObject _aboutPlayer;
    //  public float animEndTime = 3.5f;  // 종료 시간
    // public float animinitTime = 0.0f; // 시작 시간
    // public int animinitTime;
    // GameObject _spearBoobyTrap;
    public AnimationClip idleSpear;
    // public Anim anim;

    //private void Start()
    //{
    //    //  _aboutPlayer = GameObject.FindGameObjectWithTag("Player");
    //    //  _aboutPlayer = GetComponent<PlayerCtrl>();
    //    // _spearBoobyTrap = GameObject.FindGameObjectWithTag("GROUND");
    //    //  _trapAnim.clip = anim.idle;       
    //}

    //private void Update()
    //{
    //    animinitTime += Time.deltaTime; // 시작 시간 정규화     

    //}

    private void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("trab!!");

            StartCoroutine(ActionBoobyTrap());



            // _aboutPlayer.GetComponentInChildren<PlayerCtrl>().
            //  _aboutPlayer._isHit = true;
            //_aboutPlayer.hp -= 20;
        }
    }

    private IEnumerator ActionBoobyTrap()
    {
        yield return new WaitForSeconds(1.1f);

        // animinitTime = 0.0f;
        //Debug.Log("Restet Time : " + animinitTime);
        //_spearBoobyTrap.GetComponentInChildren<MeshRenderer>().enabled = true;
        //_spearBoobyTrap.GetComponentInChildren<MeshCollider>().enabled = true;

        _trapAnim.Play();

        //    animinitTime += Time.deltaTime;
        //    if (animinitTime > 5.0f) // 시작시간이 종료 시간을 넘으면
        //    {
        //        Debug.Log("Animation Stop call!!");
        //        _trapAnim.Stop(); // 에니메이션 종료
        //    }
    }


    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("Pass by PLAYER");

        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(StopSpearUpDown());
        }
    }

    private IEnumerator StopSpearUpDown()
    {
        yield return new WaitForSeconds(5.5f);
        // _trapAnim.Stop();
        //_spearBoobyTrap.GetComponentInChildren<MeshRenderer>().enabled = false;
        //_spearBoobyTrap.GetComponentInChildren<MeshCollider>().enabled = false;

        _trapAnim.Play("IdleSpear");

    }
    //public void OnAnimationStart()
    //{
    //      animinitTime= 0.0f;
    //}

    //public void OnAnimationEnd()
    //{
    //    if (animinitTime  > animEndTime) // 시작 시간이 종료 시간을 넘어서면
    //    {

    //        _trapAnim.Stop(); // 에니메이션 종료
    //    }
    //}
}
