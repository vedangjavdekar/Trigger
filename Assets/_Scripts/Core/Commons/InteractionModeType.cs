using System;
using UnityEngine;

[Flags]
public enum InteractionModeType
{
    None = 0,
    EditMode = 1,
    RunMode = 2,
}

[System.Serializable]
public class EditModeObjectData
{
    public GameObject obj;
    public EditableObjectBase editableObject;
}

[System.Serializable]
public class RunModeObjectData
{
    public GameObject obj;
    public TriggerableObjectBase triggerableObject;
}