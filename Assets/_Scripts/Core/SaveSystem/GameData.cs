using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    private static GameData m_Current = null;
    public static GameData Current
    {
        get
        {
            if (m_Current == null)
            {
                m_Current = new GameData();
            }
            return m_Current;
        }
    }

    public static void Initialize(GameData gameData)
    {
        m_Current = gameData;
    }

    public LevelData levelData = new LevelData();
}
