using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ModeText : MonoBehaviour, IGameStateHandler
{
    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
                {
                    m_TextMeshPro.SetText("Mode: Edit");
                    break;
                }
            case GameState.StartSimulationMode:
                {
                    m_TextMeshPro.SetText("Mode: Waiting for Trigger");
                    break;
                }
            case GameState.StartSimulation:
                {
                    m_TextMeshPro.SetText("Mode: Runnning");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    TextMeshProUGUI m_TextMeshPro = null;

    private void Awake()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }
}
