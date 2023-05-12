using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCheckpointOnTrigger : MonoBehaviour
{
    public Checkpoint TargetCheckpoint;

    private void OnValidate()
    {
        if (TargetCheckpoint != null) return;

        // Try to find the Target Checkpoint ON this GameObject or in the parent.
        TargetCheckpoint = GetComponent<Checkpoint>();
        if (TargetCheckpoint == null)
        {
            TargetCheckpoint = GetComponentInParent<Checkpoint>();
            if (TargetCheckpoint == null)
            {
                Debug.LogWarning($"Checkpoint Trigger on {gameObject.name} couldn't find a Checkpoint on the GameObject or its parent.");
            }
        }
    }

    // + + + + | Collision Handling | + + + + 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TargetCheckpoint.OnReached();
        }
    }
}
