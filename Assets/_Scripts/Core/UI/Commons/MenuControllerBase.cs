using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MenuControllerBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().PauseGame.performed += OnShowMenu;

        GameManager.Instance.gameInputController.GetMenuControlActions().Next.performed += OnNextItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Previous.performed += OnPreviousItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Select.performed += OnSelectItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Exit.performed += OnExitMenu;
    }

    protected virtual void OnDisable()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().PauseGame.performed -= OnShowMenu;

        GameManager.Instance.gameInputController.GetMenuControlActions().Next.performed -= OnNextItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Previous.performed -= OnPreviousItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Select.performed -= OnSelectItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Exit.performed -= OnExitMenu;
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.gameInputController.GetGlobalControlsActions().PauseGame.performed -= OnShowMenu;

        GameManager.Instance.gameInputController.GetMenuControlActions().Next.performed -= OnNextItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Previous.performed -= OnPreviousItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Select.performed -= OnSelectItem;
        GameManager.Instance.gameInputController.GetMenuControlActions().Exit.performed -= OnExitMenu;
    }

    private List<IMenuItem> m_MenuItems = null;
    protected List<IMenuItem> menuItems
    {
        get
        {
            if (m_MenuItems == null)
            {
                m_MenuItems = GetComponentsInChildren<IMenuItem>().ToList();
            }
            return m_MenuItems;
        }
    }

    protected int m_CurrentSelectedIndex = -1;

    protected virtual void OnShowMenu(InputAction.CallbackContext context) 
    {
    }

    protected virtual void OnExitMenu(InputAction.CallbackContext context) 
    { 
    }

    private void OnNextItem(InputAction.CallbackContext context)
    {
        if (m_CurrentSelectedIndex != -1)
        {
            menuItems[m_CurrentSelectedIndex].OnBlur();
        }
        m_CurrentSelectedIndex = (m_CurrentSelectedIndex + 1) % menuItems.Count;
        menuItems[m_CurrentSelectedIndex].OnFocus();
    }

    private void OnPreviousItem(InputAction.CallbackContext context)
    {
        if (m_CurrentSelectedIndex != -1)
        {
            menuItems[m_CurrentSelectedIndex].OnBlur();
        }
        m_CurrentSelectedIndex = (m_CurrentSelectedIndex - 1 + menuItems.Count) % menuItems.Count;
        menuItems[m_CurrentSelectedIndex].OnFocus();
    }
    private void OnSelectItem(InputAction.CallbackContext context)
    {
        if (m_CurrentSelectedIndex != -1)
        {
            menuItems[m_CurrentSelectedIndex].OnBlur();
        }
        menuItems[m_CurrentSelectedIndex].OnSelect();

    }

}
