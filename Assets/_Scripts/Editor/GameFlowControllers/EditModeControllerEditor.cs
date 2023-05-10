using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditModeController))]
public class EditModeControllerEditor : Editor
{
    EditModeController m_Controller;

    private void OnEnable()
    {
        m_Controller = (EditModeController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Find & Assign Indices"))
        {
            m_Controller.FindAndAssignIndices();
        }
    }
}
