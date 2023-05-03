using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterWalk : CharacterState
{
    public const float WALK_ACCELERATION_TIME = 0.35f;
    public const float WALK_DECELERATION_TIME = 0.2f;
    public float WalkAccelTimerHelper = 0f;
    public float WalkDecelTimerHelper = 0f;

    public CharacterWalk(CharacterController2D context) : base(context)
    {
    }

    public override void OnEnter()
    {
        Debug.Log($"Entering CharacterWalk");
        m_Context.IsJumping = false;
        m_Context.IsFalling = false;
        m_Context.CharacterVelocity.y = 0f;
    }

    public override void OnExit()
    {
        Debug.Log($"Exiting CharacterWalk");
    }

    protected override void PreUpdate()
    {
        // Groundcheck
        m_Context.GroundCheck();

        if (!m_Context.IsGrounded)
        {
            m_Context.ChangeState(new CharacterAir(m_Context));
        }
    }

    protected override void MidUpdate()
    {
        if (m_Context.PlayerMovementVector != Vector2.zero)
        {
            // Move
            float accelerationWalkSpeed = Mathf.Lerp(0f, m_Context.WalkSpeed, WalkAccelTimerHelper / WALK_ACCELERATION_TIME);
            m_Context.CharacterVelocity.x = m_Context.PlayerMovementVector.x * accelerationWalkSpeed;
        }
        else
        {
            // Stop Moving
            float decelerationWalkSpeed = Mathf.Lerp(m_Context.CharacterVelocity.x, 0f, WalkDecelTimerHelper / WALK_DECELERATION_TIME);
            m_Context.CharacterVelocity.x = decelerationWalkSpeed;
        }

        // Finally, MovePosition
        m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position + new Vector3((m_Context.CharacterVelocity.x * Time.deltaTime), 0f));
    }

    protected override void PostUpdate()
    {
        Mathf.Clamp(WalkAccelTimerHelper += Time.deltaTime, 0f, WALK_ACCELERATION_TIME);
        Mathf.Clamp(WalkDecelTimerHelper += Time.deltaTime, 0f, WALK_DECELERATION_TIME);
    }

    public override void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            WalkAccelTimerHelper = 0f;

            // Update Facing Direction
            m_Context.FacingRight = m_Context.PlayerMovementVector.x >= 0f;
        }
        if (ctx.canceled)
        {
            WalkDecelTimerHelper = 0f;
        }
    }

    public override void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Add Velocity, swap to AirState
            m_Context.CharacterVelocity.y += m_Context.JumpForce;
            m_Context.ChangeState(new CharacterAir(m_Context));
        }
    }

    public override void OnPowerup(InputAction.CallbackContext ctx)
    {
        //
    }
}
