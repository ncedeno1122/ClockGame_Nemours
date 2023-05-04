using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsMovePlatformScript : MonoBehaviour
{
    public float FlySpeed = 5;
    public Vector3 MoveVelocity = Vector3.zero;

    private Rigidbody m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (MoveVelocity == Vector3.zero) return;
        m_Rigidbody.MovePosition(m_Rigidbody.position + MoveVelocity * (FlySpeed * Time.deltaTime));
    }

    // + + + + | Functions | + + + +

    // Called from the HandsObserverScript
    public void Move(Vector2 movementDirection)
    {
        MoveVelocity = movementDirection;
    }
}
