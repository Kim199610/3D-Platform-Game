using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamagable
{
    void TakeDamage(float damage);
}
public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}