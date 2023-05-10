using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStateManager))]
public class GameStateManagerEditor : Editor
{
    GameStateManager m_StateManager = null;

    private void OnEnable()
    {
        m_StateManager = (GameStateManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {

            foreach (var e in System.Enum.GetNames(typeof(GameState)))
            {
                if (GUILayout.Button(e))
                {
                    m_StateManager.PushNewState((GameState)System.Enum.Parse(typeof(GameState), e));
                }
            }
        }
    }
}
