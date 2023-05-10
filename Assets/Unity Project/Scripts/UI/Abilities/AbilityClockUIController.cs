using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityClockUIController : MonoBehaviour
{
    public AbilityManager m_AbilityManager;
    public RectTransform AbilityIconsTF;
    public RectTransform ClockHandTF;
    public GameObject AbilityIconPrefab;
    public TextMeshProUGUI DebugAbilityText;

    private Dictionary<Ability, RectTransform> m_AbilityIconDictionary = new();

    private void Awake()
    {
        AbilityIconsTF = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        ClockHandTF = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        ClockHandTF.Rotate(0f, 0f, 1f * Time.deltaTime);
    }

    private void OnValidate()
    {
        if (m_AbilityIconDictionary.Count > 0)
        {
            m_AbilityIconDictionary.Clear();
            foreach (RectTransform tf in AbilityIconsTF)
            {
                Destroy(tf);
            }
        }

        if (!AbilityIconsTF)
        {
            Awake();
        }
    }

    // + + + + | Functions | + + + +

    public void RegisterAbilityManager(AbilityManager abilityManager)
    {
        if (m_AbilityManager != null) return;
        m_AbilityManager = abilityManager;

        // Create Sprite for each
        foreach (Ability ability in m_AbilityManager.TotalAbilities)
        {
            GameObject newIcon = Instantiate(AbilityIconPrefab, AbilityIconsTF);
            newIcon.GetComponent<RectTransform>().SetParent(AbilityIconsTF, false);
            newIcon.name = $"{ability.name}_Icon";
            m_AbilityIconDictionary.Add(ability, newIcon.GetComponent<RectTransform>());
            Debug.Log($"Added {ability} to Dictionary!");
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        // Show or Hide Enabled Sprites
        //foreach (Ability ability in m_AbilityManager.TotalAbilities)
        //{
        //    // Set Active
        //    m_AbilityIconDictionary.TryGetValue(ability, out RectTransform abilityIconTF);
        //    if (abilityIconTF)
        //    {
        //        abilityIconTF.gameObject.SetActive(ability.IsEnabled);
        //    }
        //}

        // Then, display them properly on the clock
        //List<Ability> enabledAbilities = m_AbilityManager.TotalAbilities.FindAll(x => x.IsEnabled);
        //for (int i = 0; i < enabledAbilities.Count; i++)
        //{
        //    Ability currAbility = enabledAbilities[i];
        //    m_AbilityIconDictionary.TryGetValue(currAbility, out RectTransform abilityIconTF);
        //    if (abilityIconTF)
        //    {
        //        abilityIconTF.position = new Vector3(
        //            Mathf.Cos(((i / 360f) / 360f) * Mathf.Deg2Rad) * 50f,
        //            Mathf.Sin(((i / 360f) / 360f) * Mathf.Deg2Rad) * 50f );
        //    }
        //}

        DebugAbilityText.text = m_AbilityManager.CurrentAbility.GetType().ToString();
    }
}
