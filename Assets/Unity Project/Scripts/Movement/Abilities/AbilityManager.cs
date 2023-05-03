using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages a list of equipped abilities and allows the CharacterController
/// to use and manipulate certain ability scripts.
/// </summary>
public class AbilityManager : MonoBehaviour
{
    private int m_CurrAbilityIndex = 0;

    public Ability CurrentAbility { get; private set; }
    public PendulumAbility PendulumAbility { get; private set; }

    public List<Ability> TotalAbilities = new();

    private void Awake()
    {
        // Get individual components
        PendulumAbility = GetComponent<PendulumAbility>();

        // Find and Equip Abilities
        TotalAbilities.AddRange(GetComponents<Ability>());
        if (TotalAbilities.Count > 0)
        {
            CurrentAbility = TotalAbilities[0];
        }
    }

    // + + + + | Functions | + + + + 

    // TODO: Test these fools

    public void NextAbility()
    {
        // Map ability index
        m_CurrAbilityIndex = TotalAbilities.Count % (++m_CurrAbilityIndex);
        CurrentAbility = TotalAbilities[m_CurrAbilityIndex];
    }

    public void PreviousAbility()
    {
        // Map ability index
        m_CurrAbilityIndex = TotalAbilities.Count % (--m_CurrAbilityIndex);
        CurrentAbility = TotalAbilities[m_CurrAbilityIndex];
    }
}
