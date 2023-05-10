using DG.Tweening;
using UnityEngine;


[DisallowMultipleComponent]
[RequireComponent(typeof(WaypointSystem))]
public class MoveInteraction : TriggerableObjectBase
{
    [SerializeField]
    float[] m_MoveVariables;
    [SerializeField]
    bool m_UseSpeeds = false;
    [SerializeField]
    bool m_ResetOnTrigger = false;
    [SerializeField]
    Transform m_ResetTransform;

    [SerializeField]
    ObjectHighlight m_ObjectHighlight = null;

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

    Rigidbody2D m_Rigidbody2D = null;

    bool m_IsMoving = false;
    float m_CurrValue = 0.0f;
    bool m_CanTrigger = true;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (m_MoveVariables.Length != waypointSystem.TotalPoints - 1)
        {
            Debug.LogError("You should provide as many speeds or times as many pairs of waypoints exist.");
            return;
        }
        if (!m_IsMoving)
        {
            return;
        }

        if (!m_UseSpeeds)
        {
            float interp = Mathf.Clamp01(m_CurrValue / m_MoveVariables[waypointSystem.CurrentStartIndex]);
            m_Rigidbody2D.MovePosition(waypointSystem.Evaluate(interp));
            m_CurrValue += Time.fixedDeltaTime;

            if (m_CurrValue >= m_MoveVariables[waypointSystem.CurrentStartIndex])
            {
                m_CurrValue = 0.0f;
                waypointSystem.NextPair();
                m_IsMoving = false;
            }
        }
        else
        {
            m_Rigidbody2D.MovePosition(Vector3.MoveTowards(m_Rigidbody2D.position, waypointSystem.CurrentEnd, m_MoveVariables[waypointSystem.CurrentStartIndex] * Time.fixedDeltaTime));
            if (m_Rigidbody2D.position == waypointSystem.CurrentEnd2D)
            {
                waypointSystem.NextPair();
                m_IsMoving = false;
            }
        }
    }

    public override void OnResetObject()
    {
        m_IsMoving = false;
        m_CanTrigger = true;
        if (m_ResetTransform)
        {
            m_CurrValue = waypointSystem.Interpolation;
            Vector3 position = waypointSystem.RestoreSavedPoint();
            m_Rigidbody2D.MovePosition(position);
        }
        else
        {
            m_CurrValue = 0.0f;
            waypointSystem.ResetDisplacement();
            m_Rigidbody2D.MovePosition(waypointSystem.CurrentStart);
        }
    }

    public override void SetHighlightEnabled(bool enable)
    {
        m_ObjectHighlight?.SetHighlightEnabled(enable);
    }

    public override void OnTriggerInput()
    {
        if(m_IsMoving)
        {
            return;
        }
        if (!m_CanTrigger)
        {
            return;
        }
        m_CanTrigger = waypointSystem.IsNextPairValid();
        m_IsMoving = true;
        if (m_ResetOnTrigger)
        {
            m_CurrValue = 0.0f;
        }
    }

    public override bool CanTriggerAgain()
    {
        return m_CanTrigger;
    }
}
