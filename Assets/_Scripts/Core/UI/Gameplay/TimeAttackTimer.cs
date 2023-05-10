using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimeAttackTimer : MonoBehaviour, IGameStateHandler
{
    private TextMeshProUGUI m_TimerText;

    bool m_TimerActive = false;
    float m_CurrTime = 0.0f;
    float m_StartTime = 0.0f;

    [SerializeField]
    CurrentBestTracker m_CurrentBestTracker;

    object m_CurrTweenID = 0;
    private void Awake()
    {
        m_TimerText = GetComponent<TextMeshProUGUI>();
    }

    public void OnGameStateEnd(GameState state)
    {
    }

    public void OnGameStateStart(GameState state)
    {
        if (state == GameState.StartSimulation)
        {
            StartTimer();
        }
        else if (state == GameState.GoalReached)
        {
            StopTimer();
            m_CurrentBestTracker.UpdateBestTime(m_CurrTime, true);
        }
        else
        {
            ResetTimer();
        }
    }

    private void StartTimer()
    {
        m_StartTime = Time.realtimeSinceStartup;
        m_CurrTime = m_StartTime;
        m_TimerActive = true;
    }

    private void StopTimer()
    {
        m_TimerActive = false;
        DOTween.Kill(m_CurrTweenID);
        m_CurrTweenID = transform.DOScale(Vector3.one, 0.3f).From(1.5f * Vector3.one).SetEase(Ease.InBack).id;
    }

    private void ResetTimer()
    {
        m_TimerActive = false;
        m_CurrTime = 0.0f;
        m_TimerText.SetText(GetTimerText(m_CurrTime));
    }

    private void Update()
    {
        if (!m_TimerActive)
        {
            return;
        }
        m_CurrTime = Time.realtimeSinceStartup - m_StartTime;
        m_TimerText.SetText(GetTimerText(m_CurrTime));
    }

    public static string GetTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60.0f);
        time -= (60 * minutes);
        int seconds = Mathf.FloorToInt(time);
        time -= seconds;
        int miliSeconds = Mathf.FloorToInt(1000 * time);

        return $"{minutes:00}:{seconds:00}.{miliSeconds:000}";
    }
}
