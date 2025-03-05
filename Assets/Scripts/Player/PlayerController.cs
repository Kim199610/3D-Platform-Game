using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidbody;
    Vector2 _curMoveInput;
    Vector2 _mouseDelta;
    float _camCurXRot;

    Animator _animator;

    [Header("Camera")]
    [SerializeField] Transform _cameraContainer;
    [SerializeField] float _lookSensitivity;
    [SerializeField] float minXLook;
    [SerializeField] float maxXLook;

    [Header("Move")]
    public float moveSpeed;
    public float jumpPower;
    public float sprintSpeedRate;
    [SerializeField] float _walkSpeed; 
    bool _isSprint;
    bool _isWalk;
    


    public bool _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();   
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        RotateLook();
    }

    void Move()
    {
        
        Vector3 moveDirection = transform.forward * _curMoveInput.y + transform.right * _curMoveInput.x;
        moveDirection *= _isWalk ? _walkSpeed : _isSprint ? moveSpeed * sprintSpeedRate : moveSpeed;
        moveDirection.y = _rigidbody.velocity.y;

        if (!_isJumping)
            _rigidbody.velocity = moveDirection;
        else
            _rigidbody.velocity += new Vector3(moveDirection.normalized.x, 0, moveDirection.normalized.z) * 0.1f;
    }
    void RotateLook()
    {
        _camCurXRot += _mouseDelta.y * _lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
        _cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0,_mouseDelta.x * _lookSensitivity, 0);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _curMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _curMoveInput = Vector2.zero;
        }
        MoveAnimationUpdate();
        if (!_isJumping)
        {
            if (_isSprint && _curMoveInput.y <= 0)
            {
                RotateBody(_curMoveInput.x != 0);
            }
            else
            {
                _animator.transform.localRotation = Quaternion.identity;
            }
        }
            
    }
    void MoveAnimationUpdate()
    {
        _animator.SetBool("Forward", _curMoveInput.y > 0);
        _animator.SetBool("Back", _curMoveInput.y < 0);
        _animator.SetBool("ForwardOrBack", _curMoveInput.y != 0);
        _animator.SetBool("Right", _curMoveInput.x > 0);
        _animator.SetBool("Left",_curMoveInput.x < 0);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _animator.SetTrigger("Jump");
            if (_curMoveInput != Vector2.zero)
            {
                RotateBody();
            }
            Debug.Log($"{_animator.transform.rotation.x},{_animator.transform.rotation.y},{_animator.transform.rotation.z}");
            _isJumping = true;
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSprint = true;
            _animator.SetBool("Sprint",_isSprint);
        }
        else if (context.canceled)
        {
            _isSprint = false;
            _animator.SetBool("Sprint", _isSprint);
        }
        if (!_isJumping)
        {
            if (_isSprint && _curMoveInput.y <= 0)
            {
                RotateBody(_curMoveInput.x != 0);
            }
            else
            {
                _animator.transform.localRotation = Quaternion.identity;
            }
        }
    }
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isWalk = true;
            _animator.SetBool("Walk", _isWalk);
        }
        else if (context.canceled)
        {
            _isWalk = false;
            _animator.SetBool("Walk", _isWalk);
        }
    }
    void RotateBody(bool isSideSprint = false)
    {
        float dgree = Mathf.Atan2(_curMoveInput.x, _curMoveInput.y);
        if(isSideSprint)
            dgree = Mathf.Sign(dgree) * (Mathf.Abs(dgree) - Mathf.PI / 4);
        _animator.transform.localRotation = Quaternion.Euler(0, dgree * Mathf.Rad2Deg, 0);
    }
    public void LandEnd()
    {
        RotateBody(_curMoveInput.x != 0);
        _isJumping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_isJumping && collision.collider is TerrainCollider)
        {
            _animator.SetTrigger("Land");
        }
    }
}
