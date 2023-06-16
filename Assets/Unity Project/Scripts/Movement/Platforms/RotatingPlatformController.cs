using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingPlatformController : MonoBehaviour
{
    [SerializeField] private bool m_IsRotatingSingleCall = false;
    [SerializeField] private float m_CurrentMasterRotation = 0f; // TODO: Clamp this value?
    public float AutonomousDeltaTimeScalar = 10f; // For Autonomous Rotation
    public float OnTriggerDeltaTimeScalar = 60f; // For Triggered/SingleCall Rotation
    public float PlatformRadius = 4f;
    public bool RotatesAutonomously = true;

    [SerializeField] private Transform m_PlatformParentTF;
    //[SerializeField] private Rigidbody m_ParentRb;
    [SerializeField] private Rigidbody[] m_PlatformRbArray;

    private void Awake()
    {
        m_PlatformParentTF = transform.GetChild(0);
        //m_ParentRb = m_PlatformParentTF.GetComponent<Rigidbody>();
        m_PlatformRbArray = m_PlatformParentTF.GetComponentsInChildren<Rigidbody>();
    }

    private void OnValidate()
    {
        Awake();
        InitializePlatformPositions();
    }

    private void FixedUpdate()
    {
        if (!RotatesAutonomously) return;

        m_CurrentMasterRotation += Time.fixedDeltaTime * AutonomousDeltaTimeScalar;

        // Test Rotation
        float platAngleOffset;
        Rigidbody platRb;
        for (int i = 0; i < m_PlatformRbArray.Length; i++)
        {
            platRb = m_PlatformRbArray[i];
            platAngleOffset = i * (360f / m_PlatformRbArray.Length);

            //

            platRb.MovePosition(m_PlatformParentTF.position + new Vector3(
                Mathf.Cos((m_CurrentMasterRotation + platAngleOffset) * Mathf.Deg2Rad) * PlatformRadius, // TODO: Introduce Offset member var
                Mathf.Sin((m_CurrentMasterRotation + platAngleOffset) * Mathf.Deg2Rad) * PlatformRadius,
                0f));
        }
    }

    // + + + + | Functions | + + + + 

    /// <summary>
    /// Positions platforms evenly around a center point.
    /// </summary>
    public void InitializePlatformPositions()
    {
        float platAngleOffset;
        Rigidbody currPlatform;

        for (int i = 0; i < m_PlatformRbArray.Length; i++)
        {
            currPlatform = m_PlatformRbArray[i];
            platAngleOffset = i * (360f / m_PlatformRbArray.Length);

            currPlatform.transform.position = m_PlatformParentTF.position + new Vector3(
                Mathf.Cos(platAngleOffset * Mathf.Deg2Rad) * PlatformRadius,
                Mathf.Sin(platAngleOffset * Mathf.Deg2Rad) * PlatformRadius,
                0f);
        }
    }

    public void HandleRotateSingleCall()
    {
        // Don't try if we're already rotating!
        if (m_IsRotatingSingleCall) return;

        IEnumerator rotateSingleCallCRT = RotateSingleCall();
        StartCoroutine(rotateSingleCallCRT);
    }

    private IEnumerator RotateSingleCall() // TODO: Implement this properly, please LOL
    {
        float degreesToRotate = (360f / m_PlatformRbArray.Length);
        float targetRotValue = m_CurrentMasterRotation + degreesToRotate;

        m_IsRotatingSingleCall = true;

        // Test Rotation
        float platAngleOffset;
        Rigidbody platRb;

        for (float rotHelper = m_CurrentMasterRotation; rotHelper <= targetRotValue; rotHelper += (Time.fixedDeltaTime * OnTriggerDeltaTimeScalar))
        {
            for (int i = 0; i < m_PlatformRbArray.Length; i++)
            {
                platRb = m_PlatformRbArray[i];
                platAngleOffset = i * (360f / m_PlatformRbArray.Length);
                //

                platRb.MovePosition(m_PlatformParentTF.position + new Vector3(
                    Mathf.Cos((rotHelper + platAngleOffset) * Mathf.Deg2Rad) * PlatformRadius, // TODO: Introduce Offset member var
                    Mathf.Sin((rotHelper + platAngleOffset) * Mathf.Deg2Rad) * PlatformRadius,
                    0f));
            }

            m_CurrentMasterRotation = rotHelper;
            yield return new WaitForFixedUpdate();
        }


        m_IsRotatingSingleCall = false;
        //Debug.Log($"Done RotatingSingleCall!");
        yield return null;
    }

}
