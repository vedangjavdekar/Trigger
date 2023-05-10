using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class RunModeButton : InputButton, IGameStateHandler
{
    [SerializeField]
    TextMeshProUGUI m_Text;

    private void OnEnable()
    {
        var buttonValue = GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToSimulationMode.bindings[0].ToDisplayString();
        m_ButtonText.SetText(buttonValue);
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToSimulationMode.performed += OnInteraction;
    }
    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToSimulationMode.performed -= OnInteraction;
    }
    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().GoToSimulationMode.performed -= OnInteraction;
    }
    
    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if(state == GameState.StartEditMode)
        {
            m_Text.SetText("Play");

        }
        else if(state == GameState.StartSimulationMode)
        {
            m_Text.SetText("Restart");
        }
    }
}
