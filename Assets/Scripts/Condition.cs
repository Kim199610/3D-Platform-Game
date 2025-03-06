using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float PassiveValue;
    [SerializeField] Image uiCurValueBar;
    [SerializeField] Image warningImg;

    private Tween warningTween;
    private void Start()
    {
        curValue = startValue;
    }
    private void Update()
    {
        if (uiCurValueBar != null)
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

    public void StartWarning()
    {
        if (warningImg!=null && warningTween == null)
        {
            warningTween = warningImg.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }
    public void StopWarning()
    {
        if (warningImg!=null && warningTween != null)
        {
            warningTween.Kill();
            warningTween = null;
            warningImg.color = Color.clear; // √ ±‚»≠
        }
    }
}
