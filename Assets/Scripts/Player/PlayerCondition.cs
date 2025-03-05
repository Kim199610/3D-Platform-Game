using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour,IDamagable
{
    [SerializeField] Condition health;
    [SerializeField] Condition stamina;

    private void Update()
    {
        health.ChangeValue(health.PassiveValue*Time.deltaTime);
        stamina.ChangeValue(stamina.PassiveValue*Time.deltaTime);
    }



    public void TakeDamage(float damage)
    {
        health.ChangeValue(-damage);
    }

}
