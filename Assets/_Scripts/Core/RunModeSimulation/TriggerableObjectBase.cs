using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerableObjectBase : MonoBehaviour, ITriggerableObject
{
    public abstract void OnResetObject();
    public abstract void OnTriggerInput();
    public abstract bool CanTriggerAgain();
    public abstract void SetHighlightEnabled(bool enable);
}
