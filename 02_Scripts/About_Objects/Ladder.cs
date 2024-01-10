using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Update: //@ 2024.01.04 
//# NOTE: The Dark Forest Scene의 사다리를 관리하기 위한 스크립트

//~ -------- PROJECT : Bramble-The Kong And Pat ----------------------------------------
public class Ladder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("사다리를 타고 올라갈 때 팥쥐 캐릭터의 움직임 보정을 위한 경계라인 구현")]
    private GameObject _lineQuad_Group;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PatJi"))
        {
            _lineQuad_Group.SetActive(true);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("PatJi"))
        {
            _lineQuad_Group.SetActive(false);
        }
    }
}
