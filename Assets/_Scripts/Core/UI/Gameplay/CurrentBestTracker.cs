using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrentBestTracker : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    float m_CurrentBestTime = 0.0f;

    bool m_ShouldAnimate = false;

    object m_CurrTweenID = 0;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += OnStartGame;
    }
    private void OnDisable()
    {
        GameManager.OnStartGame -= OnStartGame;
    }
    private void OnDestroy()
    {
        GameManager.OnStartGame -= OnStartGame;
    }

    private void OnStartGame()
    {
        if (GameData.Current.levelData.levelTimes != null)
        {
            UpdateBestTime(GameData.Current.levelData.levelTimes[SceneManager.GetActiveScene().buildIndex]);
        }
    }

    public void UpdateBestTime(float time, bool save = false)
    {
        m_ShouldAnimate = false;
        if (m_CurrentBestTime == 0)
        {
            m_CurrentBestTime = time;
            m_ShouldAnimate = true;
        }
        else if (time < m_CurrentBestTime)
        {
            m_CurrentBestTime = time;
            m_ShouldAnimate = true;
        }

        if (m_ShouldAnimate)
        {
            if (GameData.Current.levelData.levelTimes != null && save)
            {
                GameData.Current.levelData.levelTimes[SceneManager.GetActiveScene().buildIndex] = m_CurrentBestTime;
                SerializationManager.Save("gameData", GameData.Current);
            }

            DOTween.Kill(m_CurrTweenID);
            m_CurrTweenID = transform.DOScale(Vector3.one, 0.5f).From(2.0f * Vector3.one).id;
            m_Text.SetText($"Best Time: {TimeAttackTimer.GetTimerText(m_CurrentBestTime)}");
        }
    }

}
