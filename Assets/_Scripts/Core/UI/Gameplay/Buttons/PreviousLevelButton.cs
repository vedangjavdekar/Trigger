using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreviousLevelButton : InputButton
{
    private void OnEnable()
    {
        var buttonValue = GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToPreviousLevel.bindings[0].ToDisplayString();
        m_ButtonText.SetText(buttonValue);
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToPreviousLevel.performed += OnInteraction;
    }
    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToPreviousLevel.performed -= OnInteraction;
    }
    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToPreviousLevel.performed -= OnInteraction;
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.gameStateManager.currState != GameState.GoalReached)
        {
            return;
        }
        var tween = GetTween();
        tween.OnComplete(() =>
        {
            GameManager.Instance.EndGame(-1);
        });
    }
}
