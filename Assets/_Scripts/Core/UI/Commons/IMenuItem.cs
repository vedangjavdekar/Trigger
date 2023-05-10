using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuItem
{
    void OnFocus();
    void OnBlur();
    void OnSelect();
}
