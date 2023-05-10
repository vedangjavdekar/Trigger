using System;
using System.Collections.Generic;
using UnityEngine;

public class EditModeController : MonoBehaviour, IGameStateHandler
{
    [SerializeField]
    List<EditModeObjectData> m_EditModeObjects = new List<EditModeObjectData>();

    int m_EditModeSelectedIndex = -1;

    public static event Action<GameObject> OnFocusObject;

    public bool hasEditableElements => m_EditModeObjects.Count > 0;

    public void FindAndAssignIndices()
    {
        m_EditModeObjects.Clear();
        InteractionModeHandler[] objects = FindObjectsOfType<InteractionModeHandler>();
        if (objects.Length > 0)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if ((objects[i].objectActiveInMode & InteractionModeType.EditMode) != 0)
                {
                    var editableObjects = objects[i].gameObject.GetComponents<EditableObjectBase>();
                    if (editableObjects.Length > 0)
                    {
                        foreach (var editableObject in editableObjects)
                        {
                            m_EditModeObjects.Add(new EditModeObjectData
                            {
                                obj = objects[i].gameObject,
                                editableObject = editableObject,
                            });
                        }
                    }

                }
            }
        }
    }
    public void OnGameStateEnd(GameState state)
    {
        if (state == GameState.StartEditMode)
        {
            ResetAll();
        }
    }

    private void ResetAll()
    {
        foreach (var data in m_EditModeObjects)
        {
            data.editableObject.SetHighlightEnabled(false);
        }
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartEditMode)
        {
            RestoreSelectedObject();
        }
    }
    public void SelectNextObject()
    {
        if (m_EditModeSelectedIndex != -1)
        {
            m_EditModeObjects[m_EditModeSelectedIndex].editableObject.SetHighlightEnabled(false);
        }
        SetNewSelectedIndex((m_EditModeSelectedIndex + 1) % m_EditModeObjects.Count);
    }

    public void SelectPreviousObject()
    {
        if (m_EditModeSelectedIndex != -1)
        {
            m_EditModeObjects[m_EditModeSelectedIndex].editableObject.SetHighlightEnabled(false);
        }
        SetNewSelectedIndex((m_EditModeSelectedIndex - 1 + m_EditModeObjects.Count) % m_EditModeObjects.Count);
    }

    private void SetNewSelectedIndex(int index)
    {
        m_EditModeSelectedIndex = index;

        m_EditModeObjects[m_EditModeSelectedIndex].editableObject.SetHighlightEnabled(true);

        OnFocusObject?.Invoke(m_EditModeObjects[m_EditModeSelectedIndex].obj);
    }

    private void RestoreSelectedObject()
    {
        if (m_EditModeSelectedIndex == -1)
        {
            if (m_EditModeObjects.Count == 0)
            {
                return;
            }
            m_EditModeSelectedIndex = 0;
        }
        SetNewSelectedIndex(m_EditModeSelectedIndex);
    }

    public void DecrementInput(bool wasPressedThisFrame)
    {
        if (m_EditModeSelectedIndex == -1)
        {
            return;
        }
        m_EditModeObjects[m_EditModeSelectedIndex].editableObject.DecrementInput(wasPressedThisFrame);
    }

    public void IncrementInput(bool wasPressedThisFrame)
    {
        if (m_EditModeSelectedIndex == -1)
        {
            return;
        }
        m_EditModeObjects[m_EditModeSelectedIndex].editableObject.IncrementInput(wasPressedThisFrame);
    }

    public GameObject GetSelectedObject()
    {
        if (m_EditModeSelectedIndex == -1)
        {
            return null;
        }

        return m_EditModeObjects[m_EditModeSelectedIndex].obj;
    }
}
