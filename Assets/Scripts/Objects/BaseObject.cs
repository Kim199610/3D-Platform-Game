using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public BaseObjData baseObjData;
    public bool indicateInfoBool;
    protected bool indicateInfoPreBool;
    public bool showDescription;
    protected bool showdescriptionPre;

    protected ObjectUI objectUI;

    protected virtual void Start()
    {
        indicateInfoBool = false;
        indicateInfoPreBool = indicateInfoBool;
        showDescription = false;
        showdescriptionPre = showDescription;
    }
    protected virtual void Update()
    {
        if (indicateInfoPreBool != indicateInfoBool)
        {
            indicateInfoPreBool = indicateInfoBool;
            IndicateInfo(indicateInfoBool);
        }
        if(showdescriptionPre != showDescription)
        {
            showdescriptionPre = showDescription;
            objectUI?.descriptionBoxRectTransform.gameObject.SetActive(showDescription);
        }
    }
    public virtual void IndicateInfo(bool active)
    {
        if (objectUI == null)
        {
            objectUI = Instantiate(UIManager.Instance.objectUI, UIManager.Instance.objectUIs).GetComponent<ObjectUI>();
            objectUI.targetObj = this.transform;
            objectUI.Init();
        }
        objectUI.gameObject.SetActive(active);
    }
}
