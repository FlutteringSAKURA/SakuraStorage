using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpawnBall : MonoBehaviour
{

    public float timeBetweenPitches;
    public GameObject ball;
    public float launchAngle;
    public CSensorChecker sensorChecker;

    private void Start()
    {
        StartCoroutine(Pitch());
    }

    public IEnumerator Pitch()
    {
        while (true)
        {
            yield return new WaitUntil(() => sensorChecker._sensorChk);
            Vector3 launchDirection = GetLaunchDirection();
            Quaternion q = Quaternion.Euler(launchDirection);
            Instantiate(ball, transform.position, q);
            yield return new WaitUntil(() => !sensorChecker._sensorChk);
        }
    }
    private Vector3 GetLaunchDirection()
    {
        return new Vector3(
            Random.Range(-launchAngle, launchAngle), 180, Random.Range(0f, 180f));
    }
}
