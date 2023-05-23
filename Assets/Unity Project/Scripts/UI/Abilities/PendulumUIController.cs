using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumUIController : MonoBehaviour, IActivatableUI
{
    private bool StartsFromRight = true;
    public bool IsAnimating { get; private set; }
    
    private bool m_IsActivated;
    public bool IsActivated { get => m_IsActivated; set => m_IsActivated = value; }

    public float SwingAngleOffset = 45f;
    public float VisibleAlpha = 0.85f;

    private IEnumerator m_PendulumAnimationCRT;

    private RectTransform m_PendulumTf;
    private CanvasGroup m_CanvasGroup;

    private void OnEnable()
    {
        OnActivate();
    }

    private void OnDisable()
    {
        OnDeactivate();
    }

    private void Start()
    {
        m_PendulumTf = transform.GetChild(0).GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();

        // Initialize
        m_CanvasGroup.alpha = 0f;
    }

    private void OnValidate()
    {
        if (m_PendulumTf == null)
        {
            m_PendulumTf = transform.GetChild(0).GetComponent<RectTransform>();
        }
    }

    // + + + + | Functions | + + + +

    public void ToggleSwing(float animationTime)
    {
        // Reject if inactive
        if (!m_IsActivated) return;

        // Clear / Stop CRT if already running...
        if (m_PendulumAnimationCRT != null)
        {
            StopCoroutine(m_PendulumAnimationCRT);
            m_PendulumAnimationCRT = null;
        }

        // Start the CRT
        StartsFromRight = !StartsFromRight;
        m_PendulumAnimationCRT = PendulumSwingCRT(animationTime);
        StartCoroutine(m_PendulumAnimationCRT);
    }

    private IEnumerator PendulumSwingCRT(float animationTime)
    {
        // Initialize
        IsAnimating = true;
        //m_CanvasGroup.alpha = 0.3f;
        float currRotationValue = SwingAngleOffset * (StartsFromRight ? 1f : -1f);
        float targetRotationValue = SwingAngleOffset * (StartsFromRight ? -1f : 1f);

        for (float timeHelper = 0f; timeHelper < animationTime; timeHelper += Time.deltaTime)
        {
            m_PendulumTf.rotation = Quaternion.Euler(Vector3.forward * Mathf.SmoothStep(currRotationValue, targetRotationValue, timeHelper / animationTime));

            // TODO: MUST be a better way to do this...
            if (timeHelper < animationTime / 2f)
            {
                m_CanvasGroup.alpha = Mathf.SmoothStep(0f, VisibleAlpha, timeHelper / (animationTime / 4f));
            }
            else
            {
                m_CanvasGroup.alpha = Mathf.SmoothStep(VisibleAlpha, 0f, timeHelper / (animationTime));
            }

            yield return new WaitForEndOfFrame();
        }

        // Terminate
        IsAnimating = false;
        m_CanvasGroup.alpha = 0f;
        StopCoroutine(m_PendulumAnimationCRT);
    }

    public void OnActivate()
    {
        m_IsActivated = true;
    }

    public void OnDeactivate()
    {
        m_IsActivated = false;
        m_CanvasGroup.alpha = 0f;
    }
}
