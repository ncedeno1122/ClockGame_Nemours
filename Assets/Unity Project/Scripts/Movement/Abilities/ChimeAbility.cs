using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChimeAbility : Ability
{
    private float m_AbilityStartTime = 0.125f;
    private float m_AbilityEndTime = 0.125f;
    [SerializeField]
    private float m_ChimeRadius = 1f;

    private Collider[] m_SphereCastArr = new Collider[7]; // PROBABLY won't need more than 7 Chime-able things at once...
    private List<ChimeObserver> m_ChimeObservers = new List<ChimeObserver>();
    private IEnumerator m_AbilityCRT;

    private void OnDisable()
    {
        m_ChimeObservers.Clear();
    }

    public override void OnAbility()
    {
        ActivateAbility();
    }

    public override void OnAbility(InputAction.CallbackContext ctx)
    {
        //
    }

    // + + + + | Functions | + + + + 

    private void ActivateAbility()
    {
        if (m_AbilityCRT == null)
        {
            //Debug.Log("Starting AbilityCRT!");
            m_AbilityCRT = AbilityCRT();
            StartCoroutine(m_AbilityCRT);
        }
        else
        {
            //Debug.LogWarning("AbilityCRT wasn't null..?");
            StopAbilityCRT();
            ActivateAbility();
        }
    }

    public IEnumerator AbilityCRT()
    {
        // Clear Previous Results!
        ClearColliderArray();

        // Wait for AbilityStartTime
        //Debug.Log("Waiting for AbilityStartTime");
        for (float time = 0f; time < m_AbilityStartTime; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        // SphereCast && Notify Observers!
        Physics.OverlapSphereNonAlloc(m_CC2D.Rigidbody.position, m_ChimeRadius, m_SphereCastArr, LayerMask.GetMask("Entities"), QueryTriggerInteraction.Collide);
        if (m_SphereCastArr != null && m_SphereCastArr.Length > 0)
        {
            for (int i = 0; i < m_SphereCastArr.Length; i++)
            {
                Collider currCollider = m_SphereCastArr[i];
                if (currCollider == null) break;

                ChimeObserver co = currCollider.gameObject.GetComponent<ChimeObserver>();
                if (co == null) break;
                co.OnChime();
            }
        }


        // Wait for AbilityEndTime
        //Debug.Log("Waiting for AbilityEndTime");
        for (float time = 0f; time < m_AbilityEndTime; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        //Debug.Log("Advancing State");
        // Signal for State Advance
        if (m_CC2D.CurrentState is CharacterChimeState cps)
        {
            cps.AdvanceState();
        }
        else
        {
            Debug.LogError($"Cannot signal to leave Chime State - current state is {m_CC2D.CurrentState.GetType().ToString()}!");
        }

        // Terminate the CRT
        StopAbilityCRT();
    }

    public void AddObserver(ChimeObserver observer) => m_ChimeObservers.Add(observer);
    public void RemoveObserver(ChimeObserver observer) => m_ChimeObservers.Remove(observer);

    public void StopAbilityCRT()
    {
        if (m_AbilityCRT != null)
        {
            StopCoroutine(m_AbilityCRT);
            m_AbilityCRT = null;
        }
    }

    private void ClearColliderArray()
    {
        for (int i = 0; i < m_SphereCastArr.Length; i++)
        {
            m_SphereCastArr[i] = null;
        }
    }
}
