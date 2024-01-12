using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuzzySample : MonoBehaviour
{

    private const string labelText = "{0} true";
    public AnimationCurve critical;
    public AnimationCurve hurt;
    public AnimationCurve healthy;

    public InputField healthInput;

    public Text healthyLabel;
    public Text hurtLabel;
    public Text criticalLabel;

    private float criticalValue = 0f;
    private float hurtValue = 0f;
    private float healthyValue = 0f;

    private void Start()
    {
        SetLabels();
    }
    //* 모든 커브를 평가하고 부동소수점 값을 반환한다

    //# 입력된 문자열에 대한 간단한 Null 체크를 수행
    //# -> 빈 문자열을 파싱하려는 시도를 방지하기 위함(유효성 검사는 수행하지 않으므로 반드시 숫자를 입력)
    //# -> 숫자가 아닌 값을 입력하면 에러가 발생

    //# 각 AnimationCurve 변수는 Evaluate(float t)메소드를 호출
    //# -> 이 때 t는 입력 필드에서 얻은 값으로 교체 -> 그러면 다시 라벨을 가져온 값으로 갱신 
    public void EvaluateStatements()
    {
        if (string.IsNullOrEmpty(healthInput.text))
        {
            return;
        }
        float inputValue = float.Parse(healthInput.text);

        healthyValue = healthy.Evaluate(inputValue);
        hurtValue = hurt.Evaluate(inputValue);
        criticalValue = critical.Evaluate(inputValue);

        SetLabels();
    }

    //* 사용자가 입력한 체력 %에 기반해 평가된 값으로 GUI를 갱신
	//* -> 각 라벨을 가져와서 형식에 맞춘 형태로 labelText의 값을 교체
	//* {0}을 실제 값으로 변경
    private void SetLabels()
    {
        healthyLabel.text = string.Format(labelText, healthyValue);
        hurtLabel.text = string.Format(labelText, hurtValue);
        criticalLabel.text = string.Format(labelText, criticalValue);
    }
}
