using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChimeObserver : MonoBehaviour
{
    public UnityEvent OnChimeEvent;

    private void OnEnable()
    {
        // TODO: Find the Player and subscribe to their Chime Ability
        GameObject.Find("Player").GetComponent<ChimeAbility>().AddObserver(this);
    }

    // + + + + | Functions | + + + + 

    public void OnChime()
    {
        OnChimeEvent?.Invoke();
    }
}
