using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{
    public bool IsEnabled;

    protected CharacterController2D m_CC2D;

    protected void Start()
    {
        m_CC2D = GetComponent<CharacterController2D>();
    }

    public abstract void OnAbility();
    public abstract void OnAbility(InputAction.CallbackContext ctx);
}
