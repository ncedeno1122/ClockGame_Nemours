using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : GenericSingleton<LevelManager>
{
    public CharacterController2D Player;

    public List<Checkpoint> Checkpoints = new();
    public Checkpoint LastReachedCheckpoint;

    public List<ClockPieceController> ClockPieces = new();
    public List<ClockPieceController> FoundClockPieces = new();
    public LevelClockSO LevelClock; // The Clock and ClockPieces this level will showcase.
    
    public Bounds LevelBounds;
    public IconBankSO IconBank;

    private void OnValidate()
    {
        // Find Player if none
        if (Player == null)
        {
            Player = FindObjectOfType<CharacterController2D>();
        }

        // Remove 'Missing' Checkpoints
        Checkpoints.RemoveAll(x => x == null);
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
        // If the checkpoint is not known, add it...
        if (!Checkpoints.Contains(point))
        {
            Checkpoints.Add(point);
        }

        // Activate the checkpoint!
        if (!point.IsActivated)
        {
            point.Activate();
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

}
