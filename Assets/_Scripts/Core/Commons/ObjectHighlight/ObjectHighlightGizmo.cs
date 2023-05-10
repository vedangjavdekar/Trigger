using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectHighlightGizmoType
{
    None,
    Move,
    Rotate
}

public class ObjectHighlightGizmo : MonoBehaviour
{
    [Serializable]
    private struct GizmoData
    {
        public ObjectHighlightGizmoType GizmoType;
        public Sprite GizmoSprite;
    }

    [SerializeField]
    List<GizmoData> m_Gizmos = new List<GizmoData>();

    [SerializeField]
    SpriteRenderer m_GizmoRenderer;

    private void OnValidate()
    {
        if (m_Gizmos.Count < 1)
        {
            m_Gizmos.Add(new GizmoData { GizmoType = ObjectHighlightGizmoType.None, GizmoSprite = null });
        }
    }

    public void SetGizmo(bool enabled, ObjectHighlightGizmoType gizmoType)
    {

        var gizmoData = m_Gizmos.FirstOrDefault((GizmoData data) => data.GizmoType == gizmoType);
        if (m_GizmoRenderer)
        {
            m_GizmoRenderer.enabled = enabled;
            m_GizmoRenderer.sprite = gizmoData.GizmoSprite;
        }
    }
}
