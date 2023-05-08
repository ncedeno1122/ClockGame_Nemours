using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCuckooHit : MonoBehaviour
{
    public UnityEvent OnCuckooHitEvent;

    // + + + + | Functions | + + + + 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cuckoo"))
        {
            OnCuckooHitEvent?.Invoke();
        }
    }
}
