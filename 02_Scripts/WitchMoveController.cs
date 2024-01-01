using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2023.09.21
// Update: //@ 2023.09.22

// NOTE:  //# 캐릭터 컨트롤러
//#          1) 캐릭터 좌우 이동 구현
//#          2) 
//#          3) 
//#          4) 
//#          5)

public class WitchMoveController : MonoBehaviour
{
    Rigidbody2D witchRigidBody;
    float flyingSpeed = 5.0f;

    private void Start()
    {

    }

    private void Update()
    {
        FlyingControll();
    }

    void FlyingControll()
    {
        float flyingHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * flyingHorizontal * flyingSpeed * Time.deltaTime);

        float flyingVertical = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * flyingVertical * flyingSpeed * Time.deltaTime);

        float posX = transform.position.x;
        float posY = transform.position.y;

        posX = Mathf.Clamp(posX, -5.0f, 5.0f);
        posY = Mathf.Clamp(posY, -4.5f, 4.5f);
        transform.position = new Vector3(posX, posY, -1);

        /*
            if (posX > 5.0f)
            {
                transform.position = new Vector3(5.0f, posY, -1);   //오른쪽이동
                print("오른쪽");
            }
            if (posX < -5.0f)
            {
                transform.position = new Vector3(-5.0f, posY, -1); //왼쪽이동
            }
            if (posY > 4.5f)
            {
                transform.position = new Vector3(posX, 4.5f, -1);
                print("위로");
            }
            if (posY < -4.5f)
            {
                transform.position = new Vector3(posX, -4.5f, -1);
            }
        */
    }
}
