using UnityEngine;


public class ObjectHighlight : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] m_HighlightSpriteRenderers = null;

    [SerializeField]
    Color m_HighlightColor = Color.white;

    [SerializeField]
    bool m_RestoreColor = false;

    Color[] m_CachedColors;

    [SerializeField]
    ObjectHighlightGizmo m_Gizmo;

    private void Awake()
    {
        m_CachedColors = new Color[m_HighlightSpriteRenderers.Length];
    }

    public void SetHighlightEnabled(bool enabled,ObjectHighlightGizmoType gizmoType = ObjectHighlightGizmoType.None)
    {
        if (m_HighlightSpriteRenderers == null)
        {
            return;
        }

        for (int i = 0; i < m_HighlightSpriteRenderers.Length; i++)
        {
            if (m_RestoreColor)
            {
                if (m_HighlightSpriteRenderers[i].color != m_HighlightColor)
                {
                    m_CachedColors[i] = m_HighlightSpriteRenderers[i].color;
                }
                m_HighlightSpriteRenderers[i].enabled = true;
            }
            else
            {
                m_HighlightSpriteRenderers[i].enabled = enabled;
            }

            if (enabled)
            {
                m_HighlightSpriteRenderers[i].color = m_HighlightColor;
            }
            else if (m_RestoreColor)
            {
                m_HighlightSpriteRenderers[i].color = m_CachedColors[i];
            }
        }

        m_Gizmo?.SetGizmo(enabled, gizmoType);
    }
}
