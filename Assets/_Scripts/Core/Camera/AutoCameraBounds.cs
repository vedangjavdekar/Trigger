using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PolygonCollider2D))]
public class AutoCameraBounds : MonoBehaviour, IGameStateHandler
{
    [SerializeField]
    Grid m_Grid = null;
    [SerializeField]
    Tilemap m_Level = null;
    [SerializeField]
    Cinemachine.CinemachineConfiner2D m_EditModeConstraint = null;
    [SerializeField]
    Cinemachine.CinemachineConfiner2D m_RunModeConstraint = null;

    [SerializeField]
    Vector2Int m_EditModeBounds = Vector2Int.one;
    [SerializeField]
    Vector2Int m_RunModeBounds = Vector2Int.one;

    PolygonCollider2D m_PolygonCollider = null;

    PolygonCollider2D polygonCollider {
        get 
        {
            if(!m_PolygonCollider)
            {
                m_PolygonCollider = GetComponent<PolygonCollider2D>();
            }
            return m_PolygonCollider; 
        } 
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartEditMode)
        {
            RecalculateConfinerBounds(m_EditModeBounds);
            m_EditModeConstraint.InvalidateCache();
        }
        else if (state == GameState.StartSimulationMode)
        {
            RecalculateConfinerBounds(m_RunModeBounds);
            m_RunModeConstraint.InvalidateCache();
        }
    }
    
    private void RecalculateConfinerBounds(Vector2Int boundary)
    {
        
        if (m_Grid && m_Level)
        {
            m_Level.CompressBounds();

            Vector3 cellSize = m_Grid.cellSize;
            Bounds levelBounds = m_Level.localBounds;
            Vector3 levelMin = m_Level.transform.TransformPoint(levelBounds.min);
            Vector3 levelMax = m_Level.transform.TransformPoint(levelBounds.max);

            Vector2[] points = new Vector2[4]; // TL, TR, BL, BR

            points[0] = new Vector3(levelMax.x, levelMin.y, 0) + new Vector3(boundary.x * cellSize.x, -boundary.y * cellSize.y, 0);
            points[1] = new Vector3(levelMax.x, levelMax.y, 0) + new Vector3(boundary.x * cellSize.x, boundary.y * cellSize.y, 0);
            points[2] = new Vector3(levelMin.x, levelMax.y, 0) + new Vector3(-boundary.x * cellSize.x, boundary.y * cellSize.y, 0);
            points[3] = new Vector3(levelMin.x, levelMin.y, 0) + new Vector3(-boundary.x * cellSize.x, -boundary.y * cellSize.y, 0);

            polygonCollider.points = points;
        }
    }

    [ContextMenu("Edit Mode Boundary")]
    private void RecalculateEditModeBoundary()
    {
        RecalculateConfinerBounds(m_EditModeBounds);
        m_EditModeConstraint?.InvalidateCache();
    }

    [ContextMenu("Run Mode Boundary")]
    private void RecalculateRunModeBoundary()
    {

        RecalculateConfinerBounds(m_RunModeBounds);
        m_RunModeConstraint?.InvalidateCache();
    }
}
