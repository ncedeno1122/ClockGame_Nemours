using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityClockUIController : MonoBehaviour, IActivatableUI
{
    private bool m_IsActivated;
    public bool IsActivated { get => m_IsActivated; set => m_IsActivated = value; }

    private CanvasGroup m_CanvasGroup;
    public AbilityManager m_AbilityManager;
    public RectTransform AbilityIconsTF;
    public RectTransform[] m_AbilityIconTFs;
    public RectTransform ClockHandTF;
    public GameObject AbilityIconPrefab;
    public TextMeshProUGUI DebugAbilityText;
    public IconBankSO IconBank;


    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        AbilityIconsTF = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        ClockHandTF = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        // Subscribe to events
        GameManager.Instance.CurrentLevelManager.OnLevelStarted.AddListener(OnActivate);
        GameManager.Instance.CurrentLevelManager.OnLevelEnded.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        if (GameManager.Exists && GameManager.Instance.CurrentLevelManager != null)
        {
            GameManager.Instance.CurrentLevelManager?.OnLevelStarted.RemoveListener(OnActivate);
            GameManager.Instance.CurrentLevelManager?.OnLevelEnded.RemoveListener(OnDeactivate);
        }
    }

    // + + + + | Functions | + + + +

    public void RegisterAbilityManager(AbilityManager abilityManager)
    {
        // Assign AbilityManager
        m_AbilityManager = abilityManager;

        // Create Icon Array
        m_AbilityIconTFs = new RectTransform[m_AbilityManager.TotalAbilities.Length];
        //Debug.Log($"Created Ability icon array with length {m_AbilityIconTFs.Length} from OG length {m_AbilityManager.TotalAbilities.Length}");

        // Create Sprite for each
        for (int i = 0; i < m_AbilityIconTFs.Length; i++)
        {
            GameObject newIconPrefab = Instantiate(AbilityIconPrefab, AbilityIconsTF);
            newIconPrefab.name = m_AbilityManager.TotalAbilities[i].AbilityType + "_Icon";
            // TODO: Add sprite from some ScriptableObject that stores icons
            Sprite iconSprite;
            switch (m_AbilityManager.TotalAbilities[i].AbilityType)
            {
                case AbilityType.PENDULUM:
                    iconSprite = IconBank.PendulumIcon;
                    break;
                case AbilityType.HANDS:
                    iconSprite = IconBank.HandsIcon;
                    break;
                case AbilityType.CHIME:
                    iconSprite = IconBank.ChimeIcon;
                    break;
                case AbilityType.CUCKOO:
                    iconSprite = IconBank.CuckooIcon;
                    break;
                default:
                    iconSprite = IconBank.InvalidIcon;
                    break;
            }
            newIconPrefab.GetComponent<Image>().sprite = iconSprite;
            //Debug.Log($"Prefab {newIconPrefab.name} has RectTransform {newIconPrefab.GetComponent<RectTransform>()}");
            m_AbilityIconTFs[i] = newIconPrefab.GetComponent<RectTransform>();
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        // Reject if inactive
        if (!m_IsActivated) return;

        // Reject if unbound
        if (m_AbilityManager == null)
        {
            Debug.LogWarning("Cannot update AbilityClockUI, no AbilityManager is registered!");
            return;
        }

        // Show or Hide Enabled Sprites
        UpdateEnabledSprites();
        RepositionClockFaceSprites();

        //DebugAbilityText.text = m_AbilityManager.CurrentAbility.GetType().ToString();
    }

    private void RepositionClockFaceSprites()
    {
        // Then, display them properly on the clock
        int enabledAbilities = m_AbilityManager.EnabledAbilities.Count;
        int enabledAbilityIndex = 0; // Used to set icon position relative to top of circle!
        foreach (Ability ability in m_AbilityManager.EnabledAbilities)
        {
            int totalAbilityIndex = (int)ability.AbilityType - 1;
            m_AbilityIconTFs[totalAbilityIndex].anchoredPosition = new Vector3(
                    Mathf.Sin((enabledAbilityIndex * (360f / enabledAbilities)) * Mathf.Deg2Rad) * 50f,
                    Mathf.Cos((enabledAbilityIndex * (360f / enabledAbilities)) * Mathf.Deg2Rad) * 50f); // Sin for X, then Cos for Y to have icons align to the top!
            enabledAbilityIndex++;
        }

        // Then, move the hand to the current ability.
        int currAbilityIndex = m_AbilityManager.CurrAbilityIndex; // Use enum for this! But the enum MUST be in proper order!!!
        float clockHandAngle = (currAbilityIndex * (360f / enabledAbilities)) * -1f;
        ClockHandTF.rotation = Quaternion.Euler(Vector3.forward * clockHandAngle);
    }

    private void UpdateEnabledSprites()
    {
        for (int i = 0; i < m_AbilityManager.TotalAbilities.Length; i++)
        {
            if (m_AbilityManager.TotalAbilities[i].IsEnabled)
            {
                m_AbilityIconTFs[i].gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                m_AbilityIconTFs[i].gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }

    public void OnActivate()
    {
        m_IsActivated = true;
        m_CanvasGroup.alpha = 1;
        UpdateUI();
    }

    public void OnDeactivate()
    {
        m_IsActivated = false;
        m_CanvasGroup.alpha = 0f;
    }
}
