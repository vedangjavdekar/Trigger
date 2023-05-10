using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : TriggerableObjectBase
{
    [SerializeField]
    ObjectHighlight m_ObjectHighlight;

    Collider2D m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    public override void OnResetObject()
    {
        transform.localScale = Vector3.one;
        m_Collider.enabled = true;
    }

    public override void OnTriggerInput()
    {
        m_Collider.enabled = false;
        transform.DOScaleX(0.0f, 0.2f).SetEase(Ease.OutCirc);
    }

    public override void SetHighlightEnabled(bool enable)
    {
        m_ObjectHighlight?.SetHighlightEnabled(enable);
    }

    public override bool CanTriggerAgain()
    {
        return false;
    }
}
