using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{
    [SerializeField]
    GameObject m_ScoreEntryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rect = transform as RectTransform;

        if (GameData.Current.levelData.levelTimes != null)
        {
            for (int i = 0; i < GameData.Current.levelData.levelTimes.Count; i++)
            {
                GameObject scoreEntry = Instantiate(m_ScoreEntryPrefab);
                scoreEntry.transform.SetParent(rect);
                scoreEntry.transform.localScale = Vector3.one;

                if (scoreEntry.TryGetComponent(out ScoreEntry scoreEntryComp))
                {
                    scoreEntryComp.SetIndex(i);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}
