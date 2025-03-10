using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumItem : BaseItem
{
    
    public ConsumItemData consumItemData;

    private void Awake()
    {
        consumItemData = (ConsumItemData)baseItemData;
    }
}
