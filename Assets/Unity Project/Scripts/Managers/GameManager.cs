using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    private int m_TargetFrameRate = 60;
    public int TargetFrameRate { get => m_TargetFrameRate; }

    public IconBankSO IconBank;

    [SerializeField]
    private LevelManager m_CurrentLevelManager;
    public LevelManager CurrentLevelManager
    {
        get
        {
            if (m_CurrentLevelManager != null) return m_CurrentLevelManager;

            // Try to find it if we don't know it already.
            m_CurrentLevelManager = FindObjectOfType<LevelManager>();
            return m_CurrentLevelManager;
        }
    }

    private new void Awake()
    {
        Debug.LogFormat($"<color=green>{typeof(GameManager).Name}'s Awake called!</color>");
        // Set Singleton Instance to this!
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Set Framerate
        Application.targetFrameRate = m_TargetFrameRate;

        // Fetch the given IconBank!
        IconBank = FetchIconBank();

        
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        // Find LevelManager on scene open!
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Find LevelManager on scene open!
        //SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // + + + + | Functions | + + + + 

    /// <summary>
    /// Returns the used IconBank.
    /// </summary>
    /// <returns></returns>
    private IconBankSO FetchIconBank()
    {
        return Resources.Load("IconBank/MainIconBank") as IconBankSO;
    }

    /// <summary>
    /// Simply loads the next scene.
    /// </summary>
    public void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCount)
        {
            SceneManager.LoadScene(0); // Load Menu if nothing else
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    /// <summary>
    /// Simply quits the application.
    /// </summary>
    public void QuitApplication()
    {
        // TODO: Probably should do something else here. Is this necessary if it's to be WebGL?
        Application.Quit();
    }

    // + + + + | Event Handling | + + + + 

    private void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        // Find the LevelManager in this scene!
        //Debug.Log("Scene changed, trying to find LevelManager!");
        m_CurrentLevelManager = null;
        m_CurrentLevelManager = FindObjectOfType<LevelManager>();
    }

}
