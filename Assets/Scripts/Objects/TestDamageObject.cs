using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestDamageObject : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int damageRate;
    List<IDamagable> damagables = new List<IDamagable>();

    private void Start()
    {
        InvokeRepeating("DealDamage",0,damageRate);
    }
    void DealDamage()
    {
        for (int i = 0; i < damagables.Count; i++)
        {
            damagables[i].TakeDamage(damage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
        {
            damagables.Add(damagable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagables.Remove(damagable);
        }
    }
}
