using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEndPanelController : MonoBehaviour, IActivatableUI
{
    private bool m_IsActivated = false;
    public bool IsActivated { get => m_IsActivated; set => m_IsActivated = value; }

    private Vector3 m_LevelClockPanelPosition = new(-2.5f, 0f, 5f);

    private IEnumerator m_MenuOpenCRT;

    private CanvasGroup m_CanvasGroup, m_ButtonsPanelCanvasGroup;

    private Transform m_UICameraTF, m_LevelClockTF, m_ButtonPanelTF;
    private Button m_NextLevelButton, m_QuitAppButton;
    private GameObject m_LevelClockGO;
    private TextMeshProUGUI m_LevelClockText;

    private void Awake()
    {
        // Get Transforms
        m_ButtonPanelTF = transform.GetChild(1).GetChild(1);
        m_UICameraTF = transform.parent.parent;
        m_LevelClockTF = m_UICameraTF.GetChild(1); // TODO: Find this a better way?
        
        // Get Components
        m_ButtonsPanelCanvasGroup = m_ButtonPanelTF.GetComponent<CanvasGroup>(); // TODO: MUST be a better way to find this...
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_LevelClockText = transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        m_NextLevelButton = m_ButtonPanelTF.GetChild(0).GetComponent<Button>();
        m_QuitAppButton = m_ButtonPanelTF.GetChild(1).GetComponent<Button>();


    }

    private void Start()
    {
        // Create LevelClock for show
        CreateLevelClockGO();

        // Set proper button events
        m_NextLevelButton.onClick.AddListener(GameManager.Instance.LoadNextScene);
        m_QuitAppButton.onClick.AddListener(GameManager.Instance.QuitApplication);
    }

    private void OnEnable()
    {
        // Subscribe to relevant events
        GameManager.Instance.CurrentLevelManager.OnLevelEnded.AddListener(OnActivate);
        GameManager.Instance.CurrentLevelManager.OnLevelStarted.AddListener(OnDeactivate);
    }

    private void OnDisable()
    {
        // No matter what, stop all CRTs.
        StopAllCoroutines();

        // Unsub from events
        if (GameManager.Exists && GameManager.Instance.CurrentLevelManager != null)
        {
            GameManager.Instance.CurrentLevelManager.OnLevelEnded.RemoveListener(OnActivate);
            GameManager.Instance.CurrentLevelManager.OnLevelStarted.RemoveListener(OnDeactivate);
            m_NextLevelButton.onClick.RemoveListener(GameManager.Instance.LoadNextScene);
            m_QuitAppButton.onClick.RemoveListener(GameManager.Instance.QuitApplication);
        }
    }

    // + + + + | Functions | + + + + 

    private IEnumerator FadeAlphaToValue(float fadeTime, float fadeValue)
    {
        float oldAlpha = m_CanvasGroup.alpha;
        for (float timeHelper = 0f; timeHelper < fadeTime; timeHelper += Time.deltaTime)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(oldAlpha, fadeValue, timeHelper / fadeTime);
            yield return new WaitForEndOfFrame();
        }
        m_CanvasGroup.alpha = fadeValue;
    }

    private IEnumerator WaitUntilTime(float time)
    {
        for (float timeHelper = 0; timeHelper < time; timeHelper += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    private IEnumerator MenuOpenCRT()
    {
        float openFadeTime = 0.75f;
        float pauseTime = 0.5f;
        float clockFloatInTime = 1f;
        float textWriteTimeTotal = 1f;
        float buttonsFadeTime = 1f;

        // Fade Open
        yield return StartCoroutine(FadeAlphaToValue(openFadeTime, 1f));

        // Wait
        yield return StartCoroutine(WaitUntilTime(pauseTime));

        // Bring LevelClock in, show info
        for (float timeHelper = 0f; timeHelper <= clockFloatInTime; timeHelper += Time.deltaTime)
        {
            m_LevelClockGO.transform.position = Vector3.Lerp(Vector3.zero, m_LevelClockPanelPosition, timeHelper / openFadeTime);
            yield return new WaitForEndOfFrame();
        }
        m_LevelClockGO.transform.position = m_LevelClockPanelPosition;

        // Write Text
        string clockText = GameManager.Instance.CurrentLevelManager.LevelClock.Description;
        for (int textIndex = 0; textIndex <= clockText.Length; textIndex++)
        {
            m_LevelClockText.text = clockText.Substring(0, textIndex);
            yield return new WaitForSeconds(textWriteTimeTotal / clockText.Length);
        }

        // Wait again
        yield return StartCoroutine(WaitUntilTime(pauseTime));

        // Show Buttons
        for (float timeHelper = 0f; timeHelper <= buttonsFadeTime; timeHelper += Time.deltaTime)
        {
            m_ButtonsPanelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timeHelper/ buttonsFadeTime);
            yield return new WaitForEndOfFrame();
        }

        // Finally, activate Buttons
        SetButtonsActive(true);
    }

    /// <summary>
    /// Handles the instantiation of the LevelClockGameObject
    /// </summary>
    private void CreateLevelClockGO()
    {
        m_LevelClockGO = Instantiate(
            GameManager.Instance.CurrentLevelManager.LevelClock.FinalClockPrefab,
            Vector3.zero,
            Quaternion.identity,
            m_LevelClockTF);

        // Set Layer
        int levelClockLayer = LayerMask.NameToLayer("LevelClock");
        m_LevelClockGO.layer = levelClockLayer; // TODO: Create utility class for these tags, layers, etc?
        foreach(Transform clockPieceTF in m_LevelClockGO.transform)
        {
            clockPieceTF.gameObject.layer = levelClockLayer;
        }
    }

    /// <summary>
    /// Iterates through all the known buttons, sets their interactability to a given boolean.
    /// </summary>
    /// <param name="active"></param>
    private void SetButtonsActive(bool active)
    {
        foreach (Transform buttonTF in m_ButtonPanelTF)
        {
            Button btn = buttonTF.GetComponent<Button>();
            btn.interactable = active;
        }
    }

    public void OnActivate()
    {
        m_IsActivated = true;
        //m_CanvasGroup.alpha = 1f;
        m_ButtonsPanelCanvasGroup.alpha = 0f;

        m_MenuOpenCRT = MenuOpenCRT();
        StartCoroutine(m_MenuOpenCRT);
    }

    public void OnDeactivate()
    {
        m_IsActivated = false;
        m_CanvasGroup.alpha = 0f;
        m_ButtonsPanelCanvasGroup.alpha = 0f;

        // Set buttons to inactive!
        SetButtonsActive(false);
    }
}
