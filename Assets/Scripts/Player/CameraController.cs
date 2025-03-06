using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]Camera _camera;
    [SerializeField]float _targetDistance;

    [SerializeField] float cameraZoomSensitivity;
    [SerializeField] float maxZoomOut;
    [SerializeField] float minZoomOut;

    [SerializeField] LayerMask layerMask;

    Ray ray;
    RaycastHit hit;

    Vector3 _thirdCameraPos = new Vector3(0, 2f, 0);
    Vector3 _firstCameraPos = new Vector3(0, 1.55f, 0.2f);
    private void Awake()
    {
        _camera = Camera.main;
        _targetDistance = _camera.transform.localPosition.z;
    }
    private void Update()
    {
        CameraMove();
    }
    public void OnScroll(InputAction.CallbackContext context)
    {
        float scrollDelta = context.ReadValue<Vector2>().y;

        if (_camera.transform.localPosition.z >= -minZoomOut && _targetDistance >= -minZoomOut && scrollDelta >= 0)
        {
            _camera.transform.localPosition = new Vector3(0, 0, 0.1f);
            _targetDistance = 0.1f;

            transform.localPosition = _firstCameraPos;
        }
        else if(_targetDistance == 0.1f && scrollDelta < 0)
        {
            _camera.transform.localPosition = new Vector3(0, 0, -minZoomOut);
            _targetDistance = -minZoomOut;

            transform.localPosition = _thirdCameraPos;
        }
        else
        {
            _targetDistance = Mathf.Clamp(_targetDistance + (scrollDelta * 0.001f * cameraZoomSensitivity), -maxZoomOut, -minZoomOut);
        }
        
    }
    void CameraMove()
    {
        ray = new Ray(transform.position, transform.localRotation * Vector3.back);
        if (Physics.Raycast(ray, out hit, maxZoomOut,layerMask))
        {
            Debug.Log($"충돌감지 충돌물체 {hit.collider.name}");
            if (hit.distance <= -_camera.transform.localPosition.z && hit.distance < -_targetDistance)
            {
                _camera.transform.localPosition = new Vector3(0, 0, -hit.distance);
                return;
            }
        }
        _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, new Vector3(0, 0, _targetDistance), 5 * Time.deltaTime);



        //if (Physics.Raycast(ray, out hit, maxZoomOut) && hit.distance <= -_camera.transform.localPosition.z && hit.distance < -_targetDistance)
        //{
        //    _camera.transform.localPosition = new Vector3(0, 0, -hit.distance);
        //}
        //else
        //{
        //    _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, new Vector3(0, 0, _targetDistance), 5 * Time.deltaTime);
        //}

    }
}
