using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThankYouScreenMenu : MonoBehaviour, IGameStateHandler
{
    public CanvasGroup[] fadeSequence;

    public float delay;

    private bool m_SequenceInitiated = false;

    private void Awake()
    {
        foreach(var cgroup in fadeSequence)
        {
            cgroup.alpha = 0;
        }
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartEditMode)
        {
            if (!m_SequenceInitiated)
            {
                m_SequenceInitiated = true;

                var seq = DOTween.Sequence();
                seq.AppendInterval(delay);
                for (int i = 0; i < fadeSequence.Length; i++)
                {
                    seq.Append(fadeSequence[i]
                        .DOFade(1.0f, 0.4f)
                        .From(0.0f)
                        .SetEase(Ease.InCirc));
                }
            }
        }
    }
}
