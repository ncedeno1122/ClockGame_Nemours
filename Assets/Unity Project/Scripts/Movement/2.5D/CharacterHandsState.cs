using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHandsState : CharacterState
{
    private float m_StoredGroundCheckDistance;
    private float m_PreferredGroundCheckHeightOffset = 0.25f;
    private HandsAbility m_HandsAbility;
    private Vector3 m_PlayerOffsetFromPlatform;
    public Vector3 PlayerOffsetFromPlatform //TODO: Should probably remove in favor of if statement where assignment occurs...
    {
        get => m_PlayerOffsetFromPlatform;
        set
        {
            if (m_PlayerOffsetFromPlatform == Vector3.zero)
            {
                m_PlayerOffsetFromPlatform = value;
                //Debug.Log($"CharacterHandsState - Offset from the ground is {value}");
            }
        }
    } 

    public CharacterHandsState(CharacterController2D context) : base(context)
    {
        m_HandsAbility = context.AbilityManager.HandsScript;
        m_StoredGroundCheckDistance = m_Context.GroundCheckDistance;
    }

    public override void OnEnter()
    {
        // Change GroundCheckDistance
        m_Context.GroundCheckDistance = 1f;
        m_Context.GroundCheckHeightOffset = m_PreferredGroundCheckHeightOffset;

        Debug.Log("Entering HandsState!");
        
        // Audio
        m_Context.WASC.AudioSource.loop = true;
        m_Context.WASC.AudioSource.PlayOneShot(AudioManager.Instance.CurrentSoundBank.GetSFXClip(SFXClips.HANDS));
    }

    public override void OnExit()
    {
        // Revert GroundCheckDistance
        m_Context.GroundCheckDistance = m_StoredGroundCheckDistance;
        m_Context.GroundCheckHeightOffset = 0f;

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
        
        // Note: Could be 'Platform'-tagged OR 'StaticLevel'-tagged
        // Moving Platform?
        Rigidbody platformRb = m_Context.LeftCastHit.rigidbody ?? m_Context.RightCastHit.rigidbody;
        if (platformRb && platformRb.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            // If not grounded, store positional offset! NOTE: This can only happen once :D
            // TODO: Rid of property and limit here?
            PlayerOffsetFromPlatform = new Vector3(
                m_Context.Rigidbody.position.x - platformRb.position.x,
                (m_Context.LeftCastHit.rigidbody ? m_Context.LeftCastHit.distance : m_Context.RightCastHit.distance) * 2f
            );
            
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
            
            // HandsLinePlatformController
            HandsLinePlatformController hlpc = platformRb.GetComponentInParent<HandsLinePlatformController>();
            if (hlpc)
            {
                m_Context.Rigidbody.MovePosition(Vector3.Lerp(hlpc.FromPositionTf.position, hlpc.ToPositionTf.position, hlpc.NormalizedPositionValue) + m_PlayerOffsetFromPlatform);
            }
            
            // HandsBoundedFreePlatformController
            HandsBoundedFreePlatformController hbfpc =
                platformRb.GetComponentInParent<HandsBoundedFreePlatformController>();
            if (hbfpc)
            {
                m_Context.Rigidbody.MovePosition(m_Context.Rigidbody.position + hbfpc.MoveVelocity * (hbfpc.MoveSpeed * Time.deltaTime));
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
