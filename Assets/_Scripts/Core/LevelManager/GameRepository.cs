using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRepository : MonoBehaviour
{
    public static GameRepository Instance { get; private set; }

    [SerializeField]
    private ModeInputHandler m_ModeInputHandler;
    public ModeInputHandler modeInputHandler => m_ModeInputHandler;

    [SerializeField]
    private EditModeController m_EditModeController;
    public EditModeController editModeController => m_EditModeController;

    [SerializeField]
    private RunModeController m_RunModeController;
    public RunModeController runModeController => m_RunModeController;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
    }
}
