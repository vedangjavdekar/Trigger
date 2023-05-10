using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditModeButton : InputButton
{
    private void OnEnable()
    {
        var buttonValue = GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToEditMode.bindings[0].ToDisplayString();
        m_ButtonText.SetText(buttonValue);
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToEditMode.performed += OnInteraction;
    }
    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToEditMode.performed -= OnInteraction;
    }
    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToEditMode.performed -= OnInteraction;
    }
}
