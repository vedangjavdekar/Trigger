using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum VisualizerType
{
    None,
    Line,
    Circle
}

[Serializable]
public struct VisualizerData
{
    public SpriteRenderer spriteRenderer;
    public VisualizerType visualizerType;
}

public abstract class VisualizerBase : MonoBehaviour, IVisualizable
{
    public VisualizationResponseSO responseSO;

    [SerializeField]
    private bool m_DisableOnHidden = false;

    [SerializeField, Header("Visibility Settings")]
    protected VisualizationResponse m_EditStateResponse = VisualizationResponse.Visible;

    [SerializeField]
    protected VisualizationResponse m_RunStateResponse = VisualizationResponse.Visible;

    [SerializeField]
    protected List<VisualizerData> m_Data = new List<VisualizerData>();

    public abstract void CreateVisualizer(VisualizationSO creationData);
    public virtual void RemoveVisualizer()
    {
        foreach (var data in m_Data)
        {
            if (data.spriteRenderer != null)
            {
                DestroyImmediate(data.spriteRenderer.gameObject);
            }
        }
        m_Data.Clear();
    }

    protected virtual void HandleVisualizationResponse(VisualizationResponse response)
    {
        switch (response)
        {
            case VisualizationResponse.Hidden:
                foreach (var data in m_Data)
                {
                    if (m_DisableOnHidden)
                    {
                        data.spriteRenderer.gameObject.SetActive(false);
                    }
                    else if (responseSO != null)
                    {
                        data.spriteRenderer.color = responseSO.hidden;
                    }
                }
                break;
            case VisualizationResponse.SemiVisible:
                foreach (var data in m_Data)
                {
                    if (m_DisableOnHidden)
                    {
                        data.spriteRenderer.gameObject.SetActive(true);
                    }

                    if (responseSO != null)
                    {
                        data.spriteRenderer.color = responseSO.semiVisible;
                    }
                }
                break;
            case VisualizationResponse.Visible:
                foreach (var data in m_Data)
                {
                    if (m_DisableOnHidden)
                    {
                        data.spriteRenderer.gameObject.SetActive(true);
                    }

                    if (responseSO != null)
                    {
                        data.spriteRenderer.color = responseSO.visible;
                    }
                }
                break;
            default:
                break;
        }
    }


    protected GameObject AddCircle(GameObject circlePrefab, Vector3 point, float size)
    {
#if UNITY_EDITOR
        if (circlePrefab == null)
        {
            Debug.LogError("Circle Prefab is null.");
            return null;
        }
        GameObject circle = (GameObject)PrefabUtility.InstantiatePrefab(circlePrefab);
        circle.transform.position = point;
        circle.transform.localScale = Vector3.one * size;
        m_Data.Add(new VisualizerData { spriteRenderer = circle.GetComponent<SpriteRenderer>(), visualizerType = VisualizerType.Circle });
        return circle;
#else
    return null;
#endif
    }

    protected GameObject AddLine(GameObject linePrefab, Vector3 start, Vector3 end, float size)
    {
#if UNITY_EDITOR
        GameObject line = (GameObject)PrefabUtility.InstantiatePrefab(linePrefab);
        Vector3 scale = Vector3.one * size;
        scale.x = (end - start).magnitude;
        line.transform.position = 0.5f * (start + end);
        line.transform.right = (end - start).normalized;
        line.transform.localScale = scale;
        m_Data.Add(new VisualizerData { spriteRenderer = line.GetComponent<SpriteRenderer>(), visualizerType = VisualizerType.Line });
        return line;
#else
        return null;
#endif
    }
}
