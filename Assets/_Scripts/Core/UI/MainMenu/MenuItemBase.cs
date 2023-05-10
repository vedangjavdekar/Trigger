using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuItemBase : MonoBehaviour, IMenuItem
{
    public abstract void OnBlur();
    public abstract void OnFocus();
    public abstract void OnSelect();
}
