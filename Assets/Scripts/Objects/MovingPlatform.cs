using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    List<Transform> spots = new List<Transform>();
    bool movedirection;
    [SerializeField] Rigidbody _rigidbody;
    Vector3 velocity;
    Vector3 targetPos;

    private void Awake()
    {
        movedirection = true;
        for (int i = 1; i < transform.childCount; i++)
        {
            spots.Add(transform.GetChild(i));
        }
    }
    private void Start()
    {
        StartCoroutine(enumerator());
    }
    private void FixedUpdate()
    {
        velocity = targetPos - transform.position;
        _rigidbody.velocity = velocity;
    }


    IEnumerator enumerator()
    {
        for (int i = 0; i < spots.Count;)
        {
            if (i == 0)
            {
                movedirection = true;
            }
            else if (i == spots.Count - 1)
            {
                movedirection= false;
            }
            
            targetPos = _rigidbody.transform.position;
            
            yield return new WaitForSeconds(5);
            targetPos = movedirection ? spots[i + 1].position : spots[i - 1].position;
            yield return new WaitUntil(() => Vector3.Distance(_rigidbody.transform.position, targetPos) < 0.1f);
            i = movedirection ? i + 1 : i - 1;
        }
    }
}
