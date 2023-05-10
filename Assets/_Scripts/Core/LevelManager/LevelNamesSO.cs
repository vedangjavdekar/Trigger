using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Levels Data", menuName ="CustomData/LevelNames")]
public class LevelNamesSO : ScriptableObject
{
    public List<string> levelNames = new List<string>();
}
