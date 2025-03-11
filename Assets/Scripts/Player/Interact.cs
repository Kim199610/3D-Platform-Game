using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class Interact : MonoBehaviour
{
    public List<BaseItem> baseObjects = new List<BaseItem>();
    Camera _camera;
    Vector3 _cameraCenter;
    [SerializeField] float showInfoRange;
    [SerializeField] float interactRange;
    [SerializeField] float interactDistance;
    public bool showDescription;
    [SerializeField]LayerMask canHideLayerMask;
    [SerializeField] BaseItem interactableItem;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        showDescription = false;
    }
    private void Update()
    {
        interactableItem = null;
        for(int i = 0; i < baseObjects.Count; i++)
        {
            if (baseObjects[i] == null)
            {
                baseObjects.Remove(baseObjects[i]);
                continue;
            }
            if (IsShowInfo(baseObjects[i]))
            {
                IsInteractable(baseObjects[i]);
                baseObjects[i].indicateInfoBool = true;
                baseObjects[i].showDescription = showDescription;
            }
            else
            {
                baseObjects[i].indicateInfoBool = false;
            }
        }
    }

    public void OnShowDescription(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            showDescription = true;
        }
        else if (context.canceled)
        {
            showDescription = false;
        }
    }
    void IsInteractable(BaseItem item)
    {
        Vector3 itemPos = item.transform.position;
        if(Vector3.Distance(transform.position,itemPos)>interactDistance)
            return;
        if(Vector3.Distance(_camera.WorldToScreenPoint(itemPos), _cameraCenter) > interactRange)
            return;
        if (interactableItem == null || Vector3.Distance(_camera.WorldToScreenPoint(interactableItem.transform.position), _cameraCenter) > Vector3.Distance(_camera.WorldToScreenPoint(itemPos), _cameraCenter))
        {
            interactableItem = item;
        }
    }

    bool IsShowInfo(BaseItem item)
    {
        
        Vector3 itemPos = item.transform.position;
        if (Vector3.Distance(_camera.WorldToScreenPoint(itemPos), _cameraCenter) > showInfoRange)
            return false;
        Vector3 cameraPos = _camera.transform.position;
        return !Physics.Raycast(cameraPos, itemPos - cameraPos, Vector3.Distance(cameraPos, itemPos) - 1f, canHideLayerMask);


    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed && interactableItem != null)
        {
            interactableItem.InteractItem();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log($"충돌감지 {other.name}");
    //    BaseObject baseObject = other.GetComponent<BaseObject>();
    //    if(baseObject!=null)
    //        baseObjects.Add(baseObject);
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.layer == LayerMask.NameToLayer("Object"))
    //    {
    //        other.GetComponent<BaseObject>().indicateInfoBool = false;
    //        baseObjects.Remove(other.GetComponent<BaseObject>());
    //    }
    //}
}
