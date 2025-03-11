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
    [SerializeField] float exhaustTime;
    [SerializeField] LayerMask groundLayerMask;
    public List<MoveState> _curMoveState;
    float _rotateTargetDegree;

    public bool isLookable;

    [SerializeField]bool _isGround;

    bool slide;
    Vector3 slideNormalVector;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();   
        _animator = GetComponentInChildren<Animator>();
        _playerCondition = GetComponent<PlayerCondition>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isLookable = true;
        _curMoveState = new List<MoveState> {MoveState.Idle,MoveState.Run};
    }
    private void FixedUpdate()
    {

        Move();
        RotateBodyUpdate();
        _isGround = IsGround();
        _rigidbody.useGravity = (_curMoveState.Contains(MoveState.Jump));
        _animator.SetBool("Land",_isGround);

        if (!_isGround && !_curMoveState.Contains(MoveState.Jump) && CheckRealFall()) //추락판정조건
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x,0,_rigidbody.velocity.z); //추락일 경우 위로가던 속도제거=바로떨어짐
            Fall();
        }
        
    }
    private void LateUpdate()
    {
        if (isLookable)
        {
            RotateLook();
        }
        
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
            _rigidbody.velocity += new Vector3(moveDirection.normalized.x, 0, moveDirection.normalized.z) * 0.08f;
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
                case MoveState.Idle:
                    moveDirection = Vector3.zero; break;
                default:
                    moveDirection *= moveSpeed; break;
            }
        }
        moveDirection.y = _rigidbody.velocity.y;

        if (slide&&Vector3.Angle(moveDirection,slideNormalVector)>90)
        {
            moveDirection = Slide(moveDirection);
        }

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
    void ChangeAnimation() //움직임에 따라 맞는 움직임애니메이션 상태 설정
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
    void RotateBody() //움직임에 따라 몸을 돌릴 방향을 설정
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
    void JumpRotate() //점프직전 점프방향으로 몸방향 바꿈
    {
        float degree = Mathf.Atan2(_curMoveInput.x, _curMoveInput.y);
        _rotateTargetDegree = degree * Mathf.Rad2Deg;
    }
    
    void RotateBodyUpdate()//부드럽게 몸 방향 바꿈
    {
        _animator.transform.localRotation = Quaternion.RotateTowards(_animator.transform.localRotation, Quaternion.Euler(0, _rotateTargetDegree, 0), 10);
    }
    public void LandEnd() //착지모션 끝, 움직임 가능
    {
        _curMoveState.Remove(MoveState.Jump);
        RotateBody();
        ChangeStaminaPassive();
    }
    void ChangeStaminaPassive()//상태에 따라 스테미나 회복량 바꿈
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
    public void Fall()//추락판정
    {
        if(!_curMoveState.Contains(MoveState.Jump))
            _curMoveState.Add(MoveState.Jump);
        _animator.SetTrigger("Fall");
        JumpRotate();
        ChangeStaminaPassive();
    }

    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) +(transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) +(transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up*0.01f),Vector3.down),
        };
        
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    bool CheckRealFall()  //진짜 떨어지는지 여부 판별, 내리막길,작은단차 등은 추락판정에서 제외하는 기능
    {
        if (Physics.Raycast(transform.position + (transform.forward * 0.19f) + (transform.up * 0.01f), Vector3.down, 2f, groundLayerMask))
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -10, _rigidbody.velocity.z);
            return false;
        }
        return true;
    }

    private void OnCollisionStay(Collision collision)  //벽(충돌각도가 45도이상인)감지
    {
        slide = false;
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) > 45f)
            {
                slide = true;
                slideNormalVector = collision.contacts[i].normal;
            }
        }
    }
    Vector3 Slide(Vector3 moveDirection)  //벽에 비빌때 속도조정
    {
        Vector3 moveHoriz = new Vector3(moveDirection.x,0,moveDirection.z);
        Vector3 projectMoveHoriz = Vector3.ProjectOnPlane(moveHoriz, slideNormalVector).normalized;
        moveDirection = projectMoveHoriz.normalized * Mathf.Lerp(projectMoveHoriz.magnitude, moveHoriz.magnitude, 0.5f);
        return moveDirection;
    }
}
