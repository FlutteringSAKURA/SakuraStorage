using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCalculator : CBat
{
    [Header("This Script is Generate Ball For Calculator Some Data")]

    // private Rigidbody rbody;
    public Transform _baseBall;
    public Transform _bat;
    public float _generatingAngle;
    public float _setAngle_x;
    public float _setAngle_y;
    public float _setAngle_z;

    //public Quaternion rotation = Quaternion.Euler(Random.Range(-180f,180f),0,Random.Range(-180f,180f));
    [Header("> > > FOR TEST ")]
    //* TEST
    public GameObject _calcBaseBall;
    public float minHAngle;
    public float maxHAngle;
    public float minVAngle;
    public float maxVAngle;
    public float minVelocity;
    public float maxVelocity;

    private float _hAngle;
    private float _vAngle;
    private float _dirX;
    private float _dirY;
    private float _dirZ;

    private float _baseBallVelocity;
    private Vector3 _dir;

	private float _predictTime;
	private float _predictDistance;

    private void Awake()
    {
        //rbody = gameObject.GetComponent<Rigidbody>();		
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Quaternion setAngle = new Quaternion(Random.Range(-_setAngle_x, _setAngle_x), Random.Range(-_setAngle_y, _setAngle_y), 180, 0.1f);
            Instantiate(_baseBall, transform.position, setAngle);

            //Vector3 position = new Vector3(
            //Random.Range(-_generatingAngle, _generatingAngle), Random.Range(-_generatingAngle,_generatingAngle), Random.Range(-_generatingAngle,_generatingAngle));
            //*	StartCoroutine(GenerateBall());

            //float distance = Vector3.Distance(_bat.transform.position, _baseBall.transform.position);		                    

        }

        //! > > > > > > > TEST start
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LaunchBaseBall();
        }
    }

    private void LaunchBaseBall()
    {
        //Quaternion setAngle = new Quaternion(Random.Range(-_setAngle_x, _setAngle_x), Random.Range(-_setAngle_y, _setAngle_y), 180, 0.1f);        
        // Instantiate(_calcBaseBall, transform.position, setAngle);
        _dir = Vector3.forward;
        GameObject temp = Instantiate(_calcBaseBall, transform.position, Quaternion.identity);

        _hAngle = Random.Range(minHAngle, maxHAngle);
        _vAngle = Random.Range(minVAngle, maxVAngle);
        _baseBallVelocity = Random.Range(minVelocity, maxVelocity);

        _dirX = Mathf.Sin(_hAngle * Mathf.Deg2Rad) * Mathf.Cos(_vAngle * Mathf.Deg2Rad);
        _dirZ = Mathf.Cos(_hAngle * Mathf.Deg2Rad) * Mathf.Cos(_vAngle * Mathf.Deg2Rad);
        _dirY = Mathf.Sin(_vAngle * Mathf.Deg2Rad);

        _dir = Vector3.forward * _dirZ + Vector3.up * _dirY + Vector3.right * _dirX;
        _dir *= _baseBallVelocity;

        UpdateDirCanvas();
        temp.GetComponent<Rigidbody>().velocity = _dir;                

    }

    private void UpdateDirCanvas()
    {
        CCanvasBoard.instance.vDirText.text = _vAngle.ToString();
        CCanvasBoard.instance.hDirText.text = _hAngle.ToString();
        CCanvasBoard.instance.speedText.text = _baseBallVelocity.ToString();
        
		_predictTime = (-_dir.y - Mathf.Sqrt (_dir.y * _dir.y + 2 * 9.81f * transform.position.y)) / -9.81f;
		_predictDistance = (_dir - _dir.y * Vector3.up).magnitude * _predictTime;

		CCanvasBoard.instance.predictTimeText.text = _predictTime.ToString ();
		CCanvasBoard.instance.predictDistText.text = _predictDistance.ToString();

    }

    //! < < < < < < < < TEST END 

    private IEnumerator GenerateBall()
    {
        // while (true)
        // {
        yield return new WaitForSeconds(0.01f);
        Vector3 launchDirection = GetLaunchDirection();
        Quaternion q = Quaternion.Euler(launchDirection);

        Instantiate(_baseBall, transform.position, q);
        //}
    }
    private Vector3 GetLaunchDirection()
    {
        return new Vector3(
            Random.Range(-_generatingAngle, _generatingAngle), 180, Random.Range(0f, 360f));
    }

    // private void OnCollisionEnter(Collision collision) {
    // 	if(collision.gameObject.tag == "BaseBall")
    // 	{
    // 		float distance = Vector3.Distance(_bat.transform.position, _baseBall.transform.position);
    // 		//Debug.Log ("RESULT : " + distance);
    // 	}
    // }

    // private void CalcDist()
    // {
    //     float dist = Vector3.Distance(_baseBall.position, _bat.position);
    //     Debug.Log("RESULT : " + dist);
    // }
}
