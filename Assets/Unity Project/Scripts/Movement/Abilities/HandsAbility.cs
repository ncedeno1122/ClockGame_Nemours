using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsAbility : Ability
{
    private List<HandsObserver> m_HandsObservers = new();

    public override void OnAbility()
    {
        //
    }

    public override void OnAbility(InputAction.CallbackContext ctx)
    {
        //
    }

    // + + + + | Functions | + + + + 

    public void UpdateHandsObservers(Vector2 movementDirection)
    {
        if (m_HandsObservers.Count > 0)
        {
            foreach (HandsObserver observer in m_HandsObservers)
            {
                // TODO: Update them!
                observer.OnHandsUpdate(movementDirection);
            }
        }
    }

    public void AddObserver(HandsObserver observer) => m_HandsObservers.Add(observer);
    public void RemoveObserver(HandsObserver observer) => m_HandsObservers.Remove(observer);
}
