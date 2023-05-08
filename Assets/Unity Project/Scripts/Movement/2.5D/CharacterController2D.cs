using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls a set of CharacterStates that characters can use in a 2.5D environment.
/// </summary>

public class CharacterController2D : MonoBehaviour
{
    public float WalkSpeed = 7.5f;
    public float CurrentSpeed = 0f;
    public float JumpForce = 15f;
    public float GravityForce = -20f;
    public float AirSpeedX = 10f;
    public bool IsGrounded;
    public bool IsJumping = false;
    public bool IsFalling = false;

    private bool m_FacingRight = true;
    public bool FacingRight { get => m_FacingRight;
        set
        {
            if (m_FacingRight != value)
            {
                //Debug.Log($"Was facing {(m_FacingRight ? "right" : "left")}, now facing {(value ? "right" : "left")}");
                SetFacingDirection(value);
            }
            m_FacingRight = value;
        }
    }

    [Range(0.01f, 1f)]
    public float GroundCheckDistance = 0.3f;

    public Vector2 PlayerMovementVector = Vector2.zero;
    public Vector3 CharacterVelocity = Vector3.zero;

    public RaycastHit LeftCastHit;
    public RaycastHit RightCastHit;
    public GameObject LeftCastGO;
    public GameObject RightCastGO;

    public Transform ModelTransform { get; private set; }

    private BoxCollider m_BoxCollider;
    public Rigidbody Rigidbody;
    public Animator Animator;

    private CharacterState m_CurrentState;
    public CharacterState CurrentState { get => m_CurrentState; }

    private PlayerInput m_PlayerInput;
    private InputAction m_MoveIA, m_JumpIA, m_AbilityIA, m_AbilitySwapIA;

    public AbilityManager AbilityManager;

    private void Awake()
    {
        ModelTransform = transform.GetChild(0);

        m_BoxCollider = GetComponent<BoxCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();
        AbilityManager = GetComponent<AbilityManager>();
        Animator = GetComponent<Animator>();

        m_MoveIA = m_PlayerInput.actions["Move"];
        m_JumpIA = m_PlayerInput.actions["Jump"];
        m_AbilityIA = m_PlayerInput.actions["Ability"];
        m_AbilitySwapIA = m_PlayerInput.actions["AbilitySwap"];
    }

    void Start()
    {
        m_CurrentState = new CharacterWalk(this);
    }

    void Update()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.OnFixedUpdate();
        }
    }

    // + + + + | InputSystem Handlers | + + + + 

    public void OnMoveIA(InputAction.CallbackContext ctx)
    {
        PlayerMovementVector = ctx.ReadValue<Vector2>();
        m_CurrentState.OnMove(ctx);
    }
    
    public void OnJumpIA(InputAction.CallbackContext ctx)
    {
        m_CurrentState.OnJump(ctx);
    }
    
    public void OnAbilityIA(InputAction.CallbackContext ctx)
    {
        m_CurrentState.OnAbility(ctx);
    }

    public void OnAbilitySwapIA(InputAction.CallbackContext ctx)
    {
        m_CurrentState.OnAbilitySwap(ctx);
    }

    // + + + + | Functions | + + + + 

    public void ChangeState(CharacterState newState)
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.OnExit();
        }
        m_CurrentState = newState;
        m_CurrentState.OnEnter();
    }

    // Wraps AdvanceState so that our Animation machine can use it.
    public void AdvanceState()
    {
        m_CurrentState.AdvanceState();
    }

    // Performs a double raycast from either end of the collider to determine if the player is grounded.
    // TODO: This function will need to be optimized.
    public void GroundCheck()
    {
        Vector3 leftCastPoint = m_BoxCollider.transform.position + new Vector3(-m_BoxCollider.size.x/2f, -m_BoxCollider.size.y/4f);
        Vector3 rightCastPoint = m_BoxCollider.transform.position + new Vector3(m_BoxCollider.size.x/2f, -m_BoxCollider.size.y/4f);
        Vector3 localDownDirection = transform.TransformDirection(Vector3.down);
        LayerMask mask = LayerMask.GetMask("StaticLevel", "Platforms");

        // Lefthand Cast
        //RaycastHit leftCastHit;
        Physics.Raycast(leftCastPoint, localDownDirection, out LeftCastHit, GroundCheckDistance, mask, QueryTriggerInteraction.Ignore);

        // Righthand Cast
        //RaycastHit rightCastHit;
        Physics.Raycast(rightCastPoint, localDownDirection, out RightCastHit, GroundCheckDistance, mask, QueryTriggerInteraction.Ignore);

        // Determine Grounded Logic - Are we hitting a collider on both casts?
        // TODO: Must this be the same collider?
        // TODO: What about hanging-over-ledge-logic? Worry about this later.
        
        IsGrounded = (LeftCastHit.collider && !LeftCastHit.collider.isTrigger) ||
                     (RightCastHit.collider && !RightCastHit.collider.isTrigger);
        LeftCastGO = (LeftCastHit.transform?.gameObject) ? LeftCastHit.transform.gameObject : null;
        RightCastGO = (RightCastHit.transform?.gameObject) ? RightCastHit.transform.gameObject : null;
    }

    public void SetFacingDirection(bool facingRight)
    {
        // Adjust ModelTransform Facing Direction
        //ModelTransform.localScale.Scale((facingRight ? Vector3.right : Vector3.left));

        ModelTransform.localScale = new Vector3(
        (facingRight ? Mathf.Abs(ModelTransform.localScale.x) : Mathf.Abs(ModelTransform.localScale.x) * -1f),
        ModelTransform.localScale.y,
        ModelTransform.localScale.z);

        //ModelTransform.localScale.Set(
        //    ModelTransform.localScale.x * (facingRight ? 1f : -1f),
        //    ModelTransform.localScale.y,
        //    ModelTransform.localScale.z);
    }

    // + + + + | Collision Methods | + + + + 

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }

    private void OnDrawGizmos()
    {
        // Double Raycast
        m_BoxCollider = GetComponent<BoxCollider>();
        Gizmos.color = Color.blue;
        Vector3 leftCastPoint = m_BoxCollider.transform.position + new Vector3(-m_BoxCollider.size.x/2f, -m_BoxCollider.size.y/3f);
        Vector3 rightCastPoint = m_BoxCollider.transform.position + new Vector3(m_BoxCollider.size.x/2f, -m_BoxCollider.size.y/3f);
        //Vector3 localDownDirection = transform.TransformDirection(Vector3.down);
        Gizmos.DrawLine(leftCastPoint, leftCastPoint - Vector3.up * GroundCheckDistance);
        Gizmos.DrawLine(rightCastPoint, rightCastPoint - Vector3.up * GroundCheckDistance);

        // Current CharacterState
        if (m_CurrentState != null)
        {
            switch (m_CurrentState)
            {
                case CharacterWalk:
                    Gizmos.color = Color.green;
                    break;
                case CharacterAir:
                    Gizmos.color = Color.yellow;
                    break;
                case CharacterPendulumState:
                    Gizmos.color = Color.cyan;
                    break;
                case CharacterHandsState:
                    Gizmos.color = Color.red;
                    break;
                case CharacterChimeState:
                    Gizmos.color = new(1f, 0.5f, 0f, 1f); // Orange!
                    break;
                case CharacterCuckooState:
                    Gizmos.color = new(1f, 0f, 1f, 1f); // Purple!
                    break;
            }

            Gizmos.DrawSphere(transform.position + Vector3.up, 0.25f);
        }
    }
}
