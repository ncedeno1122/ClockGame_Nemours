using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandsBoundedFreePlatformController : MonoBehaviour
{
    public float MoveSpeed = 5f;

    public Vector3 MoveVelocity { get; private set; }
    public Bounds PlatformBounds;

    [SerializeField] private Rigidbody m_Rigidbody;

    private void OnValidate()
    {
        PlatformBounds.center = transform.position;
    }

    private void Awake()
    {
        m_Rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        if (!PlatformBounds.Contains(m_Rigidbody.position)) // Bounds work in local space, it seems.
        {
            Debug.Log("Moving Rigidbody to center!");
            m_Rigidbody.position = PlatformBounds.center;
        }
    }

    private void FixedUpdate()
    {
        if (MoveVelocity == Vector3.zero) return;
        if (PlatformBounds.Contains(m_Rigidbody.position + MoveVelocity * (MoveSpeed * Time.deltaTime)))
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + MoveVelocity * (MoveSpeed * Time.deltaTime));
        }
        else
        {
            // Reposition
            MoveVelocity = Vector3.zero;
            m_Rigidbody.MovePosition(PlatformBounds.ClosestPoint(m_Rigidbody.position));
        }
    }

    // + + + + | Functions | + + + + 

    public void OnMove(Vector2 movementDirection)
    {
        MoveVelocity = movementDirection;
    }

    public void OnDrawGizmos()
    {
        // Draw Bounds
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(PlatformBounds.center, PlatformBounds.size);
        
        // Rigidbody
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponentInChildren<Rigidbody>();
        }

        Gizmos.color = Color.yellow;
        Debug.DrawLine(PlatformBounds.center, m_Rigidbody.position);
        
    }
}
