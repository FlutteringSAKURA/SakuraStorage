using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.11.30 
//# NOTE: 양초 불꽃 이펙트 효과 위한 스크립트


//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------

public class CandleFire : MonoBehaviour
{
    public Light _candleFireFlame;

    public float _timeFlow = 0.0f;
    public float _fireIntensity = 0.5f;


    private void Update()
    {
        // _timeFlow += Time.deltaTime;

        // if (_timeFlow <= 2.0f)
        // {
        //     _candleFireFlame.GetComponent<Light>().intensity += _timeFlow / 2;

        //     if (_fireIntensity > 1.5f)
        //     {

        //         _fireIntensity = 0.5f;

        //         _timeFlow = 0.0f;

        //     }
        // }




        // StartCoroutine(FlickerFlame());




    }

    IEnumerator FlickerFlame()
    {
        _fireIntensity = Random.Range(0.2f, 0.7f);
        _candleFireFlame.GetComponent<Light>().intensity += _fireIntensity;
        yield return new WaitForSeconds(0.5f);

        _candleFireFlame.GetComponent<Light>().intensity = 1.0f;
        yield return new WaitForSeconds(0.5f);

        _fireIntensity = Random.Range(0.2f, 0.7f);
        _candleFireFlame.GetComponent<Light>().intensity += _fireIntensity;
        yield return new WaitForSeconds(0.5f);

        _candleFireFlame.GetComponent<Light>().intensity = 1.0f;
        yield return new WaitForSeconds(0.5f);


    }

    public void NoShadow()
    {
        _candleFireFlame.shadows = LightShadows.None;
    }
}
