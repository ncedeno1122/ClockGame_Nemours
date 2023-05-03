using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{
    public bool IsEnabled;

    protected void Start()
    {
        
    }

    public abstract void OnAbility();
    public abstract void OnAbility(InputAction.CallbackContext ctx);
}
