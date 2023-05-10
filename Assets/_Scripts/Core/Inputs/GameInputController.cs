using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputController : MonoBehaviour, IGameStateHandler
{
    private GameInputActions m_GameInputActions = null;
    public InputAction cameraMovementAction { get; private set; } = null; // Polling, hence cached

    private void Awake()
    {
        m_GameInputActions = new GameInputActions();
        cameraMovementAction = m_GameInputActions.CameraControls.Movement;
        m_GameInputActions.Help.Enable();
        m_GameInputActions.Sounds.Enable();
    }
    private void OnEnable()
    {
        m_GameInputActions.GlobalActions.GoToEditMode.performed += ChangeToEditMode;
        m_GameInputActions.GlobalActions.GoToSimulationMode.performed += ChangeToSimulationMode;
    }

    private void OnDisable()
    {
        m_GameInputActions.CameraControls.Disable();
        m_GameInputActions.GlobalActions.Disable();
    }

    private void OnDestroy()
    {
        m_GameInputActions.CameraControls.Disable();
        m_GameInputActions.GlobalActions.Disable();

        m_GameInputActions.GlobalActions.GoToEditMode.performed -= ChangeToEditMode;
        m_GameInputActions.GlobalActions.GoToSimulationMode.performed -= ChangeToSimulationMode;
    }

    public void EnableLevelControls()
    {
        m_GameInputActions.CameraControls.Enable();
        m_GameInputActions.GlobalActions.Enable();
    }

    public void DisableLevelControls()
    {
        m_GameInputActions.CameraControls.Disable();
        m_GameInputActions.GlobalActions.Disable();
    }

    public void OnHealthPanelOpen()
    {
        m_GameInputActions.CameraControls.Disable();
        m_GameInputActions.GlobalActions.Disable();

        m_GameInputActions.EditMode.Disable();
        m_GameInputActions.RunMode.Disable();
    }
    public void OnHealthPanelClosed()
    {
        m_GameInputActions.CameraControls.Enable();
        m_GameInputActions.GlobalActions.Enable();

        m_GameInputActions.EditMode.Enable();
        m_GameInputActions.RunMode.Enable();
    }

    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
                {
                    m_GameInputActions.RunMode.Disable();
                    m_GameInputActions.EditMode.Enable();
                    break;
                }
            case GameState.StartSimulationMode:
                {
                    m_GameInputActions.EditMode.Disable();
                    m_GameInputActions.RunMode.Enable();
                    break;
                }
            case GameState.StartSimulation:
                {
                    break;
                }
            case GameState.GoalReached:
                {
                    m_GameInputActions.EditMode.Disable();
                    m_GameInputActions.RunMode.Disable();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public GameInputActions.MenuActions GetMenuActions()
    {
        return m_GameInputActions.Menu;
    }
    public GameInputActions.SoundsActions GetSoundsActions()
    {
        return m_GameInputActions.Sounds;
    }

    public GameInputActions.EditModeActions GetEditModeActionController()
    {
        return m_GameInputActions.EditMode;
    }

    public GameInputActions.RunModeActions GetRunModeActionController()
    {
        return m_GameInputActions.RunMode;
    }
    public GameInputActions.CameraControlsActions GetCameraControlsActions()
    {
        return m_GameInputActions.CameraControls;
    }
    public GameInputActions.GlobalActionsActions GetGlobalControlsActions()
    {
        return m_GameInputActions.GlobalActions;
    }
    public GameInputActions.MenuActions GetMenuControlActions()
    {
        return m_GameInputActions.Menu;
    }
    public GameInputActions.HelpActions GetHelpControlActions()
    {
        return m_GameInputActions.Help;
    }

    private void ChangeToEditMode(InputAction.CallbackContext inputContext)
    {
        GameManager.Instance.gameStateManager.PushNewState(GameState.StartEditMode);
    }
    private void ChangeToSimulationMode(InputAction.CallbackContext inputContext)
    {
        GameManager.Instance.gameStateManager.PushNewState(GameState.StartSimulationMode);
    }

}
