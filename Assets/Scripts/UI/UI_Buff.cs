using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Buff : MonoBehaviour
{
    List<BaseBuff> buffs = new List<BaseBuff>();
    public void AddBuff(GameObject buffPrefab)
    {
        BaseBuff buff = buffPrefab.GetComponent<BaseBuff>();
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i] == null)
            {
                buffs.Remove(buffs[i]);
                continue;
            }
            if (buffs[i] == buff)
            {
                buffs[i].RenewalBuff();
                return;
            }
        }
        buffs.Add(buff);
        Instantiate(buffPrefab, this.transform);
    }
}
