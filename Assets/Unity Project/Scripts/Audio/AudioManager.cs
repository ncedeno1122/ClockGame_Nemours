using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralizes a SoundBank and 
/// </summary>
public class AudioManager : GenericSingleton<AudioManager>
{
    [SerializeField]
    private SoundBankSO m_CurrentSoundBank;
    public SoundBankSO CurrentSoundBank => m_CurrentSoundBank;
}
