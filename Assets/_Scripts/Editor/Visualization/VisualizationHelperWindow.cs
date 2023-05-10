using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;

public class VisualizationHelperWindow : EditorWindow
{
    static VisualizationSO m_PrefabAsset;

    List<VisualizerBase> visualizers = new List<VisualizerBase>();

    [MenuItem("Tools/Visualization/VisualizationEditor")]
    public static void GetVisualizationWindow()
    {
        GetWindow<VisualizationHelperWindow>("Visualization Helper");
    }

    private void OnEnable()
    {
        EditorSceneManager.activeSceneChangedInEditMode += EditorSceneChanged;
        RefreshVisualizers();
    }

    private void OnDisable()
    {
        Debug.Log("Clearing visualizers in Editor Window");
        visualizers.Clear();
    }

    void RefreshVisualizers()
    {
        visualizers.Clear();
        GameObject[] objects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in objects)
        {
            visualizers.AddRange(obj.GetComponentsInChildren<VisualizerBase>());
        }
    }

    private void EditorSceneChanged(Scene from, Scene to)
    {
        RefreshVisualizers();
    }

    private void OnGUI()
    {
        if(Application.isPlaying)
        {
            return;
        }
        m_PrefabAsset = (VisualizationSO)EditorGUILayout.ObjectField("PrefabAsset", m_PrefabAsset, typeof(VisualizationSO), false);

        for (int i = 0; i < visualizers.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(visualizers[i].transform.parent.name);
            if (GUILayout.Button("Create"))
            {
                if (m_PrefabAsset == null)
                {
                    EditorUtility.DisplayDialog("Prefab Not Set", "Visualization Prefab is not set. Please select a valid asset for prefab.", "Okay");
                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                visualizers[i].CreateVisualizer(m_PrefabAsset);
            }

            EditorGUILayout.EndHorizontal();
        }

    }
}
