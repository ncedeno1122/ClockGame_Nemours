using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that wraps/manages an AudioSource for ease of use by scripts. 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class WorldAudioSourceComponent : MonoBehaviour
{
    private AudioSource m_AudioSource;
    public AudioSource AudioSource => m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
}
