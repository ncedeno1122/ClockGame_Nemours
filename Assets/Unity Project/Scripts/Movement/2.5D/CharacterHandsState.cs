using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHandsState : CharacterState
{
    private float m_StoredGroundCheckDistance;
    private HandsAbility m_HandsAbility;

    public CharacterHandsState(CharacterController2D context) : base(context)
    {
        m_HandsAbility = context.AbilityManager.HandsScript;
        m_StoredGroundCheckDistance = m_Context.GroundCheckDistance;
    }

    public override void OnEnter()
    {
        // Change GroundCheckDistance
        m_Context.GroundCheckDistance = 1f;

        Debug.Log("Entering HandsState!");
        
        // Audio
        m_Context.WASC.AudioSource.loop = true;
        m_Context.WASC.AudioSource.PlayOneShot(AudioManager.Instance.CurrentSoundBank.GetSFXClip(SFXClips.HANDS));
    }

    public override void OnExit()
    {
        // Revert GroundCheckDistance
        m_Context.GroundCheckDistance = m_StoredGroundCheckDistance;

        m_HandsAbility.UpdateHandsObservers(Vector2.zero);

        Debug.Log("Exiting HandsState!");
        
        // Audio
        m_Context.WASC.AudioSource.loop = false;
        m_Context.WASC.AudioSource.Stop();
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

    // + + + + | FixedUpdate Functions | + + + +  

    protected override void PreFixedUpdate()
    {
        // Taken from CharacterWalk
        // Groundcheck
        m_Context.GroundCheck();

        if (!m_Context.IsGrounded)
        {
            //Debug.Log($"Collision Report OnDetach-\n" +
            //$"LeftColliderGO {m_Context.LeftCastGO}, cast length of {m_Context.GroundCheckDistance}," +
            //$"RightColliderGO {m_Context.RightCastGO}, cast length of {m_Context.GroundCheckDistance}.");
            // Switch to walk to see if we can re-hands!
            m_Context.ChangeState(new CharacterWalk(m_Context));
        }

        // Moving Platform?
        Rigidbody platformRb = m_Context.LeftCastHit.rigidbody ?? m_Context.RightCastHit.rigidbody;
        if (platformRb && platformRb.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            // TogglePositionBlockScript
            TogglePositionBlockScript tpbs = platformRb.GetComponent<TogglePositionBlockScript>();
            if (tpbs && tpbs.IsMoving)
            {
                //Debug.Log("MOVING WITH PLATFORM!");
                Vector3 platformStepOffset = (tpbs.ToggledOn) ? ((tpbs.OnOffset - tpbs.OffOffset) / tpbs.MoveTime)
                                                              : ((tpbs.OffOffset - tpbs.OnOffset) / tpbs.MoveTime);

                m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position + platformStepOffset * Time.deltaTime);
            }

            // TODO: Should somehow cache what platform we're on top of.

            // HandsMovePlatformScript
            HandsMovePlatformScript hmps = platformRb.GetComponent<HandsMovePlatformScript>();
            if (hmps)
            {
                m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position + hmps.MoveVelocity * (hmps.FlySpeed * Time.deltaTime));
            }
        }
    }

    protected override void MidFixedUpdate()
    {
        //
    }

    protected override void PostFixedUpdate()
    {
        m_Context.Rigidbody.velocity = Vector3.zero;
    }

    // + + + + | InputActions | + + + + 

    public override void OnAbility(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            // Exit back to proper state
            AdvanceState();
        }
    }

    public override void OnAbilitySwap(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            AdvanceState();
        }
    }

    public override void OnJump(InputAction.CallbackContext ctx)
    {
    }

    public override void OnMove(InputAction.CallbackContext ctx)
    {
        // 'Step' HandsObservers
        Vector2 movementDirection = ctx.ReadValue<Vector2>();

        m_HandsAbility.UpdateHandsObservers(movementDirection);

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

    // + + + + | Collision Handling | + + + + 

    public override void OnTriggerEnter(Collider other)
    {
        //
    }

    public override void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        //{
        //    m_Context.Rigidbody.velocity = Vector3.zero;
        //}
    }

    public override void OnTriggerExit(Collider other)
    {
        //
    }
}
