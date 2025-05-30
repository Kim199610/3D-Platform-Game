using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public UI_Inventory_ItemSlots itemSlots;
    public Slot selectItemSlot;
    [SerializeField] TextMeshProUGUI selectItemName;
    [SerializeField] TextMeshProUGUI selectItemDescription;
    [SerializeField] GameObject equipButton;
    [SerializeField] GameObject consumButton;
    [SerializeField] GameObject throwButton;
    [SerializeField] GameObject removeButton;
    [SerializeField] GameObject amountSliderUI;
    [SerializeField] Slider amountSlider;
    [SerializeField] TextMeshProUGUI sliderNumTxt;

    bool isRemove;

    private void Awake()
    {
        amountSlider = amountSliderUI.GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        amountSlider.onValueChanged.AddListener(UpdateSliderValue);
    }
    private void OnEnable()
    {
        selectItemSlot?.ActiveSelectImg(false);
        selectItemSlot = null;
        selectItemName.gameObject.SetActive(false);
        selectItemDescription.gameObject.SetActive(false);
        amountSliderUI.SetActive(false);
    }
    public int SetItemToInventory(BaseItemData baseItemData,int amount)
    {
        return itemSlots.SetItemToInventory(baseItemData, amount);
    }
    public void SetSelectItem(Slot slot)
    {
        if(selectItemSlot == slot)
            return;
        amountSliderUI.SetActive(false);
        selectItemSlot?.ActiveSelectImg(false);
        selectItemSlot = slot;
        if (selectItemSlot == null)
        {
            throwButton.SetActive(false);
            consumButton.SetActive(false);
            removeButton.SetActive(false);
            equipButton.SetActive(false);
            selectItemName.gameObject.SetActive(false);
            selectItemDescription.gameObject.SetActive(false);
            return;
        }
        selectItemSlot.ActiveSelectImg(true);
        BaseItemData baseItemData = slot.baseItemData;
        selectItemName.text = baseItemData.name;
        selectItemDescription.text = baseItemData.description;
        selectItemName.gameObject.SetActive(true);
        selectItemDescription.gameObject.SetActive(true);
        throwButton.SetActive(true);
        removeButton.SetActive(true);
        consumButton.SetActive(baseItemData.type == ItemType.Consumable);
        equipButton.SetActive(baseItemData.type == ItemType.Equipalbe);

    }
    public void OnClickConsumButton()
    {
        selectItemSlot.ConsumItem();
    }
    public void OnClickEquipButton()
    {

    }
    public void OnClickThrowButton()
    {
        if (selectItemSlot.amount > 1)
        {
            isRemove = false;
            amountSlider.maxValue = selectItemSlot.amount;
            amountSliderUI.SetActive(true);
        }
        else
        {
            ThrowItem(1);
        }
    }
    public void ThrowItem(int amount)
    {
        selectItemSlot.ThrowItem(amount);
    }
    public void OnClickRemoveButton()
    {
        if(selectItemSlot.amount > 1)
        {
            isRemove = true;
            amountSlider.maxValue = selectItemSlot.amount;
            amountSliderUI.SetActive(true);
        }
        else
        {
            RemoveItem(1);
        }
    }
    public void RemoveItem(int amount)
    {
        selectItemSlot.RemoveItem(amount);
    }
    public void OnComfirmButton()
    {
        if (isRemove)
        {
            RemoveItem((int)amountSlider.value);
        }
        else
        {
            ThrowItem((int)amountSlider.value);
        }
        amountSliderUI.SetActive(false);
    }
    void UpdateSliderValue(float value)
    {
        sliderNumTxt.text = ((int)value).ToString();
    }
}
