using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 컴포넌트 사용
using UnityEngine.UI;

// 씬 관리 사용
using UnityEngine.SceneManagement;


public class CGameController : MonoBehaviour
{

    // 게임 종료 여부
    public static bool IsGameStop = false;

    public Animator _backgroundAnimator;

    Text _flowerCountText; // 꽃점수 출력

    private void Start()
    {
        // GameObject 찾는 오브젝트 = GameoObject.Find("오브젝트이름");
        GameObject findObject = GameObject.Find("FlowerScoreText");
        _flowerCountText = findObject.GetComponent<Text>();
    }

    private void Update()
    {
        if (IsGameStop)
        {
            // 배경 스크롤 애니메이션의 속도를 0으로 (애니메이션 중지)
            _backgroundAnimator.speed = 0f;
        }
    }

    public void FlowerCountUp()
    {
        // int.Parse("숫자문자열") -> 숫자문자열을 정수로 변환
        int count = int.Parse(_flowerCountText.text);
        count++;
        // int.Tostring() -> 정수를 문자열로 변환
        _flowerCountText.text = count.ToString();
    }

    public void GameEnd(float endTime)
    {
        // 지연 시간 뒤에 씬을 전환함
        StartCoroutine("GameEndCoroutine", endTime);
    }
    IEnumerator GameEndCoroutine(float endTime)
    {
        yield return new WaitForSeconds(endTime);
        // 종료 씬으로 이동함
        SceneManager.LoadScene("GameEnd");
    }
}
