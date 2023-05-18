using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a ClockPiece mesh
/// </summary>
public class ClockPieceController : MonoBehaviour
{
    public GameObject ClockPieceGO { get; private set; }

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
        GameManager.Instance.CurrentLevelManager.OnClockPieceFound(this);
    }

    /// <summary>
    /// Doesn't deactivate, but turns collision off and other things.
    /// </summary>
    public void HideClockPiece()
    {
        gameObject.SetActive(false);
    }
}