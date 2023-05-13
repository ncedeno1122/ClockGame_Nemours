using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a ClockPiece mesh
/// </summary>
public class ClockPieceController : MonoBehaviour
{
    public GameObject ClockPieceGO { get; private set; }

    private void OnValidate()
    {
        // Don't call if we're a prefab OR if the LevelManager knows of us.
        if (gameObject.scene.name == null) return;
        if (LevelManager.Instance.ClockPieces.Contains(this)) return;

        // Try to add to LevelManager, delete if not.
        if (LevelManager.Instance.TryAddClockPiece(this))
        {

        }
        else
        {
            Debug.LogWarning($"Cannot allow new ClockPieceController; {LevelManager.Instance.ClockPieces.Count} out of {LevelManager.Instance.LevelClock.ClockPieces.Length} pieces are used.");
            DestroyImmediate(gameObject);
        }

        // If not already, try set the ClockPieceGO's position correctly.
        if (ClockPieceGO != null)
        {
            ClockPieceGO.transform.localPosition = Vector3.zero;
        }
    }

    private void Awake()
    {
        //
    }

    private void Start()
    {
        //
    }

    // + + + + | Functions | + + + + 

    /// <summary>
    /// Invoked by LevelManager to give this its own ClockPiece prefab to instantiate.
    /// </summary>
    /// <param name="pieceGO"></param>
    public void AssignClockPieceGO(GameObject pieceGO)
    {
        ClockPieceGO = Instantiate(pieceGO, this.transform, false);
        ClockPieceGO.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Invoked when the Player finds this piece!
    /// </summary>
    public void OnPieceFound()
    {
        LevelManager.Instance.OnClockPieceFound(this);
    }

    /// <summary>
    /// Doesn't deactivate, but turns collision off and other things.
    /// </summary>
    public void HideClockPiece()
    {
        gameObject.SetActive(false);
    }
}