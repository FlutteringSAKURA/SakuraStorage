using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CBaseBall : MonoBehaviour
{
    public AudioSource src;
    public TrailRenderer t;
    private Rigidbody rbody;
    private float velocityMax = 200f;
    public GameObject Calc;
    public Transform _batTr;
    public float _calcAngle;
    public bool _groundChk = false;
    public bool _foulChk = false;
    public bool _hitBatChk = false;
    // public bool _foulChk = false;

    private Transform _baseBallTr;

    // public static BaseBall instance = null;

    // public Text _distText;

    // private SphereCollider _sColider;
    // private MeshCollider _mCollider;

    //* > > > >  TEST 
    private float _timeFlew;



    private void Awake()
    {
        // if (instance == null)
        // {
        //     instance = this;
        // }
        // else if (instance != this)
        // {
        //     Destroy(gameObject);
        // }

        //! > > > TEST start - about Pitcher Bot Ball Generate.                

        // rbody = gameObject.GetComponent<Rigidbody>();
        // rbody.AddForce(transform.forward * Random.Range(4f, 6f), ForceMode.Impulse);
        
        //! < < < < End line
        //  _sColider = GetComponent<SphereCollider>();
        //  _mCollider = GetComponent<MeshCollider>();
    }

    //    public static float CalculateAngle(Vector3 _batTr, Vector3 _baseBallTr)
    // {
    //     return Quaternion.FromToRotation(Vector3.up, _batTr - _baseBallTr).eulerAngles.z;       
    // }

    // private void Update() {
    //     _calcAngle = Vector3.Angle (transform.position, _batTr.position);
    //     Debug.Log("Angle : " + _calcAngle);
    // }
    private void FixedUpdate()
    {

        //! > > > > TEST
        if (!_groundChk){
            _timeFlew += Time.deltaTime;
        }
        //! < < < < < < 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bat")
        {
            //rbody.mass = 0.125f;
            //StartCoroutine(RecoveryBatMass());
            _hitBatChk = true;
            rbody.velocity = Vector3.zero;
            src.Play();
            Physics.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider>(), gameObject.GetComponent<SphereCollider>());

            float forceMultiplier = GetBatForce(collision.gameObject.GetComponent<Rigidbody>());
            Vector3 direction = (transform.position - collision.contacts[0].point).normalized;
            rbody.AddForce(direction * forceMultiplier, ForceMode.Impulse);
            rbody.useGravity = true;

            t.enabled = true;
            Destroy(gameObject, 4f);
        }

        //* For Random hit Simulate (Input Key down -> SPACE or Tap)
		if (collision.gameObject.tag == "Ground" && !_groundChk)
        {
            _groundChk = true;
			CCanvasBoard.instance.actualTimeText.text = _timeFlew.ToString();
			CCanvasBoard.instance.actualDistText.text = Vector3.Distance (Vector3.zero, transform.position).ToString();
			Destroy(gameObject, 3f);
            StartCoroutine(ChangeTag()); // For Foul Chk;
        }
        

        //!  For Random hit Simulate (Input key down -> Tap)
        if (collision.gameObject.tag == "FoulZone" & !_groundChk)
        {
            Destroy(gameObject, 3f);
        }
    }

    private IEnumerator ChangeTag()
    {
        yield return null;
        gameObject.tag = "Untagged";
    }

    // private IEnumerator RecoveryBatMass()
    // {
    //     yield return new WaitForSeconds(0.05f);		
    // 		rbody.mass = 200f;
    // }

    private void CalcDist() //* Send Massage from Reciever Script
    {
        if (_hitBatChk == true)
        {
            float dist = Vector3.Distance(transform.position, _batTr.position);
            Debug.Log("RESULT : " + dist);
            CDistanceManager.instance.UpdateDistance(dist);
        }
    }


    private float GetBatForce(Rigidbody batRbody)
    {
        return batRbody.velocity.magnitude / velocityMax * 50f;
    }

    // public void BaseBallScript(GameObject BaseBall)
    // {
    //     GetComponent<BaseBall>();
    // }
}
