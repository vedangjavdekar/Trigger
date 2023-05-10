using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MenuControllerBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.OnStartGame += StartGame;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnStartGame -= StartGame;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.OnStartGame -= StartGame;
    }

    private void StartGame()
    {
        GameManager.Instance.gameInputController.GetMenuActions().Enable();
        m_CurrentSelectedIndex = 0;
        if (menuItems.Count > 0)
        {
            menuItems[m_CurrentSelectedIndex].OnFocus();
        }
    }

    public void PlayGame()
    {
        GameManager.Instance.gameInputController.GetMenuActions().Disable();
        GameManager.Instance.EndGame(1);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
