using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreEntry : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_LevelName;
    [SerializeField]
    TextMeshProUGUI m_LevelTime;

    public void SetIndex(int index)
    {
        if(index < 0)
        {
            return;
        }
        m_LevelName.SetText(GameManager.Instance.levelManager.GetLevelName(index));
        string formattedTime = TimeAttackTimer.GetTimerText(GameData.Current.levelData.levelTimes[index]);
        m_LevelTime.SetText(formattedTime);
    }
}
