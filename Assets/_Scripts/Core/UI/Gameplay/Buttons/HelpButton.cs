using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpButton : InputButton
{
    [SerializeField]
    private TextMeshProUGUI m_Text;

    [SerializeField]
    private CanvasGroup m_HelpMenu;
    [SerializeField]
    private RectTransform m_HelpPanel;

    [SerializeField]
    private RectTransform m_InitialTransform;
    [SerializeField]
    private RectTransform m_FinalTransform;

    private bool m_IsOpen = false;
    private bool m_IsBusy = false;

    private object m_SeqId;
    private object m_TweenId;

    private void OnEnable()
    {
        var buttonValue = GameManager.Instance.gameInputController.GetHelpControlActions().ToggleHelp.bindings[0].ToDisplayString();
        m_ButtonText.SetText(buttonValue);
        GameManager.Instance.gameInputController.GetHelpControlActions().ToggleHelp.performed += OnInteraction;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetHelpControlActions().ToggleHelp.performed -= OnInteraction;
    }
    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetHelpControlActions().ToggleHelp.performed -= OnInteraction;
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        if (m_IsBusy)
        {
            return;
        }
        base.OnInteraction(context);

        if (m_IsOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        DOTween.Kill(m_SeqId);
        var seq = DOTween.Sequence();
        m_SeqId = seq.id;

        GameManager.Instance.gameInputController.OnHealthPanelOpen();
        m_IsOpen = true;
        m_IsBusy = true;
        seq.Append(transform.DOMove(m_FinalTransform.position, 0.2f).SetEase(Ease.InOutCubic));
        seq.AppendCallback(() =>
        {
            transform.SetParent(m_FinalTransform);
            m_Text.SetText("Close");
        });
        seq.Append(m_HelpMenu.DOFade(1.0f, 0.3f).SetEase(Ease.InOutCubic));
        seq.Join(m_HelpPanel.DOScale(1.0f, 0.1f).SetEase(Ease.InOutBounce));
        seq.AppendCallback(() =>
        {
            m_IsBusy = false;
        });
    }

    private void CloseMenu()
    {
        DOTween.Kill(m_SeqId);
        var seq = DOTween.Sequence();
        m_SeqId = seq.id;

        m_IsOpen = false;

        m_IsBusy = true;
        seq.Append(m_HelpPanel.DOScale(0.0f, 0.3f).SetEase(Ease.InOutBack));
        seq.Join(m_HelpMenu.DOFade(0.0f, 0.3f).SetEase(Ease.InOutCubic));
        seq.Append(transform.DOMove(m_InitialTransform.position, 0.2f).SetEase(Ease.InOutCubic));
        seq.AppendCallback(() =>
        {
            transform.SetParent(m_InitialTransform);
            m_Text.SetText("Help");
        });
        seq.AppendCallback(() =>
        {
            GameManager.Instance.gameInputController.OnHealthPanelClosed();
            m_IsBusy = false;
        });
    }
}
