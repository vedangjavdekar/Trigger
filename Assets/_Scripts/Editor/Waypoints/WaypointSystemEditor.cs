using DG.DOTweenEditor.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor
{
    WaypointSystem m_WaypointSystem;

    float m_SimulationStepSize = 0.1f;

    private void OnEnable()
    {
        m_WaypointSystem = (WaypointSystem)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            return;
        }
        if (GUILayout.Button("Align"))
        {
            m_WaypointSystem.transform.position = m_WaypointSystem.GetWaypoints()[0].position;
        }

        if (GUILayout.Button("Create New Waypoint"))
        {
            m_WaypointSystem.CreateNewWaypoint();
        }

        GUILayout.Label("Simulation");
        GUILayout.Space(10);

        m_SimulationStepSize = EditorGUILayout.FloatField("SimulationStep", m_SimulationStepSize);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Decrement"))
        {
            m_WaypointSystem.HandleChange(-m_SimulationStepSize);
        }
        if (GUILayout.Button("Increment"))
        {
            m_WaypointSystem.HandleChange(m_SimulationStepSize);
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Reset Displacement"))
        {
            m_WaypointSystem.ResetDisplacement();
        }
    }
}
