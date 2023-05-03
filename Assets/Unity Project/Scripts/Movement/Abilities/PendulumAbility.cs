using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Maintains a list of PendulumObservers and 'ticks' them when the ability is activated.
/// </summary>
public class PendulumAbility : Ability
{
    private float m_AbilityStartTime = 0.2f;
    private float m_AbilityEndTime = 0.2f;
    private List<PendulumObserver> m_PendulumObservers = new List<PendulumObserver>();
    private IEnumerator m_AbilityCRT;

    public override void OnAbility()
    {
        ActivateAbility();
    }

    public override void OnAbility(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ActivateAbility();
        }
    }

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
            StopCoroutine(m_AbilityCRT);
            m_AbilityCRT = null;
            ActivateAbility();
        }
    }

    // + + + + | Functions | + + + + 

    public IEnumerator AbilityCRT()
    {
        // Wait for AbilityStartTime
        //Debug.Log("Waiting for AbilityStartTime");
        for (float time = 0f; time < m_AbilityStartTime; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        // Tick Observers
        //Debug.Log("Ticking Observers!");
        foreach (PendulumObserver observer in m_PendulumObservers)
        {
            observer.OnPendulum();
        }

        // Wait for AbilityEndTime
        //Debug.Log("Waiting for AbilityEndTime");
        for (float time = 0f; time < m_AbilityEndTime; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        //Debug.Log("Advancing State");
        // Signal for State Advance
        if (m_CC2D.CurrentState is CharacterPendulumState cps)
        {
            cps.AdvanceState();
        }
        else
        {
            Debug.LogError($"Cannot signal to leave Pendulum State - current state is {m_CC2D.CurrentState.GetType().ToString()}!");
        }

        // Terminate the CRT
        StopAbilityCRT();
    }

    public void AddObserver(PendulumObserver observer) => m_PendulumObservers.Add(observer);
    public void RemoveObserver(PendulumObserver observer) => m_PendulumObservers.Remove(observer);

    public void StopAbilityCRT()
    {
        if (m_AbilityCRT != null)
        {
            StopCoroutine(m_AbilityCRT);
            m_AbilityCRT = null;
        }
    }
}
