using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameStateEventHandler : MonoBehaviour
{
    IGameStateHandler[] m_GameStateHandlers = null;
    public IGameStateHandler[] gameStateHandlers
    {
        get
        {
            if (m_GameStateHandlers == null)
            {
                m_GameStateHandlers = GetComponents<IGameStateHandler>();
            }

            return m_GameStateHandlers;
        }
    }

    private void OnEnable()
    {
        GameStateManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState, GameState oldState)
    {
        foreach (IGameStateHandler handler in gameStateHandlers)
        {
            handler.OnGameStateEnd(oldState);
            handler.OnGameStateStart(newState);
        }
    }
}
