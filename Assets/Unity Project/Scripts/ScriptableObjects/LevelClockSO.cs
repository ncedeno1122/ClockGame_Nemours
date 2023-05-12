using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the data for each level's clock data.
/// </summary>
[CreateAssetMenu(fileName = "New LevelClock", menuName = "ScriptableObjects/LevelClock Data")]
public class LevelClockSO : ScriptableObject
{
    public string OfficialName;
    public string Description;
    public GameObject FinalClockPrefab;
    public GameObject[] ClockPieces;
}
