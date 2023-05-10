using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltInteraction : EditableObjectBase
{
    [SerializeField]
    float m_RotationStep = 1f;

    [SerializeField, Range(0,180)]
    float m_AngleRange = 0.0f;

    [SerializeField]
    float m_TargetAngle = 0.0f;

    float m_InitialAangle = 0.0f;

    [SerializeField]
    float m_RotationSmoothing = 0.12f;
    float m_AngleSpeed = 0.0f;

    [SerializeField]
    bool m_FullCircle = false;

    [SerializeField]
    bool m_ContinuousInput = false;

    [SerializeField]
    ObjectHighlight m_ObjectHighlight = null;

    private float minAngle => m_InitialAangle - Mathf.Abs(m_AngleRange);
    private float maxAngle => m_InitialAangle + Mathf.Abs(m_AngleRange);

    public override void DecrementInput(bool wasPressedThisFrame)
    {
        if (m_ContinuousInput)
        {
            if (m_FullCircle)
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, m_TargetAngle - m_RotationStep, m_RotationStep);
            }
            else
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, Mathf.Max(m_TargetAngle - m_RotationStep, minAngle), m_RotationStep);
            }
        }
        else if (wasPressedThisFrame)
        {
            if (m_FullCircle)
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, m_TargetAngle - m_RotationStep, m_RotationStep);
            }
            else
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, Mathf.Max(m_TargetAngle - m_RotationStep, minAngle), m_RotationStep);
            }
        }

        while (m_TargetAngle > 360.0f)
        {
            m_TargetAngle -= 360.0f;
        }
        while (m_TargetAngle < -360.0f)
        {
            m_TargetAngle += 360.0f;
        }
    }

    public override void IncrementInput(bool wasPressedThisFrame)
    {
        if (m_ContinuousInput)
        {
            if (m_FullCircle)
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, m_TargetAngle + m_RotationStep, m_RotationStep);
            }
            else
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, Mathf.Min(m_TargetAngle + m_RotationStep, maxAngle), m_RotationStep);
            }
        }
        else if (wasPressedThisFrame)
        {
            if (m_FullCircle)
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, m_TargetAngle + m_RotationStep, m_RotationStep);
            }
            else
            {
                m_TargetAngle = Mathf.MoveTowardsAngle(m_TargetAngle, Mathf.Min(m_TargetAngle + m_RotationStep, maxAngle), m_RotationStep);
            }
        }
        while (m_TargetAngle > 360.0f)
        {
            m_TargetAngle -= 360.0f;
        }
        while (m_TargetAngle < -360.0f)
        {
            m_TargetAngle += 360.0f;
        }
    }

    public override void SetHighlightEnabled(bool enable)
    {
        m_ObjectHighlight?.SetHighlightEnabled(enable, ObjectHighlightGizmoType.Rotate);
    }

    private void Start()
    {
        m_InitialAangle = transform.eulerAngles.z;
        m_TargetAngle = m_InitialAangle;
    }

    private void Update()
    {
        Vector3 eulerAngles = transform.localEulerAngles;
        eulerAngles.z = Mathf.SmoothDampAngle(eulerAngles.z, m_TargetAngle, ref m_AngleSpeed, m_RotationSmoothing);
        transform.localEulerAngles = eulerAngles;
    }
}
