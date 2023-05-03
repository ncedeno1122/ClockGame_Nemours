using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls a set of CharacterStates that characters can use in a 2.5D environment.
/// </summary>

public class CharacterController2D : MonoBehaviour
{
    public float WalkSpeed = 5f;
    public float CurrentSpeed = 0f;
    public float JumpForce = 10f;
    public float GravityForce = -10f;
    public float AirSpeedX = 2f;
    public bool IsGrounded;
    public bool IsJumping = false;
    public bool IsFalling = false;
    public bool FacingRight = true;

    [Range(0.01f, 1f), SerializeField]
    private float m_GroundCheckDistance = 0.3f;

    public Vector2 PlayerMovementVector = Vector2.zero;
    public Vector3 CharacterVelocity = Vector3.zero;

    public RaycastHit LeftCastHit;
    public RaycastHit RightCastHit;
    public GameObject LeftCastGO;
    public GameObject RightCastGO;

    private BoxCollider m_BoxCollider;
    public Rigidbody Rigidbody;

    private CharacterState m_CurrentState;
    public CharacterState CurrentState { get => m_CurrentState; }

    private PlayerInput m_PlayerInput;
    private InputAction m_MoveIA, m_JumpIA, m_AbilityIA;

    public AbilityManager AbilityManager;

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();
        AbilityManager = GetComponent<AbilityManager>();

        m_MoveIA = m_PlayerInput.actions["Move"];
        m_JumpIA = m_PlayerInput.actions["Jump"];
        m_AbilityIA = m_PlayerInput.actions["Ability"];
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
        Physics.Raycast(leftCastPoint, localDownDirection, out LeftCastHit, m_GroundCheckDistance, mask);

        // Righthand Cast
        //RaycastHit rightCastHit;
        Physics.Raycast(rightCastPoint, localDownDirection, out RightCastHit, m_GroundCheckDistance, mask);

        // Determine Grounded Logic - Are we hitting a collider on both casts?
        // TODO: Must this be the same collider?
        // TODO: What about hanging-over-ledge-logic? Worry about this later.
        
        IsGrounded = (LeftCastHit.collider && !LeftCastHit.collider.isTrigger) ||
                     (RightCastHit.collider && !RightCastHit.collider.isTrigger);
        LeftCastGO = (LeftCastHit.transform?.gameObject) ? LeftCastHit.transform.gameObject : null;
        RightCastGO = (RightCastHit.transform?.gameObject) ? RightCastHit.transform.gameObject : null;
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
        Gizmos.DrawLine(leftCastPoint, leftCastPoint - Vector3.up * m_GroundCheckDistance);
        Gizmos.DrawLine(rightCastPoint, rightCastPoint - Vector3.up * m_GroundCheckDistance);

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
            }

            Gizmos.DrawSphere(transform.position + Vector3.up, 0.25f);
        }
    }
}
