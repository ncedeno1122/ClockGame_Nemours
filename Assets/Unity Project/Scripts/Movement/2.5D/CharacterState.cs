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

    // FixedUpdate Functions

    public void OnFixedUpdate()
    {
        PreFixedUpdate();
        MidFixedUpdate();
        PostFixedUpdate();
    }

    protected abstract void PreFixedUpdate();
    protected abstract void MidFixedUpdate();
    protected abstract void PostFixedUpdate();

    // Input Functions
    public abstract void OnMove(InputAction.CallbackContext ctx);
    public abstract void OnJump(InputAction.CallbackContext ctx);
    public abstract void OnAbility(InputAction.CallbackContext ctx);
    public virtual void OnAbilitySwap(InputAction.CallbackContext ctx)
    {
        // TODO: For Button input, maybe Vector2 to go back & forth?
        if (ctx.performed)
        {
            m_Context.AbilityManager.NextAbility();
        }
    }

    // Next State - for AbilityStates!
    public abstract void AdvanceState();

    // Collision Functions
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);

}
