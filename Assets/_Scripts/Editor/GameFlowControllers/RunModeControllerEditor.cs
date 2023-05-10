using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RunModeController))]
public class RunModeControllerEditor : Editor
{
    RunModeController m_Controller;

    private void OnEnable()
    {
        m_Controller = (RunModeController)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Find & Assign Indices"))
        {
            m_Controller.FindAndAssignIndices();
        }
    }
}
