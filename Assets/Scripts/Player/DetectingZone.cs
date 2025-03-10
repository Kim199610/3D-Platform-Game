using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingZone : MonoBehaviour
{
    Interact _interact;
    private void Awake()
    {
        _interact = GetComponentInParent<Interact>();
    }
    private void OnTriggerEnter(Collider other)
    {
        BaseItem baseObject = other.GetComponent<BaseItem>();
        if (baseObject != null)
            _interact.baseObjects.Add(baseObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            BaseItem item = other.GetComponent<BaseItem>();
            if(item != null)
            {
                item.indicateInfoBool = false;
                _interact.baseObjects.Remove(item);
            }
        }
    }
}
