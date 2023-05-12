using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointNumber { get; private set; }
    public bool IsActivated { get; private set; }

    private void OnValidate()
    {
        // Are we properly named and NOT a prefab?
        if (!gameObject.name.Equals(GetFormattedName()) && gameObject.scene.name != null)
        {
            // Add these to the LevelManager's list of checkpoints
            if (LevelManager.Instance != null)
            {
                if (!LevelManager.Instance.Checkpoints.Contains(this))
                {
                    // Add this checkpoint
                    LevelManager.Instance.Checkpoints.Add(this);
                    CheckpointNumber = LevelManager.Instance.Checkpoints.IndexOf(this);
                    gameObject.name = GetFormattedName();
                }
            }
        }
    }

    // + + + + | Functions | + + + +

    public void Activate()
    {
        IsActivated = true;
    }

    public void OnReached()
    {
        LevelManager.Instance.OnCheckpointReached(this);
    }

    private string GetFormattedName() => GetType().Name + "_" + CheckpointNumber;
}
