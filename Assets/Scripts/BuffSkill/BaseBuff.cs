using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BuffName
{
    SpeedUp
}

public abstract class BaseBuff : MonoBehaviour
{
    [SerializeField] protected Image curImag;
    [SerializeField] protected float increasSpeed;
    [SerializeField] protected float duration;
    protected float startTime {  get; private set; }
    protected virtual void Start()
    {
        //StartCoroutine(BuffCorutine());
        startTime = 0f;
        BuffStart();
    }
    protected virtual void Update()
    {
        startTime += Time.deltaTime;
        if (startTime >= duration)
        {
            BuffEnd();
            Destroy(this.gameObject);
        }
    }
    protected virtual void LateUpdate()
    {
        UpdateImg();
    }
    void UpdateImg()
    {
        curImag.fillAmount = 1 - ((startTime) / duration);
    }
    //protected virtual IEnumerator BuffCorutine()                                  코루틴 사용시 코드
    //{
    //    startTime = Time.time;
    //    BuffStart();
    //    yield return new WaitUntil(() => (Time.time - startTime >= duration));
    //    BuffEnd();
    //    Destroy(this.gameObject);
    //}
    public virtual void RenewalBuff()
    {
        startTime = 0f;
        //StopCoroutine(BuffCorutine());
        //StartCoroutine(BuffCorutine());
    }
    public abstract BuffName GetBuffName();

    protected abstract void BuffStart();
    protected abstract void BuffEnd();
}
