using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPot : MonoBehaviour
{
    [SerializeField] float jumpPower;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 velocity;
            velocity = other.GetComponent<Rigidbody>().velocity;
            velocity = velocity * 0.5f;
            velocity.y = jumpPower;
            other.GetComponent<Rigidbody>().velocity = velocity;

            PlayerController controller = other.GetComponent<PlayerController>();
            controller.Fall();

        }
    }
}
