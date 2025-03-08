using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Equipalbe,
    Consumable,
    Resouce
}
public enum ConsumableType
{
    Health,
    Buff
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : BaseObjData
{
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
