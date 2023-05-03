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
        if (m_AbilityCRT != null)
        {
            m_AbilityCRT = AbilityCRT();
            StartCoroutine(m_AbilityCRT);
        }
    }

    // + + + + | Functions | + + + + 

    public IEnumerator AbilityCRT()
    {
        // Wait for AbilityEndTime
        for (float time = 0f; time < m_AbilityStartTime; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        // Tick Observers
        Debug.Log("Ticking Observers!");
        foreach (PendulumObserver observer in m_PendulumObservers)
        {
            observer.OnPendulum();
        }

        // Wait for AbilityEndTime
        for (float time = 0f; time < m_AbilityEndTime; time += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }

        // Signal for State Advance
        
    }

    public void AddObserver(PendulumObserver observer) => m_PendulumObservers.Add(observer);
    public void RemoveObserver(PendulumObserver observer) => m_PendulumObservers.Remove(observer);
}
