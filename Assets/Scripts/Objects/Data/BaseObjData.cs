using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseObjData : ScriptableObject
{
    public string objectName;
    public string description;

    public Sprite crossHiar;
    public Color crossHiarColor;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
}
