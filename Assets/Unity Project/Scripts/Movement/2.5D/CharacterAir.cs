using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAir : CharacterState
{
    public CharacterAir(CharacterController2D context) : base(context)
    {
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnJump()
    {
    }

    public override void OnMove(InputAction.CallbackContext ctx)
    {
    }

    public override void OnPowerup(InputAction.CallbackContext ctx)
    {
    }

    protected override void PreUpdate()
    {
        // Groundcheck
        m_Context.GroundCheck();

        if (m_Context.IsGrounded)
        {
            m_Context.ChangeState(new CharacterWalk(m_Context));
        }
    }

    protected override void MidUpdate()
    {
        //m_Context.Rigidbody.velocity += Vector3.up * (m_Context.GravityForce * (Time.deltaTime * Time.deltaTime));
        m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position += Vector3.up * (m_Context.GravityForce * Time.deltaTime));
    }

    protected override void PostUpdate()
    {
    }
}
