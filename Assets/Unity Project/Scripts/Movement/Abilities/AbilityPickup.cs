using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public AbilityType AbilityToGive;
    private SpriteRenderer m_SR;

    private void OnValidate()
    {
        // Try set proper Icons!
        SpriteRenderer sr = (m_SR != null) ? m_SR : GetComponent<SpriteRenderer>();
        m_SR = sr;
        if (sr && !sr.sprite.Equals(LevelManager.Instance.IconBank.GetIconFromAbilityEnumValue((int) AbilityToGive)))
        {
            sr.sprite = LevelManager.Instance.IconBank.GetIconFromAbilityEnumValue((int)AbilityToGive);
        }
    }

    private void Awake()
    {
        m_SR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Set Icon
        m_SR.sprite = LevelManager.Instance.IconBank.GetIconFromAbilityEnumValue((int)AbilityToGive);
    }

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
