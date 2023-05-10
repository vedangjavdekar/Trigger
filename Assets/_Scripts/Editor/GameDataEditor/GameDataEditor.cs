using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

public class GameDataEditor : EditorWindow
{

    Toolbar m_Toolbar;

    IntegerField m_IntegerField;
    ListView m_ListView;

    [MenuItem("Tools/GameData/GameDataEditor")]
    public static void OpenGameDataEditor()
    {
        GetWindow<GameDataEditor>("Game Data Editor");
    }


    private void CreateGUI()
    {
        m_Toolbar = new Toolbar();
        var saveButton = new Button(
            () =>
            {
                if (!Application.isPlaying)
                {
                    SerializationManager.Save("gameData", GameData.Current);
                }
            });
        saveButton.text = "Save";

        var loadButton = new Button(
            () =>
            {
                if (!Application.isPlaying)
                {
                    GameData saveData = (GameData)SerializationManager.Load("gameData");
                    if (saveData != null)
                    {
                        GameData.Initialize(saveData);
                        m_IntegerField.value = GameData.Current.levelData.currentLevel;
                        m_ListView.itemsSource = GameData.Current.levelData.levelTimes;
                    }
                }
            });
        loadButton.text = "Load";

        var resetButton = new Button(() =>
        {
           var levels = (LevelNamesSO)Resources.Load("Levels");
            GameData.Current.levelData = new LevelData();
            GameData.Current.levelData.levelTimes = new List<float>(levels.levelNames.Count);
            SerializationManager.Save("gameData", GameData.Current);

            m_IntegerField.value = GameData.Current.levelData.currentLevel;
            m_ListView.itemsSource = GameData.Current.levelData.levelTimes;
        });
        resetButton.text = "Reset";

        m_Toolbar.Add(saveButton);
        m_Toolbar.Add(loadButton);
        m_Toolbar.Add(resetButton);
        rootVisualElement.Add(m_Toolbar);

        Label levelLabel = new Label("Game Data");
        levelLabel.style.fontSize = 16;
        m_IntegerField = new IntegerField("Current Level");
        m_IntegerField.value = GameData.Current.levelData.currentLevel;
        m_IntegerField.RegisterValueChangedCallback((ChangeEvent<int> eventData) =>
        {
            if (eventData.newValue >= 0)
            {
                GameData.Current.levelData.currentLevel = eventData.newValue;
            }
            else
            {
                m_IntegerField.value = 0;
            }
        });

        Label levelTimes = new Label("Level Times");
        levelTimes.style.fontSize = 16;
        m_ListView = new ListView();

        rootVisualElement.Add(levelLabel);
        rootVisualElement.Add(m_IntegerField);
        rootVisualElement.Add(levelTimes);
        rootVisualElement.Add(m_ListView);

        if (Application.isPlaying)
        {
            m_Toolbar?.SetEnabled(false);
            m_IntegerField?.SetEnabled(false);
        }
        else
        {
            m_Toolbar?.SetEnabled(true);
            m_IntegerField?.SetEnabled(true);
        }
    }

    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            m_Toolbar?.SetEnabled(false);
            m_IntegerField?.SetEnabled(false);
        }
        else
        {
            m_Toolbar?.SetEnabled(true);
            m_IntegerField?.SetEnabled(true);
        }

    }
}
