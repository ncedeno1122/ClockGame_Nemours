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

    public Vector2 PlayerMovementVector = Vector2.zero;

    public CharacterWalk(CharacterController2D context) : base(context)
    {
    }

    public override void OnEnter()
    {
        Debug.Log($"Entering CharacterWalk");
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
        if (PlayerMovementVector != Vector2.zero)
        {
            // Move
            float accelerationWalkSpeed = Mathf.Lerp(0f, m_Context.WalkSpeed, WalkAccelTimerHelper / WALK_ACCELERATION_TIME);
            m_Context.CharacterVelocity.x = accelerationWalkSpeed;
            m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position + new Vector3(PlayerMovementVector.x * (m_Context.CharacterVelocity.x * Time.deltaTime), 0f));
        }
        else
        {
            // Stop Moving
            float decelerationWalkSpeed = Mathf.Lerp(m_Context.CharacterVelocity.x, 0f, WalkDecelTimerHelper / WALK_DECELERATION_TIME);
            float facingDirectionScalar = (m_Context.FacingRight ? 1f : -1f);
            m_Context.CharacterVelocity.x = decelerationWalkSpeed;
            m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position + new Vector3(m_Context.CharacterVelocity.x * (facingDirectionScalar * Time.deltaTime), 0f));
        }
    }

    protected override void PostUpdate()
    {
        Mathf.Clamp(WalkAccelTimerHelper += Time.deltaTime, 0f, WALK_ACCELERATION_TIME);
        Mathf.Clamp(WalkDecelTimerHelper += Time.deltaTime, 0f, WALK_DECELERATION_TIME);
    }

    public override void OnMove(InputAction.CallbackContext ctx)
    {
        PlayerMovementVector = ctx.ReadValue<Vector2>();
        
        if (ctx.started)
        {
            WalkAccelTimerHelper = 0f;

            // Update Facing Direction
            m_Context.FacingRight = PlayerMovementVector.x >= 0f;
        }
        if (ctx.canceled)
        {
            WalkDecelTimerHelper = 0f;
        }
    }

    public override void OnJump()
    {
        // Add Velocity, swap to AirState
    }

    public override void OnPowerup(InputAction.CallbackContext ctx)
    {
        //
    }
}
