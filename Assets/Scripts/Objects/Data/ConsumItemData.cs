using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Health,
    Buff
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ConsumItemData : BaseItemData
{

    [Header("Consumable")]
    public ConsumableType consumType;
    public int value;

    [Header("Equip")]
    public GameObject equipPrefab;

}
