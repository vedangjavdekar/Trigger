using UnityEngine;
using UnityEngine.InputSystem;

public class SoundButton : InputButton
{
    private void OnEnable()
    {
        var buttonValue = GameManager.Instance.gameInputController.GetSoundsActions().ToggleSounds.bindings[0].ToDisplayString();
        m_ButtonText.SetText(buttonValue);
        GameManager.Instance.gameInputController.GetSoundsActions().ToggleSounds.performed += OnInteraction;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetSoundsActions().ToggleSounds.performed -= OnInteraction;
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetSoundsActions().ToggleSounds.performed -= OnInteraction;
    }
    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        base.OnInteraction(context);
        GameManager.Instance.soundManager.ToggleSounds();
    }
}
