using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPendulumState : CharacterState
{
    private PendulumAbility m_PendulumAbility;

    public CharacterPendulumState(CharacterController2D context) : base(context)
    {
        m_PendulumAbility = context.AbilityManager.PendulumAbility;
    }

    public override void OnEnter()
    {
        //Debug.Log("Entering PendulumState!");

        m_PendulumAbility.OnAbility();
    }

    public override void OnExit()
    {
        //Debug.Log("Exiting PendulumState!");
    }

    public override void OnAbility(InputAction.CallbackContext ctx)
    {
    }

    public override void OnJump(InputAction.CallbackContext ctx)
    {
    }

    public override void OnMove(InputAction.CallbackContext ctx)
    {
    }

    protected override void PreUpdate()
    {
    }

    protected override void MidUpdate()
    {
    }

    protected override void PostUpdate()
    {
    }

    public override void AdvanceState()
    {
        if (m_Context.IsGrounded)
        {
            m_Context.ChangeState(new CharacterWalk(m_Context));
        }
        else
        {
            m_Context.ChangeState(new CharacterAir(m_Context));
        }
    }
}
