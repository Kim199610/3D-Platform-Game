using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float PassiveValue;
    public Image uiCurValueBar;

    private void Start()
    {
        curValue = startValue;
    }
    private void Update()
    {
        uiCurValueBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }
    public void ChangePassiveValue(float value)
    {
        PassiveValue = value;
    }

    public void ChangeValue(float value)
    {
        curValue = Mathf.Clamp(curValue + value, 0, maxValue);
    }
}
