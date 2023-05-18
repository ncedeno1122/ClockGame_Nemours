using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public AbilityType AbilityToGive;
    private SpriteRenderer m_SR;

    private void Awake()
    {
        m_SR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Set Icon
        m_SR.sprite = GameManager.Instance.IconBank.GetIconFromAbilityEnumValue((int)AbilityToGive);
    }

    // + + + + | Functions | + + + +

    /// <summary>
    /// Retrieves the Player's AbilityManager and gives them this AbilityType.
    /// </summary>
    public void OnPickup()
    {
        AbilityManager am = GameManager.Instance.CurrentLevelManager.Player.gameObject.GetComponent<AbilityManager>();
        if (am && !am.EnabledAbilities.Exists(x => x.AbilityType == AbilityToGive))
        {
            // Give to the Player!
            //Debug.Log($"Found Player, giving them {AbilityToGive}!");
            am.EnableAbility(am.TotalAbilities[(int)AbilityToGive - 1]);
            gameObject.SetActive(false);
        }
    }
}
