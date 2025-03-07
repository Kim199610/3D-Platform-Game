using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NormalObject", menuName = "New NormalObject")]
public class BaseObjData : ScriptableObject
{
    public string objectName;
    public string description;

    public Image crossHiar;
    public Color crossHiarColor;
    
}
