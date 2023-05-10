using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditableObjectBase : MonoBehaviour, IEditableObject
{
    public abstract void DecrementInput(bool wasPressedThisFrame);

    public abstract void IncrementInput(bool wasPressedThisFrame);

    public abstract void SetHighlightEnabled(bool enable);
}
