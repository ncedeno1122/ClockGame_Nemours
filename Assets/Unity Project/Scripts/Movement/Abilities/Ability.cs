using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{
    public bool IsEnabled;
    public AbilityType AbilityType
    {
        get
        {
            switch (this)
            {
                case PendulumAbility:
                    return AbilityType.PENDULUM;
                case HandsAbility:
                    return AbilityType.HANDS;
                case ChimeAbility:
                    return AbilityType.CHIME;
                case CuckooAbility:
                    return AbilityType.CUCKOO;
                default:
                    return AbilityType.INVALID;
            }
        }
    }

    protected CharacterController2D m_CC2D;

    protected void Start()
    {
        m_CC2D = GetComponent<CharacterController2D>();
    }

    public abstract void OnAbility();
    public abstract void OnAbility(InputAction.CallbackContext ctx);
}
