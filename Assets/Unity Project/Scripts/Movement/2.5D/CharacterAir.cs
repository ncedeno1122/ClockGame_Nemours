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
        CheckAerialState();
    }

    public override void OnExit()
    {
    }

    public override void OnJump(InputAction.CallbackContext ctx)
    {
    }

    public override void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            // Update Facing Direction
            m_Context.FacingRight = m_Context.PlayerMovementVector.x >= 0f;
        }
    }

    public override void OnAbility(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Ability currAbility = m_Context.AbilityManager.CurrentAbility;
            if (currAbility != null)
            {
                switch (currAbility)
                {
                    case PendulumAbility:
                        m_Context.ChangeState(new CharacterPendulumState(m_Context));
                        break;
                }
            }
        }
    }

    protected override void PreUpdate()
    {
        CheckAerialState();

        // Groundcheck
        if (!m_Context.IsJumping)
        {
            m_Context.GroundCheck();

            if (m_Context.IsGrounded && m_Context.IsFalling)
            {
                m_Context.ChangeState(new CharacterWalk(m_Context));
            }
        }

        // Apply Gravity
        m_Context.CharacterVelocity.y += m_Context.GravityForce * (Time.deltaTime);

        // Apply Terminal Velocity
        m_Context.CharacterVelocity.y = Mathf.Clamp(m_Context.CharacterVelocity.y, m_Context.GravityForce, Mathf.Infinity);
    }

    private void CheckAerialState()
    {
        // Jumping or Falling
        m_Context.IsJumping = m_Context.CharacterVelocity.y > 0f;
        m_Context.IsFalling = m_Context.CharacterVelocity.y <= 0f;
    }

    protected override void MidUpdate()
    {
        // Air Steering Speed
        m_Context.CharacterVelocity.x += m_Context.PlayerMovementVector.x * (m_Context.AirSpeedX * Time.deltaTime);

        m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position += (m_Context.CharacterVelocity * Time.deltaTime));
    }

    protected override void PostUpdate()
    {
    }

    public override void AdvanceState()
    {
        //
    }
}
