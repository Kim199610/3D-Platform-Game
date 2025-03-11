using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public BaseItemData baseItemData;
    public bool indicateInfoBool;
    protected bool indicateInfoPreBool;
    public bool showDescription;
    protected bool showdescriptionPre;
    public int amount;

    protected ObjectUI objectUI;

    protected virtual void Start()
    {
        indicateInfoBool = false;
        indicateInfoPreBool = indicateInfoBool;
        showDescription = false;
        showdescriptionPre = showDescription;
        amount = 1;
        gameObject.layer = LayerMask.NameToLayer("Object");
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
    public void InteractItem()
    {
        if(baseItemData.type == ItemType.interactableObject)
        {
            //상호작용 기능 호출
        }
        else
        {
            amount = UIManager.Instance.ui_Inventory.SetItemToInventory(baseItemData,amount);
            if (amount == 0)
            {
                DestroyItem();
            }
        }
    }
    void DestroyItem()
    {
        Destroy(objectUI.gameObject);
        Destroy(this.gameObject);
    }
}
