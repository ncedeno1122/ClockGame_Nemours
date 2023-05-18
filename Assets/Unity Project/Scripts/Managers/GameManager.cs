using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    private int m_TargetFrameRate = 60;
    public int TargetFrameRate { get => m_TargetFrameRate; }

    public IconBankSO IconBank;

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
        // Set Singleton Instance to this!
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Set Framerate
        Application.targetFrameRate = m_TargetFrameRate;

        // Fetch the given IconBank!
        IconBank = FetchIconBank();
    }

    private void Start()
    {
        
    }

    // + + + + | Functions | + + + + 

    private IconBankSO FetchIconBank()
    {
        return Resources.Load("IconBank/MainIconBank") as IconBankSO;
    }
}
