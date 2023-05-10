using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class RunModeCamera : MonoBehaviour,IGameStateHandler
{
    Cinemachine.CinemachineVirtualCamera m_VirtualCamera;

    [SerializeField]
    Transform m_ResetTransform;

    private void Awake()
    {
        m_VirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if(state == GameState.StartSimulationMode)
        {
            if(m_ResetTransform)
            {
                m_VirtualCamera.enabled = false;
                Vector3 newPosition = m_ResetTransform.position;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
                m_VirtualCamera.enabled = true;
            }
        }
    }

}
