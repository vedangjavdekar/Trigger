using UnityEngine;
using UnityEngine.InputSystem;

public class MusicButton : InputButton
{
    private void OnEnable()
    {
        var buttonValue = GameManager.Instance.gameInputController.GetSoundsActions().ToggleMusic.bindings[0].ToDisplayString();
        m_ButtonText.SetText(buttonValue);
        GameManager.Instance.gameInputController.GetSoundsActions().ToggleMusic.performed += OnInteraction;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetSoundsActions().ToggleMusic.performed -= OnInteraction;
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetSoundsActions().ToggleMusic.performed -= OnInteraction;
    }

    protected override void OnInteraction(InputAction.CallbackContext context)
    {
        base.OnInteraction(context);
        GameManager.Instance.soundManager.ToggleMusic();
    }
}
