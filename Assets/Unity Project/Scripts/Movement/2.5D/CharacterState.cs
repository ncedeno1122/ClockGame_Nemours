using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterState
{
    protected CharacterController2D m_Context;

    public CharacterState(CharacterController2D context)
    {
        m_Context = context;
    }

    // Enter/Exit Functions
    public abstract void OnEnter();
    public abstract void OnExit();

    // Update Functions
    public void OnUpdate()
    {
        PreUpdate();
        MidUpdate();
        PostUpdate();
    }
    protected abstract void PreUpdate();
    protected abstract void MidUpdate();
    protected abstract void PostUpdate();

    // Input Functions
    public abstract void OnMove(InputAction.CallbackContext ctx);
    public abstract void OnJump(InputAction.CallbackContext ctx);
    public abstract void OnPowerup(InputAction.CallbackContext ctx);
}
