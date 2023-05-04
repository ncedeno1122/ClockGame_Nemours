using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandsObserver : MonoBehaviour
{
    public UnityEvent<Vector2> OnHandsUpdateEvent;

    private void OnEnable()
    {
        // TODO: Find the Player and subscribe to their Pendulum Ability
        GameObject.Find("Player").GetComponent<HandsAbility>().AddObserver(this);
    }

    // + + + + | Functions | + + + + 

    public void OnHandsUpdate(Vector2 movementInput)
    {
        OnHandsUpdateEvent?.Invoke(movementInput);
    }
}
