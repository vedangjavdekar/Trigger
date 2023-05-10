using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelNamesSO))]
public class LevelNamesSOEditor : Editor
{
    LevelNamesSO m_LevelNames;

    private void OnEnable()
    {
        m_LevelNames = (LevelNamesSO)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reset"))
        {
            m_LevelNames.levelNames.Clear();
        }
        EditorGUILayout.Space(5);
        DropAreaGUI();
    }
    public void DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Drop your Scenes here.");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                {
                    if (!drop_area.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object draggedThingie in DragAndDrop.objectReferences)
                        {
                            var scene = (SceneAsset)draggedThingie;
                            if(scene != null)
                            {
                                m_LevelNames.levelNames.Add(scene.name);
                            }
                        }
                    }
                    break;
                }
        }
    }
}
