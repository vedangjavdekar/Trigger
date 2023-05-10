using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Testing")]
    public bool testCurrentScene = false;

    [SerializeField]
    private LevelNamesSO m_LevelNamesSO;

    protected int totalLevels
    {
        get
        {
            if (!m_LevelNamesSO)
            {
                return -1;
            }
            return m_LevelNamesSO.levelNames.Count;
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene from, Scene to)
    {
        GameManager.Instance.StartGame();
    }

    public string GetLevelName(int levelIndex)
    {
        if (!m_LevelNamesSO || levelIndex >= m_LevelNamesSO.levelNames.Count)
        {
            return "";
        }

        return m_LevelNamesSO.levelNames[levelIndex];
    }

    private void Start()
    {
        if (testCurrentScene)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
            else
            {
                GameManager.Instance.StartGame();
            }
        }
    }

    public void LoadLevel()
    {
        GameData saveData = (GameData)SerializationManager.Load("gameData");
        if (saveData == null)
        {
            GameData.Current.levelData = new LevelData();
            GameData.Current.levelData.levelTimes = new List<float>(totalLevels);
            SerializationManager.Save("gameData", GameData.Current);
        }
        else
        {
            GameData.Initialize(saveData);
        }

        if (totalLevels > 0)
        {
            if (totalLevels > GameData.Current.levelData.levelTimes.Count)
            {
                while (GameData.Current.levelData.levelTimes.Count != totalLevels)
                {
                    GameData.Current.levelData.levelTimes.Add(0);
                }
            }
            if (totalLevels < GameData.Current.levelData.levelTimes.Count)
            {
                while (GameData.Current.levelData.levelTimes.Count != totalLevels)
                {
                    GameData.Current.levelData.levelTimes.Remove(GameData.Current.levelData.levelTimes.Count - 1);
                }
            }
        }

        if(GameData.Current.levelData.currentLevel == totalLevels)
        {
            GameData.Current.levelData.currentLevel = totalLevels - 1;
        }

        SceneManager.LoadScene(GameData.Current.levelData.currentLevel, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        if (GameData.Current.levelData.currentLevel < totalLevels-1)
        {
            GameData.Current.levelData.currentLevel++;
            SerializationManager.Save("gameData", GameData.Current);

            SceneManager.LoadScene(GameData.Current.levelData.currentLevel, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("ThankYou", LoadSceneMode.Single);
        }
    }
    public void LoadPreviousLevel()
    {
        if (GameData.Current.levelData.currentLevel > 0)
        {
            GameData.Current.levelData.currentLevel--;
            SerializationManager.Save("gameData", GameData.Current);

            SceneManager.LoadScene(GameData.Current.levelData.currentLevel, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
