using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public List<BaseObject> baseObjects = new List<BaseObject>();
    Camera _camera;
    Vector3 _cameraCenter;
    [SerializeField] float showInfoRange;
    public bool showDescription;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        showDescription = false;
    }
    private void Update()
    {
        for(int i = 0; i < baseObjects.Count; i++)
        {
            if(Vector3.Distance(_camera.WorldToScreenPoint(baseObjects[i].transform.position), _cameraCenter) <= showInfoRange)
            {
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
