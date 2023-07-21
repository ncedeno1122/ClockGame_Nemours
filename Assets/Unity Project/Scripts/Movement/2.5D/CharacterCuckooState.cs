using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCuckooState : CharacterState
{
    private CuckooAbility m_CuckooAbility;

    public CharacterCuckooState(CharacterController2D context) : base(context)
    {
        m_CuckooAbility = context.AbilityManager.CuckooScript;
    }

    public override void OnEnter()
    {
        //Debug.Log("Entering CuckooState!");
        if (!m_CuckooAbility.IsEnabled)
        {
            AdvanceState();
        }

        m_Context.Animator.SetTrigger("CuckooTrigger");
        
        // Audio
        m_Context.WASC.AudioSource.PlayOneShot(AudioManager.Instance.CurrentSoundBank.GetSFXClip(SFXClips.CUCKOO));
    }

    public override void OnExit()
    {
        //Debug.Log("Exiting CuckooState!");
        m_Context.Animator.ResetTrigger("CuckooTrigger");
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
        //
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
    }

    public override void OnJump(InputAction.CallbackContext ctx)
    {
    }

    public override void OnMove(InputAction.CallbackContext ctx)
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

    // + + + + | Collision Handling | + + + + 

    public override void OnTriggerEnter(Collider other)
    {
        //
    }

    public override void OnTriggerStay(Collider other)
    {
        //
    }

    public override void OnTriggerExit(Collider other)
    {
        //
    }
}
