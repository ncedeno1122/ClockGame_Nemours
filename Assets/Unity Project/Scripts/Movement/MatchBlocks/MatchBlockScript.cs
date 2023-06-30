using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchBlockScript : MonoBehaviour
{
    private bool m_IsLocked = false;

    public MatchBlocksController MatchBlocksController;

    [SerializeField] private MatchBlockValue m_CurrentValue;
    public MatchBlockValue CurrentValue { get => m_CurrentValue; }

    private void Start()
    {
        UpdateGraphics();
    }

    private void OnValidate()
    {
        if (gameObject.scene != null) return; // Don't run if this is an uninstantiated prefab!
        UpdateGraphics();
    }

    // + + + + | Functions | + + + + 

    public void AdvanceValue()
    {
        if (m_IsLocked) return;

        // Advance Value
        switch (m_CurrentValue)
        {
            case MatchBlockValue.INVALID:
                Debug.LogError("Cannot Advance INVALID MatchBlockValue!");
                break;
            default:
                int enumLength = Enum.GetValues(typeof(MatchBlockValue)).Length - 1;
                MatchBlockValue nextValue = (MatchBlockValue)(((int)m_CurrentValue + 1) % enumLength);
                
                //Debug.Log($"Advancing value from {m_CurrentValue} to {nextValue}!");
                m_CurrentValue = nextValue;
                UpdateGraphics();

                break;
        }

        // Notify Controller
        MatchBlocksController.OnMatchBlockToggled(this);
    }

    private void UpdateGraphics()
    {
        // TODO: Need better way to do this
        Renderer currRenderer = transform.GetComponent<Renderer>();
        if (!m_IsLocked && Application.isPlaying)
        {

            switch (m_CurrentValue)
            {
                case MatchBlockValue.INVALID:
                    currRenderer.material.color = Color.yellow;
                    break;
                case MatchBlockValue.RED:
                    currRenderer.material.color = Color.red;
                    break;
                case MatchBlockValue.GREEN:
                    currRenderer.material.color = Color.green;
                    break;
                case MatchBlockValue.BLUE:
                    currRenderer.material.color = Color.blue;
                    break;
            }
        }
        else
        {
            currRenderer.material.color = Color.black;
        }
    }

    public void LockBlock()
    {
        m_IsLocked = true;
        UpdateGraphics();
    }
}
