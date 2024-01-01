using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    public GameObject sparkEffect;
    public GameObject fireEffect;
    // 드럼통 폭발 사운드 오디오 인스턴스 참조 선언
    public AudioClip expSfx;
    // 드럼통의 랜덤 텍스쳐 구현을 위한 선언
    public Texture[] textures;

    // [ TEST CODE ] 
    public bool _isBurning = false;
    // ============================= test code end

    // 드럼통의 Transform 위치 선언
    private Transform tr;
    private int hitCount = 0;
    // [ 배럴 코드 ] RigidBody 사용을 위한 변수 선언
    private Rigidbody rbody;

    // 오디오 소스 초기화 -> [ 공용함수 코드 ] 처리 할 경우 주석 처리
    // private AudioSource source = null;

    // [ Test Code ] -> 파워
    public float forceToBarrel;
    
    private void Start()
    {
        tr = GetComponent<Transform>();
        int idx = Random.Range(0, textures.Length - 1);
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];

        // 오디오 소스 캐시 처리 -> [ 공용함수 코드 ] 처리 할 경우 주석 처리
        // source = GetComponent<AudioSource>();

        // [ 배럴 코드 ] -> 캐시처리
        rbody = GetComponent<Rigidbody>();
        // =================================== 배럴 코드 End
    }
    
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "BULLET")
        {
            // 불꽃을 collision 위치에 생성 
            GameObject spark = (GameObject)Instantiate(sparkEffect, collision.transform.position, Quaternion.identity);
            // 4.5 초후 불꽃 파티클 이펙트 파괴
            Destroy(spark, 4.5f);
            Destroy(collision.gameObject);
            
            // 3발을 쏘면 
            if (++hitCount >= 3)
            // >> 동일 코드 >> if( hitCount++ == 2 ) 
            {
                // 드럼통을 폭파해라
                ExpBarrel();

                // [ TEST CODE ]
                _isBurning = true;
                // ====================== test code end
            }
        }
        
    }
    private void ExpBarrel()
    {
        // 폭발하면 텍스쳐 배열의 마지막 텍스쳐로 렌더링
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[textures.Length - 1];
        // Explosion Effect 생성
        GameObject exp = (GameObject)Instantiate(expEffect, tr.position, Quaternion.identity);
        // Fire Effect 생성
        GameObject fire = (GameObject)Instantiate(fireEffect, tr.position, Quaternion.identity);

        // 폭발 사운드 100의 볼륨으로 구현 -> [ 공용함수 코드 ] 처리 할 경우 주석 처리
        // source.PlayOneShot(expSfx, 1.0f);
        GameMgr.instance.PlaySfx(tr.position, expSfx);
        // ============================================= 공용함수 코드 End

        // Fire Effect의 부모위치를 드럼통으로 지정
        fire.transform.parent = tr;
        // 해당 시간 뒤에 각각의 Effect들을 파괴해라
        Destroy(exp, 4.5f);
        Destroy(fire, 8.0f);

        // 폭발력이 반경 10미터 안에 미침
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);
        foreach (Collider coll in colls)
        {
            // [ TEST CODE ]
            _isBurning = true;
            // ====================== test code end

            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            // Rigidbody 유무 체크
            // Rigidbody가 있다면
            if (rbody != null)
            {               
                // Mass값을 해당 수치로 변환;
                rbody.mass = 1.65f;
                rbody.AddExplosionForce(35.0f, tr.position, 8.0f, 500.0f);
                // [ 봄버 코드 ]
                coll.gameObject.SendMessage("OnExpDamage", SendMessageOptions.DontRequireReceiver);
                // =================================================================================== 봄버 코드 END             
            }
            Destroy(gameObject, 8.00f);
        }
    }
    
    // [ 배럴 코드 ]
    private void OnDamage(object[] _params)
    {
        Vector3 hitPos = (Vector3) _params[0];
        Vector3 firePos = (Vector3) _params[1];
        Vector3 incomeVector = hitPos - firePos; // 방향과 길이(힘)

        GameObject spark = (GameObject)Instantiate(sparkEffect, hitPos, Quaternion.identity);
        Destroy(spark, 5.0f);

       rbody.AddForceAtPosition(incomeVector.normalized * forceToBarrel, hitPos);

        if (++hitCount >= 3)
        {
            ExpBarrel();
        }
    }
 // ======================================== 배럴 코드 End 
 
    // [ 봄버 코드 ]
    private void OnExpDamage()
    { 
        // 불 옮겨 붙는 효과
        GameObject fire = (GameObject)Instantiate(fireEffect, tr.position, Quaternion.identity);
        fire.transform.parent = tr;
        Destroy(fire, 7.9f);
        StartCoroutine(DelayExp());
    }

    private IEnumerator DelayExp()
    {
        // 5초 뒤에 폭발 시켜라
        yield return new WaitForSeconds(5.0f);
        ExpBarrel();
    }
    // =============================== 봄버 코드 END
}
