using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public CharacterController2D Player;

    public List<Checkpoint> Checkpoints = new();
    public Checkpoint LastReachedCheckpoint;

    public List<ClockPieceController> ClockPieces = new();
    public List<ClockPieceController> FoundClockPieces = new();
    public LevelClockSO LevelClock; // The Clock and ClockPieces this level will showcase.
    
    public Bounds LevelBounds;

    public UnityEvent OnLevelStarted, OnLevelEnded;

    private void OnValidate()
    {
        // Find Player if none
        if (Player == null)
        {
            Player = FindObjectOfType<CharacterController2D>();
        }

        // Remove 'Missing' Checkpoints & ClockPieces
        Start();
    }

    private void Awake()
    {

    }

    private void Start()
    {
        // Try and get all Checkpoints!
        if (Checkpoints.Count > 0)
        {
            Checkpoints.Clear();
        }
        Checkpoints.AddRange(FindObjectsOfType<Checkpoint>());

        // Try and get and format all ClockPieces as well!
        if (ClockPieces.Count > 0)
        {
            ClockPieces.Clear();
        }
        ClockPieces.AddRange(FindObjectsOfType<ClockPieceController>());
        foreach (var piece in ClockPieces)
        {
            TryAddClockPiece(piece); // TODO: Want to delete invalid Clockpieces but not while iterating...
        }

        // Call LevelStartedEvent
        OnLevelStarted?.Invoke();
    }

    private void OnEnable()
    {
        //
    }

    private void OnDisable()
    {
        // Remove listeners from events
        OnLevelStarted.RemoveAllListeners();
        OnLevelEnded.RemoveAllListeners();
    }

    // + + + + | Functions | + + + + 

    public void OnCheckpointReached(Checkpoint point)
    {
        // If the checkpoint is not known, add it...
        if (!Checkpoints.Contains(point))
        {
            Checkpoints.Add(point);
        }
        
        // No matter what, set last known checkpoint to this one.
        LastReachedCheckpoint = point;
    }

    /// <summary>
    /// A helper function for ClockPieceController's OnValidate. Only allows a certain number of 
    /// ClockPieces around a level, based on the number of ClockPiece GameObjects specified in the
    /// LevelClock ScriptableObject.
    /// </summary>
    /// <param name="clockPiece"></param>
    /// <returns></returns>
    public bool TryAddClockPiece(ClockPieceController clockPiece)
    {
        if (ClockPieces.Contains(clockPiece)) return false;

        int pieceToAddIndex = ClockPieces.Count;

        // Do we have enough ClockPieces in our ScriptableObject to afford this?
        if (pieceToAddIndex < LevelClock.ClockPieces.Length)
        {
            ClockPieces.Add(clockPiece);
            clockPiece.AssignClockPieceGO(LevelClock.ClockPieces[pieceToAddIndex]);
            clockPiece.name = $"ClockPiece_{ClockPieces.IndexOf(clockPiece)}_{clockPiece.ClockPieceGO.name}";
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnClockPieceFound(ClockPieceController clockPiece)
    {
        // If the ClockPiece wasn't already found,
        if (!FoundClockPieces.Contains(clockPiece))
        {
            FoundClockPieces.Add(clockPiece);
        }

        // Then, hide it.
        clockPiece.HideClockPiece();
    }

    /// <summary>
    /// Invoked when the ExitGate is reached, starts the Level End Sequence.
    /// </summary>
    public void HandleLevelEnd()
    {
        OnLevelEnded?.Invoke();
    }

    private void OnDrawGizmos()
    {
        // Draw LevelBounds
        Gizmos.color = Color.green;
        //Gizmos.DrawCube(LevelBounds.center, LevelBounds.size);
        Gizmos.DrawWireCube(LevelBounds.center, LevelBounds.size);
    }
}
