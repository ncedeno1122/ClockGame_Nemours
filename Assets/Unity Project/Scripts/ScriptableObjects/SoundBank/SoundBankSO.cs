using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Organizes a bank of sounds for the game. 
/// </summary>
[CreateAssetMenu(fileName = "New SoundBank", menuName = "ScriptableObjects/New SoundBankSO")]
public class SoundBankSO : ScriptableObject
{
    [SerializeField] private AudioClip[] SFXClipArray;
    [SerializeField] private AudioClip[] UIClipArray;
    [SerializeField] private AudioClip[] MusicTrackArray;

    public AudioClip GetSFXClip(SFXClips clipId) => SFXClipArray[(int)clipId];
    
    public AudioClip GetUIClip(UIClips clipId) => UIClipArray[(int)clipId];
    
    public AudioClip GetMusicTrack(int trackId) => MusicTrackArray[trackId];
    public AudioClip GetRandomMusicTrack() => MusicTrackArray[Random.Range(0, MusicTrackArray.Length)];
}