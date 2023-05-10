using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterVisualizer : VisualizerBase, IGameStateHandler
{
    [SerializeField]
    Transform m_StartPoint;
    [SerializeField]
    Transform m_EndPoint;
    public override void CreateVisualizer(VisualizationSO creationData)
    {
        if (!m_StartPoint || !m_EndPoint)
        {
            Debug.LogError("Start Point or End Point is null");
            return;
        }
        RemoveVisualizer();
        AddCircle(creationData.circlePrefab, m_StartPoint.position, creationData.circleSize);
        AddCircle(creationData.circlePrefab, m_StartPoint.position, creationData.circleSize);
        AddLine(creationData.linePrefab, m_StartPoint.position, m_EndPoint.position, creationData.lineSize);
    }

    public void OnGameStateEnd(GameState state) { }
    public void OnGameStateStart(GameState state)
    {
        switch (state)
        {
            case GameState.StartEditMode:
                HandleVisualizationResponse(m_EditStateResponse);
                break;
            case GameState.StartSimulationMode:
                HandleVisualizationResponse(m_RunStateResponse);
                break;
            case GameState.GoalReached:
                break;
            default:
                break;
        }
    }
}
