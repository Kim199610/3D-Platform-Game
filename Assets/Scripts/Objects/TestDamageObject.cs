using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamageObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCondition condition = other.GetComponent<PlayerCondition>();
        condition?.TakeDamage(100f);
    }
}
