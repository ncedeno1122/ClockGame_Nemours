using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewIconBank", menuName = "ScriptableObjects/IconBank", order = 1)]
public class IconBankSO : ScriptableObject
{
    public Sprite PendulumIcon, HandsIcon, ChimeIcon, CuckooIcon, InvalidIcon;

    // Tries to return the proper Sprite icon for the enum.
    public Sprite GetIconFromAbilityEnumValue(int abilityEnumVal)
    {
        switch (abilityEnumVal)
        {
            case 0: // INVALID
                return InvalidIcon;
            case 1: // PENDULUM
                return PendulumIcon;
            case 2: // HANDS
                return HandsIcon;
            case 3: // CHIME
                return ChimeIcon;
            case 4: // CUCKOO
                return CuckooIcon;
        }
        return null;
    }
}
