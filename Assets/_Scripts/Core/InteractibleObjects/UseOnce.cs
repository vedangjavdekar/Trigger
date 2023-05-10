using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseOnce : MonoBehaviour, IGameStateHandler
{
    Collider2D m_Collider;
    Rigidbody2D m_Rigidbody;

    TweenerCore<Vector3,Vector3,VectorOptions> m_Tween;

    public bool invokeTweenManually;

    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartEditMode || state == GameState.StartSimulationMode)
        {
            m_Tween.Kill(true);
            transform.localScale = Vector3.one;
            if (m_Collider)
            {
                m_Collider.enabled = true;
            }
            if (m_Rigidbody)
            {
                m_Rigidbody.simulated = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_Collider)
        {
            m_Collider.enabled = false;
        }
        if (m_Rigidbody)
        {
            m_Rigidbody.simulated = false;
        }
        if (!invokeTweenManually)
        {
            m_Tween = transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_Collider)
        {
            m_Collider.enabled = false;
        }
        if (m_Rigidbody)
        {
            m_Rigidbody.simulated = false;
        }
        if (!invokeTweenManually)
        {
            m_Tween = transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }
    }
}
