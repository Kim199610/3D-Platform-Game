using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour,IDamagable
{
    PlayerController controller;

    [SerializeField] Condition health;
    [SerializeField] Condition stamina;
    [SerializeField] float _normalStaminaPassive;
    [SerializeField] float _runStaminaPassive;
    [SerializeField] float _sprintStaminaPassive;
    public event Action onTakeDamage;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    private void Update()
    {
        health.ChangeValue(health.PassiveValue*Time.deltaTime);
        stamina.ChangeValue(stamina.PassiveValue*Time.deltaTime);
        if(stamina.curValue <= 0)
        {
            controller.Exhauste();
        }
    }
    public void ChangeStaminaPassive(List<MoveState> moveState)
    {
        if (moveState.Contains(MoveState.Jump))
        {
            stamina.PassiveValue = _runStaminaPassive;
            return;
        }
        if (moveState.Contains(MoveState.Exhauste))
        {
            stamina.PassiveValue = _normalStaminaPassive;
            return ;
        }

        switch (moveState[0])
        {
            case MoveState.Sprint:
                stamina.PassiveValue = _sprintStaminaPassive; break;
            case MoveState.Walk:
            case MoveState.Idle:
                stamina.PassiveValue = _normalStaminaPassive; break ;
            default:
                stamina.PassiveValue = _runStaminaPassive; break;
        }
        
    }
    public bool ConsumStamina(float value)
    {
        if (stamina.curValue < 10)
        {
            StartStaminaWarning();
            Invoke("StopStaminaWarning", 2f);
            return false;
        }
            
        if(stamina.curValue < value)
            controller.Exhauste();
        stamina.ChangeValue(-value);
        return true;
    }
    public void HealHealth(int value)
    {
        health.ChangeValue(value);
    }

    public void TakeDamage(float damage)
    {
        health.ChangeValue(-damage);
        onTakeDamage?.Invoke();
    }

    public void StartStaminaWarning()
    {
        stamina.StartWarning();
    }
    public void StopStaminaWarning()
    {
        stamina.StopWarning();
    }
}
