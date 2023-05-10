using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeInputHandler : MonoBehaviour, IGameStateHandler
{
    InteractionModeType m_CurrMode = InteractionModeType.None;

    public InteractionModeType currMode => m_CurrMode;

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
                m_CurrMode = InteractionModeType.EditMode;
                break;
            case GameState.StartSimulationMode:
                m_CurrMode = InteractionModeType.RunMode;
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.gameInputController.GetEditModeActionController().Next.performed += SelectNext;
        GameManager.Instance.gameInputController.GetEditModeActionController().Previous.performed += SelectPrevious;

        GameManager.Instance.gameInputController.GetRunModeActionController().Trigger.performed += TriggerInputPressed;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameInputController.GetEditModeActionController().Next.performed -= SelectNext;
        GameManager.Instance.gameInputController.GetEditModeActionController().Previous.performed -= SelectPrevious;

        GameManager.Instance.gameInputController.GetRunModeActionController().Trigger.performed -= TriggerInputPressed;
    }
    private void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetEditModeActionController().Next.performed -= SelectNext;
        GameManager.Instance.gameInputController.GetEditModeActionController().Previous.performed -= SelectPrevious;

        GameManager.Instance.gameInputController.GetRunModeActionController().Trigger.performed -= TriggerInputPressed;
    }

    private void Update()
    {
        if (GameManager.Instance.gameStateManager.currState != GameState.StartEditMode)
        {
            return;
        }

        var controller = GameManager.Instance.gameInputController.GetEditModeActionController();

        if (controller.IncrementValue.IsPressed())
        {
            GameRepository.Instance.editModeController.IncrementInput(controller.IncrementValue.WasPressedThisFrame());
        }
        if (controller.DecrementValue.IsPressed())
        {
            GameRepository.Instance.editModeController.DecrementInput(controller.DecrementValue.WasPressedThisFrame());
        }
    }

    private void SelectNext(InputAction.CallbackContext context)
    {
        if (GameRepository.Instance.editModeController.hasEditableElements)
        {
            GameRepository.Instance.editModeController.SelectNextObject();
        }
    }

    private void SelectPrevious(InputAction.CallbackContext context)
    {
        if (GameRepository.Instance.editModeController.hasEditableElements)
        {
            GameRepository.Instance.editModeController.SelectPreviousObject();
        }
    }

    private void TriggerInputPressed(InputAction.CallbackContext context)
    {
        if (GameRepository.Instance.runModeController.hasTriggerableObjects)
        {
            GameRepository.Instance.runModeController.SelectNextObject();
        }
    }
}
