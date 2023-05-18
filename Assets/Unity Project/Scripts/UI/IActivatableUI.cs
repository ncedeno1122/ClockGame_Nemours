using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows for UI to have shared activatable / deactivatable functionality and state.
/// </summary>
public interface IActivatableUI
{
    bool IsActivated { get; set; }

    void OnActivate();
    void OnDeactivate();
}
