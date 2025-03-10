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
    public ConsumableType consumType;
    public int value;
    
    public GameObject buffPrefab;
    public float duration;
}
