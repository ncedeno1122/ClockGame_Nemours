using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePositionBlockScript : MonoBehaviour
{
    public bool ToggledOn;
    public float MoveTime = 0.25f;
    public bool IsMoving { get; private set; }
    private Vector3 m_OriginPosition;
    public Vector3 OnOffset;
    public Vector3 OffOffset;

    public List<Rigidbody> EntitiesOnBlock = new();

    private IEnumerator m_MoveCRT;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        // Set Origin
        m_OriginPosition = transform.position;
    }

    private void Start()
    {
        if (ToggledOn)
        {
            m_Rigidbody.position = m_OriginPosition + OnOffset;
        } else
        {
            m_Rigidbody.position = m_OriginPosition + OffOffset;
        }
    }

    private void OnValidate()
    {
        // Align in Editor
        if (m_OriginPosition != transform.position)
        {
            m_OriginPosition = transform.position;
        }
    }

    // + + + + | Functions | + + + + 

    public void Toggle()
    {
        // Toggle Variable
        if (!IsMoving)
        {
            ToggledOn = !ToggledOn;
            MoveToPosition();
        }
    }

    private void MoveToPosition()
    {
        if (m_MoveCRT == null)
        {
            m_MoveCRT = MoveToCRT(ToggledOn);
            StartCoroutine(m_MoveCRT);
        }
        else
        {
            StopCoroutine(m_MoveCRT);
            m_MoveCRT = null;
        }
    }

    private IEnumerator MoveToCRT(bool toggleOn)
    {
        Vector3 originPos = m_OriginPosition + (toggleOn ? OffOffset : OnOffset);
        Vector3 targetPos = m_OriginPosition + (toggleOn ? OnOffset : OffOffset);
        IsMoving = true;

        for (float time = 0f; time < MoveTime; time += Time.deltaTime)
        {
            m_Rigidbody.MovePosition(Vector3.Lerp(originPos, targetPos, time / MoveTime));

            //foreach (Rigidbody rb in EntitiesOnBlock)
            //{
            //    Vector3 relativePosition = targetPos - originPos;
            //    Debug.DrawLine(m_Rigidbody.position, m_Rigidbody.position + relativePosition, Color.magenta);
            //    //m_Rigidbody.MovePosition(m_Rigidbody.position + Vector3.Lerp(Vector3.zero, targetPos - originPos, time / MoveTime));
            //}

            yield return new WaitForFixedUpdate();
        }

        // Terminate CRT
        IsMoving = false;
        StopCoroutine(m_MoveCRT);
        m_MoveCRT = null;
    }

    // + + + + | Collision Handling | + + + + 

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(m_OriginPosition + OnOffset, Vector3.one * 0.25f);
        Gizmos.DrawLine(m_OriginPosition + OnOffset, m_OriginPosition + OffOffset);
        Gizmos.DrawCube(m_OriginPosition + OffOffset, Vector3.one * 0.25f);

    }
}
