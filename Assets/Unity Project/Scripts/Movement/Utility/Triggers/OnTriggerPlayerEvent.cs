using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A utility class used to invoke an event upon contact with the player.
/// </summary>
public class OnTriggerPlayerEvent : MonoBehaviour
{
    public UnityEvent OnPlayerTriggerEvent;

    // + + + + | Collision Handling | + + + + 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerTriggerEvent?.Invoke();
        }
    }
}
