using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour,IDamagable
{
    [SerializeField] Condition health;
    [SerializeField] Condition stamina;
    [SerializeField] float _normalStaminaPassive;
    [SerializeField] float _runStaminaPassive;
    [SerializeField] float _sprintStaminaPassive;

    private void Update()
    {
        health.ChangeValue(health.PassiveValue*Time.deltaTime);
        stamina.ChangeValue(stamina.PassiveValue*Time.deltaTime);
    }
    public void ChangeStaminaPassive(MoveState moveState)
    {
        switch (moveState)
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
        if (stamina.curValue < value)
            return false;
        stamina.ChangeValue(-value);
        return true;
    }

    public void TakeDamage(float damage)
    {
        health.ChangeValue(-damage);
    }

}
