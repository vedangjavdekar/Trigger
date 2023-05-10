using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(WaypointSystem))]
public class MovableInteraction : EditableObjectBase, IGameStateHandler
{
    [SerializeField]
    private Transform m_TargetPoint;

    [SerializeField]
    private ObjectHighlight m_ObjectHighlight;

    [SerializeField]
    private float m_SmoothingTime;

    [SerializeField]
    private float m_StepSize;

    [SerializeField]
    private bool m_ContinuousInput = false;
    private Vector3 m_SmoothingVelocity;

    bool m_IsActive = false;

    WaypointSystem m_WaypointSystem; 

    WaypointSystem waypointSystem
    {
        get
        {
            if (m_WaypointSystem == null)
            {
                m_WaypointSystem = GetComponent<WaypointSystem>();
            }
            return m_WaypointSystem;
        }
    }

    private void Start()
    {
        if (!m_TargetPoint)
        {
            Debug.LogError("Required Transforms are not set.");
            return;
        }
        m_TargetPoint.position = waypointSystem.position;
        transform.position = waypointSystem.position;
    }

    public override void DecrementInput(bool wasPressedThisFrame)
    {
        if (!m_TargetPoint)
        {
            Debug.LogError("Required Transforms are not set.");
            return;
        }
        if (m_ContinuousInput || (!m_ContinuousInput && wasPressedThisFrame))
        {
            waypointSystem.HandleChange(-m_StepSize);
            m_TargetPoint.position = waypointSystem.position;
        }
    }

    public override void IncrementInput(bool wasPressedThisFrame)
    {
        if (!m_TargetPoint)
        {
            Debug.LogError("Required Transforms are not set.");
            return;
        }

        if (m_ContinuousInput || (!m_ContinuousInput && wasPressedThisFrame))
        {
            waypointSystem.HandleChange(m_StepSize);
            m_TargetPoint.position = waypointSystem.position;
        }
    }

    public override void SetHighlightEnabled(bool enable)
    {
        m_ObjectHighlight?.SetHighlightEnabled(enable, ObjectHighlightGizmoType.Move);
    }

    private void Update()
    {
        if (!m_IsActive)
        {
            return;
        }
        transform.position = Vector3.SmoothDamp(transform.position, m_TargetPoint.position, ref m_SmoothingVelocity, m_SmoothingTime);
    }

    public void OnGameStateStart(GameState state)
    {
        if(m_IsActive && state != GameState.StartEditMode)
        {
            waypointSystem.SaveCurrentPoint();
        }
        m_IsActive = (state == GameState.StartEditMode);
    }
    public void OnGameStateEnd(GameState state) { }

    
}
