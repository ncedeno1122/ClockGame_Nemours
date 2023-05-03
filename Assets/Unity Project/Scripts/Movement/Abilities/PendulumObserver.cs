using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PendulumObserver : MonoBehaviour
{
    public UnityEvent OnPendulumEvent;

    private void OnEnable()
    {
        // TODO: Find the Player and subscribe to their Pendulum Ability
        GameObject.Find("Player").GetComponent<PendulumAbility>().AddObserver(this);
    }

    // + + + + | Functions | + + + + 

    public void OnPendulum()
    {
        OnPendulumEvent?.Invoke();
    }
}
