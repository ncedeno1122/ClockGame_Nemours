using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public AbilityType AbilityToGive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager am = other.GetComponent<AbilityManager>();
            if (am && !am.EnabledAbilities.Exists(x=>x.AbilityType == AbilityToGive))
            {
                // Give to the Player!
                Debug.Log($"Found Player, giving them {AbilityToGive}!");
                am.EnableAbility(am.TotalAbilities[(int)AbilityToGive - 1]);
                gameObject.SetActive(false);
            }
        }
    }
}
