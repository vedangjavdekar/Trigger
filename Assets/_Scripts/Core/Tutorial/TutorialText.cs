using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialText : MonoBehaviour
{
    [System.Serializable]
    private struct TutorialFlowData
    {
        public KeyCode key;
        public string text;
        public bool waitUntil;
        public GameState waitUntilState;
    }

    [SerializeField]
    private TutorialFlowData[] m_FlowData;

    [SerializeField]
    int m_CurrIndex = -1;

    TextMeshPro m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshPro>();

        if (m_CurrIndex + 1 < m_FlowData.Length)
        {
            m_Text.SetText(m_FlowData[m_CurrIndex + 1].text);
            m_CurrIndex++;
        }
    }

    private void Update()
    {
        if (m_CurrIndex >= m_FlowData.Length)
        {
            m_Text.enabled = false;
            return;
        }

        if (!m_FlowData[m_CurrIndex].waitUntil)
        {
            if (Input.GetKeyDown(m_FlowData[m_CurrIndex].key))
            {
                if (m_CurrIndex + 1 < m_FlowData.Length)
                {
                    m_Text.SetText(m_FlowData[m_CurrIndex + 1].text);
                }
                m_CurrIndex++;
            }
        }
        else
        {
            if (m_FlowData[m_CurrIndex].waitUntilState == GameManager.Instance.gameStateManager.currState)
            {
                if (m_CurrIndex + 1 < m_FlowData.Length)
                {
                    m_Text.SetText(m_FlowData[m_CurrIndex + 1].text);
                }
                m_CurrIndex++;
            }
        }
    }
}
