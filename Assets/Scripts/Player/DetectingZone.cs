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
        BaseObject baseObject = other.GetComponent<BaseObject>();
        if (baseObject != null)
            _interact.baseObjects.Add(baseObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            other.GetComponent<BaseObject>().indicateInfoBool = false;
            _interact.baseObjects.Remove(other.GetComponent<BaseObject>());
        }
    }
}
