using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaypointInteraction
{
    List<Transform> GetWaypoints();
    
    bool cycle { get; }
}
