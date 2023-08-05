using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandsLinePlatformController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float m_NormalizedPositionValue;
    public float NormalizedPositionValue => m_NormalizedPositionValue;

    private float m_NormalizedPositionVelocity = 0f;
    public float MoveSpeed = 0.35f;
    [SerializeField] private Vector2 m_DirectionFromTowardsTo = new();
    public Vector3 MoveVelocity;

    [SerializeField] private Rigidbody m_PlatformRb;

    public Transform FromPositionTf, ToPositionTf, CosmeticTrackMeshTf;

    private void OnValidate()
    {
        Awake();
    }

    private void Awake()
    {
        // Get Rigidbody
        m_PlatformRb = GetComponentInChildren<Rigidbody>();
        
        // Get From & To Position
        foreach (Transform tf in transform)
        {
            switch (tf.name)
            {
                case "FromPosition":
                    FromPositionTf = tf;
                    break;
                case "ToPosition":
                    ToPositionTf = tf;
                    break;
                case "CosmeticTrackMesh":
                    CosmeticTrackMeshTf = tf;
                    break;
                default:
                    break;
            }
        }

        if (FromPositionTf == null || ToPositionTf == null)
        {
            Debug.LogWarning($"Null transforms for {gameObject.name}! FromPositionTf: {FromPositionTf}, ToPositionTf: {ToPositionTf}");
        }
        else
        {
            // Start at the proper location
            m_PlatformRb.position = Vector3.Lerp(FromPositionTf.position, ToPositionTf.position, m_NormalizedPositionValue);
            m_PlatformRb.transform.position = Vector3.Lerp(FromPositionTf.position, ToPositionTf.position, m_NormalizedPositionValue);
            m_DirectionFromTowardsTo = (ToPositionTf.position - FromPositionTf.position).normalized;
        }
        
        // Size CosmeticTrackMesh
        CosmeticTrackMeshTf.position = Vector3.Lerp(ToPositionTf.position, FromPositionTf.position, 0.5f);
        CosmeticTrackMeshTf.localScale = new Vector3(
            0.25f,
            0.25f,
            Vector3.Distance(ToPositionTf.position, FromPositionTf.position));
        CosmeticTrackMeshTf.LookAt(ToPositionTf);
        CosmeticTrackMeshTf.localPosition += Vector3.forward; // Send it away from the camera
    }

    void Start()
    {
        
        
    }

    private void FixedUpdate()
    {
        // Update Position with velocity
        MoveVelocity = m_DirectionFromTowardsTo * m_NormalizedPositionVelocity;
        m_NormalizedPositionValue = Mathf.Clamp01(m_NormalizedPositionValue + (m_NormalizedPositionVelocity * (Time.deltaTime * MoveSpeed)));
        m_PlatformRb.position = Vector3.Lerp(FromPositionTf.position, ToPositionTf.position, m_NormalizedPositionValue);
    }

    // + + + + | Functions | + + + +

    public void OnMove(Vector2 movementDirection)
    {
        // TODO: Should REALLY make an interface for this :D
        Vector2 normalizedMoveDirection = movementDirection.normalized;
        float dotValue = Vector2.Dot(m_DirectionFromTowardsTo, normalizedMoveDirection);
        
        //Debug.Log($"Dot Value is {dotValue} for DirectionFromTowardsTo {m_DirectionFromTowardsTo} and MoveDirection {normalizedMoveDirection}");

        if (dotValue == 0f) m_NormalizedPositionVelocity = 0f;
        else
        {
            // Move velocity from the From towards the To Position ALWAYS!
            m_NormalizedPositionVelocity = 1f * (dotValue > 0f ? 1f: -1f) * (m_DirectionFromTowardsTo.y > 0f ? 1f : -1f);
        }
    }

    public void OnDrawGizmos()
    {
        //Awake();
        if (FromPositionTf && ToPositionTf)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(FromPositionTf.position, Vector3.one * 0.25f);
            Gizmos.DrawCube(ToPositionTf.position, Vector3.one * 0.25f);
            Gizmos.DrawLine(FromPositionTf.position, ToPositionTf.position);
        }
    }
}
