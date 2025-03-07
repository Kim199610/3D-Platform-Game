using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class BaseObjData : ScriptableObject
{
    public string objectName;
    public string description;

    public Image crossHiar;
    public Color crossHiarColor;

    public GameObject uiPrefab;
    
}
