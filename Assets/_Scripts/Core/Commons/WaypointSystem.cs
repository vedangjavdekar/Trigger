using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WaypointSystem : MonoBehaviour, IWaypointInteraction
{
    [SerializeField]
    private List<Transform> m_WayPoints = new List<Transform>();
    public int TotalPoints => cycle ? m_WayPoints.Count + 1 : m_WayPoints.Count;

    private Transform m_StartPoint;
    public Vector3 CurrentStart => m_StartPoint.position;
    public Vector2 CurrentStart2D => new(m_StartPoint.position.x, m_StartPoint.position.y);

    private Transform m_EndPoint;
    public Vector3 CurrentEnd => m_EndPoint.position;
    public Vector2 CurrentEnd2D => new(m_EndPoint.position.x, m_EndPoint.position.y);

    private int m_CurrentEndIndex = 1;
    public int CurrentEndIndex => m_CurrentEndIndex;
    public int CurrentStartIndex
    {
        get
        {
            if(cycle)
            {
                int index = m_CurrentEndIndex- 1;
                if(index < 0)
                {
                    index += m_WayPoints.Count;
                }
                return index;
            }
            else
            {
                return Mathf.Max(m_CurrentEndIndex - 1, 0);
            }
        }
    }

    private float m_CurrentLerpPercent = 0.0f;
    public float Interpolation => m_CurrentLerpPercent;

    [SerializeField]
    private bool m_Cycle = false;

    public bool cycle => m_Cycle;

    private struct WaypointSavedPoint
    {
        public float interp;
        public int endIndex;
    }

    WaypointSavedPoint m_ResetPoint;

    private void Awake()
    {
        m_ResetPoint = new WaypointSavedPoint { endIndex = 1, interp = 0.0f };
        m_StartPoint = m_WayPoints[0];
        m_EndPoint = m_WayPoints[1];
    }

    private void OnValidate()
    {
        if (m_StartPoint == null)
        {
            if (m_WayPoints.Count < 1)
            {
                GameObject obj1 = new GameObject("Waypoint.001");
                obj1.transform.position = transform.position;
                obj1.transform.SetParent(transform.parent);
                m_WayPoints.Add(obj1.transform);
            }
            m_StartPoint = m_WayPoints[0];
        }
        if (m_EndPoint == null)
        {
            if (m_WayPoints.Count < 2)
            {
                GameObject obj2 = new GameObject("Waypoint.002");
                obj2.transform.position = transform.position + Vector3.up;
                obj2.transform.SetParent(transform.parent);
                m_WayPoints.Add(obj2.transform);
            }
            m_EndPoint = m_WayPoints[1];
            m_CurrentEndIndex = 1;
        }

        for (int i = 0; i < m_WayPoints.Count; i++)
        {
            m_WayPoints[i].name = $"Waypoint.{i:000}";
        }
    }

    public Vector3 position
    {
        get
        {
            if (!m_StartPoint || !m_EndPoint)
            {
                return transform.position;
            }

            return Vector3.Lerp(m_StartPoint.position, m_EndPoint.position, m_CurrentLerpPercent);
        }
    }

    public void HandleChange(float delta)
    {
        m_CurrentLerpPercent += delta;
        if (m_CurrentLerpPercent > 1)
        {
            if (m_CurrentEndIndex + 1 < m_WayPoints.Count)
            {
                float lerpDelta = m_CurrentLerpPercent - 1.0f;
                m_CurrentLerpPercent = Mathf.Clamp01(lerpDelta);

                m_CurrentEndIndex++;
                m_StartPoint = m_EndPoint;
                m_EndPoint = m_WayPoints[m_CurrentEndIndex];
            }
            else
            {
                m_CurrentLerpPercent = 1.0f;
            }
        }
        if (m_CurrentLerpPercent < 0)
        {
            if (m_CurrentEndIndex - 1 >= 1)
            {
                float lerpDelta = m_CurrentLerpPercent + 1.0f;
                m_CurrentLerpPercent = Mathf.Clamp01(lerpDelta);

                m_EndPoint = m_StartPoint;
                m_StartPoint = m_WayPoints[m_CurrentEndIndex - 2];
                m_CurrentEndIndex--;
            }
            else
            {
                m_CurrentLerpPercent = 0.0f;
            }
        }
    }

    public bool IsNextPairValid()
    {
        if (cycle)
        {
            return true;
        }
        return m_CurrentEndIndex + 1 < m_WayPoints.Count;
    }

    public bool NextPair()
    {
        if (!cycle)
        {
            if (m_CurrentEndIndex + 1 >= m_WayPoints.Count)
            {
                return false;
            }
            m_CurrentEndIndex++;
        }
        else
        {
            m_CurrentEndIndex = (m_CurrentEndIndex + 1) % m_WayPoints.Count;
        }
        m_StartPoint = m_EndPoint;
        m_EndPoint = m_WayPoints[m_CurrentEndIndex];
        return true;
    }
    public Vector3 Evaluate(float interpolation)
    {
        return Vector3.Lerp(m_StartPoint.position, m_EndPoint.position, interpolation);
    }

    public void SaveCurrentPoint()
    {
        m_ResetPoint.interp = m_CurrentLerpPercent;
        m_ResetPoint.endIndex = m_CurrentEndIndex;
    }

    public Vector3 RestoreSavedPoint()
    {
        m_StartPoint = m_WayPoints[m_ResetPoint.endIndex - 1];
        m_EndPoint = m_WayPoints[m_ResetPoint.endIndex];
        m_CurrentEndIndex = m_ResetPoint.endIndex;
        m_CurrentLerpPercent = m_ResetPoint.interp;

        return position;
    }


    public void ResetDisplacement()
    {
        m_StartPoint = m_WayPoints[0];
        m_EndPoint = m_WayPoints[1];
        m_CurrentEndIndex = 1;
        m_CurrentLerpPercent = 0;
    }

    public void CreateNewWaypoint()
    {
        GameObject obj1 = new GameObject($"Waypoint.{m_WayPoints.Count:000}");
        if (m_WayPoints.Count > 0)
        {
            obj1.transform.position = m_WayPoints[m_WayPoints.Count - 1].position + Vector3.up;
        }
        else
        {
            obj1.transform.position = transform.position;
        }

        obj1.transform.SetParent(transform.parent);
        m_WayPoints.Add(obj1.transform);
    }

    public void ClearEmptyWaypoints()
    {
        for (int i = m_WayPoints.Count - 1; i >= 0; i--)
        {
            if (m_WayPoints[i] == null)
            {
                m_WayPoints.RemoveAt(i);
            }
        }
    }

    public List<Transform> GetWaypoints()
    {
        return m_WayPoints;
    }

    private void OnDrawGizmos()
    {
        ClearEmptyWaypoints();


        Gizmos.color = Color.green;
        for (int i = 0; i < m_WayPoints.Count - 1; i++)
        {
            Gizmos.DrawWireSphere(m_WayPoints[i].position, 0.2f);
            Gizmos.DrawWireSphere(m_WayPoints[i + 1].position, 0.2f);
            Gizmos.DrawLine(m_WayPoints[i].position, m_WayPoints[i + 1].position);
        }

        if (m_StartPoint && m_EndPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_StartPoint.position, 0.2f);
            Gizmos.DrawWireSphere(m_EndPoint.position, 0.2f);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(Vector3.Lerp(m_StartPoint.position, m_EndPoint.position, m_CurrentLerpPercent), 0.25f);
        }
    }
}
