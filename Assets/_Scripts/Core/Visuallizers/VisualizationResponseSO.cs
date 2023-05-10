using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Visualization Response", menuName ="Visualization/VisualizationResponse")]
public class VisualizationResponseSO : ScriptableObject
{
    public Color visible = Color.white;
    public Color semiVisible = Color.white;
    public Color hidden = Color.white;
}
