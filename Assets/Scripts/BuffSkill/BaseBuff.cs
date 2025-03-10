using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBuff : MonoBehaviour
{
    [SerializeField] protected Image curImag;
    [SerializeField] protected float increasSpeed;
    [SerializeField] protected float duration;
    protected float startTime;

    protected virtual void Start()
    {
        StartCoroutine(BuffCorutine());
        //BuffStart();              코루틴사용으로 필요없음
    }
    protected virtual void LateUpdate()
    {
        UpdateImg();
    }
    void UpdateImg()
    {
        //if(Time.time - startTime >= duration)                 코루틴 사용으로 필요없음
        //{
        //    BuffEnd();
        //}
        curImag.fillAmount = 1 - ((Time.time - startTime) / duration);
    }
    //void BuffStart()                                  코루틴 사용으로 필요없음
    //{
    //    startTime = Time.time;
    //    playerController.moveSpeed += increasSpeed;
    //}
    //void BuffEnd()
    //{
    //    playerController.moveSpeed -= increasSpeed;
    //    Destroy(this.gameObject);
    //}
    protected virtual IEnumerator BuffCorutine()
    {
        startTime = Time.time;
        BuffStart();
        yield return new WaitUntil(() => (Time.time - startTime >= duration));
        BuffEnd();
        Destroy(this.gameObject);
    }
    public virtual void RenewalBuff()
    {
        startTime = Time.time;
    }

    protected abstract void BuffStart();
    protected abstract void BuffEnd();
}
