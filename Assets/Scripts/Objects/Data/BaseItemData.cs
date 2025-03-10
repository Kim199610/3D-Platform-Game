using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ItemType
{
    Equipalbe,
    Consumable,
    Resouce,
    interactableObject
}
public class BaseItemData : ScriptableObject
{
    public ItemType type;
    public string objectName;
    public string description;

    public Sprite crossHiar;
    public Color crossHiarColor;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    public Sprite icon;
    public GameObject dropPrefab;
}
