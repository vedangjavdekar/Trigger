using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class NextLevelPanel : MonoBehaviour, IGameStateHandler
{
    CanvasGroup m_CanvasGroup;

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if(state == GameState.GoalReached)
        {
            ShowPanel();
        }
        else
        {
            HidePanel(false);
        }
    }

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }
    private void ShowPanel()
    {
        m_CanvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.InOutCubic);
    }

    private void HidePanel(bool animate = true)
    {
        if(!animate)
        {
            m_CanvasGroup.alpha = 0.0f;
            return;
        }

        m_CanvasGroup.DOFade(0.0f, 0.5f).SetEase(Ease.InOutCubic);
    }
}
