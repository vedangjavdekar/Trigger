using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBall : TriggerableObjectBase, IGameStateHandler
{
    Rigidbody2D m_Rigidbody = null;

    Vector3 m_ResetPosition;
    
    [SerializeField]
    Vector2 m_StartVelocity = Vector2.zero;
    object m_SeqId = 0;

    [SerializeField]
    ObjectHighlight m_ObjectHighlight = null;
    [SerializeField]
    private float m_MaxSpeed = 1000;

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
                {
                    ResetPosition();
                    break;
                }
            case GameState.StartSimulationMode:
                {
                    ResetPosition();
                    break;
                }
            case GameState.StartSimulation:
                {
                    StartSimulation();
                    break;
                }
            case GameState.GoalReached:
                {
                    GoalReached();
                    break;
                }
            case GameState.Undefined:
            default:
                {
                    break;
                }
        }
    }

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_ResetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_MaxSpeed);
    }

    public void ResetPosition()
    {
        SetRigidbodyEnabled(false, Vector2.zero);
        transform.localScale = Vector3.one;
        transform.position = m_ResetPosition;
    }

    private void GoalReached()
    {
        SetRigidbodyEnabled(false, Vector2.zero);
    }

    private void StartSimulation()
    {
        SetRigidbodyEnabled(true, m_StartVelocity);
    }

    private void SetRigidbodyEnabled(bool enabled, Vector2 velocity)
    {
        if (m_Rigidbody)
        {
            m_Rigidbody.velocity = velocity;
            m_Rigidbody.simulated = enabled;
        }
    }

    public void OnReachedGoal(Transform ReachTarget,float duration)
    {
        DOTween.Kill(m_SeqId);
        var seq = DOTween.Sequence();
        m_SeqId = seq.id;
        GameManager.Instance.gameInputController.GetGlobalControlsActions().Disable();
        seq.Append(transform.DOMove(ReachTarget.position, duration).SetEase(Ease.InOutBack));
        seq.AppendCallback(()=> {
            transform.localScale = Vector3.zero;
            GameManager.Instance.gameInputController.GetGlobalControlsActions().Enable();
        });
    }

    public override void OnTriggerInput()
    {
        GameManager.Instance.gameStateManager.PushNewState(GameState.StartSimulation);
    }

    public override void OnResetObject()
    {
        ResetPosition();
    }

    public override void SetHighlightEnabled(bool enable)
    {
        m_ObjectHighlight?.SetHighlightEnabled(enable);
    }

    public override bool CanTriggerAgain()
    {
        return false;
    }
}
