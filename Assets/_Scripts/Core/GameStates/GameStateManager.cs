using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Undefined = 0,
    StartEditMode,
    StartSimulationMode,
    StartSimulation,
    GoalReached
}


public class GameStateManager : MonoBehaviour
{
    public static event Action<GameState, GameState> OnGameStateChanged;

    private GameState m_CurrState = GameState.Undefined;
    public GameState currState => m_CurrState;

    private bool m_StateChangePending = false;

    private GameState m_NewState = GameState.Undefined;

    public void PushNewState(GameState newState)
    {
        if (newState != GameState.Undefined && newState != m_CurrState)
        {
            m_NewState = newState;
            m_StateChangePending = true;
        }
    }

    public void OverrideState(GameState newState)
    {
        OnGameStateChanged?.Invoke(newState, m_CurrState);
        m_CurrState = newState;
    }

    private void Update()
    {
        if (m_StateChangePending)
        {
            OnGameStateChanged?.Invoke(m_NewState, m_CurrState);
            m_CurrState = m_NewState;
            m_StateChangePending = false;
        }
    }
}
