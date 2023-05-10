using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    CanvasGroup m_CanvasGroup;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_Text = GetComponentInChildren<TextMeshProUGUI>();

        if (m_Text != null)
        {
            m_Text.SetText(SceneManager.GetActiveScene().name);
            m_Text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += OnStartGame;
        GameManager.OnEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= OnStartGame;
        GameManager.OnEndGame -= OnEndGame;
    }
    private void OnDestroy()
    {
        GameManager.OnStartGame -= OnStartGame;
        GameManager.OnEndGame -= OnEndGame;
    }

    private void OnEndGame(int direction)
    {
        m_CanvasGroup.blocksRaycasts = true;
        m_CanvasGroup.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                GameManager.Instance.levelManager.LoadLevel();
            }
            else
            {
                if (direction < 0)
                {
                    GameManager.Instance.levelManager.LoadPreviousLevel();
                }
                else if (direction > 0)
                {
                    GameManager.Instance.levelManager.LoadNextLevel();
                }
            }
        });
    }
   
    private void OnStartGame()
    {
        m_CanvasGroup.blocksRaycasts = true;
        var sequence = DOTween.Sequence();
        if (m_Text)
        {
            sequence.Append(m_Text.DOFade(1.0f, 1.0f));
            sequence.AppendInterval(1.0f);
            sequence.Append(m_Text.DOFade(0.0f, 1.0f));
            sequence.AppendInterval(1.0f);
        }
        sequence.Append(m_CanvasGroup.DOFade(0.0f, 1.0f)
            .OnComplete(() =>
            {
                m_CanvasGroup.interactable = false;
                m_CanvasGroup.blocksRaycasts = false;
                GameManager.Instance.gameInputController.EnableLevelControls();
            }));
    }
}
