using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerModelAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody _rigidbody;
    PlayerController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponentInParent<Rigidbody>();
        controller = GetComponentInParent<PlayerController>();
    }
    public void IdleChange_Random()
    {
        if (Random.value > 0.2f)
        {
            animator.SetTrigger("IdleChange");
        }
    }
    public void IdleChange()
    {
        animator.SetTrigger("IdleChange");
    }
    public void Jump()
    {
        _rigidbody.AddForce(Vector3.up * controller.jumpPower, ForceMode.Impulse);
    }
    public void LandStart()
    {
        _rigidbody.velocity *= 0.05f;
    }
    public void LandEnd()
    {
        controller.LandEnd();
    }
}
