using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour,IGameStateHandler
{
    Collider2D m_Collider;

    private void Awake()
    {
        m_Collider= GetComponent<Collider2D>();
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if (!m_Collider)
        {
            return;
        }
        switch (state)
        {
            case GameState.StartSimulationMode:
            case GameState.StartSimulation:
                m_Collider.enabled = true;
                break;
            case GameState.GoalReached:
            case GameState.StartEditMode:
            default:
                m_Collider.enabled = false;
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.gameStateManager.PushNewState(GameState.StartSimulationMode);
        }
    }
}
