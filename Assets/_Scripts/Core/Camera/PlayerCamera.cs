using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IGameStateHandler
{
    [SerializeField]
    Cinemachine.CinemachineVirtualCamera m_EditModeCamera = null;
    [SerializeField]
    EditModeCameraController m_EditModeCameraController = null;

    [SerializeField]
    Cinemachine.CinemachineVirtualCamera m_RunModeCamera = null;

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
                m_RunModeCamera.enabled = false;

                m_EditModeCameraController.enabled = true;
                m_EditModeCamera.enabled = true;
                break;
            case GameState.StartSimulationMode:
                m_EditModeCameraController.enabled = false;
                m_EditModeCamera.enabled = false;

                m_RunModeCamera.enabled = true;
                break;
            default:
                break;
        }
    }
}
