using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Visualization Prefab Asset", menuName = "Visualization/VisualizationPrefabAsset")]
public class VisualizationSO : ScriptableObject
{
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public float circleSize;
    public float lineSize;
}
