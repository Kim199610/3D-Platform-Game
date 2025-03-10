using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory_ItemSlots : MonoBehaviour
{
    [SerializeField]Slot[] inventorySlots;

    private void Awake()
    {
        inventorySlots = GetComponentsInChildren<Slot>(true);
    }

    public int SetItemToInventory(BaseItemData baseItemData,int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].baseItemData == baseItemData)
            {
                amount = inventorySlots[i].SetItem(baseItemData, amount);
                if (amount == 0)
                {
                    return 0;
                }
            }
        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            amount = inventorySlots[i].SetItem(baseItemData, amount);
            if (amount == 0)
            {
                return 0;
            }
        }
        //�κ��丮 â�� �����ϴٴ� �˸�
        return amount;
    }
}
