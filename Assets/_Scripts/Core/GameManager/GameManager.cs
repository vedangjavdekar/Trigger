using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion

    [SerializeField]
    private GameStateManager m_GameStateManager = null;
    public GameStateManager gameStateManager => m_GameStateManager;
    [SerializeField]
    private GameInputController m_GameInputController = null;
    public GameInputController gameInputController => m_GameInputController;

    [SerializeField]
    private LevelManager m_LevelManager = null;
    public LevelManager levelManager => m_LevelManager;

    [SerializeField]
    private SoundManager m_SoundManager = null;
    public SoundManager soundManager => m_SoundManager;


    public static event Action OnStartGame;
    public static event Action<int> OnEndGame;

    public void StartGame()
    {
        OnStartGame?.Invoke();
        gameStateManager.OverrideState(GameState.StartEditMode);
    }

    public void EndGame(int direction)
    {
        m_GameInputController.DisableLevelControls();
        OnEndGame?.Invoke(direction);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
    }
}
