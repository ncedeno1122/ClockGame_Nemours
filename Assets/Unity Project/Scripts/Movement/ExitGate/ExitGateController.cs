using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGateController : MonoBehaviour
{
    private bool m_IsExiting = false;

    private LevelManager m_LevelManager;

    private WorldAudioSourceComponent m_WASC;

    private void Start()
    {
        // Get current LevelManager;
        m_LevelManager = GameManager.Instance.CurrentLevelManager;
        m_WASC = GetComponentInChildren<WorldAudioSourceComponent>();
    }

    // + + + + | Functions | + + + + 

    private bool CanPlayerExit() => m_LevelManager.FoundClockPieces.Count == m_LevelManager.ClockPieces.Count;

    /// <summary>
    /// Invoked by the Player, determines if they can exit the level and triggers the exit sequence.
    /// </summary>
    /// <returns></returns>
    public bool TryExit()
    {
        if (m_IsExiting) return false;
        if (!CanPlayerExit()) return false;

        // If the Player CAN exit,
        m_LevelManager.HandleLevelEnd();
        m_IsExiting = true;
        Debug.Log("Player can exit!");
        
        // Audio
        m_WASC.AudioSource.loop = false;
        m_WASC.AudioSource.PlayOneShot(AudioManager.Instance.CurrentSoundBank.GetSFXClip(SFXClips.EXIT_GATE));

        return true;
    }


}
