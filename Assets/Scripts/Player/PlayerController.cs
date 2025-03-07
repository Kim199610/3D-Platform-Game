using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
public enum MoveState
{
    Sprint,
    Run,
    Walk,
    Idle,
    Jump,
    Exhauste
}
public class PlayerController : MonoBehaviour
{
    

    Rigidbody _rigidbody;
    Vector2 _curMoveInput;
    Vector2 _mouseDelta;
    float _camCurXRot;
    PlayerCondition _playerCondition;

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
    [SerializeField] float jumpStamina;

    List<MoveState> _curMoveState;
    [SerializeField] float exhaustTime;

    float _rotateTargetDegree;

   

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();   
        _animator = GetComponentInChildren<Animator>();
        _playerCondition = GetComponent<PlayerCondition>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _curMoveState = new List<MoveState> {MoveState.Idle,MoveState.Run};
    }
    private void FixedUpdate()
    {
        Move();
        RotateBodyUpdate();
    }
    private void LateUpdate()
    {
        RotateLook();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }
    void RotateLook()
    {
        _camCurXRot += _mouseDelta.y * _lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
        _cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, _mouseDelta.x * _lookSensitivity, 0);
    }
    void Move()
    {
        
        Vector3 moveDirection = transform.forward * _curMoveInput.y + transform.right * _curMoveInput.x;
        

        if (_curMoveState.Contains(MoveState.Jump))
        {
            _rigidbody.velocity += new Vector3(moveDirection.normalized.x, 0, moveDirection.normalized.z) * 0.1f;
            return;
        }
        if (_curMoveState.Contains(MoveState.Exhauste))
        {
            moveDirection *= _walkSpeed;
        }
        else
        {
            switch (_curMoveState[0])
            {
                case MoveState.Sprint:
                    moveDirection *= moveSpeed * sprintSpeedRate; break;
                case MoveState.Walk:
                    moveDirection *= _walkSpeed; break;
                default:
                    moveDirection *= moveSpeed; break;
            }
        }
        moveDirection.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = moveDirection;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _curMoveState.Remove(MoveState.Idle);
            _curMoveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            if (!_curMoveState.Contains(MoveState.Idle))
            {
                _curMoveState.Insert(0, MoveState.Idle);
            }
            _curMoveInput = Vector2.zero;
        }
        MoveAnimationUpdate();
        RotateBody();
        ChangeStaminaPassive();

    }
    void MoveAnimationUpdate()
    {
        _animator.SetBool("Forward", _curMoveInput.y > 0);
        _animator.SetBool("Back", _curMoveInput.y < 0);
        _animator.SetBool("ForwardOrBack", _curMoveInput.y != 0);
        _animator.SetBool("Right", _curMoveInput.x > 0);
        _animator.SetBool("Left",_curMoveInput.x < 0);
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!_curMoveState.Contains(MoveState.Exhauste) && context.started && !_curMoveState.Contains(MoveState.Jump))
        {
            if (!_playerCondition.ConsumStamina(jumpStamina))
                return;
            _curMoveState.Add(MoveState.Jump);
            _animator.SetTrigger("Jump");
            if (_curMoveInput != Vector2.zero)
            {
                JumpRotate();
            }
        }
        ChangeStaminaPassive();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_curMoveState.Contains(MoveState.Sprint))
                _curMoveState.Insert(0,MoveState.Sprint);
        }
        else if (context.canceled)
        {
            _curMoveState.Remove(MoveState.Sprint);
        }
        RotateBody();
        ChangeAnimation();
        ChangeStaminaPassive();
    }
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_curMoveState.Contains(MoveState.Walk))
                _curMoveState.Insert(0, MoveState.Walk);
        }
        else if (context.canceled)
        {
            _curMoveState.Remove(MoveState.Walk);
        }
        RotateBody();
        ChangeAnimation();
        ChangeStaminaPassive();
    }
    void ChangeAnimation()
    {
        if (!_curMoveState.Contains(MoveState.Exhauste))
        {
            _animator.SetBool("Sprint", _curMoveState[0] == MoveState.Sprint);
            _animator.SetBool("Walk", _curMoveState[0] == MoveState.Walk);
        }
        else
        {
            _animator.SetBool("Sprint", false);
            _animator.SetBool("Walk", true);
        }
    }
    void RotateBody()
    {
        if (_curMoveState.Contains(MoveState.Jump))
            return;
        if (_curMoveState.Contains(MoveState.Exhauste) || _curMoveState[0] != MoveState.Sprint)
        {
            _rotateTargetDegree = 0;
        }
        else
        {
            float degree = Mathf.Atan2(_curMoveInput.x, _curMoveInput.y);
            if (_curMoveInput.x != 0)
                degree = Mathf.Sign(degree) * (Mathf.Abs(degree) - Mathf.PI / 4);
            _rotateTargetDegree = degree * Mathf.Rad2Deg;
        }
    }
    void JumpRotate()
    {
        float degree = Mathf.Atan2(_curMoveInput.x, _curMoveInput.y);
        _rotateTargetDegree = degree * Mathf.Rad2Deg;
    }

    void RotateBodyUpdate()
    {
        _animator.transform.localRotation = Quaternion.RotateTowards(_animator.transform.localRotation, Quaternion.Euler(0, _rotateTargetDegree, 0), 10);
    }
    public void LandEnd()
    {
        _curMoveState.Remove(MoveState.Jump);
        RotateBody();
        ChangeStaminaPassive();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_curMoveState.Contains(MoveState.Jump) && collision.collider is TerrainCollider)
        {
            _animator.SetTrigger("Land");
        }
    }
    void ChangeStaminaPassive()
    {
        _playerCondition.ChangeStaminaPassive(_curMoveState);
    }
    public void Exhauste()
    {
        _playerCondition.StartStaminaWarning();

        if (!_curMoveState.Contains(MoveState.Exhauste))
            _curMoveState.Add(MoveState.Exhauste);

        RotateBody();
        ChangeAnimation();
        ChangeStaminaPassive();

        Invoke("ExhausteEnd",exhaustTime);
    }
    void ExhausteEnd()
    {
        _playerCondition.StopStaminaWarning();
        _curMoveState.Remove(MoveState.Exhauste);
        RotateBody();
        ChangeAnimation();
        ChangeStaminaPassive();
    }
    public void Fall()
    {
        _curMoveState.Add(MoveState.Jump);
        _animator.SetTrigger("Fall");
        JumpRotate();
        ChangeStaminaPassive();
    }

}
