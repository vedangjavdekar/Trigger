using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisualizable
{
    void CreateVisualizer(VisualizationSO creationData);
    void RemoveVisualizer();
}
