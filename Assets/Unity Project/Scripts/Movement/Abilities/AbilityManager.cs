using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages a list of equipped abilities and allows the CharacterController
/// to use and manipulate certain ability scripts.
/// </summary>
public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private int m_CurrAbilityIndex = 0;
    public int CurrAbilityIndex { get => m_CurrAbilityIndex; private set => m_CurrAbilityIndex = value; } 

    public Ability CurrentAbility { get; private set; }
    public PendulumAbility PendulumScript { get; private set; }
    public HandsAbility HandsScript { get; private set; }
    public ChimeAbility ChimeScript { get; private set; }
    public CuckooAbility CuckooScript { get; private set; }

    public AbilityClockUIController AbilityClockUIController;

    public Ability[] TotalAbilities;
    public List<Ability> EnabledAbilities = new();

    private void Awake()
    {
        // Get individual components
        PendulumScript = GetComponent<PendulumAbility>();
        HandsScript = GetComponent<HandsAbility>();
        ChimeScript = GetComponent<ChimeAbility>();
        CuckooScript = GetComponent<CuckooAbility>();

        // Find and Equip Abilities
        TotalAbilities = new Ability[] { PendulumScript, HandsScript, ChimeScript, CuckooScript };

        // Search for Inspector-enabled Abilities
        foreach (Ability ability in TotalAbilities)
        {
            if (ability.IsEnabled && !EnabledAbilities.Contains(ability))
            {
                Debug.Log($"Found ability {ability.AbilityType} from Inspector. Enabling!");
                EnableAbility(ability);
            }
        }

        // Set CurrentAbility
        if (EnabledAbilities.Count > 0)
        {    
            CurrentAbility = EnabledAbilities[0];
        }
    }

    private void Start()
    {
        AbilityClockUIController.RegisterAbilityManager(this);

        //EnableAbility(PendulumScript);
        //EnableAbility(HandsScript);
        //EnableAbility(ChimeScript);
        //EnableAbility(CuckooScript);
    }

    // + + + + | Functions | + + + + 

    public void NextAbility()
    {
        // Map ability index
        m_CurrAbilityIndex = (int) Mathf.Repeat(m_CurrAbilityIndex + 1, EnabledAbilities.Count);
        CurrentAbility = EnabledAbilities[m_CurrAbilityIndex];
        Debug.Log($"NextAbility! CurrentAbility is {CurrentAbility}");
        AbilityClockUIController.UpdateUI();
    }

    public void PreviousAbility()
    {
        // Map ability index
        m_CurrAbilityIndex = (int)Mathf.Repeat(m_CurrAbilityIndex - 1, EnabledAbilities.Count);
        CurrentAbility = EnabledAbilities[m_CurrAbilityIndex];
        Debug.Log($"PreviousAbility! CurrentAbility is {CurrentAbility}");
        AbilityClockUIController.UpdateUI();
    }

    public void EnableAbility(Ability ability)
    {
        switch (ability.AbilityType)
        {
            case AbilityType.PENDULUM:
                PendulumScript.IsEnabled = true;
                break;
            case AbilityType.HANDS:
                HandsScript.IsEnabled = true;
                break;
            case AbilityType.CHIME:
                ChimeScript.IsEnabled = true;
                break;
            case AbilityType.CUCKOO:
                CuckooScript.IsEnabled = true;
                break;
            default:
                return; // DON'T add invalid ability to list...
        }
        //Debug.Log($"Enabled {ability.AbilityType}!");
        EnabledAbilities.Add(ability);
        AbilityClockUIController.UpdateUI();
    }
}
