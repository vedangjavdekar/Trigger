using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum VisualizationResponse
{
    Hidden = 0,
    SemiVisible,
    Visible,
}
public class MovementVisualizer : VisualizerBase, IGameStateHandler
{
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
    public override void CreateVisualizer(VisualizationSO creationData)
    {
        IWaypointInteraction wayPoints = GetComponent<IWaypointInteraction>();
        if (wayPoints != null)
        {
            RemoveVisualizer();
            List<Transform> points = wayPoints.GetWaypoints();
            AddCircle(creationData.circlePrefab, points[0].position, creationData.circleSize);
            for (int i = 1; i < points.Count; i++)
            {
                AddLine(creationData.linePrefab, points[i - 1].position, points[i].position, creationData.lineSize);
                AddCircle(creationData.circlePrefab, points[i].position, creationData.circleSize);
            }
            if(points.Count > 2 && wayPoints.cycle)
            {
                AddLine(creationData.linePrefab, points[points.Count - 1].position, points[0].position, creationData.lineSize);
            }
        }
        else
        {
            Debug.LogError("Movement Visualizer requires waypoint interaction");
        }
    }
}
