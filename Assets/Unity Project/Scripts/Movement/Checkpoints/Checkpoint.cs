using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private int m_CheckpointNumber;
    public int CheckpointNumber { get => m_CheckpointNumber; private set => m_CheckpointNumber = value; }

    public bool IsActivated { get; private set; }

    private void OnValidate()
    {
        AutoAssignCheckpointNumber();
    }

    // + + + + | Functions | + + + +

    private void AutoAssignCheckpointNumber()
    {
        // TODO: This can probably be much better? NOT regex though, it's slow bc my queries stink...

        // Hacky String Way, by Name
        char[] charArr2 = gameObject.name.ToCharArray();
        List<char> resultCharList = new List<char>();
        bool isThereADigitInName = false;

        //Stopwatch stringSWatch = new Stopwatch();
        //stringSWatch.Start();

        // Find all chars in order
        for (int i = 0; i < gameObject.name.Length; i++)
        {
            if (Char.IsDigit(charArr2[i]))
            {
                resultCharList.Add(charArr2[i]);
                isThereADigitInName = true;
            }
        }

        // If there aren't any digits in the name, break!
        if (!isThereADigitInName)
        {
            CheckpointNumber = 0; // MUST be the first checkpoint then!
            return;
        }

        // Assemble string from them
        string resultString = "";
        for (int i = 0; i < resultCharList.Count; i++)
        {
            resultString += resultCharList[i];
        }

        if (Int32.TryParse(resultString, out int resultInt))
        {
            Console.WriteLine($"Changed Checkpoint number from {CheckpointNumber} to {resultInt}");
            CheckpointNumber = resultInt;
        }
        else
        {
            Console.WriteLine("Error parsing resultInt...");
        }

        //stringSWatch.Stop();
        //TimeSpan ts3 = stringSWatch.Elapsed;
        //Console.WriteLine(" String Way Time: " + ts3);
    }

    public void Activate()
    {
        IsActivated = true;
    }

    public void OnReached()
    {
        if (!IsActivated) IsActivated = true;

        GameManager.Instance.CurrentLevelManager.OnCheckpointReached(this);
    }

    private string GetFormattedName() => GetType().Name + "_" + CheckpointNumber;
}
