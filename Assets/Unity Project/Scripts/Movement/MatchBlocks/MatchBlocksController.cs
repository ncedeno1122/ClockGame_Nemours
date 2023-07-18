using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MatchBlocksController : MonoBehaviour
{
    [SerializeField] private List<MatchBlockScript> m_MatchBlocks;
    
    public MatchBlockValue TargetValue;
    public UnityEvent OnMatchEvent;

    private WorldAudioSourceComponent m_WASC;

    private void Awake()
    {
        m_MatchBlocks = GetComponentsInChildren<MatchBlockScript>().ToList();
        m_WASC = GetComponentInChildren<WorldAudioSourceComponent>();
    }

    private void Start()
    {
        InitializeMatchBlockChildren();
    }

    // + + + + | Functions | + + + + 

    private void InitializeMatchBlockChildren()
    {
        // Set this as their observer class
        foreach (MatchBlockScript mbs in m_MatchBlocks)
        {
            mbs.MatchBlocksController = this; // TODO: Safer to add via GetComponentInParent from child class?
        }
    }

    private void LockMatchBlockChildren()
    {
        Debug.Log("MatchBlock complete! Locking child blocks!");
        foreach (MatchBlockScript mbs in m_MatchBlocks)
        {
            mbs.LockBlock(); // TODO: Perhaps do this via event in children? This is easier, but hardly elegant imo...
        }
    }

    private bool AreMatchingTargetValue()
    {
        foreach (MatchBlockScript mbs in m_MatchBlocks)
        {
            if (mbs.CurrentValue != TargetValue) return false;
        }
        return true;
    }

    public void OnMatchBlockToggled(MatchBlockScript matchBlock)
    {
        if (m_MatchBlocks.Contains(matchBlock))
        {
            if (AreMatchingTargetValue()) // Do we have a Match?
            {
                LockMatchBlockChildren();
                OnMatchEvent?.Invoke();
                
                // Audio
                m_WASC.AudioSource.pitch = 1f;
                m_WASC.AudioSource.PlayOneShot(AudioManager.Instance.CurrentSoundBank.GetSFXClip(SFXClips.CORRECT_CHIME));
            }
            else // If not,
            {
                // Audio
                m_WASC.AudioSource.pitch = Random.Range(0.75f, 1.25f);
                m_WASC.AudioSource.PlayOneShot(AudioManager.Instance.CurrentSoundBank.GetSFXClip(SFXClips.OPTION_BELL_1));
            }
        }
    }
}
