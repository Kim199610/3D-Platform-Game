using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI itemNameText;
    public GameObject selectedImg;
    [SerializeField] Image icon;
    public int amount { get; private set; }
        
    

    public BaseItemData baseItemData;

    private void Start()
    {
        updateSlot();
    }
    public int SetItem(BaseItemData inputBaseItemData,int inputAmount)
    {
        if(baseItemData == null)
        {
            baseItemData = inputBaseItemData;
        }
        if (inputBaseItemData != baseItemData)
        {
            return inputAmount;
        }
        if(amount + inputAmount > baseItemData.maxStackAmount)
        {
            amount = baseItemData.maxStackAmount;
            inputAmount = amount+inputAmount-baseItemData.maxStackAmount;
        }
        else if(amount + inputAmount < 0)
        {
            amount = 0;
            inputAmount =  inputAmount-amount;
        }
        else
        {
            amount = amount + inputAmount;
            inputAmount = 0;
        }
        updateSlot();
        return inputAmount;
    }

    void updateSlot()
    {
        if (baseItemData != null && amount != 0)
        {
            amountText.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
            icon.sprite = baseItemData.icon;
            amountText.text = amount.ToString();
            amountText.gameObject.SetActive(amount > 1);
        }
        else
        {
            baseItemData = null;
            amountText.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
        }
    }
    public void ConsumItem()
    {
        ConsumItemData consumItemData = (ConsumItemData)baseItemData;

        switch (consumItemData.consumType)
        {
            case ConsumableType.Health:
                {
                    PlayerCondition condition = GameManager.Instance.Player.condition;
                    condition.HealHealth(consumItemData.value);
                    break;
                }
            case ConsumableType.Buff:
                {
                    SkillManager.instance.ui_Buff.AddBuff(consumItemData.buffPrefab);
                    break;
                }
        }
        amount--;
        updateSlot();
    }

    public void OnSlotClick()
    {
        if(baseItemData == null)
        {
            UIManager.Instance.ui_Inventory.SetSelectItem(null);
        }
        else
        {
            UIManager.Instance.ui_Inventory.SetSelectItem(this);
        }
    }
    public void ActiveSelectImg(bool value)
    {
        selectedImg.SetActive(value);
    }

    public void ThrowItem(int value)
    {
        Player player = GameManager.Instance.Player;
        BaseItem dropItem = Instantiate(baseItemData.dropPrefab, player.transform.position + player.transform.forward + Vector3.up, Quaternion.identity).GetComponent<BaseItem>();
        dropItem.amount = value;
        amount -= value;
        updateSlot();
    }
    public void RemoveItem(int value)
    {
        amount -= value;
        updateSlot();
    }
}
