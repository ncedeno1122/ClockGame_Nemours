using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : GenericSingleton<LevelManager>
{
    public List<Checkpoint> Checkpoints = new(); // Assigned in Inspector
    public Checkpoint LastReachedCheckpoint;
    public Bounds LevelBounds;
    public IconBankSO IconBank;

    private void OnValidate()
    {
        // Remove 'Missing' Checkpoints
        Checkpoints.RemoveAll(x => x == null);

        foreach (Checkpoint cp in Checkpoints)
        {
            if (cp == null)
            {
                Debug.Log($"FOUND NULL CHECKPOINT {cp}!!!");
            }
        }
    }

    private void Start()
    {
        // Try and get all Checkpoints!
        if (Checkpoints.Count > 0)
        {
            Checkpoints.Clear();
        }
        Checkpoints.AddRange(FindObjectsOfType<Checkpoint>());
    }

    private void OnDrawGizmos()
    {
        // Draw LevelBounds
        Gizmos.color = Color.green;
        //Gizmos.DrawCube(LevelBounds.center, LevelBounds.size);
        Gizmos.DrawWireCube(LevelBounds.center, LevelBounds.size);
    }

    // + + + + | Functions | + + + + 

    public void OnCheckpointReached(Checkpoint point)
    {
        // If the checkpoint
        if (!Checkpoints.Contains(point))
        {
            Checkpoints.Add(point);
        }

        // Activate the checkpoint
        if (!point.IsActivated)
        {
            point.Activate();
        }
        // Set last known checkpoint to this one.
        LastReachedCheckpoint = point;
    }

}
