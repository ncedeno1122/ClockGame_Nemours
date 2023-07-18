using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Names the AudioSource according to its AudioMixerGroup.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioSourceNamingUtility : MonoBehaviour
{
    private void OnValidate()
    {
        // Get Audio Source
        if (TryGetComponent<AudioSource>(out AudioSource audioSrc))
        {
            // Rename if not already named properly.
            audioSrc.name = GetAudioMixerGroupName(audioSrc);
        }
    }

    /// <summary>
    /// Returns a string name for an AudioSource based on its AudioMixerGroup.
    /// </summary>
    /// <param name="audioSrc"></param>
    /// <returns></returns>
    private string GetAudioMixerGroupName(AudioSource audioSrc)
    {
        AudioMixerGroup amg = audioSrc.outputAudioMixerGroup;
        if (amg != null)
        {
            return amg.name + "_AudioSource";
        }

        return "AudioSource";
    }
}
