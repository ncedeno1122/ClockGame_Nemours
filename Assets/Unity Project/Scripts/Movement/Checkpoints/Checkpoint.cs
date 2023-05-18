using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointNumber { get; private set; }
    public bool IsActivated { get; private set; }

    // + + + + | Functions | + + + +

    public void Activate()
    {
        IsActivated = true;
    }

    public void OnReached()
    {
        GameManager.Instance.CurrentLevelManager.OnCheckpointReached(this);
    }

    private string GetFormattedName() => GetType().Name + "_" + CheckpointNumber;
}
