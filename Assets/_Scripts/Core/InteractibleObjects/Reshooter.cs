using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Reshooter : MonoBehaviour, IGameStateHandler
{
    private Collider2D m_Collider2D;

    private float m_VelocityMagnitude = 0.0f;
    [SerializeField]
    private float m_MinimumVelocity = 20.0f;

    private Rigidbody2D m_PlayerBody;
    private PlayerBall m_PlayerBallObject;

    [SerializeField]
    private Transform m_ShooterCircle;

    [SerializeField]
    private AudioClip m_Audio;

    private UseOnce m_UseOnce;

    private Sequence m_Sequence;

    public void OnGameStateEnd(GameState state)
    {
        m_Sequence.Kill(true);
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartEditMode || state == GameState.StartSimulationMode)
        {
            m_Collider2D.enabled = false;
            if (m_PlayerBallObject)
            {
                m_PlayerBallObject.ResetPosition();
            }
        }
        else if (state == GameState.StartSimulation)
        {
            ResetReshooter();
        }
    }

    private void Awake()
    {
        m_Collider2D = GetComponent<Collider2D>();
        m_UseOnce = GetComponent<UseOnce>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.soundManager.PlaySound(m_Audio);
        m_PlayerBallObject = collision.gameObject.GetComponent<PlayerBall>();
        m_PlayerBody = collision.attachedRigidbody;
        m_VelocityMagnitude = m_PlayerBody.velocity.magnitude;
        m_PlayerBody.simulated = false;
        m_Collider2D.enabled = false;

        if (m_UseOnce)
        {
            SetBallVelocity();
        }
        else
        {
            PlaySequence();
        }
    }

    private void PlaySequence()
    {
        m_Sequence.Kill(true);
        m_Sequence = DOTween.Sequence();
        m_Sequence.Append(m_PlayerBallObject.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBack));
        m_Sequence.Join(m_PlayerBallObject.transform.DOMove(transform.position, 0.1f).SetEase(Ease.InOutQuad));
        m_Sequence.AppendCallback(() =>
        {
            SetBallVelocity();
        });
        m_Sequence.Append(m_ShooterCircle.DOScale(Vector3.one, 0.25f)
            .From(1.2f * Vector3.one)
            .SetEase(Ease.InOutBack));
        m_Sequence.AppendInterval(2f);
        m_Sequence.AppendCallback(() =>
        {
            ResetReshooter();
        });
    }

    void ResetReshooter()
    {
        m_Collider2D.enabled = true;
    }

    void SetBallVelocity()
    {
        m_PlayerBallObject.transform.position = transform.position;
        m_PlayerBallObject.transform.localScale = Vector3.one;
        m_PlayerBody.simulated = true;
        m_PlayerBody.velocity = transform.right * Mathf.Max(m_VelocityMagnitude, m_MinimumVelocity);

    }
}
