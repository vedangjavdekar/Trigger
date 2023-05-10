using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimulatedBody : MonoBehaviour, IGameStateHandler
{
    Vector3 m_ResetPosition;

    Rigidbody2D m_Rigidbody;

    Rigidbody2D rigidBody2D
    {
        get
        {
            if(!m_Rigidbody)
            {
                m_Rigidbody= GetComponent<Rigidbody2D>();
            }

            return m_Rigidbody;
        }
    }

    private void Awake()
    {
        m_ResetPosition= transform.position;
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if(state == GameState.StartEditMode || state == GameState.StartSimulationMode)
        {
            transform.position = m_ResetPosition;
            rigidBody2D.velocity = Vector3.zero;
            rigidBody2D.simulated = false;
        }
        else if(state == GameState.StartSimulation)
        {
            rigidBody2D.simulated = true;
        }
    }
}
