using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : BaseObject
{
    
    public ItemData ItemData;

    private void Awake()
    {
        ItemData = (ItemData)baseObjData;
    }
}
