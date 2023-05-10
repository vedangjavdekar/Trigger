using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotator : MonoBehaviour, IGameStateHandler
{
    Rigidbody2D m_Rigidbody2D = null;
    Rigidbody2D rigidBody2D
    {
        get
        {
            if (!m_Rigidbody2D)
            {
                m_Rigidbody2D = GetComponent<Rigidbody2D>();
            }
            return m_Rigidbody2D;
        }
    }

    float m_InitialAngle = 0.0f;
    float m_StartAngle;

    [SerializeField]
    float m_StepAngle = 45.0f;
    [SerializeField]
    float m_LerpTime = 0.4f;
    float m_CurrInterp;

    bool m_IsBusy = false;

    private void Awake()
    {
        m_InitialAngle = rigidBody2D.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_IsBusy)
        {
            return;
        }

        m_StartAngle = rigidBody2D.rotation;
        m_CurrInterp = 0.0f;
        m_IsBusy = true;
    }

    private void FixedUpdate()
    {
        if (!m_IsBusy)
        {
            return;
        }
        Rotate();
    }

    void Rotate()
    {
        m_CurrInterp += Time.fixedDeltaTime;
        float newAngle = Mathf.LerpAngle(m_StartAngle, m_StartAngle + m_StepAngle, m_CurrInterp / m_LerpTime);
        rigidBody2D.MoveRotation(newAngle);
        m_IsBusy = m_CurrInterp <= m_LerpTime;
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
            case GameState.StartSimulationMode:
                m_IsBusy = false;
                m_CurrInterp = 0.0f;
                rigidBody2D.rotation = m_InitialAngle;
                break;
            default:
                break;
        }
    }
}
