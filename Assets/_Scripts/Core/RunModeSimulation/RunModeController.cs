using System.Collections.Generic;
using UnityEngine;

public class RunModeController : MonoBehaviour, IGameStateHandler
{
    [SerializeField]
    List<RunModeObjectData> m_RunModeObjects = new List<RunModeObjectData>();

    private int m_RunModeSelectedIndex = -1;

    public bool hasTriggerableObjects => m_RunModeObjects.Count > 0;

    public void FindAndAssignIndices()
    {
        m_RunModeObjects.Clear();
        InteractionModeHandler[] interactiveObjects = FindObjectsOfType<InteractionModeHandler>();
        if (interactiveObjects.Length > 0)
        {
            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                if ((interactiveObjects[i].objectActiveInMode & InteractionModeType.RunMode) != 0)
                {
                    var triggerableObjects = interactiveObjects[i].gameObject.GetComponents<TriggerableObjectBase>();
                    if (triggerableObjects.Length > 0)
                    {

                        foreach (var triggerableObject in triggerableObjects)
                        {
                            m_RunModeObjects.Add(new RunModeObjectData()
                            {
                                obj = interactiveObjects[i].gameObject,
                                triggerableObject = triggerableObject
                            });
                        }
                    }
                }
            }
        }

    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartEditMode)
        {
            ResetAll();
        }
        if (state == GameState.StartSimulationMode)
        {
            ResetAll();
            if (m_RunModeObjects.Count > 0)
            {
                m_RunModeSelectedIndex = 0;
                m_RunModeObjects[m_RunModeSelectedIndex].triggerableObject.SetHighlightEnabled(true);
            }
            else
            {
                Debug.LogWarning("No RunMode Objects");
            }
        }
    }

    public void SelectNextObject()
    {
        if (m_RunModeSelectedIndex == -1 || m_RunModeSelectedIndex >= m_RunModeObjects.Count)
        {
            return;
        }

        m_RunModeObjects[m_RunModeSelectedIndex].triggerableObject.OnTriggerInput();
        if (!m_RunModeObjects[m_RunModeSelectedIndex].triggerableObject.CanTriggerAgain())
        {
            m_RunModeObjects[m_RunModeSelectedIndex].triggerableObject.SetHighlightEnabled(false);
            m_RunModeSelectedIndex++;
            if (m_RunModeSelectedIndex < m_RunModeObjects.Count)
            {
                m_RunModeObjects[m_RunModeSelectedIndex].triggerableObject.SetHighlightEnabled(true);
            }
        }
    }

    void ResetAll()
    {
        GameObject editModeSelectedObject = GameRepository.Instance.editModeController.GetSelectedObject();
        foreach (var data in m_RunModeObjects)
        {
            data.triggerableObject?.OnResetObject();
            if (editModeSelectedObject == data.obj)
            {
                continue;
            }
            data.triggerableObject.SetHighlightEnabled(false);
        }
    }
}
