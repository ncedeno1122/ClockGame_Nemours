using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : GenericSingleton<LevelManager>
{
    public List<Checkpoint> Checkpoints = new(); // Assigned in Inspector
    public Checkpoint LastReachedCheckpoint;
    public Bounds LevelBounds;

    private void OnValidate()
    {
        // Remove 'Missing' Checkpoints
        Checkpoints.RemoveAll(x => x == null);
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
        // Activate the checkpoint
        if (!point.IsActivated)
        {
            point.Activate();
        }
        // Set last known checkpoint to this one.
        LastReachedCheckpoint = point;
    }

}
